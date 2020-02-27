using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Main.Views.Command;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class CommandViewModel : ElementViewModelBase<CommandElement>, IViewLifecycleReceiver
    {
        #region variable

        bool _isOpend;
        CommandItemViewModel? _currentSelectedItem;
        CommandItemViewModel? _selectedItem;
        string _inputValue = string.Empty;
        InputState _inputState;
        //bool _isActive;

        #endregion

        public CommandViewModel(CommandElement model, IGeneralTheme generalTheme, ICommandTheme commandTheme, IPlatformTheme platformTheme, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
            CommandTheme = commandTheme;
            PlatformTheme = platformTheme;

            ThemeProperties = new ThemeProperties(this);

            CommandItemCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<ICommandItem>, CommandItemViewModel>(Model.CommandItems) {
                ToViewModel = m => new CommandItemViewModel(m.Data, IconBox, DispatcherWrapper, LoggerFactory),
            };
            CommandItems = CommandItemCollection.GetDefaultView();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);

            HideWaitTimer = new DispatcherTimer(DispatcherPriority.Normal) {
                Interval = Model.HideWaitTime,
            };
            HideWaitTimer.Tick += HideWaitTimer_Tick;

            PlatformTheme.Changed += PlatformTheme_Changed;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
        }


        #region property
        public RequestSender ScrollSelectedItemRequest { get; } = new RequestSender();

        IGeneralTheme GeneralTheme { get; }
        ICommandTheme CommandTheme { get; }
        IPlatformTheme PlatformTheme { get; }

        DispatcherTimer HideWaitTimer { get; }

        ModelViewModelObservableCollectionManagerBase<WrapModel<ICommandItem>, CommandItemViewModel> CommandItemCollection { get; }
        public ICollectionView CommandItems { get; }

        public CommandItemViewModel? CurrentSelectedItem
        {
            get => this._currentSelectedItem;
            set => SetProperty(ref this._currentSelectedItem, value);
        }

        public CommandItemViewModel? SelectedItem
        {
            get => this._selectedItem;
            set
            {
                SetProperty(ref this._selectedItem, value);
                if(SelectedItem != null) {
                    CurrentSelectedItem = SelectedItem;
                }
            }
        }

        ThemeProperties ThemeProperties { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        IDpiScaleOutputor DpiScaleOutputor { get; set; } = new EmptyDpiScaleOutputor();
        TextBox? InputCommand { get; set; }

        public double WindowWidth
        {
            get => Model.Width;
            set => Model.ChangeViewWidthDelaySave(value);
        }

        public bool IsOpend
        {
            get => this._isOpend;
            set => SetProperty(ref this._isOpend, value);
        }

        public IconBox IconBox => Model.IconBox;

        CancellationTokenSource? InputCancellationTokenSource { get; set; }

        public string InputValue
        {
            get => this._inputValue;
            set
            {
                CurrentSelectedItem = SelectedItem;
                SetProperty(ref this._inputValue, value);

                if(InputCancellationTokenSource != null) {
                    Logger.LogDebug("入力中の何かしらをキャンセル");
                    InputCancellationTokenSource.Cancel();
                }

                InputCancellationTokenSource = new CancellationTokenSource();

                var isEmpty = string.IsNullOrWhiteSpace(this._inputValue);
                if(isEmpty) {
                    InputState = InputState.Empty;
                } else {
                    InputState = InputState.Finding;
                }


                Model.UpdateCommandItemsAsync(this._inputValue, InputCancellationTokenSource.Token).ContinueWith(t => {
                    if(t.IsCanceled) {
                        Logger.LogTrace("cancel!");
                    } else if(t.IsCompletedSuccessfully) {
                        InputCancellationTokenSource?.Dispose();
                        InputCancellationTokenSource = null;

                        SelectedItem = CommandItemCollection.ViewModels.FirstOrDefault();
                        if(SelectedItem == null && !string.IsNullOrWhiteSpace(this._inputValue)) {
                            CurrentSelectedItem = null;
                            InputState = InputState.NotFound;
                        } else if(!isEmpty) {
                            InputState = InputState.Listup;
                        }
                    }
                });
            }
        }

        public FontViewModel Font { get; private set; }

        public InputState InputState
        {
            get => this._inputState;
            set => SetProperty(ref this._inputState, value);
        }

        #region theme

        [ThemeProperty]
        public Thickness ViewBorderThickness => CommandTheme.GetViewBorderThickness();
        [ThemeProperty]
        public Thickness ViewResizeThickness
        {
            get
            {
                var thickness = CommandTheme.GetViewBorderThickness();
                thickness.Top = 0;
                thickness.Bottom = 0;
                return thickness;
            }
        }
        [ThemeProperty]
        public Brush ViewActiveBorderBrush => CommandTheme.GetViewBorderBrush(true);
        [ThemeProperty]
        public Brush ViewInactiveBorderBrush => CommandTheme.GetViewBorderBrush(false);

        [ThemeProperty]
        public Brush ViewActiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(true);
        [ThemeProperty]
        public Brush ViewInactiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(false);

        [ThemeProperty]
        public double GripWidth => CommandTheme.GetGripWidth();
        [ThemeProperty]
        public Brush GripActiveBrush => CommandTheme.GetGripBrush(true);
        [ThemeProperty]
        public Brush GripInactiveBrush => CommandTheme.GetGripBrush(false);

        [ThemeProperty]
        public Thickness SelectedIconMargin => CommandTheme.GetSelectedIconMargin(IconBox);

        [ThemeProperty]
        public Thickness InputBorderThickness => CommandTheme.GetInputBorderThickness();

        [ThemeProperty]
        public Brush InputEmptyBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Finding);
        [ThemeProperty]
        public Brush InputListupBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Listup);
        [ThemeProperty]
        public Brush InputNotFoundBorderBrush => CommandTheme.GetInputBorderBrush(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyBackground => CommandTheme.GetInputBackground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBackground => CommandTheme.GetInputBackground(InputState.Finding);
        [ThemeProperty]
        public Brush InputListupBackground => CommandTheme.GetInputBackground(InputState.Listup);
        [ThemeProperty]
        public Brush InputNotFoundBackground => CommandTheme.GetInputBackground(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyForeground => CommandTheme.GetInputForeground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingForeground => CommandTheme.GetInputForeground(InputState.Finding);
        [ThemeProperty]
        public Brush InputListupForeground => CommandTheme.GetInputForeground(InputState.Listup);
        [ThemeProperty]
        public Brush InputNotFoundForeground => CommandTheme.GetInputForeground(InputState.NotFound);

        [ThemeProperty]
        public ControlTemplate ExecuteButtonControlTemplate => CommandTheme.GetExecuteButtonControlTemplate(IconBox);

        #endregion

        #endregion

        #region command

        public ICommand ExecuteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Logger.LogInformation("コマンドアイテムの起動: {0}", SelectedItem!.Header);
                SelectedItem.Execute(DpiScaleOutputor.GetOwnerScreen());

                // 役目は終わったのでコマンドランチャーを閉じる
                Model.HideView(false);
                InputValue = string.Empty;
            },
            () => SelectedItem != null
        ).ObservesProperty(() => SelectedItem));

        public ICommand UpSelectItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UpDownSelectItem(true);
            }
        ));

        public ICommand DownSelectItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UpDownSelectItem(false);
            }
        ));

        public ICommand ViewActivatedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                HideWaitTimer.Stop();
            }
        ));
        public ICommand ViewDeactivatedCommand => GetOrCreateCommand(() => new DelegateCommand<Window>(
            o => {
                if(o.IsVisible) {
                    HideWaitTimer.Stop();
                    HideWaitTimer.Start();
                }
            }
        ));

        #endregion

        #region function

        void UpDownSelectItem(bool isUp)
        {
            if(CommandItemCollection.ViewModels.Count == 0) {
                Logger.LogTrace("列挙アイテムなし");
                return;
            }

            if(SelectedItem == null) {
                // 多分ここには来ないはずだけど一応
                SelectedItem = isUp
                    ? CommandItemCollection.ViewModels.First()
                    : CommandItemCollection.ViewModels.Last()
                ;
            } else {
                var index = CommandItemCollection.ViewModels.IndexOf(SelectedItem);
                if(isUp) {
                    SelectedItem = index == 0
                        ? CommandItemCollection.ViewModels.Last()
                        : CommandItemCollection.ViewModels[index - 1]
                    ;
                } else {
                    SelectedItem = index == CommandItemCollection.ViewModels.Count - 1
                        ? CommandItemCollection.ViewModels[0]
                        : CommandItemCollection.ViewModels[index + 1]
                    ;
                }
            }

            ScrollSelectedItemRequest.Send();
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            DpiScaleOutputor = (IDpiScaleOutputor)window;
        }

        public void ReceiveViewLoaded(Window window)
        {
            Model.UpdateCommandItemsAsync(string.Empty, CancellationToken.None).ContinueWith(t => {
                SelectedItem = CommandItemCollection.ViewModels.FirstOrDefault();
            });
        }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
            Model.HideView(false);
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
            HideWaitTimer.Stop();
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PlatformTheme.Changed -= PlatformTheme_Changed;
                    PropertyChangedHooker.Dispose();
                    CommandItemCollection.Dispose();
                    Font.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.Begin(() => {
                foreach(var themePropertyName in ThemeProperties.GetPropertyNames()) {
                    RaisePropertyChanged(themePropertyName);
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

        private void HideWaitTimer_Tick(object? sender, EventArgs e)
        {
            Model.HideView(false);
            InputValue = string.Empty;
            HideWaitTimer.Stop();
        }


    }
}