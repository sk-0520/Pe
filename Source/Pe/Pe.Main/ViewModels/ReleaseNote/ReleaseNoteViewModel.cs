using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CefSharp;
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

        public ReleaseNoteViewModel(ReleaseNoteElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            //PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            //PropertyChangedHooker.AddHook(nameof(), nameof());
        }

        #region property

        //PropertyChangedHooker PropertyChangedHooker { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Release => Model?.UpdateItem.Release ?? DateTime.UtcNow;
        public Version Version => Model?.UpdateItem.Version ?? new Version();
        public string Revision => Model?.UpdateItem.Revision ?? string.Empty;
        public bool IsCheckOnly => Model?.IsCheckOnly ?? true;

        public IReadOnlyUpdateInfo? UpdateInfo => Model?.UpdateInfo;

        #endregion

        #region command

        public ICommand DownloadCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // CanExecute に対してどうこうする手間がしんどい
                if(IsCheckOnly || UpdateInfo?.State == UpdateState.Error) {
                    Model.StartDownload();
                    RaisePropertyChanged(nameof(IsCheckOnly));
                }
            }
        ));

        public ICommand UpdateCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // CanExecute に対してどうこうする手間がしんどい
                if(UpdateInfo?.IsReady ?? false) {
                    Model.StartUpdate();
                }
            }
        ));

        #endregion

        #region function

        #endregion

        #region ElementViewModelBase

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            var view = (ReleaseNoteWindow)window;
            view.webView.LifeSpanHandler = new PlatformLifeSpanHandler(LoggerFactory);
            view.webView.RequestHandler = new PlatformRequestHandler(LoggerFactory);
            view.webView.MenuHandler = new DisableContextMenuHandler();

            Model.LoadReleaseNoteDocumentAsync().ContinueWith(t => {
                if(IsDisposed) {
                    Logger.LogTrace("close");
                    return;
                }

                if(t.IsCompletedSuccessfully) {
                    var htmlSource = t.Result;
                    view.webView.LoadHtml(htmlSource, Model.UpdateItem.NoteUri.ToString());
                } else {
                    view.webView.LoadHtml(Properties.Resources.File_ReleaseNote_ErrorReleaseNote, nameof(Properties.Resources.File_ReleaseNote_ErrorReleaseNote));
                }
            });
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }


        public void ReceiveViewClosed(Window window, bool isUserOperation)
        { }

        #endregion


    }
}
