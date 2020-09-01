using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemExtension
{
    public class LauncherItemExtensionViewModel: ElementViewModelBase<LauncherItemExtensionElement>, IViewLifecycleReceiver
    {
        public LauncherItemExtensionViewModel(LauncherItemExtensionElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing(window);
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(window, isUserOperation);
        }

        #endregion

    }
}
