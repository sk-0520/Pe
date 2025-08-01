using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    /// <summary>
    /// タブ幅からタブストップのコレクションを生成するコンバーター。
    /// </summary>
    [ValueConversion(typeof(double), typeof(IEnumerable<double>))]
    public class TabWidthToTabStopsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double tabWidth && tabWidth > 0)
            {
                // タブストップのコレクションを生成（最大20個まで）
                var tabStops = new List<double>();
                for (int i = 1; i <= 20; i++)
                {
                    tabStops.Add(i * tabWidth);
                }
                return tabStops;
            }

            // デフォルトのタブストップ（32ピクセル間隔）
            return new List<double> { 32, 64, 96, 128, 160, 192, 224, 256, 288, 320 };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}