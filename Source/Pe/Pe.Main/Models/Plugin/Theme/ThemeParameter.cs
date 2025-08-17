using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <inheritdoc cref="IThemeParameter"/>
    internal class ThemeParameter: PluginParameterBase, IThemeParameter
    {
        public ThemeParameter(IPluginInformation pluginInformation, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(pluginInformation, viewManager, platformTheme, imageLoader, mediaConverter, policy, contextDispatcher, loggerFactory)
        { }

        #region IThemeParameter

        #endregion
    }
}
