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
using ContentTypeTextNet.Pe.Main.Views.Help;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Help
{
    public class HelpViewModel: ElementViewModelBase<HelpElement>, IViewLifecycleReceiver
    {
        public HelpViewModel(HelpElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region IViewLifecycleReceiver

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ReceiveViewInitialized(Window window)
        {
            var view = (HelpWindow)window;
                view.webView.NavigateToString("<html>HTML!</html>");
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
