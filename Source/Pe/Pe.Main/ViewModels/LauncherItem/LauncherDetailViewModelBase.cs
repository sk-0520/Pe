using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Mvvm.Commands;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public interface ILauncherFilePath
    {
        #region property

        bool ExistsPath { get; }
        bool CanExecutePath { get; }
        bool CanCopyPath { get; }

        bool ExistsParentDirectory { get; }
        bool CanOpenParentDirectory { get; }
        bool CanCopyParentDirectory { get; }

        bool CanCopyOption { get; }

        #endregion
    }

    public interface ILauncherWorkingDirectoryPath
    {
        #region property

        bool ExistsWorkingDirectory { get; }
        bool CanOpenWorkingDirectory { get; }
        bool CanCopyWorkingDirectory { get; }

        #endregion
    }

    public abstract class LauncherDetailViewModelBase: SingleModelViewModelBase<LauncherItemElement>, ILauncherItemId
    {
        #region define

        protected enum IconKind
        {
            Main,
            Tooltip,
        }

        #endregion

        #region variable

        private bool _nowLoading;
        private bool _nowMainExecuting;

        private object? _mainIcon;
        private object? _tooltipIcon;

        #endregion

        protected LauncherDetailViewModelBase(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IContextDispatcher contextDispatcher, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Screen = screen;
            KeyGestureGuide = keyGestureGuide;
            ContextDispatcher = contextDispatcher;
            LauncherToolbarTheme = launcherToolbarTheme;
        }

        #region property

        private ICommandFactory CommandFactory { get; } = new CommandFactory();
        protected IScreen Screen { get; }
        protected IKeyGestureGuide KeyGestureGuide { get; }
        protected IContextDispatcher ContextDispatcher { get; }
        protected ILauncherToolbarTheme LauncherToolbarTheme { get; }
        public object MainIcon => this._mainIcon ??= GetIcon(IconKind.Main, true);
        public object TooltipIcon => this._tooltipIcon ??= GetIcon(IconKind.Tooltip, false);

        public virtual string? Name => Model.Name;
        public string? Comment => Model.Comment;
        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        public bool NowLoading
        {
            get => this._nowLoading;
            private set => SetProperty(ref this._nowLoading, value);
        }

        public bool NowMainExecuting
        {
            get => this._nowMainExecuting;
            set => SetProperty(ref this._nowMainExecuting, value);
        }


        protected virtual bool CanExecuteMain => true;

        public ObservableCollection<string>? ExecuteKeyGestures { get; private set; }
        public bool HasExecuteKeyGestures { get; set; }

        #endregion

        #region command

        private ICommand? _LoadCommand;
        public ICommand LoadCommand => this._LoadCommand ??= CommandFactory.Create(
            async () => {
                await LoadAsync(CancellationToken.None);
            }
        );

        private ICommand? _UnloadCommand;
        public ICommand UnloadCommand => this._UnloadCommand ??= CommandFactory.Create(
            () => {
                UnloadAsync(CancellationToken.None);
            }
        );

        private ICommand? _ExecuteMainCommand;
        public ICommand ExecuteMainCommand => this._ExecuteMainCommand ??= CommandFactory.Create(
            () => {
                _ = ExecuteMainAsync(CancellationToken.None);
            },
            () => CanExecuteMain
        );

        private ICommand? _CustomizeCommand;
        public ICommand CustomizeCommand => this._CustomizeCommand ??= CommandFactory.Create(
            async () => {
                await Model.OpenCustomizeViewAsync(Screen, CancellationToken.None);
            }
        );

        #endregion

        #region function

        protected abstract Task LoadImplAsync(CancellationToken cancellationToken);
        protected abstract Task UnloadImplAsync(CancellationToken cancellationToken);

        private async Task LoadAsync(CancellationToken cancellationToken)
        {
            NowLoading = true;

            await LoadImplAsync(cancellationToken);

            if(Model is null) {
                Logger.LogDebug("Model is null.");
                return;
            }

            var keys = KeyGestureGuide.GetLauncherItemKeys(LauncherItemId);
            ExecuteKeyGestures = new ObservableCollection<string>(keys);
            HasExecuteKeyGestures = ExecuteKeyGestures.Any();

            RaisePropertyChanged(nameof(HasExecuteKeyGestures));
            RaisePropertyChanged(nameof(ExecuteKeyGestures));

            NowLoading = false;
        }

        private Task UnloadAsync(CancellationToken cancellationToken)
        {
            return UnloadImplAsync(cancellationToken);
        }

        protected abstract Task ExecuteMainImplAsync(CancellationToken cancellationToken);

        protected async Task ExecuteMainAsync(CancellationToken cancellationToken)
        {
            if(CanExecuteMain) {
                NowMainExecuting = true;
                try {
                    await ExecuteMainImplAsync(cancellationToken);
                } finally {
                    NowMainExecuting = false;
                }
            }
        }

        protected abstract object GetIcon(IconKind iconKind, bool isEnabledBadge);

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(this._mainIcon is IDisposable mainIcon) {
                        mainIcon.Dispose();
                        this._mainIcon = null;
                    }
                    if(this._tooltipIcon is IDisposable tooltipIcon) {
                        tooltipIcon.Dispose();
                        this._tooltipIcon = null;
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId => Model.LauncherItemId;

        #endregion
    }
}
