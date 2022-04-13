using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Plugin
{
    public class PluginWebInstallViewModel: ElementViewModelBase<PluginWebInstallElement>, IViewLifecycleReceiver
    {
        #region variable

        bool _nowInstalling;
        string? _exceptionMessage;

        #endregion

        public PluginWebInstallViewModel(PluginWebInstallElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string PluginIdOrInfoUrl
        {
            get => Model.PluginIdOrInfoUrl;
            set => SetModelValue(value);
        }

        public bool NowInstalling
        {
            get => this._nowInstalling;
            private set => SetProperty(ref this._nowInstalling, value);
        }

        public string ExceptionMessage
        {
            get => this._exceptionMessage ?? string.Empty;
            private set => SetProperty(ref this._exceptionMessage, value);
        }


        #endregion

        #region command

        public ICommand InstallCommand => GetOrCreateCommand(() => new DelegateCommand<Window>(
            async o => {
                ExceptionMessage = string.Empty;
                NowInstalling = true;
                try {
                    await Model.GetPluginAsync();
                } catch(Exception ex) {
                    ExceptionMessage = ex.Message;
                } finally {
                    NowInstalling = false;
                }
            },
            o => !NowInstalling
        ));

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
        }

        #endregion
    }
}
