using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class GeneralsSettingEditorElement: SettingEditorElementBase
    {
        public GeneralsSettingEditorElement(EnvironmentParameters environmentParameters, IReadOnlyList<IPlugin> themePlugins, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            AppExecuteSettingEditor = new AppExecuteSettingEditorElement(environmentParameters, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppGeneralSettingEditor = new AppGeneralSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, themePlugins, DatabaseStatementLoader, LoggerFactory);
            AppUpdateSettingEditor = new AppUpdateSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppNotifyLogSettingEditor = new AppNotifyLogSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppLauncherToolbarSettingEditor = new AppLauncherToolbarSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppCommandSettingEditor = new AppCommandSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppNoteSettingEditor = new AppNoteSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppStandardInputOutputSettingEditor = new AppStandardInputOutputSettingEditorElement(MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);

            Editors = new List<GeneralSettingEditorElementBase>() {
                AppExecuteSettingEditor,
                AppGeneralSettingEditor,
                AppUpdateSettingEditor,
                AppNotifyLogSettingEditor,
                AppLauncherToolbarSettingEditor,
                AppCommandSettingEditor,
                AppNoteSettingEditor,
                AppStandardInputOutputSettingEditor,
            };
        }

        #region property

        public AppExecuteSettingEditorElement AppExecuteSettingEditor { get; }
        public AppGeneralSettingEditorElement AppGeneralSettingEditor { get; }
        public AppUpdateSettingEditorElement AppUpdateSettingEditor { get; }
        public AppNotifyLogSettingEditorElement AppNotifyLogSettingEditor { get; }
        public AppLauncherToolbarSettingEditorElement AppLauncherToolbarSettingEditor { get; }
        public AppCommandSettingEditorElement AppCommandSettingEditor { get; }
        public AppNoteSettingEditorElement AppNoteSettingEditor { get; }
        public AppStandardInputOutputSettingEditorElement AppStandardInputOutputSettingEditor { get; }

        IList<GeneralSettingEditorElementBase> Editors { get; }

        #endregion

        #region function

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            foreach(var editor in Editors) {
                editor.Initialize();
            }
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            foreach(var editor in Editors) {
                editor.Save(contextsPack);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var editor in Editors) {
                        editor.Dispose();
                    }
                    Editors.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
