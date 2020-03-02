using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public class CommandElement : ElementBase, IViewShowStarter, IFlushable
    {
        public CommandElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IMainDatabaseLazyWriter mainDatabaseLazyWriter, CustomConfiguration customConfiguration, IOrderManager orderManager, IWindowManager windowManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
            OrderManager = orderManager;
            WindowManager = windowManager;
            NotifyManager = notifyManager;

            IconClearTimer = new Timer() {
                Interval = customConfiguration.Command.IconClearWaitTime.TotalMilliseconds,
            };
            IconClearTimer.Elapsed += IconClearTimerr_Elapsed;

            ViewCloseTimer = new Timer() {
                Interval = customConfiguration.Command.ViewCloseWaitTime.TotalMilliseconds,
            };
            ViewCloseTimer.Elapsed += ViewCloseTimer_Elapsed;

            LauncherItemCommandFinder = new LauncherItemCommandFinder(MainDatabaseBarrier, StatementLoader, OrderManager, NotifyManager, LoggerFactory);

            var commandFinders = new List<ICommandFinder>() {
                LauncherItemCommandFinder
            };
            CommandFinders = commandFinders;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        IOrderManager OrderManager { get; }
        IWindowManager WindowManager { get; }
        INotifyManager NotifyManager { get; }
        public bool ViewCreated { get; private set; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();
        public FontElement? Font { get; private set; }

        public bool FindTag { get; private set; }
        public double Width { get; private set; }
        public TimeSpan HideWaitTime { get; private set; }
        public IconBox IconBox { get; private set; }

        Timer ViewCloseTimer { get; }
        Timer IconClearTimer { get; }

        LauncherItemCommandFinder LauncherItemCommandFinder { get; }
        IReadOnlyCollection<ICommandFinder> CommandFinders { get; }

        #endregion

        #region function

        public void HideView(bool force)
        {
            Debug.Assert(ViewCreated);

            Flush();
            if(force) {
                ClearIcon();
                CloseView();
            } else {
                WindowManager.GetWindowItems(WindowKind.Command).First().Window.Hide();
                StartIconClear();
            }
        }


        private void StartIconClear()
        {
            IconClearTimer.Stop();
            IconClearTimer.Start();
        }

        private void StopIconClear()
        {
            StopViewClose();
            if(IconClearTimer.Enabled) {
                Logger.LogTrace("アイコンキャッシュ破棄待機 停止");
            }
            IconClearTimer.Stop();
        }

        private void ClearIcon()
        {
            Logger.LogDebug("アイコンキャッシュ破棄開始");

            LauncherItemCommandFinder.ClearIcon();

            StopIconClear();
            Logger.LogDebug("アイコンキャッシュ破棄終了");
        }

        private void StartViewClose()
        {
            ViewCloseTimer.Stop();
            ViewCloseTimer.Start();
        }
        private void StopViewClose()
        {
            if(ViewCloseTimer.Enabled) {
                Logger.LogTrace("ビュー破棄待機 停止");
            }
            ViewCloseTimer.Stop();
        }
        private void CloseView()
        {
            Logger.LogDebug("ビュー破棄開始");

            var view = WindowManager.GetWindowItems(WindowKind.Command).First().Window;
            view.Dispatcher.Invoke(() => {
                view.Close();
                ViewCreated = false;
                Logger.LogDebug("ビュー破棄終了");
            });
            StopViewClose();
        }

        private void RefreshSetting()
        {
            SettingAppCommandSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appCommandSettingEntityDao = new AppCommandSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appCommandSettingEntityDao.SelectSettingCommandSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            Font.Initialize();

            IconBox = setting.IconBox;
            Width = setting.Width;
            HideWaitTime = setting.HideWaitTime;
            FindTag = setting.FindTag;
        }

        public void Refresh()
        {
            // アイテム一覧とったりなんかしたりあれこれしたり
            RefreshSetting();

            // 諦め
            LauncherItemCommandFinder.FindTag = FindTag;
            LauncherItemCommandFinder.IconBox = IconBox;

            foreach(var commandFinder in CommandFinders) {
                commandFinder.Refresh();
            }
        }

        IEnumerable<ICommandItem> ListupCommandItems(string inputValue, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            var simpleRegexFactory = new SimpleRegexFactory(LoggerFactory);
            var regex = simpleRegexFactory.CreateFilterRegex(inputValue);

            foreach(var commandFinder in CommandFinders) {
                var items = commandFinder.ListupCommandItems(inputValue, regex, hitValuesCreator, cancellationToken);
                foreach(var item in items) {
                    yield return item;
                }
            }
        }



        public Task<IReadOnlyList<ICommandItem>> ListupCommandItemsAsync(string inputValue, CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                Logger.LogTrace("検索開始");
                var stopwatch = Stopwatch.StartNew();

                var hitValuesCreator = new HitValuesCreator(LoggerFactory);

                var commandItems = new List<ICommandItem>();
                foreach(var item in ListupCommandItems(inputValue, hitValuesCreator, cancellationToken)) {
                    commandItems.Add(item);
                    Logger.LogDebug(string.Join(" - ", item.HeaderValues.Select(i => i.Value)));
                }
                cancellationToken.ThrowIfCancellationRequested();

                Logger.LogTrace("検索終了: {0}", stopwatch.Elapsed);

                return (IReadOnlyList<ICommandItem>)commandItems
                    .OrderByDescending(i => i.Score)
                    .ToList()
                ;
            }, cancellationToken);
        }

        public void ChangeViewWidthDelaySave(double width)
        {
            var diff = Math.Abs(Width - width);
            if(diff < double.Epsilon) {
                Logger.LogTrace("{Width} - {width}: {Abs} < {Epsilon}", Width, width, diff, double.Epsilon);
                return;
            }
            Width = width;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new AppCommandSettingEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatCommandSettingWidth(Width, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                IconClearTimer.Elapsed -= IconClearTimerr_Elapsed;
                ViewCloseTimer.Elapsed -= ViewCloseTimer_Elapsed;

                IconClearTimer.Dispose();
                ViewCloseTimer.Dispose();

                foreach(var commandFinder in CommandFinders) {
                    commandFinder.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return true;
            }
        }

        public void StartView()
        {
            static void MoveCursorPosition(WindowItem item)
            {
                var hWnd = HandleUtility.GetWindowHandle(item.Window);
                if(hWnd != IntPtr.Zero) {
                    var deviceCursorPosition = MouseUtility.GetDevicePosition();
                    NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, (int)deviceCursorPosition.X, (int)deviceCursorPosition.Y, 0, 0, SWP.SWP_NOSIZE);
                }
            }

            WindowItem windowItem;
            if(!ViewCreated) {
                windowItem = OrderManager.CreateCommandWindow(this);
                windowItem.Window.Show();
                MoveCursorPosition(windowItem);
                ViewCreated = true;
            } else {
                StopIconClear();
                windowItem = WindowManager.GetWindowItems(WindowKind.Command).First();
                if(windowItem.Window.IsVisible) {
                    MoveCursorPosition(windowItem);
                    windowItem.Window.Activate();
                } else {
                    MoveCursorPosition(windowItem);
                    windowItem.Window.Show();
                }
            }

            windowItem.Window.Activate();
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return false;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

        #region IFlushable

        public void Flush()
        {
            MainDatabaseLazyWriter.SafeFlush();
        }

        #endregion

        private void IconClearTimerr_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearIcon();
            StartViewClose();
        }

        private void ViewCloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CloseView();
        }



    }
}
