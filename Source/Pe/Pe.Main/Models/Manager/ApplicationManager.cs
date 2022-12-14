using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.CrashReport.Models;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.Widget;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    internal partial class ApplicationManager: DisposerBase, IOrderManager
    {
        internal ApplicationManager(ApplicationInitializer initializer)
        {
            Logging = initializer.Logging ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.Logging));
            Logger = Logging.Factory.CreateLogger(GetType());
            IsFirstStartup = initializer.IsFirstStartup;


#if DEBUG
            IsDebugDevelopMode = initializer.IsDebugDevelopMode;
#endif

            ApplicationDiContainer = initializer.DiContainer ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.DiContainer));
            var customConfiguration = ApplicationDiContainer.Get<ApplicationConfiguration>();

            PlatformThemeLoader = ApplicationDiContainer.Build<PlatformThemeLoader>();
            PlatformThemeLoader.Changed += PlatformThemeLoader_Changed;
            ApplicationDiContainer.Register<IPlatformTheme, PlatformThemeLoader>(PlatformThemeLoader);

            WindowManager = initializer.WindowManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.WindowManager));
            OrderManager = ApplicationDiContainer.Build<OrderManagerImpl>(); //initializer.OrderManager;
            NotifyManagerImpl = initializer.NotifyManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.NotifyManager));
            StatusManagerImpl = initializer.StatusManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.StatusManager));
            ClipboardManager = initializer.ClipboardManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.ClipboardManager));
            UserAgentManager = initializer.UserAgentManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.UserAgentManager));
            ApplicationMutex = initializer.Mutex ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.Mutex));

            NotifyManagerImpl.LauncherGroupItemRegistered += NotifyManagerImpl_LauncherGroupItemRegistered;

            ApplicationDiContainer.Register<IWindowManager, WindowManager>(WindowManager);
            ApplicationDiContainer.Register<IOrderManager, IOrderManager>(this);
            ApplicationDiContainer.Register<INotifyManager, NotifyManager>(NotifyManagerImpl);
            ApplicationDiContainer.Register<IStatusManager, StatusManager>(StatusManagerImpl);
            ApplicationDiContainer.Register<IClipboardManager, ClipboardManager>(ClipboardManager);
            ApplicationDiContainer.Register<IUserAgentManager, UserAgentManager>(UserAgentManager);
            ApplicationDiContainer.Register<IHttpUserAgentFactory, IHttpUserAgentFactory>(UserAgentManager);

            ApplicationDiContainer.Register<LauncherItemAddonViewSupporterCollection, LauncherItemAddonViewSupporterCollection>(DiLifecycle.Singleton);

            var addonContainer = ApplicationDiContainer.Build<AddonContainer>();
            var themeContainer = ApplicationDiContainer.Build<ThemeContainer>();
            PluginContainer = ApplicationDiContainer.Build<PluginContainer>(addonContainer, themeContainer);
            ApplicationDiContainer.Register<ILauncherItemAddonFinder, ILauncherItemAddonFinder>(DiLifecycle.Transient, () => new LauncherItemAddonFinder(PluginContainer.Addon, LoggerFactory));

            // ??????????????????????????????????????????
            ApplicationDiContainer.Register<PluginContainer, PluginContainer>(PluginContainer);

            // ?????????IF???DI??????
            ApplicationDiContainer.Register<IGeneralTheme, IGeneralTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetGeneralTheme());
            ApplicationDiContainer.Register<ILauncherToolbarTheme, ILauncherToolbarTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetLauncherToolbarTheme());
            ApplicationDiContainer.Register<INoteTheme, INoteTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNoteTheme());
            ApplicationDiContainer.Register<ICommandTheme, ICommandTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetCommandTheme());
            ApplicationDiContainer.Register<INotifyLogTheme, INotifyLogTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNotifyTheme());
            //// ????????????IF???DI??????
            //ApplicationDiContainer.Register<ICommandFinder, CommandFinderAddonWrapper>(DiLifecycle.Transient, () => PluginContainer.Addon.GetCommandFinder());

            // ??????????????????????????????????????????(????????????????????????????????????????????????)
            var fullscreenWatcher = ApplicationDiContainer.Build<FullscreenWatcher>();
            var fullscreen = customConfiguration.Platform.Fullscreen;
            foreach(var item in fullscreen.IgnoreWindowClasses) {
                fullscreenWatcher.IgnoreFullscreenWindowClassNames.Add(item);
            }
            foreach(var item in fullscreen.IgnoreClassAndTexts) {
                fullscreenWatcher.ClassAndTexts.Add(item);
            }
            fullscreenWatcher.TopmostOnly = fullscreen.TopmostOnly;
            fullscreenWatcher.ExcludeNoActive = fullscreen.ExcludeNoActive;
            fullscreenWatcher.ExcludeToolWindow = fullscreen.ExcludeToolWindow;
            ApplicationDiContainer.Register<IFullscreenWatcher, FullscreenWatcher>(fullscreenWatcher);

            KeyboradHooker = new KeyboradHooker(LoggerFactory);
            MouseHooker = new MouseHooker(LoggerFactory);
            KeyActionChecker = new KeyActionChecker(LoggerFactory);
            KeyActionAssistant = new KeyActionAssistant(LoggerFactory);

            CronScheduler = new CronScheduler(LoggerFactory);

            ApplicationUpdateInfo = ApplicationDiContainer.Build<UpdateInfo>();

            NotifyLogElement = ApplicationDiContainer.Build<NotifyLogElement>();
            NotifyLogElement.Initialize();

            LazyScreenElementReset = ApplicationDiContainer.Build<LazyAction>(nameof(LazyScreenElementReset), customConfiguration.Platform.ScreenElementsResetWaitTime);

            LowScheduler = new System.Timers.Timer(customConfiguration.Schedule.LowSchedulerTime.TotalMilliseconds);
            LowScheduler.Elapsed += LowScheduler_Elapsed;

            if(!string.IsNullOrWhiteSpace(initializer.TestPluginDirectoryPath)) {
                TestPluginDirectory = new DirectoryInfo(initializer.TestPluginDirectoryPath);
                TestPluginName = initializer.TestPluginName;
            }
        }

        #region property

        ApplicationLogging Logging { get; set; }
        ILoggerFactory LoggerFactory => Logging.Factory;
        ApplicationDiContainer ApplicationDiContainer { get; set; }
        bool IsFirstStartup { get; }

#if DEBUG
        bool IsDebugDevelopMode { get; }
