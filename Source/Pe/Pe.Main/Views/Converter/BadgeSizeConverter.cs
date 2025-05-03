using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(IconBox), typeof(double))]
    public class BadgeSizeConverter: IValueConverter
    {
        #region property

        public double Scale { get; set; } = 1;

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)(IconBox)value / 2) * Scale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
