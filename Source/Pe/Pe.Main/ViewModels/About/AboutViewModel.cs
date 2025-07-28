using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.About;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Views.ReleaseNote;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using ContentTypeTextNet.Pe.Main.Views.About;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using Microsoft.Web.WebView2.Wpf;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Mvvm.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.About
{
    public class AboutViewModel: ElementViewModelBase<AboutElement>, IViewLifecycleReceiver
    {
        #region variable

        private string _uninstallBatchFilePath = string.Empty;

        #endregion

        public AboutViewModel(AboutElement model, IWebViewInitializer webViewInitializer, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            WebViewInitializer = webViewInitializer;
            ComponentCollection = new ObservableCollection<AboutComponentItemViewModel>(model.Components.Select(i => new AboutComponentItemViewModel(i, LoggerFactory)));
            ComponentItems = CollectionViewSource.GetDefaultView(ComponentCollection);
            ComponentItems.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AboutComponentItemViewModel.Kind)));
            ComponentItems.SortDescriptions.Add(new SortDescription(nameof(AboutComponentItemViewModel.Sort), ListSortDirection.Ascending));
        }

        #region property
        private ICommandFactory CommandFactory { get; } = new CommandFactory();

        private IWebViewInitializer WebViewInitializer { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();
        public RequestSender FileSelectRequest { get; } = new RequestSender();
        public RequestSender OutputSettingRequest { get; } = new RequestSender();
        public RequestSender ShowMessageRequest { get; } = new RequestSender();

        private ObservableCollection<AboutComponentItemViewModel> ComponentCollection { get; }
        public ICollectionView ComponentItems { get; }

        /// <summary>
        /// 削除対象。
        /// </summary>
        /// <remarks>
        /// <para>初期値ではユーザーデータを一応残しておく。</para>
        /// </remarks>
        private UninstallTarget UninstallTargets { get; set; } = UninstallTarget.Application | UninstallTarget.Batch | UninstallTarget.Machine | UninstallTarget.Temporary;
        public string UninstallBatchFilePath
        {
            get => this._uninstallBatchFilePath;
            set => SetProperty(ref this._uninstallBatchFilePath, value);
        }

        public bool UninstallTargetUser
        {
            get => UninstallTargets.HasFlag(UninstallTarget.User);
            set => ChangeUninstallTarget(UninstallTarget.User, value);
        }
        public bool UninstallTargetMachine
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Machine);
            set => ChangeUninstallTarget(UninstallTarget.Machine, value);
        }
        public bool UninstallTargetTemporary
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Temporary);
            set => ChangeUninstallTarget(UninstallTarget.Temporary, value);
        }
        public bool UninstallTargetApplication
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Application);
            set => ChangeUninstallTarget(UninstallTarget.Application, value);
        }

        public bool UninstallTargetBatch
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Batch);
            set => ChangeUninstallTarget(UninstallTarget.Batch, value);
        }

        #endregion

        #region command

        private ICommand? _OpenLicenseCommand;
        public ICommand OpenLicenseCommand => this._OpenLicenseCommand ??= CommandFactory.Create<AboutComponentItemViewModel>(
            o => {
                Model.OpenUri(o.LicenseUri);
            }
        );

        private ICommand? _OpenUriCommand;
        public ICommand OpenUriCommand => this._OpenUriCommand ??= CommandFactory.Create<AboutComponentItemViewModel>(
            o => {
                Model.OpenUri(o.Uri);
            }
        );

        private ICommand? _OpenForumUriCommand;
        public ICommand OpenForumUriCommand => this._OpenForumUriCommand ??= CommandFactory.Create(
            () => {
                Model.OpenForumUri();
            }
        );

        private ICommand? _OpenRepositoryUriCommand;
        public ICommand OpenRepositoryUriCommand => this._OpenRepositoryUriCommand ??= CommandFactory.Create(
            () => {
                Model.OpenRepositoryUri();
            }
        );

        private ICommand? _OpenWebsiteUriCommand;
        public ICommand OpenWebsiteUriCommand => this._OpenWebsiteUriCommand ??= CommandFactory.Create(
            () => {
                Model.OpenWebsiteUri();
            }
        );

        private ICommand? _CopyShortInformationCommand;
        public ICommand CopyShortInformationCommand => this._CopyShortInformationCommand ??= CommandFactory.Create(
            () => {
                Model.CopyShortInformation();
            }
        );

        private ICommand? _CopyLongInformationCommand;
        public ICommand CopyLongInformationCommand => this._CopyLongInformationCommand ??= CommandFactory.Create(
            () => {
                Model.CopyLongInformation();
            }
        );

        private ICommand? _OpenApplicationDirectoryCommand;
        public ICommand OpenApplicationDirectoryCommand => this._OpenApplicationDirectoryCommand ??= CommandFactory.Create(
            () => {
                Model.OpenApplicationDirectory();
            }
        );

        private ICommand? _OpenUserDirectoryCommand;
        public ICommand OpenUserDirectoryCommand => this._OpenUserDirectoryCommand ??= CommandFactory.Create(
            () => {
                Model.OpenUserDirectory();
            }
        );

        private ICommand? _OpenMachineDirectoryCommand;
        public ICommand OpenMachineDirectoryCommand => this._OpenMachineDirectoryCommand ??= CommandFactory.Create(
            () => {
                Model.OpenMachineDirectory();
            }
        );

        private ICommand? _OpenTemporaryDirectoryCommand;
        public ICommand OpenTemporaryDirectoryCommand => this._OpenTemporaryDirectoryCommand ??= CommandFactory.Create(
            () => {
                Model.OpenTemporaryDirectory();
            }
        );

        private ICommand? _OutputSettingCommand;
        public ICommand OutputSettingCommand => this._OutputSettingCommand ??= CommandFactory.Create(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    OutputSettingRequest,
                    string.Empty,
                    false,
                    new[] {
                        new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_About_OutputHtml, "html", "*.html"),
                    },
                    r => {
                        var path = r.ResponseFilePaths[0];
                        try {
                            Logger.LogDebug("path: {Path}", path);
                            Model.OutputHtmlSetting(path);
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                );
            }
        );

