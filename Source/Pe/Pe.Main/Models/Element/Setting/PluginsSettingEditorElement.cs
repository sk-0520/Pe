using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginsSettingEditorElement: SettingEditorElementBase
    {
        #region define

        private class StringProgress: IProgress<string>
        {
            public StringProgress(ILoggerFactory loggerFactory)
            {
                Logger = loggerFactory.CreateLogger(GetType());
            }

            #region property

            private ILogger Logger { get; }

            #endregion

            #region IProgress

            public void Report(string value)
            {
                Logger.LogInformation("{0}", value);
            }

            #endregion
        }

        class DoubleProgress: IProgress<double>
        {
            public DoubleProgress(ILoggerFactory loggerFactory)
            {
                Logger = loggerFactory.CreateLogger(GetType());
            }

            #region property

            private ILogger Logger { get; }

            #endregion

            #region IProgress

            public void Report(double value)
            {
                Logger.LogInformation("{0}", value);
            }

            #endregion
        }

        #endregion

        internal PluginsSettingEditorElement(PluginContainer pluginContainer, IPluginConstructorContext pluginConstructorContext, PauseReceiveLogDelegate pauseReceiveLog, PreferencesContextFactory preferencesContextFactory, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, EnvironmentParameters environmentParameters, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, statementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            PluginContainer = pluginContainer;
            PluginConstructorContext = pluginConstructorContext;
            PreferencesContextFactory = preferencesContextFactory;
            PauseReceiveLog = pauseReceiveLog;

            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            EnvironmentParameters = environmentParameters;

            PluginItems = new ReadOnlyObservableCollection<PluginSettingEditorElement>(PluginItemsImpl);
            InstallPluginItems = new ReadOnlyObservableCollection<PluginInstallItemElement>(InstallPluginItemsImpl);

            PluginInstaller = new PluginInstaller(PluginContainer, PluginConstructorContext, PauseReceiveLog, EnvironmentParameters, DatabaseStatementLoader, LoggerFactory);
        }

        #region property

        private PreferencesContextFactory PreferencesContextFactory { get; }
        private IHttpUserAgentFactory UserAgentFactory { get; }
        private IPlatformTheme PlatformTheme { get; }
        private EnvironmentParameters EnvironmentParameters { get; }

        private PluginContainer PluginContainer { get; }
        private IPluginConstructorContext PluginConstructorContext { get; }
        private PauseReceiveLogDelegate PauseReceiveLog { get; }

        private ObservableCollection<PluginSettingEditorElement> PluginItemsImpl { get; } = new ObservableCollection<PluginSettingEditorElement>();
        public ReadOnlyObservableCollection<PluginSettingEditorElement> PluginItems { get; }

        private ObservableCollection<PluginInstallItemElement> InstallPluginItemsImpl { get; } = new ObservableCollection<PluginInstallItemElement>();
        public ReadOnlyObservableCollection<PluginInstallItemElement> InstallPluginItems { get; }

        private PluginInstaller PluginInstaller { get; }

        #endregion

        #region function

        public void CancelInstall(PluginId pluginId)
        {
            var cancelled = PluginInstaller.CancelInstall(pluginId, InstallPluginItemsImpl.Select(i => i.Data), TemporaryDatabaseBarrier);
            if(cancelled) {
                var removeTarget = InstallPluginItemsImpl.First(i => i.Data.PluginId == pluginId);
                InstallPluginItemsImpl.Remove(removeTarget);
            }
        }

        private void MergeInstallPlugin(PluginInstallItemElement element)
        {
            var oldElement = InstallPluginItemsImpl.FirstOrDefault(i => i.Data.PluginId == element.Data.PluginId);
            if(oldElement != null) {
                InstallPluginItemsImpl.Remove(oldElement);
            }
            InstallPluginItemsImpl.Add(element);

        }

        internal async Task InstallLocalPluginAsync(FileInfo archiveFile)
        {
            var ext = PluginInstaller.GetArchiveExtension(archiveFile);
            var pluginFileName = PluginInstaller.GetPluginName(archiveFile);

            var pluginInstallData = await PluginInstaller.InstallPluginArchiveAsync(pluginFileName, archiveFile, ext, true, InstallPluginItemsImpl.Select(i => i.Data), PluginInstallAssemblyMode.Process, TemporaryDatabaseBarrier);
            var element = new PluginInstallItemElement(pluginInstallData, LoggerFactory);
            element.Initialize();
            MergeInstallPlugin(element);
        }

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            IList<PluginStateData> pluginStates;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var pluginsEntityDao = new PluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                pluginStates = pluginsEntityDao.SelectePlguinStateData().ToList();
            }

            IList<PluginInstallData> installDataItems;
            using(var context = TemporaryDatabaseBarrier.WaitRead()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                installDataItems = installPluginsEntityDao.SelectInstallPlugins().ToList();
            }

            foreach(var installDataItem in installDataItems) {
                var element = new PluginInstallItemElement(installDataItem, LoggerFactory);
                element.Initialize();
                InstallPluginItemsImpl.Add(element);
            }

            // 標準テーマがなければ追加
            if(!pluginStates.Any(i => i.PluginId == DefaultTheme.Informations.PluginIdentifiers.PluginId)) {
                pluginStates.Insert(0, new Data.PluginStateData() {
                    PluginName = DefaultTheme.Informations.PluginIdentifiers.PluginName,
                    PluginId = DefaultTheme.Informations.PluginIdentifiers.PluginId,
                    State = Data.PluginState.Enable,
                });
            }

            foreach(var pluginState in pluginStates) {
                var plugin = PluginContainer.Plugins.FirstOrDefault(i => pluginState.PluginId == i.PluginInformations.PluginIdentifiers.PluginId);
                var element = new PluginSettingEditorElement(pluginState, plugin, PreferencesContextFactory, MainDatabaseBarrier, DatabaseStatementLoader, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
                element.Initialize();
                PluginItemsImpl.Add(element);
            }
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            foreach(var element in PluginItems) {
                if(element.SupportedPreferences && element.StartedPreferences) {
                    element.SavePreferences(contextsPack);
                }

                element.Save(contextsPack);
            }
        }

        #endregion
    }
}
