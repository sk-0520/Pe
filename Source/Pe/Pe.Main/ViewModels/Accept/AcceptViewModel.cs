using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Accept;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Views.Accept;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Accept
{
    public class AcceptViewModel : SingleModelViewModelBase<AcceptElement>, IDialogCommand, IDialogService, IViewLifecycleReceiver, IBuildStatus
    {
        public AcceptViewModel(AcceptElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        //public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();
        public RequestSender CloseRequest { get; } = new RequestSender();

        public bool IsEnabledTelemetry
        {
            get => Model.IsEnabledTelemetry;
            set => SetModelValue(value);
        }

        public UpdateKind UpdateKind
        {
            get => Model.UpdateKind;
            set => SetModelValue(value);
        }

        public bool UpdateKindIsAuto
        {
            get => Model.UpdateKind == UpdateKind.Auto;
            set
            {
                SetModelValue(value ? UpdateKind.Auto : UpdateKind.Notify, nameof(Model.UpdateKind));
            }
        }

        #endregion

        #region command

        public ICommand OpenUriCommand => GetOrCreateCommand(() => new DelegateCommand<string>(
           (o) => {
               try {
                   var uri = new Uri(o);
                   OpenUri(uri);
               }catch(Exception ex) {
                   Logger.LogError(ex, ex.Message);
               }
           }
        ));


        #endregion

        #region function

        void OpenUri(Uri uri)
        {
            var process = Process.Start(new ProcessStartInfo(uri.ToString()) {
                UseShellExecute = true,
            });
        }

        #endregion

        #region IDialogCommand

        public ICommand AffirmativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ThrowIfDisposed();

                Model.Accepted = true;
                CloseRequest.Send();
            }
        ));
        public ICommand NegativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ThrowIfDisposed();

                CloseRequest.Send();
            }
        ));

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            var view = (AcceptWindow)window;
            //view.documentAccept.do
            using(var stream = Model.GetAcceptDocumentXamlStream()) {
                view.documentAccept.Document = (FlowDocument)XamlReader.Load(stream);
            }
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        { }


        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        { }

        #endregion

        #region IDialogService

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBuildStatus
        public BuildType BuildType => BuildStatus.BuildType;
        public Version Version => BuildStatus.Version;
        public string Revision => BuildStatus.Revision;
        #endregion
    }
}
