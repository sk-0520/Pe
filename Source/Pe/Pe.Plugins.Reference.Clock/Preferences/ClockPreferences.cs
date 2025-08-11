using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Views;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Preferences
{
    public class ClockPreferences: PreferencesBase
    {
        public ClockPreferences(IPlugin plugin)
            : base(plugin)
        { }

        #region proeprty

        private ClockSettingViewModel? SettingViewModel { get; set; }

        #endregion

        #region function
        #endregion

        #region PreferencesBase

        public override UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext, IPreferencesParameter preferencesParameter)
        {
            ClockWidgetSetting? clockWidgetSetting;
            if(!preferencesLoadContext.Storage.Persistence.Normal.TryGet<ClockWidgetSetting>(ClockConstants.WidgetSettingKey, out clockWidgetSetting)) {
                clockWidgetSetting = new ClockWidgetSetting();
            }

            SettingViewModel = new ClockSettingViewModel(clockWidgetSetting, preferencesParameter.SkeletonImplements, preferencesParameter.ContextDispatcher, preferencesParameter.LoggerFactory);

            var control = new ClockSettingControl() {
                DataContext = SettingViewModel,
            };

            return control;
        }

        public override void CheckPreferences(IPreferencesCheckContext preferencesCheckContext)
        {
        }

        public override void SavePreferences(IPreferencesSaveContext preferencesSaveContext)
        {
            Debug.Assert(SettingViewModel != null);

            preferencesSaveContext.Storage.Persistence.Normal.Set(ClockConstants.WidgetSettingKey, SettingViewModel.WidgetSetting);
        }

        public override void EndPreferences(IPreferencesEndContext preferencesEndContext)
        {
            SettingViewModel = null;
        }

        #endregion
    }
}
