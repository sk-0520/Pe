using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    /// <summary>
    /// 各設定項目の親。
    /// </summary>
    public abstract class SettingEditorElementBase : ElementBase
    {
        protected SettingEditorElementBase(ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            SettingNotifyManager = settingNotifyManager;
            ClipboardManager = clipboardManager;

            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            TemporaryDatabaseBarrier = temporaryDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;

            IdFactory = idFactory;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            DispatcherWrapper = dispatcherWrapper;

            SettingNotifyManager.LauncherItemRemoved += SettingNotifyManager_LauncherItemRemoved;
        }

        #region property

        protected ISettingNotifyManager SettingNotifyManager { get; }
        protected IClipboardManager ClipboardManager { get; }

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        protected ITemporaryDatabaseBarrier TemporaryDatabaseBarrier { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }
        protected IIdFactory IdFactory { get; }
        protected IImageLoader ImageLoader { get; }
        protected IMediaConverter MediaConverter { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        public bool IsLoaded { get; private set; }
        #endregion

        #region function

        protected abstract void LoadImpl();

        public void Load()
        {
            if(IsLoaded) {
                throw new InvalidOperationException(nameof(IsLoaded));
            }

            LoadImpl();

            IsLoaded = true;
        }

        protected abstract void SaveImpl(IDatabaseContextsPack commandPack);

        public void Save(IDatabaseContextsPack commandPack)
        {
            if(!IsLoaded) {
                throw new InvalidOperationException(nameof(IsLoaded));
            }

            SaveImpl(commandPack);
        }

        protected virtual void ReceiveLauncherItemRemoved(Guid launcherItemId)
        { }

        #endregion


        #region ElementBase

        protected override void InitializeImpl()
        {
            //NOTE: 設定処理では初期かではなくページ切り替え処理であれこれ頑張る
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                SettingNotifyManager.LauncherItemRemoved -= SettingNotifyManager_LauncherItemRemoved;
            }

            base.Dispose(disposing);
        }


        #endregion

        private void SettingNotifyManager_LauncherItemRemoved(object? sender, LauncherItemRemovedEventArgs e)
        {
            ReceiveLauncherItemRemoved(e.LauncherItemId);
        }


    }
}
