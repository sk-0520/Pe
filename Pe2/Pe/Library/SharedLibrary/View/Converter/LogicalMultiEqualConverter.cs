﻿namespace ContentTypeTextNet.Library.SharedLibrary.View.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;

	public class LogicalMultiEqualConverter: IMultiValueConverter
	{
		public virtual object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return values[0] == values[1];
		}

		public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
}
