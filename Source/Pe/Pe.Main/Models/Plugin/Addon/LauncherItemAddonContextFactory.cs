using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonContextFactory: PluginContextFactoryBase
    {
        public LauncherItemAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        {
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
        IMediaConverter MediaConverter { get; }
        IPolicy Policy { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region function

        public LauncherItemAddonContextWorker CreateWorker(IPluginInformations pluginInformations, Guid launcherItemId)
        {
            return new LauncherItemAddonContextWorker(this, pluginInformations, launcherItemId, LoggerFactory);
        }


        LauncherItemAddonFiles CreateLauncherItemAddonFile(IPluginInformations pluginInformations)
        {
            var dirName = ConvertDirectoryName(pluginInformations.PluginIdentifiers);

            var pluginFile = new LauncherItemAddonFiles(
                new LauncherItemAddonFileStorage(GetUserDirectory(pluginInformations.PluginIdentifiers)),
                new LauncherItemAddonFileStorage(GetMachineDirectory(pluginInformations.PluginIdentifiers)),
                new LauncherItemAddonFileStorage(GetTemporaryDirectory(pluginInformations.PluginIdentifiers))
            );

            return pluginFile;
        }

        LauncherItemAddonPersistents CreateLauncherItemAddonPersistentContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginPersistent = new LauncherItemAddonPersistents(
                new LauncherItemAddonPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseContextsPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new LauncherItemAddonPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseContextsPack.Large, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new LauncherItemAddonPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseContextsPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistent;
        }

        LauncherItemAddonStorage CreateLauncherItemAddonStorage(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginStorage = new LauncherItemAddonStorage(
                CreateLauncherItemAddonFile(pluginInformations),
                CreateLauncherItemAddonPersistentContext(pluginInformations, databaseContextsPack, isReadOnly)
            );

            return pluginStorage;
        }

        public LauncherItemAddonContext CreateContext(IPluginInformations pluginInformations, Guid launcherItemId, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformations, databaseContextsPack, isReadOnly);
            return new LauncherItemAddonContext(pluginInformations.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesLoadContext CreatePreferencesLoadContext(IPluginInformations pluginInformations, Guid launcherItemId, IDatabaseContextsPack databaseContextsPack)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformations, databaseContextsPack, true);
            return new LauncherItemPreferencesLoadContext(pluginInformations.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesCheckContext CreatePreferencesCheckContext(IPluginInformations pluginInformations, Guid launcherItemId, IDatabaseContextsPack databaseContextsPack)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformations, databaseContextsPack, true);
            return new LauncherItemPreferencesCheckContext(pluginInformations.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesSaveContext CreatePreferencesSaveContext(IPluginInformations pluginInformations, Guid launcherItemId, IDatabaseContextsPack databaseContextsPack)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformations, databaseContextsPack, false);
            return new LauncherItemPreferencesSaveContext(pluginInformations.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesEndContext CreatePreferencesEndContext(IPluginInformations pluginInformations, Guid launcherItemId)
        {
            return new LauncherItemPreferencesEndContext(pluginInformations.PluginIdentifiers, launcherItemId);
        }

        /// <summary>
        /// ここにあるのは設計ミス！
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="launcherItemId"></param>
        /// <returns></returns>
        internal LauncherItemExtensionExecuteParameter CreateExtensionExecuteParameter(IPluginInformations pluginInformations, Guid launcherItemId, ILauncherItemAddonViewSupporter launcherItemAddonViewSupporter)
        {
            var launcherItemExtensionExecuteParameter = new LauncherItemExtensionExecuteParameter(
                launcherItemId,
                launcherItemAddonViewSupporter,
                new SkeletonImplements(),
                pluginInformations,
                UserAgentManager,
                PlatformTheme,
                ImageLoader,
                MediaConverter,
                Policy,
                DispatcherWrapper,
                LoggerFactory
            );
            return launcherItemExtensionExecuteParameter;
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "launcher-item-data";

        #endregion

    }
}
