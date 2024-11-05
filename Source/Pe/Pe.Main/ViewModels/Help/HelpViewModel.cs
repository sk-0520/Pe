using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Help;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Views.Help;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Help
{
    public class HelpViewModel: ElementViewModelBase<HelpElement>, IViewLifecycleReceiver
    {
        public HelpViewModel(HelpElement model, IWebViewInitializer webViewInitializer, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            WebViewInitializer = webViewInitializer;
        }

        #region property

        private IWebViewInitializer WebViewInitializer { get; }

        private string HtmlFileRawUri => "file://" + Model.HtmlFile.FullName.Replace(Path.DirectorySeparatorChar, '/');

        #endregion

        #region IViewLifecycleReceiver

        public async Task ReceiveViewInitializedAsync(Window window)
        {
            var view = (HelpWindow)window;

            await WebViewInitializer.WaitInitializeAsync(CancellationToken.None);

            view.webView.CoreWebView2.Navigate(HtmlFileRawUri);
        }

        public Task ReceiveViewLoadedAsync(Window window)
        {
            return Task.CompletedTask;
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        { }

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion

    }
}
