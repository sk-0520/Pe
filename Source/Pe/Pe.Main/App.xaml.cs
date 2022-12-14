using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application
    {
        #region property

        ApplicationManager? ApplicationManager { get; set; }
        ILogger? Logger { get; set; }
        RunMode RunMode { get; set; }
        bool CachedUnhandledException { get; set; }

        #endregion

        #region function

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            var stopwatch = Stopwatch.StartNew();

            base.OnStartup(e);
#if DEBUG
            DebugStartup();
#endif
            var initializer = new ApplicationInitializer();
            var accepted = initializer.Initialize(this, e);
            Debug.Assert(initializer.Logging != null);
            if(!accepted) {
                Shutdown();
                return;
            }

            Logger = initializer.Logging.Factory.CreateLogger(GetType());
            RunMode = initializer.RunMode;

            switch(initializer.RunMode) {
                case Models.Data.RunMode.Normal: {
                        ApplicationManager = new ApplicationManager(initializer);

                        if(!ApplicationManager.Startup(this, e)) {
                            Shutdown();
                            return;
                        }

                        var viewModel = ApplicationManager.CreateViewModel();
                        ApplicationManager.Execute();

                        var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
                        notifyIcon.DataContext = viewModel;

                        Dispatcher.BeginInvoke(new Action<Stopwatch>(sw => {
                            Logger.LogInformation("?????????????????? ????????????: {0}", sw.Elapsed);
                            ApplicationManager.StartupEnd();
                        }), System.Windows.Threading.DispatcherPriority.SystemIdle, stopwatch);
                    }
                    break;

                case Models.Data.RunMode.CrashReport: {
                        ShutdownMode = ShutdownMode.OnMainWindowClose;
                        var options = new Core.Models.CommandLineSimpleConverter<CrashReport.Models.Data.CrashReportOptions>(new Core.Models.CommandLine(e.Args, false)).GetMappingData();
                        if(options == null) {
                            Logger.LogError("??????????????????????????????????????????: {0}", string.Join(" ", e.Args));
                            Shutdown(-1);
                            return;
                        }
                        var model = new CrashReport.Models.Element.CrashReportElement(options, initializer.Logging.Factory);
                        model.Initialize();
                        var viewModel = new CrashReport.ViewModels.CrashReportViewModel(model, new Models.Telemetry.UserTracker(initializer.Logging.Factory), new ApplicationDispatcherWrapper(Timeout.InfiniteTimeSpan), initializer.Logging.Factory);
                        MainWindow = new CrashReport.Views.CrashReportWindow() {
                            DataContext = viewModel,
                        };
                        MainWindow.Show();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

        }

        #endregion

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if(!CachedUnhandledException) {
                CachedUnhandledException = true;
            } else {
                // ??????????????????????????????????????????????????????
                Shutdown(2);
                return;
            }

            if(Logger != null) {
                Logger.LogError(e.Exception, "{0}, {1}", e.Dispatcher.Thread.ManagedThreadId, e.Exception.Message);
                Debug.Assert(ApplicationManager != null);
                Logger.LogInformation("RunMode: {0}", RunMode);
                Logger.LogInformation("CanSendCrashReport: {0}", ApplicationManager.CanSendCrashReport);
                if(RunMode == RunMode.Normal && ApplicationManager.CanSendCrashReport) {
                    // ??????????????????????????????
                    Logger.LogInformation("?????????????????????????????????????????????????????????");
                    var outputFile = ApplicationManager.OutputRawCrashReport(e.Exception);
                    Logger.LogInformation("??????????????????????????????????????????: {0}", outputFile);
                    Logger.LogInformation("???????????????????????????????????????????????????...");
                    ApplicationManager.ExecuteCrashReport(outputFile);

                    e.Handled = ApplicationManager.UnhandledExceptionHandled;
                }
            } else {
                MessageBox.Show(e.Exception.ToString());
            }

            Shutdown(1);
        }
    }
}
