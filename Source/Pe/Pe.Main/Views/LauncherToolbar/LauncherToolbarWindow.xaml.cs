using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherToolbar
{
    /// <summary>
    /// LauncherToolbarWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherToolbarWindow: Window
    {
        public LauncherToolbarWindow()
        {
            InitializeComponent();
        }

        #region property

        [Inject]
        ILogger? Logger { get; set; }
        LauncherToolbarViewModel ViewModel => (LauncherToolbarViewModel)DataContext;

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        ICommand? _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return this._CloseCommand ?? (this._CloseCommand = new DelegateCommand<RequestEventArgs>(
                    o => {
                        Close();
                    }
                ));
            }
        }

        public ICommand OpenCommonMessageDialogCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                var result = MessageBox.Show(this, parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
                var response = new YesNoResponse();
                switch(result) {
                    case MessageBoxResult.Yes:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = true;
                        break;

                    case MessageBoxResult.No:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = false;
                        break;

                    default:
                        response.ResponseIsCancel = true;
                        response.ResponseIsYes = false;
                        break;
                }

                o.Callback(response);
            }
        ));

        #endregion

        #region function

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);
        }

        #endregion


        private void LauncherContentControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(!ViewModel.IsVerticalLayout) {
                this.scrollViewer.ScrollToHorizontalOffset(this.scrollViewer.HorizontalOffset + -e.Delta);
                e.Handled = true;
            }
        }
    }
}
