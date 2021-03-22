using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    public class IsNotEmptyCollectionConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var collection = value as IEnumerable<object>;
            if(collection != null) {
                return collection.Any();
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
