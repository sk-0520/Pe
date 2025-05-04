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
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// BadgeControl.xaml の相互作用ロジック
    /// </summary>
    public partial class BadgeControl: UserControl
    {
        public BadgeControl()
        {
            InitializeComponent();
        }

        #region BadgeShape

        public static readonly DependencyProperty BadgeShapeProperty = DependencyProperty.Register(
            nameof(BadgeShape),
            typeof(BadgeShape),
            typeof(BadgeControl),
            new FrameworkPropertyMetadata(
                default(BadgeShape),
                new PropertyChangedCallback(OnBadgeShapeChanged)
            )
        );

        public BadgeShape BadgeShape
        {
            get { return (BadgeShape)GetValue(BadgeShapeProperty); }
            set { SetValue(BadgeShapeProperty, value); }
        }

        private static void OnBadgeShapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BadgeControl control) {
            }
        }

        #endregion

        #region BaseColor

        public static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register(
            nameof(BaseColor),
            typeof(Color),
            typeof(BadgeControl),
            new FrameworkPropertyMetadata(
                Colors.Transparent,
                new PropertyChangedCallback(OnBaseColorChanged)
            )
        );

        public Color BaseColor
        {
            get { return (Color)GetValue(BaseColorProperty); }
            set { SetValue(BaseColorProperty, value); }
        }

        private static void OnBaseColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BadgeControl control) {
            }
        }

        #endregion

        //#region AutoForeground

        //public static readonly DependencyProperty AutoForegroundProperty = DependencyProperty.Register(
        //    nameof(AutoForeground),
        //    typeof(bool),
        //    typeof(BadgeControl),
        //    new FrameworkPropertyMetadata(
        //        true,
        //        new PropertyChangedCallback(OnAutoForegroundChanged)
        //    )
        //);

        //public bool AutoForeground
        //{
        //    get { return (bool)GetValue(AutoForegroundProperty); }
        //    set { SetValue(AutoForegroundProperty, value); }
        //}

        //private static void OnAutoForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if(d is BadgeControl control) {
        //    }
        //}

        //#endregion

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(BadgeControl),
            new FrameworkPropertyMetadata(
                string.Empty,
                new PropertyChangedCallback(OnTextChanged)
            )
        );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BadgeControl control) {
            }
        }

        #endregion
    }
}
