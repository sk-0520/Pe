using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    [ValueConversion(typeof(Color), typeof(Color))]
    public class AutoColorConverter: IValueConverter
    {
        #region property

        /// <summary>
        /// 透明度を保持しない。
        /// </summary>
        public bool Opaque { get; set; } = false;

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color color) {
                if(Opaque) {
                    return MediaUtility.GetAutoColor(MediaUtility.GetOpaqueColor(color));
                }
                return MediaUtility.GetAutoColor(color);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
