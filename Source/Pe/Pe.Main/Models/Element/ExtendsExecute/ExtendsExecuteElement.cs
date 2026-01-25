using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using System.Threading;
using ContentTypeTextNet.Pe.Main.Models.Applications.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute
{
    public class ExtendsExecuteElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        private bool _isVisible;

        #endregion

        public ExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, LauncherRedoData launcherRedoData, IScreen screen, IOrderManager orderManager, INotifyManager notifyManager, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            CaptionName = captionName;
            LauncherFileData = launcherFileData;
            EnvironmentVariables = launcherEnvironmentVariables;
            LauncherRedoData = launcherRedoData;
            Screen = screen;

            OrderManager = orderManager;
            NotifyManager = notifyManager;
            ContextDispatcher = contextDispatcher;
        }

        #region property

        public LauncherFileData LauncherFileData { get; protected set; }
        public IReadOnlyList<LauncherEnvironmentVariableData> EnvironmentVariables { get; protected set; }
        public IReadOnlyList<LauncherHistoryData> HistoryOptions { get; protected set; } = new List<LauncherHistoryData>();
        public IReadOnlyList<LauncherHistoryData> HistoryWorkDirectories { get; protected set; } = new List<LauncherHistoryData>();

        public LauncherRedoData LauncherRedoData { get; protected set; }

        public IScreen Screen { get; }

        private IOrderManager OrderManager { get; }
        private INotifyManager NotifyManager { get; }
        private IContextDispatcher ContextDispatcher { get; }

        private bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public string CaptionName { get; protected set; }

        #endregion

        #region function

        public virtual async Task<ILauncherExecuteResult> ExecuteAsync(LauncherFileData fileData, IReadOnlyList<LauncherEnvironmentVariableData> environmentVariables, IReadOnlyLauncherRedoData redoData, IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try {
                var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache.Instance, OrderManager, NotifyManager, ContextDispatcher, LoggerFactory);
                var result = await launcherExecutor.ExecuteAsync(LauncherItemKind.File, fileData, fileData, environmentVariables, redoData, screen, cancellationToken);
                return result;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherFileExecuteResult.Error(ex);
            }
        }

        public virtual bool RemoveHistory(LauncherHistoryKind kind, DateTime lastExecuteTimestamp)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            // 独立した何かなのでここでは何もしない
            return Task.CompletedTask;
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

                return IsVisible;
            }
        }

        public void StartView()
        {
            var windowItem = OrderManager.CreateExtendsExecuteWindow(this);
            windowItem.Window.Show();
            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            IsVisible = false;
            return true;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool, CancellationToken)"/>
        public Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            ViewCreated = false;
            return Task.CompletedTask;
        }

        #endregion

    }

    public sealed class LauncherExtendsExecuteElement: ExtendsExecuteElement, ILauncherItemId
    {
        public LauncherExtendsExecuteElement(LauncherItemId launcherItemId, IScreen screen, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, INotifyManager notifyManager, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(string.Empty, new LauncherFileData(), new List<LauncherEnvironmentVariableData>(), new LauncherRedoData(), screen, orderManager, notifyManager, contextDispatcher, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        /// <summary>
        /// 後付けオプション設定。
        /// </summary>
        /// <remarks>
        /// <para>null を許容しているけど設定未設定の判別のみに使用するため null は設定不可。</para>
        /// <para>設定には <see cref="SetOption(string)"/> を使用する。</para>
        /// </remarks>
        public string? CustomOption { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="CustomOption"/>の設定処理。
        /// </summary>
        /// <param name="option"></param>
        public void SetOption(string option)
        {
            ThrowIfDisposed();

            CustomOption = option ?? throw new ArgumentNullException(nameof(option));
        }

        /// <summary>
        /// 履歴を削除する。
        /// </summary>
        /// <param name="kind"><see cref="LauncherHistoryKind"/>。</param>
        /// <param name="lastExecuteTimestamp">実行日時。</param>
        /// <returns></returns>
        public override bool RemoveHistory(LauncherHistoryKind kind, [DateTimeKind(DateTimeKind.Utc)] DateTime lastExecuteTimestamp)
        {
            ThrowIfDisposed();

            bool removed;

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var daoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                var launcherItemHistoriesEntityDao = daoFactory.Create<LauncherItemHistoriesEntityDao>();

                var removedCount = launcherItemHistoriesEntityDao.DeleteHistoryByLauncherItemId(LauncherItemId, kind, lastExecuteTimestamp);
                if(1 < removedCount) {
                    // ここに来ることはないと思ってるけどテーブルレイアウトは許容可能なデータなので一応警告
                    Logger.LogWarning("削除数が1を超過: {0}", removedCount);
                }
                removed = removedCount != 0;

                context.Commit();
            }

            return removed;
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion

        #region ExtendsExecuteElement

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            LauncherItemData launcherItem;
            LauncherFileData fileData;
            IEnumerable<LauncherEnvironmentVariableData> envItems;
            IEnumerable<LauncherHistoryData> histories;
            LauncherRedoData launcherRedoData;

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var daoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                var launcherItemsEntityDao = daoFactory.Create<LauncherItemsEntityDao>();
                var launcherFilesEntityDao = daoFactory.Create<LauncherFilesEntityDao>();
                var launcherEnvVarsEntityDao = daoFactory.Create<LauncherEnvVarsEntityDao>();
                var launcherItemHistoriesEntityDao = daoFactory.Create<LauncherItemHistoriesEntityDao>();
                var launcherRedoItemsEntityDao = daoFactory.Create<LauncherRedoItemsEntityDao>();
                var launcherRedoSuccessExitCodesEntityDao = daoFactory.Create<LauncherRedoSuccessExitCodesEntityDao>();

                launcherItem = launcherItemsEntityDao.SelectLauncherItem(LauncherItemId);
                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                envItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId);
                histories = launcherItemHistoriesEntityDao.SelectHistories(LauncherItemId);

                if(launcherRedoItemsEntityDao.SelectExistsLauncherRedoItem(LauncherItemId)) {
                    launcherRedoData = launcherRedoItemsEntityDao.SelectLauncherRedoItem(LauncherItemId);
                    var values = launcherRedoSuccessExitCodesEntityDao.SelectRedoSuccessExitCodes(LauncherItemId);
                    launcherRedoData.SuccessExitCodes.SetRange(values);
                } else {
                    launcherRedoData = LauncherRedoData.GetDisable();
                }
            }

            LauncherFileData = fileData;
            EnvironmentVariables = envItems.ToList();
            LauncherRedoData = launcherRedoData;
            CaptionName = launcherItem.Name; // ?? launcherItem.Code ?? Path.GetFileNameWithoutExtension(LauncherFileData.Path) ?? LauncherItemId.ToString("D");

            var histories2 = histories.ToList();
            HistoryOptions = histories2.Where(i => i.Kind == LauncherHistoryKind.Option).ToList();
            HistoryWorkDirectories = histories2.Where(i => i.Kind == LauncherHistoryKind.WorkDirectory).ToList();

            return Task.CompletedTask;
        }

        public override async Task<ILauncherExecuteResult> ExecuteAsync(LauncherFileData fileData, IReadOnlyList<LauncherEnvironmentVariableData> environmentVariables, IReadOnlyLauncherRedoData redoData, IScreen screen, CancellationToken cancellationToken)
        {
            var result = await base.ExecuteAsync(fileData, environmentVariables, redoData, screen, cancellationToken);

            if(result.Success) {
                using(var context = MainDatabaseBarrier.WaitWrite()) {
                    var daoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                    var launcherItemsEntityDao = daoFactory.Create<LauncherItemsEntityDao>();
                    var launcherItemHistoriesEntityDao = daoFactory.Create<LauncherItemHistoriesEntityDao>();

                    launcherItemsEntityDao.UpdateExecuteCountIncrement(LauncherItemId, DatabaseCommonStatus.CreateCurrentAccount());

                    var item = launcherItemsEntityDao.SelectLauncherItem(LauncherItemId);
                    if(item.Kind == LauncherItemKind.File) {
                        var launcherFilesEntityDao = daoFactory.Create<LauncherFilesEntityDao>();
                        var launcherFileData = launcherFilesEntityDao.SelectFile(LauncherItemId);

                        launcherItemHistoriesEntityDao.DeleteHistory(LauncherItemId, LauncherHistoryKind.Option, fileData.Option);
                        if(launcherFileData.Option != fileData.Option) {
                            launcherItemHistoriesEntityDao.InsertHistory(LauncherItemId, LauncherHistoryKind.Option, fileData.Option, DateTime.UtcNow, DatabaseCommonStatus.CreateCurrentAccount());
                        }

                        launcherItemHistoriesEntityDao.DeleteHistory(LauncherItemId, LauncherHistoryKind.WorkDirectory, fileData.WorkDirectoryPath);
                        if(launcherFileData.WorkDirectoryPath != fileData.WorkDirectoryPath) {
                            launcherItemHistoriesEntityDao.InsertHistory(LauncherItemId, LauncherHistoryKind.WorkDirectory, fileData.WorkDirectoryPath, DateTime.UtcNow, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                    }

                    context.Commit();
                }
            }

            return result;
        }

        #endregion
    }
}
