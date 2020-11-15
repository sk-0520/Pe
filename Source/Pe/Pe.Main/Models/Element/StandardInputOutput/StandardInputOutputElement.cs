using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput
{
    public class StandardInputOutputElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;
        bool _preparatedReceive;
        bool _processExited;

        #endregion

        public StandardInputOutputElement(string captionName, Process process, IScreen screen, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if(!process.EnableRaisingEvents) {
                throw new ArgumentException($"{nameof(process)}.{process.EnableRaisingEvents}");
            }

            CaptionName = captionName;
            Process = process;
            Screen = screen;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;

            Process.Exited += Process_Exited;
        }

        #region property

        public string CaptionName { get; }
        public Process Process { get; }
        IScreen Screen { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IOrderManager OrderManager { get; }

        public StreamReceiver? OutputStreamReceiver { get; private set; }
        public StreamReceiver? ErrorStreamReceiver { get; private set; }

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public bool PreparatedReceive
        {
            get => this._preparatedReceive;
            private set => SetProperty(ref this._preparatedReceive, value);
        }

        public bool ProcessExited
        {
            get => this._processExited;
            private set => SetProperty(ref this._processExited, value);
        }

        public FontElement? Font { get; private set; }
        public Color OutputForegroundColor { get; private set; }
        public Color OutputBackgroundColor { get; private set; }
        public Color ErrorForegroundColor { get; private set; }
        public Color ErrorBackgroundColor { get; private set; }

        public bool IsTopmost { get; set; }

        #endregion

        #region function

        public void PreparateReceiver()
        {
            ThrowIfDisposed();

            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, exit code = {1}, exit time = {2}", Process.Id, Process.ExitCode, Process.ExitTime);
                return;
            }

            OutputStreamReceiver = new StreamReceiver(Process.StandardOutput, LoggerFactory);
            ErrorStreamReceiver = new StreamReceiver(Process.StandardError, LoggerFactory);
            //ProcessStandardOutputReceiver = new ProcessStandardOutputReceiver(Process, LoggerFactory);

            PreparatedReceive = true;
        }

        public void RunReceiver()
        {
            Debug.Assert(PreparatedReceive);
            ThrowIfDisposed();

            OutputStreamReceiver!.StartReceive();
            ErrorStreamReceiver!.StartReceive();
            //ProcessStandardOutputReceiver!.StartReceive();
        }

        public void Kill()
        {
            ThrowIfDisposed();

            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, name = {1}, exit coe = {2}, exit time = {3}", Process.Id, Process.ProcessName, Process.ExitCode, Process.ExitTime);
                return;
            }

            Process.Kill();
        }

        public void SendInputValue(string value)
        {
            ThrowIfDisposed();

            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, name = {1}, exit coe = {2}, exit time = {3}", Process.Id, Process.ProcessName, Process.ExitCode, Process.ExitTime);
                return;
            }

            Process.StandardInput.Write(value);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            SettingAppStandardInputOutputSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appStandardInputOutputSettingEntityDao.SelectSettingStandardInputOutputSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            Font.Initialize();

            OutputForegroundColor = setting.OutputForegroundColor;
            OutputBackgroundColor= setting.OutputBackgroundColor;
            ErrorForegroundColor = setting.ErrorForegroundColor;
            ErrorBackgroundColor = setting.ErrorBackgroundColor;

            IsTopmost = setting.IsTopmost;
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
            var windowItem = OrderManager.CreateStandardInputOutputWindow(this);
            windowItem.Window.Show();
            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            if(!ProcessExited) {
                Logger.LogWarning("実行中のプロセス: id = {0}, name = {1}", Process.Id, Process.ProcessName);
                return false;
            }

            return true;
        }

        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public void ReceiveViewClosed(bool isUserOperation)
        {
            ViewCreated = false;
        }


        #endregion

        private void Process_Exited(object? sender, EventArgs e)
        {
            ProcessExited = true;
            OutputStreamReceiver?.Dispose();
            ErrorStreamReceiver?.Dispose();
        }


    }
}
