using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IAddonParameter"/>
    public class AddonParameter: PluginParameterBase, IAddonParameter
    {
        public AddonParameter(ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, platformTheme, dispatcherWrapper, loggerFactory)
        {
            UserAgentFactory = userAgentFactory;
            AddonExecutor = new AddonExecutor(PluginInformations, LoggerFactory);
            SkeletonImplements = skeletonImplements;
        }

        #region IAddonParameter

        /// <inheritdoc cref="IAddonParameter.UserAgentFactory"/>
        public IUserAgentFactory UserAgentFactory { get; }
        /// <inheritdoc cref="IAddonParameter.AddonExecutor"/>
        public IAddonExecutor AddonExecutor { get; }
        /// <inheritdoc cref="IAddonParameter.SkeletonImplements"/>
        public ISkeletonImplements SkeletonImplements { get; }
        #endregion
    }
}
