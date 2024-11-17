using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Commands;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Platform;

namespace ContentTypeTextNet.Pe.Main.Views.About
{
    /// <summary>
    /// AboutWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutWindow: Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }

        #region property

        private DialogRequestReceiver DialogRequestReceiver { get; }

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

        private ICommand? _FileSelectCommand;
        public ICommand FileSelectCommand => this._FileSelectCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        );

        private ICommand? _OutputSettingCommand;
        public ICommand OutputSettingCommand => this._OutputSettingCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        );

        private ICommand? _OpenCommonMessageDialogCommand;
        public ICommand OpenCommonMessageDialogCommand => this._OpenCommonMessageDialogCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                Forms.TaskDialog.ShowDialog(
                    HandleUtility.GetWindowHandle(this),
                    parameter.ToTaskDialogPage(),
                    Forms.TaskDialogStartupLocation.CenterOwner
                );
            }
        );

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "自動生成")]
        private async void root_SourceInitialized(object sender, System.EventArgs e)
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
            WebViewInitializer.Dispose();
        }

        private void CoreWebView2_NewWindowRequested(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            var systemExecutor = new SystemExecutor();
            systemExecutor.OpenUri(new System.Uri(e.Uri));
        }
    }
}
