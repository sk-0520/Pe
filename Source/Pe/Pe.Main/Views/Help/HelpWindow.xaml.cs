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
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Help
{
    /// <summary>
    /// HelpWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HelpWindow: Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        #region property

        [DiInjection]
        private WebViewInitializer WebViewInitializer { get; set; } = default!;
        [DiInjection]
        private EnvironmentParameters EnvironmentParameters { get; set; } = default!;
        [DiInjection]
        private ICultureService CultureService { get; set; } = default!;

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => Close()
        );

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        private async void Window_SourceInitialized(object sender, EventArgs e)
        {
            await WebViewInitializer.InitializeAsync(this.webView, EnvironmentParameters, CultureService);
            this.webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            this.webView.Unloaded += WebView_Unloaded;
            this.webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        private void WebView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.webView.CoreWebView2.NewWindowRequested -= CoreWebView2_NewWindowRequested;
            this.webView.Unloaded -= WebView_Unloaded;
        }

        private void CoreWebView2_NewWindowRequested(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            var systemExecutor = new SystemExecutor();
            systemExecutor.OpenUri(new System.Uri(e.Uri));
        }

    }
}
