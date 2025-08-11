using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class PreferencesParameter: PluginParameterBase, IPreferencesParameter
    {
        public PreferencesParameter(ISkeletonImplements skeletonImplements, IPluginInformation pluginInformation, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(pluginInformation, viewManager, platformTheme, imageLoader, mediaConverter, policy, contextDispatcher, loggerFactory)
        {
            SkeletonImplements = skeletonImplements;
            HttpUserAgentFactory = userAgentFactory;
        }

        #region IPreferencesParameter

        public IHttpUserAgentFactory HttpUserAgentFactory { get; }
        public ISkeletonImplements SkeletonImplements { get; }
        #endregion
    }
}
