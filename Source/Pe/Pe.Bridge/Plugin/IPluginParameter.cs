using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// プラグイン機能構築時に Pe から渡されるデータ。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPluginParameter
    {
        #region property

        /// <inheritdoc cref="IViewManager"/>
        IViewManager ViewManager { get; }
        /// <inheritdoc cref="IPlatformTheme"/>
        IPlatformTheme PlatformTheme { get; }
        /// <inheritdoc cref="IImageLoader"/>
        IImageLoader ImageLoader { get; }
        /// <inheritdoc cref="IMediaConverter"/>
        IMediaConverter MediaConverter { get; }
        /// <inheritdoc cref="IPolicy"/>
        IPolicy Policy { get; }
        /// <inheritdoc cref="IContextDispatcher"/>
        IContextDispatcher ContextDispatcher { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
