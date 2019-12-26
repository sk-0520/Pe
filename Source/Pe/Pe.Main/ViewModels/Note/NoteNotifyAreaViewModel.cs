using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Theme;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteNotifyAreaViewModel : SingleModelViewModelBase<NoteElement>, INotifyArea
    {
        public NoteNotifyAreaViewModel(NoteElement model, IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            WindowManager = windowManager;
            DispatcherWrapper = dispatcherWrapper;
            NoteTheme = noteTheme;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
        }

        #region property

        IWindowManager WindowManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        INoteTheme NoteTheme { get; }

        PropertyChangedHooker PropertyChangedHooker { get; }

        public bool IsVisible => Model.IsVisible;

        #endregion

        #region command
        #endregion

        #region function
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

        #endregion

        #region INotifyArea

        public string MenuHeader => Model.Title ?? "<からもじれつ>";
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture? MenuKeyGesture { get; }
        public DependencyObject? MenuIcon => DispatcherWrapper.Get(() => NoteTheme.GetIconImage(IconBox.Small, Model.IsCompact, Model.IsLocked, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor)));
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked { get; } = false;

        public ICommand MenuCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(IsVisible) {
                    var target = WindowManager.GetWindowItems(WindowKind.Note)
                       .First(i => ((NoteViewModel)i.ViewModel).NoteId == Model.NoteId)
                    ;
                    var hWnd = HandleUtility.GetWindowHandle(target.Window);
                    WindowsUtility.ShowActive(hWnd);
                    //target.Window.Activate();
                } else {
                    Model.ChangeVisibleDelaySave(true);
                    Model.StartView();
                }
            },
            () => MenuIsEnabled
        ));

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }


    }
}
