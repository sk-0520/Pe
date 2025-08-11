using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public interface IPluginArguments
    {
        #region property

        /// <inheritdoc cref="IHttpUserAgentFactory"/>
        IHttpUserAgentFactory HttpUserAgentFactory { get; }
        /// <inheritdoc cref="IPlatformTheme"/>
        IPlatformTheme PlatformTheme { get; }
        /// <inheritdoc cref="IImageLoader"/>
        IImageLoader ImageLoader { get; }
        /// <inheritdoc cref="IMediaConverter"/>
        IMediaConverter MediaConverter { get; }
        /// <inheritdoc cref="IContextDispatcher"/>
        IContextDispatcher ContextDispatcher { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }

        #endregion
    }
    public class PluginArguments: IPluginArguments
    {
        public PluginArguments(IHttpUserAgentFactory httpUserAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
        {
            HttpUserAgentFactory = httpUserAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            ContextDispatcher = contextDispatcher;
            LoggerFactory = loggerFactory;
        }

        #region IPluginArguments

        public IHttpUserAgentFactory HttpUserAgentFactory { get; }
        public IPlatformTheme PlatformTheme { get; }
        public IImageLoader ImageLoader { get; }
        public IMediaConverter MediaConverter { get; }
        public IContextDispatcher ContextDispatcher { get; }
        public ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
