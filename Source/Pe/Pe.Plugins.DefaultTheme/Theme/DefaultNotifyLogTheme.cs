using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    /// <inheritdoc cref="INotifyLogTheme"/>
    internal class DefaultNotifyLogTheme: DefaultThemeBase, INotifyLogTheme
    {
        public DefaultNotifyLogTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region property

        private byte BaseAlpha { get; } = 180;

        #endregion

        #region function

        private Color ToBaseColor(Color color)
        {
            color.A = BaseAlpha;
            return color;
        }

        #endregion

        #region INotifyTheme

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderThickness"/>
        public Thickness GetViewBorderThickness()
        {
            return new Thickness(2);
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderBrush"/>
        public Brush GetViewBorderBrush()
        {
            var color = PlatformTheme.GetTaskbarColor();
            color.A = 255;
            return new SolidColorBrush(color).GetFreezed();
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderCornerRadius"/>
        public CornerRadius GetViewBorderCornerRadius()
        {
            return new CornerRadius(2);
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBackgroundBrush"/>
        public Brush GetViewBackgroundBrush()
        {
            var colors = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(colors).GetFreezed();
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewPaddingThickness"/>
        public Thickness GetViewPaddingThickness()
        {
            return new Thickness(2);
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHeaderForegroundBrush(bool)"/>
        public Brush GetHeaderForegroundBrush(bool isTopmost)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(ToBaseColor(MediaUtility.GetAutoColor(color)));
        }
        /// <inheritdoc cref="INotifyLogTheme.GetContentForegroundBrush(bool)"/>
        public Brush GetContentForegroundBrush(bool isTopmost)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(ToBaseColor(MediaUtility.GetAutoColor(color)));
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHyperlinkForegroundBrush(HyperlinkState)"/>
        public Brush GetHyperlinkForegroundBrush(bool isMouseOver)
        {
            var color = MediaUtility.GetAutoColor(PlatformTheme.GetTaskbarColor());
            color.A = isMouseOver
                ? (byte)255
                : BaseAlpha
            ;
            return new SolidColorBrush(color);
        }

        #endregion
    }
}
