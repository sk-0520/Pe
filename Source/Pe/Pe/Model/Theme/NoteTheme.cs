using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    public enum NoteCaption
    {
        Compact,
        Topmost,
        Close,
    }

    public interface INoteTheme
    {
        #region function

        [return: PixelKind(Px.Logical)]
        double GetCaptionHeight();
        [return: PixelKind(Px.Logical)]
        double GetCaptionFontSize(double baseFontSize);
        FontFamily GetCaptionFontFamily(FontFamily baseFontFamily);

        [return: PixelKind(Px.Logical)]
        Thickness GetBorderThickness();
        [return: PixelKind(Px.Logical)]
        Size GetResizeGripSize();

        IReadOnlyColorPair<Brush> GetCaptionBrush(IReadOnlyColorPair<Color> baseColor);
        Brush GetBorderBrush(IReadOnlyColorPair<Color> baseColor);
        Brush GetContentBrush(IReadOnlyColorPair<Color> baseColor);

        DependencyObject GetCaptionImage(NoteCaption noteCaption, bool isEnabled, IReadOnlyColorPair<Color> baseColor);
        DependencyObject GetResizeGripImage(IReadOnlyColorPair<Color> baseColor);

        #endregion
    }

    public class NoteTheme : ThemeBase, INoteTheme
    {
        public NoteTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region property
        #endregion

        #region function


        string GetResourceKey(NoteCaption noteCaption, bool isEnabled)
        {
            switch(noteCaption) {
                case NoteCaption.Compact:
                    if(isEnabled) {
                        return "Image-NoteCaption-Compact-Enabled";
                    } else {
                        return "Image-NoteCaption-Compact-Disabled";
                    }

                case NoteCaption.Topmost:
                    if(isEnabled) {
                        return "Image-NoteCaption-Topmost-Enabled";
                    } else {
                        return "Image-NoteCaption-Topmost-Disabled";
                    }

                case NoteCaption.Close:
                    return "Image-NoteCaption-Close";

                default:
                    throw new NotSupportedException();
            }
        }

        DependencyObject GetCaptionImageCore(NoteCaption noteCaption, bool isEnabled, IReadOnlyColorPair<Color> baseColor)
        {
            var viewBox = new Viewbox();
            using(Initializer.BeginInitialize(viewBox)) {
                viewBox.Width = GetCaptionHeight();
                viewBox.Height = viewBox.Width;

                var canvas = new Canvas();
                using(Initializer.BeginInitialize(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.BeginInitialize(path)) {
                        var resourceKey = GetResourceKey(noteCaption, isEnabled);
                        var geometry = (Geometry)Application.Current.Resources[resourceKey];
                        FreezableUtility.SafeFreeze(geometry);
                        path.Data = geometry;
                        path.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Foreground));
                        //path.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(MediaUtility.GetAutoColor(baseColor.Foreground)));
                        //path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                }
                viewBox.Child = canvas;
            }

            return viewBox;
        }


        #endregion

        #region INoteTheme

        [return: PixelKind(Px.Logical)]
        public double GetCaptionHeight()
        {
            return SystemParameters.CaptionHeight;
        }

        [return: PixelKind(Px.Logical)]
        public double GetCaptionFontSize(double baseFontSize)
        {
            return SystemFonts.CaptionFontSize;
        }

        public FontFamily GetCaptionFontFamily(FontFamily baseFontFamily)
        {
            return SystemFonts.CaptionFontFamily;
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetBorderThickness()
        {
            return SystemParameters.WindowResizeBorderThickness;
        }
        [return: PixelKind(Px.Logical)]
        public Size GetResizeGripSize()
        {
            var thickness = GetBorderThickness();

            return new Size(
                thickness.Right * 6,
                thickness.Bottom * 6
            );
        }

        public IReadOnlyColorPair<Brush> GetCaptionBrush(IReadOnlyColorPair<Color> baseColor)
        {
            var pair = ColorPair.Create(new SolidColorBrush(baseColor.Foreground), new SolidColorBrush(baseColor.Background));

            FreezableUtility.SafeFreeze(pair.Foreground);
            FreezableUtility.SafeFreeze(pair.Background);

            return pair;
        }

        public Brush GetBorderBrush(IReadOnlyColorPair<Color> baseColor)
        {
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Background));
        }

        public Brush GetContentBrush(IReadOnlyColorPair<Color> baseColor)
        {
            /*
            旧PeではXAML上でこれをかけ合わせてた
            <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
                <GradientStop Color="#A0ffffff" Offset="0" />
                <GradientStop Color="Transparent" Offset="0.4"/>
                <GradientStop Color="Transparent" Offset="0.8"/>
                <GradientStop Color="#20101010" Offset="1"/>
            </LinearGradientBrush>
            */
            var headColor = Color.FromArgb(0xa0, 0xff, 0xff, 0xff);
            var tailColor = Color.FromArgb(0x20, 0x10, 0x10, 0x10);
            var collection = new GradientStopCollection(new[] {
                new GradientStop(Color.FromArgb(
                    baseColor.Background.A,
                    (byte)(baseColor.Background.R + (headColor.R - baseColor.Background.R) * (headColor.A / 255.0)),
                    (byte)(baseColor.Background.G + (headColor.G - baseColor.Background.G) * (headColor.A / 255.0)),
                    (byte)(baseColor.Background.B + (headColor.B - baseColor.Background.B) * (headColor.A / 255.0))
                ), 0),
                new GradientStop(baseColor.Background, 0.4),
                new GradientStop(baseColor.Background, 0.8),
                new GradientStop(Color.FromArgb(
                    baseColor.Background.A,
                    (byte)(baseColor.Background.R + (tailColor.R - baseColor.Background.R) * (tailColor.A / 255.0)),
                    (byte)(baseColor.Background.G + (tailColor.G - baseColor.Background.G) * (tailColor.A / 255.0)),
                    (byte)(baseColor.Background.B + (tailColor.B - baseColor.Background.B) * (tailColor.A / 255.0))
                ), 1),
            });
            var gradation = new LinearGradientBrush(collection, new Point(0, 0), new Point(0, 1));
            return FreezableUtility.GetSafeFreeze(gradation);
        }

        public DependencyObject GetCaptionImage(NoteCaption noteCaption, bool isEnabled, IReadOnlyColorPair<Color> baseColor)
        {
            return DispatcherWapper.Get(() => GetCaptionImageCore(noteCaption, isEnabled, baseColor));
        }

        public DependencyObject GetResizeGripImage(IReadOnlyColorPair<Color> baseColor)
        {
            var viewBox = new Viewbox();
            using(Initializer.BeginInitialize(viewBox)) {
                viewBox.Width = GetResizeGripSize().Width;
                viewBox.Height = GetResizeGripSize().Height;

                var canvas = new Canvas();
                using(Initializer.BeginInitialize(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.BeginInitialize(path)) {
                        var resourceKey = "Image-Note-ResizeGrip";
                        var geometry = (Geometry)Application.Current.Resources[resourceKey];
                        FreezableUtility.SafeFreeze(geometry);
                        path.Data = geometry;
                        path.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(MediaUtility.GetAutoColor(baseColor.Foreground)));
                        path.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Foreground));
                        path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                }
                viewBox.Child = canvas;
            }

            return viewBox;
        }


        #endregion
    }
}