#endif
        ILogger Logger { get; set; }
        PlatformThemeLoader PlatformThemeLoader { get; }

        WindowManager WindowManager { get; set; }
        OrderManagerImpl OrderManager { get; set; }
        NotifyManager NotifyManagerImpl { get; set; }
        public INotifyManager NotifyManager => NotifyManagerImpl;
        StatusManager StatusManagerImpl { get; set; }
        public IStatusManager StatusManager => StatusManagerImpl;
        ClipboardManager ClipboardManager { get; set; }
        UserAgentManager UserAgentManager { get; set; }

        Mutex ApplicationMutex { get; }

        ObservableCollection<LauncherGroupElement> LauncherGroupElements { get; } = new ObservableCollection<LauncherGroupElement>();
        ObservableCollection<LauncherToolbarElement> LauncherToolbarElements { get; } = new ObservableCollection<LauncherToolbarElement>();
        ObservableCollection<NoteElement> NoteElements { get; } = new ObservableCollection<NoteElement>();
        ObservableCollection<StandardInputOutputElement> StandardInputOutputs { get; } = new ObservableCollection<StandardInputOutputElement>();
        ObservableCollection<LauncherItemExtensionElement> LauncherItemExtensions { get; } = new ObservableCollection<LauncherItemExtensionElement>();
        CommandElement? CommandElement { get; set; }
        NotifyLogElement NotifyLogElement { get; }
        //FeedbackElement? FeedbackElement { get; set; }
        HwndSource? MessageWindowHandleSource { get; set; }
        //IDispatcherWapper? MessageWindowDispatcherWapper { get; set; }

        KeyboradHooker KeyboradHooker { get; }
        MouseHooker MouseHooker { get; }
        KeyActionChecker KeyActionChecker { get; }
        KeyActionAssistant KeyActionAssistant { get; }

        PluginContainer PluginContainer { get; }

        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public UpdateInfo ApplicationUpdateInfo { get; }

        public bool CanCallNotifyAreaMenu { get; private set; }

        public bool CanSendCrashReport => ApplicationDiContainer.Get<GeneralConfiguration>().CanSendCrashReport;
        public bool UnhandledExceptionHandled => ApplicationDiContainer.Get<GeneralConfiguration>().UnhandledExceptionHandled;

        private bool ResetWaiting { get; set; }
        private LazyAction LazyScreenElementReset { get; }

        private DirectoryInfo? TestPluginDirectory { get; }
        private string TestPluginName { get; } = string.Empty;

        private ObservableCollection<WidgetElement> Widgets { get; } = new ObservableCollection<WidgetElement>();
        #endregion

        #region function

        /// <summary>
        /// ?????????????????????????????????????????????
        /// </summary>
        public void ShowSettingView()
        {
            SaveWidgets();
            using var viewPausing = PauseAllViews();

            Logger.LogDebug("??????????????????????????????");
            var lazyWriterPack = ApplicationDiContainer.Get<IDatabaseLazyWriterPack>();
            var lazyWriterItemMap = new Dictionary<IDatabaseLazyWriter, IDisposable>();
            foreach(var lazyWriter in lazyWriterPack.Items) {
                lazyWriter.Flush();
                var pausing = lazyWriter.Pause();
                lazyWriterItemMap.Add(lazyWriter, pausing);
            }

            // ??????DB??????????????????????????????
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var settingDirectory = environmentParameters.TemporarySettingDirectory;
            var directoryCleaner = new DirectoryCleaner(settingDirectory, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, LoggerFactory);
            directoryCleaner.Clear(false);

            var settings = new {
                Main = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.MainFile.Name)),
                File = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.LargeFile.Name)),
            };
            //var settingDatabaseFile = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.SettingFile.Name));
            //var fileDatabaseFile = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.FileFile.Name));

            environmentParameters.MainFile.CopyTo(settings.Main.FullName);
            environmentParameters.LargeFile.CopyTo(settings.File.FullName);

            // DI?????????????????????????????????
            var container = ApplicationDiContainer.Scope();
            var factory = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(settings.Main, true, false),
                new ApplicationDatabaseFactory(settings.File, true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            container
                .Register<IDiContainer, DiContainer>((DiContainer)container) // ???????????????
                .Register<ISettingNotifyManager, SettingNotifyManager>(DiLifecycle.Singleton)
                .RegisterDatabase(factory, lazyWriterWaitTimePack, LoggerFactory)
            ;

            var workingDatabasePack = ApplicationDiContainer.Build<IDatabaseAccessorPack>();
            var settingDatabasePack = container.Build<IDatabaseAccessorPack>();
            PersistentHelper.Copy(workingDatabasePack.Temporary, settingDatabasePack.Temporary);

            var settingElement = new SettingContainerElement(container, Logging.PauseReceiveLog, container.Build<ILoggerFactory>());
            settingElement.Initialize();
            var windowItem = OrderManager.CreateSettingWindow(settingElement);
            WindowManager.Register(windowItem);
            var dialogResult = windowItem.Window.ShowDialog();

            static void EndPreferences(SettingContainerElement settingElement, ILogger logger)
            {
                foreach(var element in settingElement.PluginsSettingEditor.PluginItems) {
                    if(element.SupportedPreferences && element.StartedPreferences) {
                        logger.LogTrace("?????????????????????????????????: {0}({1})", element.PluginState.PluginName, element.PluginState.PluginId);
                        element.EndPreferences();
                    }
                }
            }

            if(settingElement.IsSubmit) {
                Logger.LogInformation("????????????????????????????????????????????????");
                CloseViews(false);
                DisposeElements();

                // ?????????DB????????????DB???????????????
                var pack = ApplicationDiContainer.Get<IDatabaseAccessorPack>();
                var stoppings = (new IDatabaseAccessor[] { pack.Main, pack.Large })
                    .Select(i => i.PauseConnection())
                    .ToList()
                ;

                // ??????????????????????????????
                //string userBackupDirectoryPath;
                //using(var commander = container.Get<IMainDatabaseBarrier>().WaitRead()) {
                //    var appGeneralSettingEntityDao = container.Build<AppGeneralSettingEntityDao>(commander, commander.Implementation);
                //    userBackupDirectoryPath = appGeneralSettingEntityDao.SelectUserBackupDirectoryPath();
                //}
                //try {
                //    BackupSettingsCore(
                //        environmentParameters.UserSettingDirectory,
                //        environmentParameters.UserBackupDirectory,
                //        DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                //        environmentParameters.Configuration.Backup.SettingCount,
                //        userBackupDirectoryPath
                //    );
                //} catch(Exception ex) {
                //    Logger.LogError(ex, "??????????????????????????????: {0}", ex.Message);
                //}
                BackupSettingsDefault(container);

                var accessorPack = container.Get<IDatabaseAccessorPack>();
                var databaseSetupper = container.Build<DatabaseSetupper>();
                foreach(var accessor in accessorPack.Items) {
                    databaseSetupper.Tune(accessor);
                }

                settings.Main.CopyTo(environmentParameters.MainFile.FullName, true);
                settings.File.CopyTo(environmentParameters.LargeFile.FullName, true);

                foreach(var stopping in stoppings) {
                    stopping.Dispose();
                }
                var cultureServiceChanger = ApplicationDiContainer.Build<CultureServiceChanger>(CultureService.Instance);
                cultureServiceChanger.ChangeCulture();

                PersistentHelper.Copy(settingDatabasePack.Temporary, workingDatabasePack.Temporary);

                Logger.LogInformation("????????????????????????????????????");

                EndPreferences(settingElement, Logger);

                // ??????????????????????????????????????????????????????
                foreach(var launcherItemId in settingElement.AllLauncherItems.Where(i => !i.IsLazyLoad).Select(i => i.LauncherItemId)) {
                    OrderManager.RefreshLauncherItemElement(launcherItemId);
                }

                ApplyThemeSetting();
                RebuildHook();
                RebuildSchedulerSetting();
                ResetNotifyArea();
                ExecuteElements();

                if(CommandElement != null) {
                    if(CommandElement.IsInitialized) {
                        CommandElement.Refresh();
                    }
                }
                NotifyLogElement.Refresh();
                NotifyManager.SendSettingChanged();
            } else {
                Logger.LogInformation("?????????????????????????????????????????????????????????");
                EndPreferences(settingElement, Logger);
            }

            Logger.LogDebug("??????????????????????????????");
            foreach(var pair in lazyWriterItemMap) {
                if(settingElement.IsSubmit) {
                    // ?????????????????????????????????????????????????????????????????????????????????(????????????)?????????
                    pair.Key.ClearStock();
                }
                pair.Value.Dispose();
            }

            settingElement.Dispose();
            container.UnregisterDatabase();
            container.Dispose();
        }

        /// <summary>
        /// ???????????????????????????????????????????????????????????????????????????
        /// <para>?????????????????????????????????????????????????????????</para>
        /// </summary>
        /// <returns></returns>
        IDisposable PauseAllViews()
        {
            StopPlatform();
            StopScheduler();
            StopHook();
            UninitializeSystem();

            KeyActionChecker.Reset();
            NotifyManagerImpl.ClearAllLogs();
            KeyboardNotifyLogId = Guid.Empty;

            if(CommandElement != null) {
                if(CommandElement.ViewCreated) {
                    CommandElement.HideView(true);
                }
            }
            if(NotifyLogElement.ViewCreated) {
                NotifyLogElement.HideView(true);
            }

            var silent = NotifyLogElement.ToSilent();
            var changing = StatusManagerImpl.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(Bridge.Plugin.Addon.BackgroundKind.KeyboardHook)) {
                    var context = new BackgroundAddonProxyRunPauseContext(true);
                    BackgroundAddon.RunPause(context);
                }
            }

            return new ActionDisposer(d => {
                StartHook();
                StartScheduler();
                StartBackground();
                StartPlatform();
                InitializeSystem();

                if(BackgroundAddon != null) {
                    if(BackgroundAddon.IsSupported(Bridge.Plugin.Addon.BackgroundKind.KeyboardHook)) {
                        var context = new BackgroundAddonProxyRunPauseContext(false);
                        BackgroundAddon.RunPause(context);
                    }
                }

                if(changing.Success) {
                    changing.SuccessValue?.Dispose();
                }
                silent.Dispose();
            });
        }

        public void ShowStartupView(bool isFirstSetup)
        {
            using var viewPausing = isFirstSetup
                ? new ActionDisposer(d => { Logger.LogInformation("?????????????????????????????????"); })
                : PauseAllViews()
            ;

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.Startup.StartupElement, ViewModels.Startup.StartupViewModel, Views.Startup.StartupWindow>()
                ;
                var startupModel = diContainer.New<Element.Startup.StartupElement>();
                startupModel.Initialize();
                var view = diContainer.Build<Views.Startup.StartupWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.Startup, startupModel, view));

                view.ShowDialog();

                if(!isFirstSetup) {
                    if(startupModel.IsRegisteredLauncher) {
                        ResetScreenViewElements();
                    }
                }
            }
        }

#if DEBUG
        async Task StartDebugDevelopModeAsync()
        {
            var importProgramsElement = ApplicationDiContainer.Build<Element.Startup.ImportProgramsElement>();
            await importProgramsElement.LoadProgramsAsync();
            await importProgramsElement.ImportAsync();

            var mainBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();

            var commandKeyActionData = new KeyActionData() {
                KeyActionId = idFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Command,
                KeyActionContent = string.Empty,
                Comment = "debug-dev-mode",
            };
            var commandKeyMappings = new[] {
                new KeyMappingData() {
                    Alt = ModifierKey.None,
                    Control = ModifierKey.Any,
                    Shift = ModifierKey.Any,
                    Super = ModifierKey.None,
                    Key = System.Windows.Input.Key.Space,
                }
            };

            var hideToolbarKeyActionData = new KeyActionData() {
                KeyActionId = idFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.LauncherToolbar,
                KeyActionContent = KeyActionContentLauncherToolbar.AutoHiddenToHide.ToString(),
                Comment = "debug-dev-mode",
            };
            var hideToolbarKeyMappings = new[] {
                new KeyMappingData() {
                    Alt = ModifierKey.None,
                    Control = ModifierKey.None,
                    Shift = ModifierKey.None,
                    Super = ModifierKey.None,
                    Key = System.Windows.Input.Key.Escape,
                },
                new KeyMappingData() {
                    Alt = ModifierKey.None,
                    Control = ModifierKey.None,
                    Shift = ModifierKey.None,
                    Super = ModifierKey.None,
                    Key = System.Windows.Input.Key.Escape,
                }
            };


            var pressedOptionConverter = new PressedOptionConverter();

            using(var context = mainBarrier.WaitWrite()) {
                var status = new DatabaseCommonStatus() {
                    Account = "????",
                    ProgramName = "????",
                    ProgramVersion = BuildStatus.Version,
                };
                var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(context, context.Implementation);
                var keyActionsEntityDao = ApplicationDiContainer.Build<KeyActionsEntityDao>(context, context.Implementation);
                var keyOptionsEntityDao = ApplicationDiContainer.Build<KeyOptionsEntityDao>(context, context.Implementation);
                var keyMappingsEntityDao = ApplicationDiContainer.Build<KeyMappingsEntityDao>(context, context.Implementation);

                var userIdManager = ApplicationDiContainer.Build<UserIdManager>();
                appExecuteSettingEntityDao.UpdateExecuteSettingAcceptInput(userIdManager.CreateFromRandom(), true, status);

                keyActionsEntityDao.InsertKeyAction(commandKeyActionData, status);
                keyOptionsEntityDao.InsertOption(commandKeyActionData.KeyActionId, KeyActionPressOption.ThroughSystem.ToString(), false.ToString(), status);
                foreach(var item in commandKeyMappings.Counting()) {
                    keyMappingsEntityDao.InsertMapping(commandKeyActionData.KeyActionId, item.Value, item.Number, status);
                }

                keyActionsEntityDao.InsertKeyAction(hideToolbarKeyActionData, status);
                keyOptionsEntityDao.InsertOption(hideToolbarKeyActionData.KeyActionId, KeyActionPressOption.ThroughSystem.ToString(), true.ToString(), status);
                foreach(var item in hideToolbarKeyMappings.Counting()) {
                    keyMappingsEntityDao.InsertMapping(hideToolbarKeyActionData.KeyActionId, item.Value, item.Number, status);
                }


                context.Commit();
            }
        }
