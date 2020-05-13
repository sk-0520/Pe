using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Manager
{
    internal class ManagerViewModel: ViewModelBase, IBuildStatus
    {
        #region variable

        bool _isOpenNoteMenu;
        bool _isOpenSystemMenu;
        bool _isOpenContextMenu;
        bool _isEnabledManager = true;

        #endregion

        public ManagerViewModel(ApplicationManager applicationManager, IUserTracker userTracker, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ApplicationManager = applicationManager;
            UserTracker = userTracker;

            LauncherToolbarCollection = ApplicationManager.GetLauncherNotifyCollection();
            LauncherToolbarItems = LauncherToolbarCollection.ViewModels;

            NoteCollection = ApplicationManager.GetNoteCollection();
            NoteVisibleItems = NoteCollection.CreateView();
            NoteHiddenItems = NoteCollection.CreateView();
            NoteVisibleItems.Filter = o => ((NoteNotifyAreaViewModel)o).IsVisible;
            NoteHiddenItems.Filter = o => !((NoteNotifyAreaViewModel)o).IsVisible;

            ApplicationManager.StatusManager.StatusChanged += StatusManager_StatusChanged;

        }

        #region property

        // 月初だけ windows アイコンを古くする(理由はない)
        public bool ShowPlatformOldVersion => DateTime.Now.Day == 1;

        ApplicationManager ApplicationManager { get; }
        IUserTracker UserTracker { get; }

        ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> LauncherToolbarCollection { get; }
        public ReadOnlyObservableCollection<LauncherToolbarNotifyAreaViewModel> LauncherToolbarItems { get; }

        ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> NoteCollection { get; }
        public ICollectionView NoteVisibleItems { get; }
        public ICollectionView NoteHiddenItems { get; }


        public bool IsOpenNoteMenu
        {
            get => this._isOpenNoteMenu;
            set
            {
                if(SetProperty(ref this._isOpenNoteMenu, value)) {
                    if(IsOpenNoteMenu) {
                        NoteVisibleItems.Refresh();
                        NoteHiddenItems.Refresh();
                    }
                }
            }
        }

        public bool IsOpenSystemMenu
        {
            get => this._isOpenSystemMenu;
            set
            {
                SetProperty(ref this._isOpenSystemMenu, value);
                if(IsOpenSystemMenu) {
                    RaisePropertyChanged(nameof(IsEnabledHook));
                }
            }
        }

        public bool IsEnabledHook
        {
            get => ApplicationManager.IsEnabledHook;
        }

        public bool IsDisabledSystemIdle => ApplicationManager.IsDisabledSystemIdle;
        public bool IsSupportedExplorer => ApplicationManager.IsSupportedExplorer;

        public bool IsEnabledManager
        {
            get => this._isEnabledManager;
            set => SetProperty(ref this._isEnabledManager, value);
        }

        public bool IsOpenContextMenu
        {
            get => this._isOpenContextMenu;
            set
            {
                if(SetProperty(ref this._isOpenContextMenu, value)) {
                    if(IsOpenContextMenu) {
                        RaisePropertyChanged(nameof(ShowPlatformOldVersion));
                    }
                }
                Logger.LogDebug("[#530調査] IsOpenContextMenu: {0}", IsOpenContextMenu);
            }
        }


        public IReadOnlyUpdateInfo UpdateInfo => ApplicationManager.ApplicationUpdateInfo;
        #endregion

        #region command


        public ICommand CreateNoteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                NativeMethods.GetCursorPos(out var rawCursorPosition);
                var deviceCursorPosition = PodStructUtility.Convert(rawCursorPosition);
                var currentScreen = Screen.FromDevicePoint(deviceCursorPosition);

                var noteElement = ApplicationManager.CreateNote(currentScreen, Models.Data.NoteStartupPosition.CenterScreen);
                noteElement.StartView();
            }
        ));

        public ICommand CompactAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.CompactAllNotes();
            }
        ));
        public ICommand MoveZOrderTopAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.MoveZOrderAllNotes(true);
             }
         ));

        public ICommand MoveZOrderBottomAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.MoveZOrderAllNotes(false);
            }
        ));

        public ICommand ShowCommandViewCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => {
               ApplicationManager.ShowCommandView();
           }
       ));

        public ICommand OpenSettingCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowSettingView();
            }
        ));
        public ICommand OpenStartupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowStartupView(false);
            }
        ));
        public ICommand OpenHelpCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ShowHelp();
            }
        ));

        public ICommand OpenAboutCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowAboutView();
            }
        ));

        public ICommand UpdateCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.ExecuteUpdateAsync(Models.Data.UpdateCheckKind.CheckOnly).ConfigureAwait(false);
             }
         ));

        public ICommand ToggleHookCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => {
               ApplicationManager.ToggleHook();
               RaisePropertyChanged(nameof(IsEnabledHook));
           }
        ));
        public ICommand ToggleDisabledSystemIdleCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ToggleDisableSystemIdle();
                RaisePropertyChanged(nameof(IsDisabledSystemIdle));
            }
        ));

        public ICommand ToggleSupportExplorerCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ToggleSupportExplorer();
                RaisePropertyChanged(nameof(IsSupportedExplorer));
            }
        ));


        public ICommand ExitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.Exit(false);
            }
        ));

        public ICommand NoUpdateExitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.Exit(true);
            }
        ));

        public ICommand ShowFeedbackViewCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ShowFeedbackView();
            }
        ));


        #endregion

        #region function
        #endregion

        #region IBuildStatus

        public BuildType BuildType => BuildStatus.BuildType;

        public Version Version => BuildStatus.Version;
        public string Revision => BuildStatus.Revision;

        #endregion

        private void StatusManager_StatusChanged(object? sender, StatusChangedEventArgs e)
        {
            if(e.StatusProperty == StatusProperty.CanCallNotifyAreaMenu) {
                IsEnabledManager = (bool)e.NewValue!;
                Logger.LogDebug("[#530調査] IsEnabledManager: {0}", IsEnabledManager);
            }
        }

    }
}
