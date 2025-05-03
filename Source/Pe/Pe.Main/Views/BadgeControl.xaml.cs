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

        #region Badge

        public static readonly DependencyProperty BadgeProperty = DependencyProperty.Register(
            nameof(Badge),
            typeof(BadgeViewModelBase),
            typeof(BadgeControl),
            new FrameworkPropertyMetadata(
                default(BadgeViewModelBase),
                new PropertyChangedCallback(OnBadgeChanged)
            )
        );

        public BadgeViewModelBase? Badge
        {
            get { return GetValue(BadgeProperty) as BadgeViewModelBase; }
            set { SetValue(BadgeProperty, value); }
        }

        private static void OnBadgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BadgeControl control) {
            }
        }

        #endregion
    }
}
