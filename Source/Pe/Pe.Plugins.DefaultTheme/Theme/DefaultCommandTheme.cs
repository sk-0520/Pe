using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultCommandTheme: DefaultThemeBase, ICommandTheme
    {
        public DefaultCommandTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region ICommandTheme

        public Brush GetGripBrush(bool isActive)
        {
            //var color = PlatformTheme.GetTaskbarColor();
            //var fore = MediaUtility.GetAutoColor(color);
            //return new SolidColorBrush(fore);
            return GetResourceValue<Brush>(nameof(DefaultCommandTheme), nameof(GetGripBrush));
        }

        [return: PixelKind(Px.Logical)]
        public double GetGripWidth()
        {
            return 8;
        }

        public Thickness GetSelectedIconMargin(in IconScale iconScale) => new Thickness(1);

        public Thickness GetInputBorderThickness()
        {
            return new Thickness(2);
        }

        public Brush GetInputBorderBrush(InputState inputState)
        {
            var colors = PlatformTheme.GetApplicationThemeColors(PlatformTheme.ApplicationThemeKind);

            return inputState switch {
                InputState.Empty => new SolidColorBrush(colors.Border).GetFreezed(),
                InputState.Finding => new SolidColorBrush(colors.Control).GetFreezed(),
                InputState.Complete => new SolidColorBrush(PlatformTheme.GetAccentColors(PlatformTheme.AccentColor).Base).GetFreezed(),
                InputState.NotFound => new SolidColorBrush(colors.Foreground).GetFreezed(),
                _ => throw new NotImplementedException(),
            };
        }

        public Brush GetInputBackground(InputState inputState)
        {
            var colors = PlatformTheme.GetApplicationThemeColors(PlatformTheme.ApplicationThemeKind);
            return new SolidColorBrush(colors.Background).GetFreezed();
        }

        public Brush GetInputForeground(InputState inputState)
        {
            var colors = PlatformTheme.GetApplicationThemeColors(PlatformTheme.ApplicationThemeKind);
            return new SolidColorBrush(colors.Foreground).GetFreezed();
        }

        public Brush GetViewBackgroundBrush(bool isActive)
        {
            var colors = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(colors).GetFreezed();
        }

        public Thickness GetViewBorderThickness()
        {
            return new Thickness(2);
        }

        public Brush GetViewBorderBrush(bool isActive)
        {
            var color = PlatformTheme.GetTaskbarColor();
            color.A = (byte)(isActive ? 0xff : 0x80);
            return new SolidColorBrush(color).GetFreezed();
        }

        public ControlTemplate GetExecuteButtonControlTemplate(in IconScale iconScale)
        {
            return GetResourceValue<ControlTemplate>(nameof(DefaultCommandTheme), nameof(GetExecuteButtonControlTemplate));
        }


        #endregion
    }
}