#endif

        public void ShowAboutView()
        {
            using var viewPausing = PauseAllViews();

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.About.AboutElement, ViewModels.About.AboutViewModel, Views.About.AboutWindow>()
                ;
                var model = diContainer.New<Element.About.AboutElement>();
                model.Initialize();

                var view = diContainer.Build<Views.About.AboutWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.About, model, view));

                view.ShowDialog();
            }
        }

        public void ShowHelp()
        {
            try {
                var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
                var systemExecutor = ApplicationDiContainer.Build<SystemExecutor>();
                systemExecutor.ExecuteFile(environmentParameters.HelpFile.FullName);
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        void ShowUpdateReleaseNoteCore(UpdateItemData updateItem, bool isCheckOnly)
        {
            var element = ApplicationDiContainer.Build<Element.ReleaseNote.ReleaseNoteElement>(ApplicationUpdateInfo, updateItem, isCheckOnly);
            element.Initialize();
            var view = ApplicationDiContainer.Build<Views.ReleaseNote.ReleaseNoteWindow>();
            view.DataContext = ApplicationDiContainer.Build<ViewModels.ReleaseNote.ReleaseNoteViewModel>(element);
            WindowManager.Register(new WindowItem(WindowKind.Release, element, view));
            view.Show();
        }

        private void ShowUpdateReleaseNote(UpdateItemData updateItem, bool isCheckOnly)
        {
            var windowItem = WindowManager.GetWindowItems(WindowKind.Release);
            if(windowItem.Any()) {
                // ?????????
                ApplicationDiContainer.Build<IDispatcherWrapper>().Begin(() => {
                    var window = windowItem.FirstOrDefault();
                    if(window != null) {
                        window.Window.Activate();
                    } else {
                        ShowUpdateReleaseNoteCore(updateItem, isCheckOnly);
                    }
                }, DispatcherPriority.ApplicationIdle);
                return;
            }

            ApplicationDiContainer.Build<IDispatcherWrapper>().Begin(() => {
                ShowUpdateReleaseNoteCore(updateItem, isCheckOnly);
            }, DispatcherPriority.ApplicationIdle);
        }

        private void InstallLatestPlugins()
        {
            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();

            var directoryMover = ApplicationDiContainer.Build<DirectoryMover>();

            var dirs = environmentParameters.MachinePluginInstallDirectory.EnumerateDirectories();
            foreach(var dir in dirs) {
                var destDirPath = Path.Combine(environmentParameters.MachinePluginModuleDirectory.FullName, dir.Name);
                var destDir = new DirectoryInfo(destDirPath);
                Logger.LogInformation("?????????????????????: {0}", destDirPath);
                directoryMover.Move(dir, destDir);
            }

            var directoryCleaner = new DirectoryCleaner(environmentParameters.MachinePluginInstallDirectory, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, LoggerFactory);
            directoryCleaner.Clear(false);
        }

        private void LoadPlugins()
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();

            // ???????????????????????????
            var pluginStateItems = ApplicationDiContainer.Build<IMainDatabaseBarrier>().ReadData(c => {
                var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(c, c.Implementation);
                return pluginsEntityDao.SelectePlguinStateData().ToList();
            });

            // ????????????????????????????????????????????????
            var uninstallPlugins = pluginStateItems.Where(i => i.State == PluginState.Uninstall);
            var uninstalledPlugins = new List<PluginStateData>();
            foreach(var uninstallPlugin in uninstallPlugins) {
                // ??????????????????????????????????????????????????????????????????????????????????????????
                using var pack = PersistentHelper.WaitWritePack(
                    ApplicationDiContainer.Build<IMainDatabaseBarrier>(),
                    ApplicationDiContainer.Build<ILargeDatabaseBarrier>(),
                    ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>(),
                    DatabaseCommonStatus.CreateCurrentAccount()
                );
                try {
                    var uninstaller = ApplicationDiContainer.Build<PluginUninstaller>(pack, environmentParameters.MachinePluginModuleDirectory);
                    uninstaller.Uninstall(uninstallPlugin);
                    pack.Commit();
                    uninstalledPlugins.Add(uninstallPlugin);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }

            var enabledPlugins = pluginStateItems.Except(uninstalledPlugins).ToArray();

            // ??????????????????????????????????????????????????????DLL??????
            var pluginFiles = PluginContainer.GetPluginFiles(environmentParameters.MachinePluginModuleDirectory, environmentParameters.ApplicationConfiguration.Plugin.IgnoreBaseFileNames, environmentParameters.ApplicationConfiguration.Plugin.Extentions);

            FileInfo? testPluginFile = null;
            if(TestPluginDirectory != null) {
                var pluginName = string.IsNullOrWhiteSpace(TestPluginName) ? TestPluginDirectory.Name : TestPluginName;
                testPluginFile = PluginContainer.GetPluginFile(TestPluginDirectory, pluginName, environmentParameters.ApplicationConfiguration.Plugin.Extentions);
            }

            // ???????????????????????????????????????????????????????????????????????????????????????????????????
            var pluginLoadStateItems = new List<PluginLoadStateData>();
            var pluginConstructorContext = ApplicationDiContainer.Build<PluginConstructorContext>();
            foreach(var pluginFile in pluginFiles) {
                var loadStateData = PluginContainer.LoadPlugin(pluginFile, enabledPlugins, BuildStatus.Version, pluginConstructorContext, Logging.PauseReceiveLog);
                pluginLoadStateItems.Add(loadStateData);
            }

            PluginLoadStateData? testPluginLoadState = null;
            if(testPluginFile != null) {
                testPluginLoadState = PluginContainer.LoadPlugin(testPluginFile, enabledPlugins, BuildStatus.Version, pluginConstructorContext, Logging.PauseReceiveLog);
                pluginLoadStateItems.Add(testPluginLoadState);
            }

            // ????????????????????????????????????
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            using(var context = barrier.WaitWrite()) {
                var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(context, context.Implementation);
                foreach(var pluginLoadStateItem in pluginLoadStateItems) {
                    // ???????????????ID?????????????????????????????????????????????????????????????????????
                    if(pluginLoadStateItem.PluginId == Guid.Empty && pluginLoadStateItem.LoadState == PluginState.IllegalAssembly) {
                        Logger.LogWarning("??????????????? {0} ???????????????????????????", pluginLoadStateItem.PluginName);
                        if(pluginLoadStateItem == testPluginLoadState) {
#if DEBUG
                            if(Debugger.IsAttached) {
                                Debugger.Break();
                            }
#endif
                            Logger.LogWarning("?????????????????????????????????????????????????????????????????????????????????: {0}, {1}", testPluginFile!.FullName, pluginLoadStateItem.LoadState);
                        }
                        continue;
                    }

                    var pluginStateData = new PluginStateData() {
                        PluginId = pluginLoadStateItem.PluginId,
                        PluginName = pluginLoadStateItem.PluginName,
                        State = pluginLoadStateItem.LoadState
                    };

                    if(pluginLoadStateItem == testPluginLoadState) {
                        if(pluginStateData.State == PluginState.Disable) {
#if DEBUG
                            if(Debugger.IsAttached) {
                                Debugger.Break();
                            }
#endif
                            Logger.LogWarning("?????????????????????????????????????????????????????????????????????????????????????????????: {0}, {1}", testPluginFile!.FullName, pluginLoadStateItem.LoadState);
                            continue;
                        }
                    }

                    if(pluginsEntityDao.SelecteExistsPlugin(pluginLoadStateItem.PluginId)) {
                        pluginsEntityDao.UpdatePluginStateData(pluginStateData, DatabaseCommonStatus.CreateCurrentAccount());
                    } else {
                        pluginsEntityDao.InsertPluginStateData(pluginStateData, DatabaseCommonStatus.CreateCurrentAccount());
                    }
                }

                context.Commit();
            }

            var enabledPluginLoadStateItems = pluginLoadStateItems
                .Where(i => i.LoadState == PluginState.Enable)
                .ToList()
            ;
            var disabledPluginLoadStateItems = pluginLoadStateItems
                .Except(enabledPluginLoadStateItems)
                .Where(i => i.WeekLoadContext != null)
                .ToList()
            ;
            if(0 < disabledPluginLoadStateItems.Count) {
                // ????????????????????????????????????
                foreach(var counter in new Counter(10)) {
                    if(counter.IsFirst || counter.IsLast || (counter.CurrentCount == counter.MaxCount / 2)) {
                        GarbageCollection(true);
                    } else {
                        GarbageCollection(false);
                    }

                    var unloadedItems = new List<PluginLoadStateData>();
                    foreach(var disabledPluginLoadStateItem in disabledPluginLoadStateItems) {
                        if(disabledPluginLoadStateItem.WeekLoadContext!.TryGetTarget(out _)) {
                            Logger.LogInformation("[{0}/{1}] ?????????????????????: {2}, {3}", counter.CurrentCount, counter.MaxCount, disabledPluginLoadStateItem.PluginName, disabledPluginLoadStateItem.PluginId);
                        } else {
                            Logger.LogInformation("[{0}/{1}] ?????????????????????: {2}, {3}", counter.CurrentCount, counter.MaxCount, disabledPluginLoadStateItem.PluginName, disabledPluginLoadStateItem.PluginId);
                            unloadedItems.Add(disabledPluginLoadStateItem);
                        }
                    }
                    foreach(var removeItem in unloadedItems) {
                        disabledPluginLoadStateItems.Remove(removeItem);
                    }
                    if(disabledPluginLoadStateItems.Count == 0) {
                        break;
                    }
                }
                if(0 < disabledPluginLoadStateItems.Count) {
                    GarbageCollection(true);
                    foreach(var disabledPluginLoadStateItem in disabledPluginLoadStateItems) {
                        if(disabledPluginLoadStateItem.WeekLoadContext!.TryGetTarget(out _)) {
                            Logger.LogWarning("[LAST] ???????????????????????????: {0}, {1}", disabledPluginLoadStateItem.PluginName, disabledPluginLoadStateItem.PluginId);
                        } else {
                            Logger.LogInformation("[LAST] ?????????????????????: {0}, {1}", disabledPluginLoadStateItem.PluginName, disabledPluginLoadStateItem.PluginId);
                        }
                    }
                }

                if(disabledPluginLoadStateItems.Count == 0) {
                    Logger.LogInformation("?????????????????????????????????");
                } else {
                    Logger.LogWarning("????????????????????????????????????: {0}", disabledPluginLoadStateItems.Count);
                }
            }

            var applicationPluginTypes = new List<Type>() {
                typeof(DefaultTheme),
            };
            var applicationPlugins = new List<IPlugin>(applicationPluginTypes.Count);
            foreach(var type in applicationPluginTypes) {
                using var context = ApplicationDiContainer.Build<PluginConstructorContext>();
                var appPlugin = (IPlugin)ApplicationDiContainer.New(type, new object[] { context });
                applicationPlugins.Add(appPlugin);
            }

            var initializedPlugins = new List<IPlugin>(enabledPluginLoadStateItems.Count + applicationPlugins.Count);

            var databaseBarrierPack = ApplicationDiContainer.Build<IDatabaseBarrierPack>();

            // Pe?????????????????????
            foreach(var plugin in applicationPlugins) {
                using(var readerPack = databaseBarrierPack.WaitRead()) {
                    using var context = pluginContextFactory.CreateInitializeContext(plugin.PluginInformations, readerPack);
                    plugin.Initialize(context);
                }
                initializedPlugins.Add(plugin);
            }

            // ?????????????????????
            foreach(var pluginLoadStateItem in enabledPluginLoadStateItems) {
                Debug.Assert(pluginLoadStateItem.Plugin != null);

                Logger.LogInformation("??????????????????????????????: {0}, {1}", pluginLoadStateItem.PluginName, pluginLoadStateItem.PluginId);
                var plugin = pluginLoadStateItem.Plugin;
                try {
                    using(var readerPack = databaseBarrierPack.WaitRead()) {
                        using var context = pluginContextFactory.CreateInitializeContext(plugin.PluginInformations, readerPack);
                        plugin.Initialize(context);
                    }
                    initializedPlugins.Add(plugin);
                } catch(Exception ex) {
                    Logger.LogError(ex, "??????????????????????????????: {0}, {1}, {2}", ex.Message, pluginLoadStateItem.PluginName, pluginLoadStateItem.PluginId);
                    if(pluginLoadStateItem.WeekLoadContext!.TryGetTarget(out var loadContext)) {
                        Logger.LogWarning("?????????????????????????????????????????????????????????: {0}, {1}", pluginLoadStateItem.PluginName, pluginLoadStateItem.PluginId);
                        loadContext.Unload();
                    } else {
                        Logger.LogError("??????????????????????????????????????????: {0}, {1}", pluginLoadStateItem.PluginName, pluginLoadStateItem.PluginId);
                    }
                }
            }

            foreach(var plugin in initializedPlugins) {
                Logger.LogInformation("??????????????????????????????: {0}, {1}, {2}", plugin.PluginInformations.PluginIdentifiers.PluginName, plugin.PluginInformations.PluginVersions.PluginVersion, plugin.PluginInformations.PluginIdentifiers.PluginId);
                PluginContainer.AddPlugin(plugin);
            }

            // ??????????????????????????????
            if(0 < initializedPlugins.Count) {
                using(var context = barrier.WaitWrite()) {
                    var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(context, context.Implementation);
                    foreach(var initializedPlugin in initializedPlugins) {
                        pluginsEntityDao.UpdatePluginRunningState(
                            initializedPlugin.PluginInformations.PluginIdentifiers.PluginId,
                            initializedPlugin.PluginInformations.PluginVersions.PluginVersion,
                            BuildStatus.Version,
                            DatabaseCommonStatus.CreateCurrentAccount()
                        );
                    }

                    context.Commit();
                }
            }
        }

        private void UnloadPlugins()
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
            var plugins = PluginContainer.Plugins.Where(i => i.IsInitialized).ToList();
            var themePlugins = plugins.Where(i => i.IsLoaded(PluginKind.Theme)).Select(i => new { Plugin = i, Kind = PluginKind.Theme });
            var addonPlugins = plugins.Where(i => i.IsLoaded(PluginKind.Addon)).Select(i => new { Plugin = i, Kind = PluginKind.Addon });

            using(var writer = pluginContextFactory.BarrierWrite()) {
                foreach(var item in addonPlugins.Concat(themePlugins)) {
                    using var context = pluginContextFactory.CreateUnloadContext(item.Plugin.PluginInformations, writer);
                    try {
                        item.Plugin.Unload(item.Kind, context);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "{0}({1}) {2}", item.Plugin.PluginInformations.PluginIdentifiers.PluginName, item.Plugin.PluginInformations.PluginIdentifiers.PluginId, ex.Message);
                    }
                }

                foreach(var plugin in plugins) {
                    using var context = pluginContextFactory.CreateUninitializeContext(plugin.PluginInformations, writer);
                    try {
                        plugin.Uninitialize(context);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "{0}({1}) {2}", plugin.PluginInformations.PluginIdentifiers.PluginName, plugin.PluginInformations.PluginIdentifiers.PluginId, ex.Message);
                    }
                }

                pluginContextFactory.Save();
            }

        }

        private void ApplyThemeSetting()
        {
            var themePluginId = ApplicationDiContainer.Build<IMainDatabaseBarrier>().ReadData(c => {
                var appGeneralSettingEntityDao = ApplicationDiContainer.Build<AppGeneralSettingEntityDao>(c, c.Implementation);
                try {
                    return appGeneralSettingEntityDao.SelectThemePluginId();
                } catch(Exception ex) {
                    Logger.LogWarning(ex, "????????????????????????ID?????????????????????????????????????????????");
                    return DefaultTheme.Informations.PluginIdentifiers.PluginId;
                }
            });
            ApplyCurrentTheme(themePluginId);
        }

        private void ApplyCurrentTheme(Guid themePluginId)
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();

            PluginContainer.Theme.SetCurrentTheme(themePluginId, pluginContextFactory);
        }

        void SetStaticPlatformTheme()
        {
            var themes = new[] { PlatformThemeKind.Dark, PlatformThemeKind.Light };
            foreach(var theme in themes) {
                var themeKey = theme.ToString();
                var colors = PlatformThemeLoader.GetApplicationThemeColors(theme);
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BackgroundColor"] = colors.Background;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ForegroundColor"] = colors.Foreground;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ControlColor"] = colors.Control;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BorderColor"] = colors.Border;

                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BackgroundBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Background));
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ForegroundBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Foreground));
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ControlBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Control));
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BorderBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Border));

            }
        }

        void SetDynamicPlatformTheme()
        {
            ApplicationDiContainer.Get<IDispatcherWrapper>().VerifyAccess();

            var colors = PlatformThemeLoader.ApplicationThemeKind switch {
                PlatformThemeKind.Dark => (active: "Dark", inactive: "Light"),
                PlatformThemeKind.Light => (active: "Light", inactive: "Dark"),
                _ => throw new NotImplementedException(),
            };

            Application.Current.Resources["PlatformTheme-ThemeColors-BackgroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BackgroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ForegroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ForegroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ControlColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ControlColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors-BorderColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BorderColor"];

            Application.Current.Resources["PlatformTheme-ThemeColors2-BackgroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BackgroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ForegroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ForegroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ControlColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ControlColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-BorderColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BorderColor"];

            Application.Current.Resources["PlatformTheme-ThemeColors-BackgroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BackgroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ForegroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ForegroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ControlBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ControlBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors-BorderBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BorderBrush"];

            Application.Current.Resources["PlatformTheme-ThemeColors2-BackgroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BackgroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ForegroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ForegroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ControlBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ControlBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-BorderBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BorderBrush"];


            var accent = PlatformThemeLoader.GetAccentColors(PlatformThemeLoader.AccentColor);
            Application.Current.Resources["PlatformTheme-AccentColors-AccentColor"] = accent.Accent;
            Application.Current.Resources["PlatformTheme-AccentColors-BaseColor"] = accent.Base;
            Application.Current.Resources["PlatformTheme-AccentColors-HighlightColor"] = accent.Highlight;
            Application.Current.Resources["PlatformTheme-AccentColors-ActiveColor"] = accent.Active;
            Application.Current.Resources["PlatformTheme-AccentColors-DisableColor"] = accent.Disable;

            var text = PlatformThemeLoader.GetTextColor(accent);
            Application.Current.Resources["PlatformTheme-AccentTextColors-AccentColor"] = text.Accent;
            Application.Current.Resources["PlatformTheme-AccentTextColors-BaseColor"] = text.Base;
            Application.Current.Resources["PlatformTheme-AccentTextColors-HighlightColor"] = text.Highlight;
            Application.Current.Resources["PlatformTheme-AccentTextColors-ActiveColor"] = text.Active;
            Application.Current.Resources["PlatformTheme-AccentTextColors-DisableColor"] = text.Disable;

            void ApplyAccentBrush(string name)
            {
                var color = (Color)Application.Current.Resources[name + "Color"];
                var brush = FreezableUtility.GetSafeFreeze(new SolidColorBrush(color));
                Application.Current.Resources[name + "Brush"] = brush;
            }
            var names = new[] {
                "PlatformTheme-AccentColors-Accent",
                "PlatformTheme-AccentColors-Base",
                "PlatformTheme-AccentColors-Highlight",
                "PlatformTheme-AccentColors-Active",
                "PlatformTheme-AccentColors-Disable",

                "PlatformTheme-AccentTextColors-Accent",
                "PlatformTheme-AccentTextColors-Base",
                "PlatformTheme-AccentTextColors-Highlight",
                "PlatformTheme-AccentTextColors-Active",
                "PlatformTheme-AccentTextColors-Disable",
            };
            foreach(var name in names) {
                ApplyAccentBrush(name);
            }

            if(Logger.IsEnabled(LogLevel.Debug)) {
                Logger.LogDebug("????????????????????????: #{0:x2}{1:x2}{2:x2}{3:x2}", accent.Accent.A, accent.Accent.R, accent.Accent.G, accent.Accent.B);
            }
        }

        (bool isEnabledTelemetry, string userId) GetTelemetry()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            SettingAppExecuteSettingData setting;
            using(var context = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(context, context.Implementation);
                setting = dao.SelectSettingExecuteSetting();
            }

            if(!setting.IsEnabledTelemetry) {
                Logger.LogInformation("??????????????????: ??????");
                return (false, string.Empty);
            }

            var userIdManager = ApplicationDiContainer.Build<UserIdManager>();
            if(!userIdManager.IsValidUserId(setting.UserId)) {
                Logger.LogWarning("????????????ID?????????: {0}", setting.UserId);
                return (false, string.Empty);
            }

            return (setting.IsEnabledTelemetry, setting.UserId);
        }

        void StartupUsageStatistics()
        {
            var userTracker = new UserTracker(LoggerFactory);
            ApplicationDiContainer.Register<IUserTracker, UserTracker>(userTracker);

            //var configuration = ApplicationDiContainer.Build<Configuration>();
            var setting = GetTelemetry();
            if(setting.isEnabledTelemetry) {
                //AppCenter.Start(
                //    configuration.Api.AppCenter,
                //    typeof(Crashes),
                //    typeof(Analytics)
                //);
                //AppCenter.SetUserId(setting.UserId);
            }
        }

        private void LoggingInformation()
        {
            var infoCollector = ApplicationDiContainer.Build<ApplicationInformationCollector>();
            infoCollector.Header = string.Empty;
            infoCollector.Indent = string.Empty;

            var s = infoCollector.GetLongInformation();
            Logger.LogInformation("[????????????]" + Environment.NewLine + s);
        }

        public bool Startup(App app, StartupEventArgs e)
        {
            StartupUsageStatistics();
            LoggingInformation();

            //var initializer = new ApplicationInitializer();
            //if(!initializer.Initialize(e.Args)) {
            //    return false;
            //}
            //ApplicationDiContainer.Register<IPlatformTheme, PlatformThemeLoader>(PlatformThemeLoader);

            //setting.UserId

            //ApplicationDiContainer.Get<IDispatcherWrapper>().Invoke(() => {
            SetStaticPlatformTheme();
            SetDynamicPlatformTheme();
            //});

            MakeMessageWindow();

            InstallLatestPlugins();
            LoadPlugins();

            ApplyThemeSetting();

            Logger = LoggerFactory.CreateLogger(GetType());
            Logger.LogDebug("???????????????");

            if(IsFirstStartup) {
                // ??????????????????????????????
#if DEBUG
                if(IsDebugDevelopMode) {
                    Task.Run(() => {
                        return StartDebugDevelopModeAsync();
                    }).Wait();
                } else {
                    ShowStartupView(true);
                }
#else
                ShowStartupView(true);
#endif
            }

            Issue737();

            var tuner = ApplicationDiContainer.Build<DatabaseTuner>();
            tuner.Tune();

            return true;
        }

        private void Issue737()
        {
            var startupRegister = new StartupRegister(LoggerFactory);
            if(startupRegister.Exists()) {
                var result = startupRegister.GetStartupParameter();
                if(result.Success) {
                    if(result.SuccessValue.DelayStartup) {
                        Logger.LogInformation("#737 ????????????");
                        startupRegister.Register(result.SuccessValue);
                    }
                }
            }
        }

        public ManagerViewModel CreateViewModel()
        {
            var viewModel = new ManagerViewModel(this, ApplicationDiContainer.Build<IKeyGestureGuide>(), ApplicationDiContainer.Build<IUserTracker>(), LoggerFactory);
            return viewModel;
        }

        IReadOnlyList<LauncherGroupElement> CreateLauncherGroupElements()
        {
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            IList<Guid> launcherGroupIds;
            using(var context = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<LauncherGroupsEntityDao>(context, context.Implementation);
                launcherGroupIds = dao.SelectAllLauncherGroupIds().ToList();
            }

            var result = new List<LauncherGroupElement>(launcherGroupIds.Count);
            foreach(var launcherGroupId in launcherGroupIds) {
                var element = CreateLauncherGroupElement(launcherGroupId);
                result.Add(element);
            }

            return result;
        }

        IReadOnlyList<LauncherToolbarElement> CreateLauncherToolbarElements(ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
        {
            var screens = Screen.AllScreens;
            var result = new List<LauncherToolbarElement>(screens.Length);

            foreach(var screen in screens.OrderByDescending(i => i.Primary)) {
                var element = CreateLauncherToolbarElement(screen, launcherGroups);
                result.Add(element);
            }

            return result;
        }

        IReadOnlyList<NoteElement> CreateNoteElements()
        {
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            IList<Guid> noteIds;
            using(var context = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<NotesEntityDao>(context, context.Implementation);
                noteIds = dao.SelectAllNoteIds().ToList();
            }

            var result = new List<NoteElement>(noteIds.Count);
            foreach(var noteId in noteIds) {
                var element = CreateNoteElement(noteId, default(IScreen), NoteStartupPosition.Setting);
                result.Add(element);
            }

            return result;
        }

        public ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> GetLauncherNotifyCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel>(LauncherToolbarElements) {
                ToViewModel = m => ApplicationDiContainer.Build<LauncherToolbarNotifyAreaViewModel>(m)
            };
            return collection;
        }

        public ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> GetNoteCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel>(NoteElements) {
                ToViewModel = m => ApplicationDiContainer.Build<NoteNotifyAreaViewModel>(m)
            };
            return collection;
        }

        public ModelViewModelObservableCollectionManagerBase<WidgetElement, WidgetNotifyAreaViewModel> GetWidgetCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<WidgetElement, WidgetNotifyAreaViewModel>(Widgets) {
                ToViewModel = m => ApplicationDiContainer.Build<WidgetNotifyAreaViewModel>(m),
            };
            return collection;
        }

        public NoteElement CreateNote(IScreen dockScreen, NoteStartupPosition noteStartupPosition)
        {
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();
            var noteId = idFactory.CreateNoteId();
            Logger.LogInformation("new note id: {0}, {1}", noteId, ObjectDumper.GetDumpString(dockScreen));
            var noteElement = CreateNoteElement(noteId, dockScreen, noteStartupPosition);

            NoteElements.Add(noteElement);

            return noteElement;
        }

        public void CompactAllNotes()
        {
            var noteItems = WindowManager.GetWindowItems(WindowKind.Note)
                .Select(i => i.ViewModel)
                .Cast<NoteViewModel>()
                .Where(i => !i.IsLocked)
                .Where(i => i.IsVisible)
                .Where(i => !i.IsCompact)
                .ToList()
            ;
            foreach(var note in noteItems) {
                note.ToggleCompactCommand.ExecuteIfCanExecute(null);
            }
        }

        public void MoveZOrderAllNotes(bool isTop)
        {
            if(isTop) {
                var noteElements = NoteElements
                    .Where(i => i.IsVisible)
                    .Where(i => !i.IsTopmost)
                    .ToList()
                ;
                var dispatcherWrapper = ApplicationDiContainer.Get<IDispatcherWrapper>();
                dispatcherWrapper.Begin(() => {
                    foreach(var noteElement in noteElements) {
                        noteElement.SetTopmost(true);
                    }
                }, DispatcherPriority.SystemIdle).ContinueWith(t => {
                    foreach(var noteElement in noteElements) {
                        noteElement.SetTopmost(false);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            } else {
                var noteItems = WindowManager.GetWindowItems(WindowKind.Note)
                    .Where(i => !i.Window.Topmost)
                    .Where(i => i.Window.IsVisible)
                    .ToList()
                ;
                foreach(var noteItem in noteItems) {
                    var hWnd = HandleUtility.GetWindowHandle(noteItem.Window);
                    WindowsUtility.MoveZoderBttom(hWnd);
                }
            }
        }

        void ExecuteElements()
        {
            var currentActiveWindowHandle = NativeMethods.GetActiveWindow();
            //if(currentActiveWindowHandle == IntPtr.Zero) {
            //    currentActiveWindowHandle = NativeMethods.GetForegroundWindow();
            //}

            // ??????????????????
            var launcherGroups = CreateLauncherGroupElements();
            LauncherGroupElements.AddRange(launcherGroups);

            // ????????????????????????
            var launcherToolbars = CreateLauncherToolbarElements(new ReadOnlyObservableCollection<LauncherGroupElement>(LauncherGroupElements));
            LauncherToolbarElements.AddRange(launcherToolbars);

            // ??????????????????
            var notes = CreateNoteElements();
            NoteElements.AddRange(notes);

            var viewShowStaters = Enumerable.Empty<IViewShowStarter>()
                .Concat(notes)
                .Concat(launcherToolbars)
                .Where(i => i.CanStartShowView)
                .ToList()
            ;
            foreach(var viewShowStater in viewShowStaters) {
                viewShowStater.StartView();
            }

            ExecuteWidgets();

#if DEBUG
            if(IsDevDebug) {
                Logger.LogWarning($"{nameof(IsDevDebug)}?????????");
                return;
            }
#endif
            ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                // ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
                if(currentActiveWindowHandle != IntPtr.Zero && currentActiveWindowHandle != MessageWindowHandleSource?.Handle) {
                    WindowsUtility.ShowActive(currentActiveWindowHandle);
                }
                MoveZOrderAllNotes(false);
            }, DispatcherPriority.SystemIdle);
        }

        void ExecuteWidgets()
        {
            //TODO: ???????????????????????????????????????????????????????????????
            if(Widgets.Count == 0) {
                //var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
                var widgetAddonContextFactory = ApplicationDiContainer.Build<WidgetAddonContextFactory>();
                var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
                var mainDatabaseLazyWriter = ApplicationDiContainer.Build<IMainDatabaseLazyWriter>();
                var databaseStatementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();
                var cultureService = ApplicationDiContainer.Build<CultureService>();
                var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
                var dispatcherWrapper = ApplicationDiContainer.Build<IDispatcherWrapper>();

                foreach(var widget in PluginContainer.Addon.GetWidgets()) {
                    var element = new WidgetElement(widget, widget.Addon, widgetAddonContextFactory, mainDatabaseBarrier, mainDatabaseLazyWriter, databaseStatementLoader, cultureService, WindowManager, NotifyManager, environmentParameters, dispatcherWrapper, LoggerFactory);
                    element.Initialize();
                    Widgets.Add(element);
                }
            }

            var showWidgets = new List<WidgetElement>(Widgets.Count);
            using(var context = ApplicationDiContainer.Build<IMainDatabaseBarrier>().WaitRead()) {
                var pluginWidgetSettingsEntityDao = ApplicationDiContainer.Build<PluginWidgetSettingsEntityDao>(context, context.Implementation);
                foreach(var element in Widgets) {
                    if(pluginWidgetSettingsEntityDao.SelectExistsPluginWidgetSetting(element.PluginId)) {
                        var setting = pluginWidgetSettingsEntityDao.SelectPluginWidgetSetting(element.PluginId);
                        if(setting.IsVisible) {
                            showWidgets.Add(element);
                        }
                    }
                }
            }

            // ViewModel?????????????????????????????????????????????????????????????????
            foreach(var element in showWidgets) {
                var viewModel = ApplicationDiContainer.Build<TemporaryWidgetViewModel>(element);
                element.ShowView(viewModel);
            }
        }

        void SaveWidgets()
        {
            foreach(var widget in Widgets.Where(i => i.ViewCreated)) {
                widget.SaveStatus(true);
            }
        }

        void CloseWidgets()
        {
            foreach(var widget in Widgets.Where(i => i.ViewCreated)) {
                widget.HideView();
            }
        }

        void CloseLauncherItemExtensions()
        {
            foreach(var launcherItemExtension in LauncherItemExtensions.Where(i => i.HasView)) {
                launcherItemExtension.CloseView();
            }
        }

        public void Execute()
        {
            Logger.LogInformation("???????????????");
#if DEBUG
            DebugExecuteBefore();
#endif
            InitializeSystem();
            InitializeHook();
            InitializeScheduler();

            StartPlatform();

            ExecuteElements();

#if DEBUG
            DebugExecuteAfter();
#endif
        }

        void CloseViewsCore(WindowKind windowKind)
        {
            var windowItems = WindowManager.GetWindowItems(windowKind).ToList();
            foreach(var windowItem in windowItems) {
                try {
                    if(windowItem.IsOpened) {
                        if(!windowItem.IsClosed) {
                            if(windowItem.Window.IsVisible) {
                                Logger.LogTrace("??????????????????????????????????????????: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                                windowItem.Window.Close();
                            } else {
                                Logger.LogTrace("????????????????????????: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                            }
                        } else {
                            Logger.LogTrace("??????????????????????????????????????????????????????????????????: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                        }
                    } else {
                        Logger.LogTrace("????????????????????????????????????????????????????????????????????????: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                    }
                } catch(System.ComponentModel.Win32Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }
        }

        void CloseLauncherToolbarViews() => CloseViewsCore(WindowKind.LauncherToolbar);

        void CloseNoteViews() => CloseViewsCore(WindowKind.Note);

        void CloseLauncherCustomizeViews() => CloseViewsCore(WindowKind.LauncherCustomize);

        void CloseExtendsExecuteViews() => CloseViewsCore(WindowKind.ExtendsExecute);
        void CloseStandardInputOutputViews() => CloseViewsCore(WindowKind.StandardInputOutput);

        void CloseViews(bool saveWidgets)
        {
            CloseStandardInputOutputViews();
            CloseLauncherCustomizeViews();
            CloseExtendsExecuteViews();
            CloseLauncherToolbarViews();
            CloseNoteViews();

            if(saveWidgets) {
                SaveWidgets();
            }
            CloseWidgets();
            CloseLauncherItemExtensions();
        }

        void DisposeElementsCore<TElement>(ICollection<TElement> elements)
            where TElement : ElementBase
        {
            foreach(var element in elements) {
                element.Dispose();
            }
            elements.Clear();
        }

        void DisposeLauncherToolbarElements() => DisposeElementsCore(LauncherToolbarElements);
        void DisposeLauncherGroupElements() => DisposeElementsCore(LauncherGroupElements);
        void DisposeNoteElements() => DisposeElementsCore(NoteElements);

        void DisposeElements()
        {
            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();
        }

        private void DisposeWebView()
        {
            CefSharp.Cef.Shutdown();
        }

        private void PrepareLatestPlugins()
        {
            var temporaryBarrier = ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>();
            IList<PluginInstallData> installDataItems;
            using(var context = temporaryBarrier.WaitRead()) {
                var installPluginsEntityDao = ApplicationDiContainer.Build<InstallPluginsEntityDao>(context, context.Implementation);
                installDataItems = installPluginsEntityDao.SelectInstallPlugins().ToList();
            }

            if(!installDataItems.Any()) {
                return;
            }

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var pluginMap = PluginContainer.Plugins.ToDictionary(i => i.PluginInformations.PluginIdentifiers.PluginId, i => i);
            var directoryMover = ApplicationDiContainer.Build<DirectoryMover>();
            foreach(var installDataItem in installDataItems) {
                if(installDataItem.PluginInstallMode == PluginInstallMode.New) {
                    // ??????????????????????????????ID????????????????????????????????????????????????
                    var destDirPath = Path.Combine(environmentParameters.MachinePluginInstallDirectory.FullName, PluginUtility.ConvertDirectoryName(installDataItem.PluginId));
                    var srcDir = new DirectoryInfo(installDataItem.PluginDirectoryPath);
                    var destDir = new DirectoryInfo(destDirPath);
                    Logger.LogInformation("????????????????????????: ?????????????????????: {0}, {1} -> {2}", installDataItem.PluginId, srcDir.FullName, destDir.FullName);
                    directoryMover.Move(srcDir, destDir);
                } else {
                    Debug.Assert(installDataItem.PluginInstallMode == PluginInstallMode.Update);
                    // ????????????????????????????????????????????????????????????????????????????????????????????????
                    if(!pluginMap.TryGetValue(installDataItem.PluginId, out var plugin)) {
                        Logger.LogWarning("??????????????????????????????????????????????????????: {0}", installDataItem.PluginId);
                        continue;
                    }

                    var pluginDirPath = Path.GetDirectoryName(plugin.GetType().Assembly.Location)!;
                    var srcDir = new DirectoryInfo(installDataItem.PluginDirectoryPath);
                    var destDir = new DirectoryInfo(pluginDirPath);
                    Logger.LogInformation("????????????????????????: ?????????????????????: {0}, {1} -> {2}", installDataItem.PluginId, srcDir.FullName, destDir.FullName);
                    directoryMover.Move(srcDir, destDir);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreUpdate">???????????????????????????????????????</param>
        public void Exit(bool ignoreUpdate)
        {
            Logger.LogInformation("????????????");

            if(BackgroundAddon != null) {
                var backgroundAddonProxyRunShutdownContext = new BackgroundAddonProxyRunShutdownContext();
                BackgroundAddon.RunShutdown(backgroundAddonProxyRunShutdownContext);
            }

            PrepareLatestPlugins();

            UnloadPlugins();

            BackupSettingsDefault(ApplicationDiContainer);

            if(!ignoreUpdate && ApplicationUpdateInfo.IsReady) {
                Debug.Assert(ApplicationUpdateInfo.Path != null);

                Logger.LogInformation("??????????????????????????????");

                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = ApplicationUpdateInfo.Path.Path;
                process.StartInfo.Arguments = ApplicationUpdateInfo.Path.Option;
                process.StartInfo.WorkingDirectory = ApplicationUpdateInfo.Path.WorkDirectoryPath;

                Logger.LogInformation("path: {0}", process.StartInfo.FileName);
                Logger.LogInformation("args: {0}", process.StartInfo.Arguments);

                try {
                    process.Start();
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }


            StopHook();
            DisposeHook();

            StopScheduler();

            UninitializeSystem();

            CloseViews(true);
            DisposeElements();
            DisposeWebView();

            Dispose();

            Logger.LogInformation("????????????");

            NLog.LogManager.Shutdown();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// TODO: <see cref="ApplicationUpdateScriptFactory.CreateUpdateExecutePathParameter"/> ?????????
        /// </summary>
        public void Reboot()
        {
            Logger.LogInformation("???????????????");

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);

            var powerShellArguments = new PowerShellArguments();
            var psResult = powerShellArguments.GetPowerShellFromCommandName(environmentExecuteFile);
            if(!psResult.Success) {
                Logger.LogError("PowerShell ????????????????????????????????????????????????");
                return;
            }
            var ps = psResult.SuccessValue!;

            var psCommands = powerShellArguments.CreateParameters(true, new[] {
                KeyValuePair.Create("-File", environmentParameters.EtcRebootScriptFile.FullName),
                KeyValuePair.Create("-LogPath", environmentParameters.TemporaryRebootLogFile.FullName),
                KeyValuePair.Create("-ProcessId", Process.GetCurrentProcess().Id.ToString()),
                KeyValuePair.Create("-WaitSeconds", TimeSpan.FromSeconds(10).TotalMilliseconds.ToString()),
                KeyValuePair.Create("-ExecuteCommand", environmentParameters.RootApplication.FullName)
            });
            psCommands.AddRange(powerShellArguments.ConvertOptions());

            var psCommand = string.Join(" ", psCommands);

            Logger.LogInformation("reboot path: {0}", ps);
            Logger.LogInformation("reboot args: {0}", psCommand);

            try {
                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = ps;
                process.StartInfo.Arguments = psCommand;
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.Start();
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            Exit(true);
        }

        public void ShowCommandView()
        {
            if(CommandElement == null) {
                CommandElement = ApplicationDiContainer.Build<CommandElement>();
                CommandElement.Initialize();

                var commandFinders = new ICommandFinder[] {
                    ApplicationDiContainer.Build<LauncherItemCommandFinder>(),
                    ApplicationDiContainer.Build<ApplicationCommandFinder>(CreateApplicationCommandParameters()),
                    PluginContainer.Addon.GetCommandFinder(),
                };

                foreach(var commandFinder in commandFinders) {
                    CommandElement.AddCommandFinder(commandFinder);
                }
                CommandElement.Refresh();
            }

            CommandElement.StartView();
        }

        public void ShowFeedbackView()
        {
            var items = WindowManager.GetWindowItems(WindowKind.Feedback).ToList();
            if(items.Count != 0) {
                foreach(var item in items) {
                    WindowManager.Flash(item);
                }
                return;
            }

            var feedbackElement = ApplicationDiContainer.Build<FeedbackElement>();
            feedbackElement.Initialize();
            var windowItem = OrderManager.CreateFeedbackWindow(feedbackElement);
            WindowManager.Register(windowItem);
            windowItem.Window.Show();
        }

        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        /// <param name="sourceDirectory">?????????????????????????????????????????????</param>
        /// <param name="targetDirectory">??????????????????????????????????????????????????????</param>
        /// <param name="backupFileWithoutExtensionName">??????????????????????????????????????????????????????????????????</param>
        /// <param name="enabledCount">????????????????????????????????????????????????</param>
        /// <param name="userBackupDirectoryPath">???????????????????????????????????????????????????????????????</param>
        void BackupSettings(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, string backupFileWithoutExtensionName, int enabledCount, string userBackupDirectoryPath)
        {
            try {
                // ?????????????????????????????????????????????
                var settingBackupper = new SettingBackupper(LoggerFactory);
                settingBackupper.BackupUserSetting(sourceDirectory, targetDirectory, backupFileWithoutExtensionName, enabledCount);

                // ???????????????????????????????????????
                var expandeduserBackupDirectoryPath = Environment.ExpandEnvironmentVariables(userBackupDirectoryPath ?? string.Empty);
                if(!string.IsNullOrWhiteSpace(expandeduserBackupDirectoryPath)) {
                    var dir = new DirectoryInfo(expandeduserBackupDirectoryPath);
                    settingBackupper.BackupUserSettingToCustomDirectory(sourceDirectory, dir);
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "????????????????????????");
            }
        }

        void BackupSettingsDefault(IDiContainer diContainer)
        {
            var environmentParameters = diContainer.Get<EnvironmentParameters>();

            // ??????????????????????????????
            string userBackupDirectoryPath;
            using(var context = diContainer.Get<IMainDatabaseBarrier>().WaitRead()) {
                var appGeneralSettingEntityDao = diContainer.Build<AppGeneralSettingEntityDao>(context, context.Implementation);
                userBackupDirectoryPath = appGeneralSettingEntityDao.SelectUserBackupDirectoryPath();
            }
            var versionConverter = new VersionConverter();
            ;
            BackupSettings(
                environmentParameters.UserSettingDirectory,
                environmentParameters.UserBackupDirectory,
                DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "_ver." + versionConverter.ConvertDisplayVersion(BuildStatus.Version, "-"),
                environmentParameters.ApplicationConfiguration.Backup.SettingCount,
                userBackupDirectoryPath
            );
        }

        private void ClearScreenViewElements()
        {
            CloseLauncherToolbarViews();
            CloseNoteViews();
            SaveWidgets();
            CloseWidgets();
            //CloseLauncherItemExtensions(); // ????????????????????????????????????

            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();
        }

        private void ResetScreenViewElements()
        {
            ClearScreenViewElements();

            ResetNotifyArea();

            ExecuteElements();
        }

        void ResetNotifyArea()
        {
            var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)Application.Current.FindResource("root");
            var viewModel = notifyIcon.DataContext;
            Logger.LogDebug("???????????????????????????");
            notifyIcon.DataContext = null;
            ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                notifyIcon.DataContext = viewModel;
                Logger.LogDebug("???????????????????????????");
            }, DispatcherPriority.SystemIdle);

        }

        private void DelayResetScreenViewElements()
        {
            void DelayExecuteElements()
            {
                LazyScreenElementReset.DelayAction(() => {
                    ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(ResetScreenViewElements, DispatcherPriority.SystemIdle);
                    ResetWaiting = false;
                });
            }

            if(!ResetWaiting) {
                ResetWaiting = true;
                ClearScreenViewElements();
                DelayExecuteElements();
            } else {
                DelayExecuteElements();
            }
        }

        public async Task DelayCheckUpdateAsync()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            UpdateKind updateKind;
            using(var context = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppUpdateSettingEntityDao>(context, context.Implementation);
                updateKind = dao.SelectSettingUpdateSetting().UpdateKind;
            }

            if(updateKind == UpdateKind.None) {
                return;
            }

            var updateWait = ApplicationDiContainer.Build<ApplicationConfiguration>().General.UpdateWait;
            await Task.Delay(updateWait).ConfigureAwait(false);
            var updateCheckKind = updateKind switch {
                UpdateKind.Notify => UpdateCheckKind.CheckOnly,
                _ => UpdateCheckKind.Update,
            };
            await ExecuteUpdateAsync(updateCheckKind).ConfigureAwait(false);
        }

        public async Task ExecuteUpdateAsync(UpdateCheckKind updateCheckKind)
        {
            if(ApplicationUpdateInfo.State == UpdateState.None || ApplicationUpdateInfo.State == UpdateState.Error) {
                if(ApplicationUpdateInfo.State == UpdateState.Error) {
                    Logger.LogInformation("?????????????????????????????????");
                }
            } else {
                if(ApplicationUpdateInfo.IsReady) {
                    Logger.LogInformation("??????????????????????????????");
                } else {
                    Logger.LogInformation("?????????????????????????????????");
                }

                if(ApplicationUpdateInfo.UpdateItem != null) {
                    ShowUpdateReleaseNote(ApplicationUpdateInfo.UpdateItem, false);
                } else {
                    Logger.LogInformation("?????????????????????????????????");
                }

                return;
            }

            var updateChecker = ApplicationDiContainer.Build<UpdateChecker>();

            ApplicationUpdateInfo.State = UpdateState.Checking;
            {
                var appVersion = await updateChecker.CheckApplicationUpdateAsync().ConfigureAwait(false);
                if(appVersion == null) {
                    Logger.LogInformation("????????????????????????");
                    ApplicationUpdateInfo.State = UpdateState.None;
                    return;
                }
                ApplicationUpdateInfo.UpdateItem = appVersion;
            }

            Logger.LogInformation("????????????????????????: {0}", ApplicationUpdateInfo.UpdateItem.Version);

            // CheckApplicationUpdateAsync ???????????????
            //if(BuildStatus.Version < ApplicationUpdateInfo.UpdateItem.MinimumVersion) {
            //    Logger.LogWarning("????????????????????????????????????????????????????????????????????????: ?????? = {0}, ?????? = {1}", BuildStatus.Version, appVersion.MinimumVersion);
            //    UpdateInfo.State = UpdateState.None;
            //    return;
            //}

            Logger.LogInformation("????????????????????????");

            var updateDownloader = ApplicationDiContainer.Build<UpdateDownloader>();

            if(updateCheckKind != UpdateCheckKind.ForceUpdate) {
                try {
                    ShowUpdateReleaseNote(ApplicationUpdateInfo.UpdateItem, updateCheckKind == UpdateCheckKind.CheckOnly);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    ApplicationUpdateInfo.SetError(ex.Message);
                    return;
                }
            }
            if(updateCheckKind == UpdateCheckKind.CheckOnly) {
                ApplicationUpdateInfo.State = UpdateState.None;
                return;
            }

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var versionConverter = new VersionConverter();
            var downloadFileName = versionConverter.ConvertFileName(BuildStatus.Name, ApplicationUpdateInfo.UpdateItem.Version, ApplicationUpdateInfo.UpdateItem.Platform, ApplicationUpdateInfo.UpdateItem.ArchiveKind);
            var downloadFilePath = Path.Combine(environmentParameters.MachineUpdateArchiveDirectory.FullName, downloadFileName);
            var downloadFile = new FileInfo(downloadFilePath);
            try {
                var skipDownload = false;
                downloadFile.Refresh();
                if(downloadFile.Exists) {
                    ApplicationUpdateInfo.State = UpdateState.Checksumming;
                    skipDownload = await updateDownloader.ChecksumAsync(ApplicationUpdateInfo.UpdateItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.ChecksumProgress, ApplicationUpdateInfo.CurrentLogProgress));
                }
                if(skipDownload) {
                    Logger.LogInformation("???????????????????????????????????????????????????");
                    IProgress<double> progress = ApplicationUpdateInfo.DownloadProgress;
                    progress.Report(1);
                } else {
                    downloadFile.Delete(); // ?????????????????????
                    ApplicationUpdateInfo.State = UpdateState.Downloading;
                    await updateDownloader.DownloadApplicationArchiveAsync(ApplicationUpdateInfo.UpdateItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.DownloadProgress, ApplicationUpdateInfo.CurrentLogProgress)).ConfigureAwait(false);

                    // ?????????????????????????????????????????????????????????????????????????????????
                    downloadFile.Refresh();

                    ApplicationUpdateInfo.State = UpdateState.Checksumming;
                    var checksumOk = await updateDownloader.ChecksumAsync(ApplicationUpdateInfo.UpdateItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.ChecksumProgress, ApplicationUpdateInfo.CurrentLogProgress));
                    if(!checksumOk) {
                        Logger.LogError("??????????????????????????????");
                        ApplicationUpdateInfo.SetError(Properties.Resources.String_Download_ChecksumError);
                        return;
                    }
                }
            } catch(Exception ex) {
                ApplicationUpdateInfo.SetError(ex.Message);
                return;
            }

            Logger.LogInformation("????????????????????????????????????");
            ApplicationUpdateInfo.State = UpdateState.Extracting;
            try {
                var directoryCleaner = new DirectoryCleaner(environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, LoggerFactory);
                directoryCleaner.Clear(false);

                var archiveExtractor = ApplicationDiContainer.Build<ArchiveExtractor>();
                archiveExtractor.Extract(downloadFile, environmentParameters.TemporaryApplicationExtractDirectory, ApplicationUpdateInfo.UpdateItem.ArchiveKind, new UserNotifyProgress(ApplicationUpdateInfo.ExtractProgress, ApplicationUpdateInfo.CurrentLogProgress));

                var scriptFactory = ApplicationDiContainer.Build<ApplicationUpdateScriptFactory>();
                var exeutePathParameter = scriptFactory.CreateUpdateExecutePathParameter(environmentParameters.EtcUpdateScriptFile, environmentParameters.TemporaryDirectory, environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.RootDirectory);
                ApplicationUpdateInfo.Path = exeutePathParameter;
                ApplicationUpdateInfo.State = UpdateState.Ready;

                // ???????????????????????????????????????????????????
                var fileRotation = new FileRotation();
                fileRotation.ExecuteExtensions(
                    environmentParameters.MachineUpdateArchiveDirectory,
                    new[] { "zip", "7z" },
                    environmentParameters.ApplicationConfiguration.Backup.ArchiveCount,
                    ex => {
                        Logger.LogWarning(ex, ex.Message);
                        return true;
                    }
                );

                // ??????????????????????????????
                ShowUpdateReleaseNote(ApplicationUpdateInfo.UpdateItem, false);

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                ApplicationUpdateInfo.SetError(ex.Message);
            }

        }

        internal FileInfo OutputRawCrashReport(Exception exception)
        {
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var versionConverter = new VersionConverter();
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HHmmss_fff'Z'");
            var fileName = versionConverter.ConvertFileName(timestamp, BuildStatus.Version, BuildStatus.Revision, "dmp");
            var filePath = Path.Combine(environmentParameters.TemporaryCrashReportDirectory.FullName, fileName);

            static Dictionary<string, string?> CreateInfoMap(IEnumerable<PlatformInformationItem> items) => items.ToDictionary(k => k.Key, v => Convert.ToString(v.Value));
            void ExceptionWrapper(Action action)
            {
                try {
                    action();
                } catch(Exception ex) {
                    // ???????????????
                    Logger.LogError(ex, ex.Message);
                }
            }

            var rawData = new CrashReportRawData() {
                Version = BuildStatus.Version,
                Revision = BuildStatus.Revision,
                Exception = exception.ToString(),
                Timestamp = DateTime.UtcNow,
            };

            ExceptionWrapper(() => {
                rawData.UserId = ApplicationDiContainer.Get<IMainDatabaseBarrier>().ReadData(c => {
                    var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(c, c.Implementation);
                    var userIdManager = new UserIdManager(LoggerFactory);
                    return userIdManager.SafeGetOrCreateUserId(appExecuteSettingEntityDao);
                });
            });

            string TrimFunc(string s) => s.Substring(3);

            var info = new ApplicationInformationCollector(environmentParameters);
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetApplication))] = CreateInfoMap(info.GetApplication()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetEnvironmentParameter))] = CreateInfoMap(info.GetEnvironmentParameter()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetCPU))] = CreateInfoMap(info.GetCPU()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetOS))] = CreateInfoMap(info.GetOS()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetRuntimeInformation))] = CreateInfoMap(info.GetRuntimeInformation()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetEnvironment))] = CreateInfoMap(info.GetEnvironment()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetEnvironmentVariables))] = CreateInfoMap(info.GetEnvironmentVariables()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetScreen))] = CreateInfoMap(info.GetScreen()));

            // ????????????????????????????????????????????????
            rawData.LogItems = Logging.GetLogItems().Select(i => LogItem.Create(i)).ToList();

            var file = new FileInfo(filePath);
            using(var stream = file.Create()) {
                var serializer = new CrashReportSerializer();
                serializer.Save(rawData, stream);
            }