#if DEBUG
        private ICommand? _DebugOutputSettingCommand;
        public ICommand DebugOutputSettingCommand => this._DebugOutputSettingCommand ??= CommandFactory.Create(
            () => {
                try {
                    var path = "x:\\a.html";
                    Model.OutputHtmlSetting(path);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }
        );

#endif

        private ICommand? _SelectUninstallBatchFilePathCommand;
        public ICommand SelectUninstallBatchFilePathCommand => this._SelectUninstallBatchFilePathCommand ??= CommandFactory.Create(
             () => {
                 var dialogRequester = new DialogRequester(LoggerFactory);
                 dialogRequester.SelectFile(
                     FileSelectRequest,
                     string.Empty,
                     false,
                     new[] {
                        new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_About_Uninstall, "bat", "*.bat"),
                     },
                     r => {
                         var path = r.ResponseFilePaths[0];
                         try {
                             UninstallBatchFilePath = path;
                         } catch(Exception ex) {
                             Logger.LogError(ex, ex.Message);
                         }
                     }
                 );
             }
         );

        private ICommand? _CreateUninstallBatchCommand;
        public ICommand CreateUninstallBatchCommand => this._CreateUninstallBatchCommand ??= CommandFactory.Create(
            () => {
                if(Model.CheckCreateUninstallBatch(UninstallBatchFilePath, UninstallTargets)) {
                    try {
                        Model.CreateUninstallBatch(UninstallBatchFilePath, UninstallTargets);

                        try {
                            var systemExecutor = new SystemExecutor();
                            systemExecutor.OpenDirectoryWithFileSelect(UninstallBatchFilePath);
                        } catch(Exception ex) {
                            Logger.LogWarning(ex, ex.Message);
                        }

                        ShowMessageRequest.Send(new CommonMessageDialogRequestParameter() {
                            Caption = Properties.Resources.String_About_Uninstall_Create_Caption,
                            Message = Properties.Resources.String_About_Uninstall_Create_Message,
                            Buttons = [Forms.TaskDialogButton.OK],
                            DefaultButton = Forms.TaskDialogButton.OK,
                            Icon = Forms.TaskDialogIcon.Information,
                        });
                    } catch(Exception ex) {
                        Logger.LogError(ex, ex.Message);
                        ShowMessageRequest.Send(new CommonMessageDialogRequestParameter() {
                            Caption = Properties.Resources.String_About_Uninstall_Create_Caption,
                            Message = ex.ToString(),
                            Buttons = [Forms.TaskDialogButton.OK],
                            DefaultButton = Forms.TaskDialogButton.OK,
                            Icon = Forms.TaskDialogIcon.Error,
                        });
                    }
                }
            }
        );

        #endregion

        #region function

        private void ChangeUninstallTarget(UninstallTarget uninstallTarget, bool isEnabled, [CallerMemberName] string callerMemberName = "")
        {
            if(isEnabled) {
                UninstallTargets |= uninstallTarget;
            } else {
                UninstallTargets &= ~uninstallTarget;
            }

            RaisePropertyChanged(callerMemberName);
        }

        private string GetRuntimePath(CoreWebView2 coreWebView)
        {
            var processId = (int)coreWebView.BrowserProcessId;
            try {
                var process = Process.GetProcessById(processId);
                var fileName = process.MainModule?.FileName;
                return System.IO.Path.GetDirectoryName(fileName) ?? throw new WebView2RuntimeNotFoundException("failed to get runtime path");
            } catch(Exception e) {
                Logger.LogError(e, e.Message);
                return e.Message;
            }
        }

        #endregion

        #region IViewLifecycleReceiver

        public async Task ReceiveViewInitializedAsync(Window window, CancellationToken cancellationToken)
        {
            var view = (AboutWindow)window;

            await WebViewInitializer.WaitInitializeAsync(cancellationToken);

            var options = new CoreWebView2EnvironmentOptions();
            var parameter = new Dictionary<string,string>() {
                ["ASSEMBLY_NAME"] = typeof(WebView2).Assembly.FullName!,
                ["SDK_VERSION"] = options.TargetCompatibleBrowserVersion,
                ["RUNTIME_VERSION"] = view.webView.CoreWebView2.Environment.BrowserVersionString,
                ["RUNTIME_PATH"] = GetRuntimePath(view.webView.CoreWebView2),
            };
            var html = TextUtility.ReplaceFromDictionary(Properties.Resources.File_About_WebView, parameter);
            view.webView.NavigateToString(html);
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
