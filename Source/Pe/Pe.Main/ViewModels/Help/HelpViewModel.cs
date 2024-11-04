using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
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

        #endregion

        #region IViewLifecycleReceiver

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveViewInitializedAsync(Window window)
        {
            var view = (HelpWindow)window;
            return WebViewInitializer.WaitInitializeAsync(CancellationToken.None).ContinueWith(t => {
                view.webView.NavigateToString("<html>HTML!</html>");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
