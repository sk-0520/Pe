﻿/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.Define;

    public class HotTrackConverter: IMultiValueConverter
    {

        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //	var color = (Color)value;
        //	var ui = (UIElement)parameter;

        //	var brush = new LinearGradientBrush();
        //	brush.EndPoint = new System.Windows.Point(0.5, 1.0);
        //	brush.StartPoint = new System.Windows.Point(0.5, 0.0);
        //	brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(0xFF, color.R, color.G, color.B), 0.00));
        //	brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(0x00, color.R, color.G, color.B), 1.00));
        //	return brush;
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //	throw new NotImplementedException();
        //}
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color;
            try {
                color = (Color)values[0];
            } catch(InvalidCastException ex) {
                Debug.WriteLine(ex);
                color = SystemColors.DesktopColor;
            }
            var dockType = (DockType)values[1];
            var length = (double)values[2];
            var isEnabledCorrection = values[3] as bool?;

            if(isEnabledCorrection.GetValueOrDefault() && dockType == DockType.Right) {
                dockType = DockType.Left;
            }

            var brush = new LinearGradientBrush();
            switch(dockType) {
                case DockType.None:
                case DockType.Bottom:
                    brush.StartPoint = new Point(0.5, 0.0);
                    brush.EndPoint = new Point(0.5, 1.0);
                    break;
                case DockType.Left:
                    brush.StartPoint = new Point(1.0, 0.5);
                    brush.EndPoint = new Point(0.0, 0.5);
                    break;
                case DockType.Top:
                    brush.StartPoint = new Point(0.5, 1.0);
                    brush.EndPoint = new Point(0.5, 0.0);
                    break;
                case DockType.Right:
                    brush.StartPoint = new Point(0.0, 0.5);
                    brush.EndPoint = new Point(1.0, 0.5);
                    break;
                default:
                    throw new NotImplementedException();
            }
            //brush.EndPoint = new Point(0.5, 1.0);
            //brush.StartPoint = new Point(0.5, 0.0);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, color.R, color.G, color.B), 1.00));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0xef, color.R, color.G, color.B), 1 - length));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, color.R, color.G, color.B), 0.00));
            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
