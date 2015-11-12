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
namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// IntegerSliderControl.xaml の相互作用ロジック
    /// </summary>
    public partial class IntegerSliderControl: CommonDataUserControl
    {
        public IntegerSliderControl()
        {
            InitializeComponent();
        }

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(int),
            typeof(IntegerSliderControl),
            new FrameworkPropertyMetadata(
                default(int),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnValueChanged)
            )
        );

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as IntegerSliderControl;
            if(ctrl != null) {
                ctrl.Value = (int)e.NewValue;
            }
        }

        #endregion

        #region MaximumProperty

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(int),
            typeof(IntegerSliderControl),
            new FrameworkPropertyMetadata(
                int.MaxValue,
                new PropertyChangedCallback(OnMaximumChanged)
            )
        );

        public int Maximum
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as IntegerSliderControl;
            if(ctrl != null) {
                ctrl.Maximum = (int)e.NewValue;
            }
        }

        #endregion

        #region MinimumProperty

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(int),
            typeof(IntegerSliderControl),
            new FrameworkPropertyMetadata(
                int.MinValue,
                new PropertyChangedCallback(OnMinimumChanged)
            )
        );

        public int Minimum
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as IntegerSliderControl;
            if(ctrl != null) {
                ctrl.Minimum = (int)e.NewValue;
            }
        }

        #endregion
    }
}
