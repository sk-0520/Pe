using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Views.ReleaseNote;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ReleaseNote
{
    public class ReleaseNoteViewModel: ElementViewModelBase<ReleaseNoteElement>, IViewLifecycleReceiver
    {
        public ReleaseNoteViewModel(ReleaseNoteElement model, IWebViewInitializer webViewInitializer, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            //PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            //PropertyChangedHooker.AddHook(nameof(), nameof());
            WebViewInitializer = webViewInitializer;
        }

        #region property

        //PropertyChangedHooker PropertyChangedHooker { get; }

        private IWebViewInitializer WebViewInitializer { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Release => Model?.NewVersionItem.Release ?? DateTime.UtcNow;
        public Version Version => Model?.NewVersionItem.Version ?? new Version();
        public string Revision => Model?.NewVersionItem.Revision ?? string.Empty;
        public bool IsCheckOnly => Model?.IsCheckOnly ?? true;

        public IReadOnlyNewVersionInfo? UpdateInfo => Model?.NewVersionInfo;

        #endregion

        #region command

        private ICommand? _DownloadCommand;
        public ICommand DownloadCommand => this._DownloadCommand ??= new DelegateCommand(
            () => {
                // CanExecute に対してどうこうする手間がしんどい
                if(IsCheckOnly || UpdateInfo?.State == NewVersionState.Error) {
                    Model.StartDownload();
                    RaisePropertyChanged(nameof(IsCheckOnly));
                }
            }
        );

        private ICommand? _UpdateCommand;
        public ICommand UpdateCommand => this._UpdateCommand ??= new DelegateCommand(
            () => {
                // CanExecute に対してどうこうする手間がしんどい
                if(UpdateInfo?.IsReady ?? false) {
                    Model.StartUpdate();
                }
            }
        );

        #endregion

        #region function

        #endregion

        #region ElementViewModelBase

        #endregion

        #region IViewLifecycleReceiver

        public async Task ReceiveViewInitializedAsync(Window window, CancellationToken cancellationToken)
        {
            var view = (ReleaseNoteWindow)window;


            var waitInitializeTask = WebViewInitializer.WaitInitializeAsync(cancellationToken);
            var releaseNoteTask = Model.LoadReleaseNoteDocumentAsync(cancellationToken);

            try {
                await Task.WhenAll(waitInitializeTask, releaseNoteTask);
                if(IsDisposed) {
                    Logger.LogTrace("closed");
                    return;
                }
                var htmlSource = releaseNoteTask.Result;
                view.webView.NavigateToString(htmlSource);

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                view.webView.NavigateToString(Properties.Resources.File_ReleaseNote_ErrorReleaseNote);
            }
        }

        public Task ReceiveViewLoadedAsync(Window window, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }


        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