#if false
            using(var stream = file.Open(FileMode.Open)) {
                var serializer = new CrashReportSerializer();
                var data = serializer.Load<CrashReportRawData>(new KeepStream(stream));
                var diffStream = new MemoryStream();
                serializer.Save(data, new KeepStream(diffStream));
                stream.Position = 0;
                diffStream.Position = 0;
                Debug.Assert(stream.Length == diffStream.Length);
                var s = new byte[stream.Length];
                var d = new byte[diffStream.Length];
                stream.Read(s);
                diffStream.Read(d);
                Debug.Assert(s.SequenceEqual(d));
            }
#endif
            return file;
        }

        internal void ExecuteCrashReport(FileInfo rawReport)
        {
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var saveReportFilePath = Path.Combine(environmentParameters.MachineCrashReportDirectory.FullName, Path.ChangeExtension(rawReport.Name, "json"));

            var currentCommands = Environment.GetCommandLineArgs()
                .Skip(1)
                .Select(i => CommandLine.Escape(i))
                .ToList()
            ;

            var autoSend = ApplicationDiContainer.Get<IMainDatabaseBarrier>().ReadData(c => {
                var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(c, c.Implementation);
                var setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
                return setting.IsEnabledTelemetry;
            });

            var args = new List<string> {
                "--run-mode", "crash-report",
                "--language", System.Globalization.CultureInfo.CurrentCulture.Name,
                "--post-uri", CommandLine.Escape(environmentParameters.ApplicationConfiguration.Api.CrashReportUri.OriginalString),
                "--src-uri", CommandLine.Escape(environmentParameters.ApplicationConfiguration.Api.CrashReportSourceUri.OriginalString),
                "--report-raw-file", CommandLine.Escape(rawReport.FullName),
                "--report-save-file", CommandLine.Escape(saveReportFilePath),
                "--execute-command", CommandLine.Escape(environmentParameters.RootApplication.FullName),
                "--execute-argument", CommandLine.Escape(string.Join(" ", currentCommands)),
            };
            if(autoSend) {
                args.Add("--auto-send");
            }
            args.AddRange(currentCommands);

            var arg = string.Join(' ', args);

            var systemExecutor = new SystemExecutor();
            var commandPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe");
            Logger.LogInformation("path: {0}", commandPath);
            Logger.LogInformation("args: {0}", arg);
            systemExecutor.ExecuteFile(commandPath, arg);
        }

        internal void StartupEnd()
        {
#if DEBUG
            if(!IsDevDebug) {
#endif
                StartHook();
                StartScheduler();
                StartBackground();
#if DEBUG
            }
#endif

            DelayCheckUpdateAsync().ConfigureAwait(false);
#if DEBUG
            DebugStartupEnd();
#endif
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1215:\"GC.Collect\" should not be called")]
        private void GarbageCollection(bool full)
        {
            var old = GC.GetTotalMemory(false);
            var startTimestamp = DateTime.UtcNow;

            if(full) {
                var currentMode = System.Runtime.GCSettings.LargeObjectHeapCompactionMode;
                Logger.LogTrace("LargeObjectHeapCompactionMode: {0}", currentMode);
                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                try {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                } finally {
                    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = currentMode;
                }
            } else {
                GC.Collect(0);
                GC.Collect(1);
            }
            var endTimestamp = DateTime.UtcNow;
            var now = GC.GetTotalMemory(false);
            var sizeConverter = ApplicationDiContainer.Build<Core.Models.SizeConverter>();
            Logger.LogInformation(
                "GC(FULL:{0}): {1}({2}) -> {3}({4}), ??????: {5}({6}), ????????????: {7}",
                full,
                sizeConverter.ConvertHumanReadableByte(old), old,
                sizeConverter.ConvertHumanReadableByte(now), now,
                sizeConverter.ConvertHumanReadableByte(old - now), old - now,
                endTimestamp - startTimestamp
            );
        }

        #endregion

        #region IOrderManager

        public void AddRedoItem(RedoExecutor redoExecutor) => OrderManager.AddRedoItem(redoExecutor);

        public void StartUpdate(UpdateTarget target, UpdateProcess process)
        {
            Debug.Assert(target == UpdateTarget.Application);

            switch(process) {
                case UpdateProcess.Download:
                    ExecuteUpdateAsync(UpdateCheckKind.Update).ConfigureAwait(false);
                    break;

                case UpdateProcess.Update:
                    Exit(false);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId)
        {
            return OrderManager.CreateLauncherGroupElement(launcherGroupId);
        }
        public LauncherToolbarElement CreateLauncherToolbarElement(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
        {
            return OrderManager.CreateLauncherToolbarElement(dockScreen, launcherGroups);
        }

        public LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId)
        {
            return OrderManager.GetOrCreateLauncherItemElement(launcherItemId);
        }
        public void RefreshLauncherItemElement(Guid launcherItemId) => OrderManager.RefreshLauncherItemElement(launcherItemId);

        public LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, IScreen screen)
        {
            return OrderManager.CreateCustomizeLauncherItemContainerElement(launcherItemId, screen);
        }

        public ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen)
        {
            return OrderManager.CreateExtendsExecuteElement(captionName, launcherFileData, launcherEnvironmentVariables, screen);
        }

        public LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, IScreen screen)
        {
            return OrderManager.CreateLauncherExtendsExecuteElement(launcherItemId, screen);
        }


        public NoteElement CreateNoteElement(Guid noteId, IScreen? screen, NoteStartupPosition startupPosition)
        {
            return OrderManager.CreateNoteElement(noteId, screen, startupPosition);
        }
        public bool RemoveNoteElement(Guid noteId)
        {
            var targetElement = NoteElements.FirstOrDefault(i => i.NoteId == noteId);
            if(targetElement == null) {
                Logger.LogWarning("???????????????: ???????????? {0}", noteId);
                return false;
            }

            var entitiesRemover = ApplicationDiContainer.Build<EntitiesRemover>();
            entitiesRemover.Items.Add(new NoteRemover(noteId, LoggerFactory));

            try {
                var reuslt = entitiesRemover.Execute();
                if(reuslt.Sum(i => i.Items.Count) == 0) {
                    Logger.LogWarning("????????????????????????: ?????????????????????: {0}", noteId);
                    return false;
                }
                NoteElements.Remove(targetElement);
                targetElement.Dispose();
                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, "????????????????????????: {0} {1}", ex.Message, noteId);
            }

            return false;
        }

        public NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind)
        {
            return OrderManager.CreateNoteContentElement(noteId, contentKind);
        }

        public SavingFontElement CreateFontElement(DefaultFontKind defaultFontKind, Guid fontId, ParentUpdater parentUpdater)
        {
            return OrderManager.CreateFontElement(defaultFontKind, fontId, parentUpdater);
        }

        /// <inheritdoc cref="IOrderManager.CreateStandardInputOutputElement(string, Process, IScreen)"/>
        public StandardInputOutputElement CreateStandardInputOutputElement(string caption, Process process, IScreen screen)
        {
            var element = OrderManager.CreateStandardInputOutputElement(caption, process, screen);
            StandardInputOutputs.Add(element);
            return element;
        }

        /// <inheritdoc cref="IOrderManager.CreateLauncherItemExtensionElement(IPluginInformations, Guid)"/>
        public LauncherItemExtensionElement CreateLauncherItemExtensionElement(IPluginInformations pluginInformations, Guid launcherItemId)
        {
            var element = OrderManager.CreateLauncherItemExtensionElement(pluginInformations, launcherItemId);
            LauncherItemExtensions.Add(element);
            return element;
        }

        public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
        {
            var windowItem = OrderManager.CreateLauncherToolbarWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element)
        {
            var windowItem = OrderManager.CreateCustomizeLauncherItemWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element)
        {
            var windowItem = OrderManager.CreateExtendsExecuteWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateNoteWindow(NoteElement element)
        {
            var windowItem = OrderManager.CreateNoteWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateCommandWindow(CommandElement element)
        {
            var windowItem = OrderManager.CreateCommandWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }


        public WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element)
        {
            var windowItem = OrderManager.CreateStandardInputOutputWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateNotifyLogWindow(NotifyLogElement element)
        {
            var windowItem = OrderManager.CreateNotifyLogWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }


        public WindowItem CreateSettingWindow(SettingContainerElement element)
        {
            var windowItem = OrderManager.CreateSettingWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }




        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    HeartBeatSender?.Dispose();

                    ApplicationMutex.ReleaseMutex();
                    ApplicationMutex.Dispose();

                    LazyScreenElementReset.Dispose();

                    CloseViews(false);
                    DisposeElements();
                    DisposeWebView();

                    CronScheduler.Dispose();

                    //MessageWindowDispatcherWapper?.Begin(() => {
                    //    MessageWindowHandleSource?.Dispose();
                    //    Dispatcher.CurrentDispatcher.InvokeShutdown();
                    //});
                    MessageWindowHandleSource?.Dispose();

                    NotifyManagerImpl.Dispose();
                    OrderManager.Dispose();

                    WindowManager.Dispose();
                    StatusManagerImpl.Dispose();
                    ClipboardManager.Dispose();
                    UserAgentManager.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void NotifyManagerImpl_LauncherGroupItemRegistered(object? sender, LauncherGroupItemRegisteredEventArgs e)
        {
            var launcherGroupElement = OrderManager.CreateLauncherGroupElement(e.LauncherGroupId);
            LauncherGroupElements.Add(launcherGroupElement);
        }

        private void PlatformThemeLoader_Changed(object? sender, EventArgs e)
        {
            ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                SetDynamicPlatformTheme();
            }, DispatcherPriority.ApplicationIdle);
        }



    }
}
