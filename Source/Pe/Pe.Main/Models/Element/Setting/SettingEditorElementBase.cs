using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    /// <summary>
    /// 各設定項目の親。
    /// </summary>
    public abstract class SettingEditorElementBase: ElementBase
    {
        protected SettingEditorElementBase(ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            SettingNotifyManager = settingNotifyManager;
            ClipboardManager = clipboardManager;

            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            TemporaryDatabaseBarrier = temporaryDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;

            IdFactory = idFactory;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;

            SettingNotifyManager.LauncherItemRemoved += SettingNotifyManager_LauncherItemRemoved;
        }

        #region property

        protected ISettingNotifyManager SettingNotifyManager { get; }
        protected IClipboardManager ClipboardManager { get; }

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        protected ITemporaryDatabaseBarrier TemporaryDatabaseBarrier { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }
        protected IIdFactory IdFactory { get; }
        protected IImageLoader ImageLoader { get; }
        protected IMediaConverter MediaConverter { get; }
        protected IPolicy Policy { get; }
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

        protected abstract void SaveImpl(IDatabaseContextsPack contextsPack);

        public void Save(IDatabaseContextsPack contextsPack)
        {
            if(!IsLoaded) {
                throw new InvalidOperationException(nameof(IsLoaded));
            }

            SaveImpl(contextsPack);
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
