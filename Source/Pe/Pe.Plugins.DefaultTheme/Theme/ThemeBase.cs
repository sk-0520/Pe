using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    public abstract class ThemeBase
    {
        public ThemeBase(IThemeParameter parameter)
        {
            PlatformTheme = parameter.PlatformTheme;
            DispatcherWrapper = parameter.DispatcherWrapper;
            Logger = parameter.LoggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }
        protected IPlatformTheme PlatformTheme { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region function

        protected double GetHorizontal(Thickness thickness) => thickness.Left + thickness.Right;

        protected double GetVertical(Thickness thickness) => thickness.Top + thickness.Bottom;

        protected Color GetMenuImageColor(bool getBoxColor, bool isActiveColor)
        {
            byte alpha = 80;

            if(getBoxColor) {
                if(isActiveColor) {
                    return SystemColors.ActiveCaptionColor;
                } else {
                    var color = SystemColors.InactiveCaptionColor;
                    return Color.FromArgb(alpha, color.R, color.G, color.B);
                }
            } else {
                if(isActiveColor) {
                    return SystemColors.ActiveCaptionTextColor;
                } else {
                    var color = SystemColors.InactiveCaptionTextColor;
                    return Color.FromArgb(alpha, color.R, color.G, color.B);
                }
            }
        }

        /// <summary>
        /// 箱形の要素を作成する。
        /// <para>同じようなものを共通の見栄えで作成するため細かい部分は内部実装に隠ぺいする。</para>
        /// </summary>
        /// <param name="borderColor">境界線の色。</param>
        /// <param name="fillColor">背景色</param>
        /// <param name="size">サイズ。</param>
        /// <returns>生成された要素。</returns>
        public static FrameworkElement CreateBox(Color borderColor, Color fillColor, Size size)
        {
            var box = new Rectangle();
            using(Initializer.Begin(box)) {
                box.Width = size.Width;
                box.Height = size.Height;
                box.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(borderColor));
                box.StrokeThickness = 1;
                box.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(fillColor));
            }
            return box;
        }

        protected Effect GetStrongEffect() => (Effect)Application.Current.Resources["Effect-Strong"];

        protected TValue GetResourceValue<TValue>(string interfaceName, string methodName)
        {
            var key = interfaceName + '.' + methodName;
            return (TValue)Application.Current.Resources[key];

        }

        #endregion
    }
}
