using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginSettingEditorElement: ElementBase, IPluginId
    {
        internal PluginSettingEditorElement(PluginStateData pluginState, IPlugin? plugin, PreferencesContextFactory preferencesContextFactory, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginState = pluginState;
            Plugin = plugin;
            PreferencesContextFactory = preferencesContextFactory;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;

            if(Plugin is IPreferences preferences) {
                SupportedPreferences = true;
                Preferences = preferences;
            } else {
                SupportedPreferences = false;
            }

            var appPluginIds = new[] {
                DefaultTheme.Informations.PluginIdentifiers.PluginId
            };
            CanUninstall = !appPluginIds.Any(i => i == Plugin?.PluginInformations.PluginIdentifiers.PluginId);

            if(CanUninstall) {
                using(var context = MainDatabaseBarrier.WaitRead()) {
                    var pluginsEntityDao = new PluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    var data = pluginsEntityDao.SelectePlguinStateDataByPLuginId(PluginId);
                    if(data != null) {
                        MarkedUninstall = data.State == Data.PluginState.Uninstall;
                    }
                }
            }
        }

        #region property

        public PluginStateData PluginState { get; }
        public IPlugin? Plugin { get; }
        PreferencesContextFactory PreferencesContextFactory { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IHttpUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
        IMediaConverter MediaConverter { get; }
        IPolicy Policy { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        public bool SupportedPreferences { get; }
        IPreferences? Preferences { get; }

        public bool StartedPreferences { get; private set; }

        public Version PluginVersion { get; private set; } = new Version();

        /// <summary>
        /// ??????????????????????????????????????????
        /// </summary>
        public bool MarkedUninstall { get; private set; }

        /// <summary>
        /// ????????????????????????????????????????????????
        /// <para>Pe ???????????????????????????????????????</para>
        /// </summary>
        public bool CanUninstall { get; private set; }

        #endregion

        #region function

        public UserControl BeginPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(!StartedPreferences);
            Debug.Assert(Plugin != null);

            UserControl result;
            using(var reader = PreferencesContextFactory.BarrierRead()) {
                using var context = PreferencesContextFactory.CreateLoadContext(Plugin.PluginInformations, reader);
                var skeleton = new SkeletonImplements();
                var parameter = new PreferencesParameter(skeleton, Plugin.PluginInformations, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
                result = Preferences.BeginPreferences(context, parameter);
            }
            StartedPreferences = true;
            return result;
        }

        public bool CheckPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            bool hasError;
            using(var reader = PreferencesContextFactory.BarrierRead()) {
                using var context = PreferencesContextFactory.CreateCheckContext(Plugin.PluginInformations, reader);
                Preferences.CheckPreferences(context);
                hasError = context.HasError;
            }
            return hasError;
        }

        /// <summary>
        /// ????????????????????????????????????
        /// </summary>
        /// <param name="databaseContextsPack"></param>
        public void SavePreferences(IDatabaseContextsPack databaseContextsPack)
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            using var context = PreferencesContextFactory.CreateSaveContext(Plugin.PluginInformations, databaseContextsPack);
            Preferences.SavePreferences(context);
        }

        public void EndPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            // NOTE: ???????????????????????????????????????????????????????????????

            using(var reader = PreferencesContextFactory.BarrierRead()) {
                using var context = PreferencesContextFactory.CreateEndContext(Plugin.PluginInformations, reader);
                Preferences.EndPreferences(context);
            }
        }

        public void ToggleUninstallMark()
        {
            MarkedUninstall = !MarkedUninstall;
        }

        /// <summary>
        /// ?????????????????????????????????????????????????????????????????????????????????
        /// <para>?????????????????????????????????????????????????????????????????????????????????</para>
        /// </summary>
        /// <param name="contextsPack"></param>
        public void Save(IDatabaseContextsPack contextsPack)
        {
            var pluginsEntityDao = new PluginsEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);

            if(CanUninstall && MarkedUninstall) {
                var pluginState = new PluginStateData() {
                    PluginId = PluginState.PluginId,
                    PluginName = PluginState.PluginName,
                    State = ContentTypeTextNet.Pe.Main.Models.Data.PluginState.Uninstall,
                };
                pluginsEntityDao.UpdatePluginStateData(pluginState, contextsPack.CommonStatus);
            } else if(!MarkedUninstall) {
                var pluginState = new PluginStateData() {
                    PluginId = PluginState.PluginId,
                    PluginName = PluginState.PluginName,
                    State = ContentTypeTextNet.Pe.Main.Models.Data.PluginState.Enable, // TODO: ???????????????????????????????????????????????????
                };
                pluginsEntityDao.UpdatePluginStateData(pluginState, contextsPack.CommonStatus);
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            if(Plugin != null) {
                PluginVersion = Plugin.PluginInformations.PluginVersions.PluginVersion;
            } else {
                var pluginVersion = MainDatabaseBarrier.ReadData(c => {
                    var pluginsEntityDao = new PluginsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                    return pluginsEntityDao.SelectLastUsePluginVersion(PluginId);
                });
                if(pluginVersion != null) {
                    PluginVersion = pluginVersion;
                }
            }
        }

        #endregion

        #region IPLuginId

        public Guid PluginId => PluginState.PluginId;

        #endregion
    }
}
