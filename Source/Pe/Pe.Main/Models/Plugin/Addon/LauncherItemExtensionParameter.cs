using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <summary>
    /// <see cref="IAddon.CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter)"/> で渡されるデータ。
    /// </summary>
    public class LauncherItemExtensionCreateParameter: AddonParameter, ILauncherItemExtensionCreateParameter
    {
        public LauncherItemExtensionCreateParameter(LauncherItemId launcherItemId, ILauncherItemAddonContextWorker contextWorker, ISkeletonImplements skeletonImplements, IPluginInformation pluginInformation, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IHashAlgorithmFactory hashAlgorithmGenerator, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(skeletonImplements, pluginInformation, userAgentFactory, viewManager, hashAlgorithmGenerator, platformTheme, imageLoader, mediaConverter, policy, contextDispatcher, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ContextWorker = contextWorker;
        }

        #region ILauncherItemExtensionBuildParameter

        public LauncherItemId LauncherItemId { get; }

        public ILauncherItemAddonContextWorker ContextWorker { get; }

        #endregion
    }
}
