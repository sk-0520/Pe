using System.Collections.ObjectModel;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class GeneralsSettingEditorViewModel: SettingEditorViewModelBase<GeneralsSettingEditorElement>
    {
        public GeneralsSettingEditorViewModel(GeneralsSettingEditorElement model, ApplicationConfiguration applicationConfiguration, IGeneralTheme generalTheme, IImageLoader imageLoader, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, contextDispatcher, loggerFactory)
        {
            AppExecuteSettingEditor = new AppExecuteSettingEditorViewModel(Model.AppExecuteSettingEditor, ContextDispatcher, LoggerFactory);
            AppGeneralSettingEditor = new AppGeneralSettingEditorViewModel(Model.AppGeneralSettingEditor, applicationConfiguration.General.SupportCultures, imageLoader, ContextDispatcher, LoggerFactory);
            AppUpdateSettingEditor = new AppUpdateSettingEditorViewModel(Model.AppUpdateSettingEditor, ContextDispatcher, LoggerFactory);
            AppNotifyLogSettingEditor = new AppNotifyLogSettingEditorViewModel(Model.AppNotifyLogSettingEditor, ContextDispatcher, LoggerFactory);
            AppLauncherToolbarSettingEditor = new AppLauncherToolbarSettingEditorViewModel(Model.AppLauncherToolbarSettingEditor, ContextDispatcher, LoggerFactory);
            AppCommandSettingEditor = new AppCommandSettingEditorViewModel(Model.AppCommandSettingEditor, generalTheme, ContextDispatcher, LoggerFactory);
            AppNoteSettingEditor = new AppNoteSettingEditorViewModel(Model.AppNoteSettingEditor, applicationConfiguration.Note, generalTheme, ContextDispatcher, LoggerFactory);
            AppStandardInputOutputSettingEditor = new AppStandardInputOutputSettingEditorViewModel(Model.AppStandardInputOutputSettingEditor, generalTheme, ContextDispatcher, LoggerFactory);
            AppProxySettingEditor = new AppProxySettingEditorViewModel(Model.AppProxySettingEditor, ContextDispatcher, LoggerFactory);

            EditorItems = new ObservableCollection<IGeneralSettingEditor>() {
                AppExecuteSettingEditor,
                AppGeneralSettingEditor,
                AppUpdateSettingEditor,
                AppNotifyLogSettingEditor,
                AppLauncherToolbarSettingEditor,
                AppCommandSettingEditor,
                AppNoteSettingEditor,
                AppStandardInputOutputSettingEditor,
                AppProxySettingEditor,
            };
        }

        #region property

        private AppExecuteSettingEditorViewModel AppExecuteSettingEditor { get; }
        private AppGeneralSettingEditorViewModel AppGeneralSettingEditor { get; }
        private AppUpdateSettingEditorViewModel AppUpdateSettingEditor { get; }
        private AppNotifyLogSettingEditorViewModel AppNotifyLogSettingEditor { get; }
        private AppLauncherToolbarSettingEditorViewModel AppLauncherToolbarSettingEditor { get; }
        private AppCommandSettingEditorViewModel AppCommandSettingEditor { get; }
        private AppNoteSettingEditorViewModel AppNoteSettingEditor { get; }
        private AppStandardInputOutputSettingEditorViewModel AppStandardInputOutputSettingEditor { get; }
        private AppProxySettingEditorViewModel AppProxySettingEditor { get; }

        [IgnoreValidation]
        public ObservableCollection<IGeneralSettingEditor> EditorItems { get; }

        #endregion

        #region function
        #endregion

        #region command
        #endregion

        #region SettingEditorViewModelBase
        public override string Header => Properties.Resources.String_Setting_Generals_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var editor in EditorItems) {
                        editor.Dispose();
                    }
                    EditorItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
