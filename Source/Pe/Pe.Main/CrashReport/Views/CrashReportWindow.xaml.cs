using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Views
{
    /// <summary>
    /// CrashReportWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CrashReportWindow : Window
    {
        public CrashReportWindow()
        {
            InitializeComponent();
            Language = CultureService.Current.GetXmlLanguage();
        }

        #region property

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand CloseCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => Close()
        ));

        #endregion


        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hWnd = HandleUtility.GetWindowHandle(this);
            var style = (int)WindowsUtility.GetWindowLong(hWnd, (int)GWL.GWL_STYLE);
            style &= ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX | WS.WS_SYSMENU);
            WindowsUtility.SetWindowLong(hWnd, (int)GWL.GWL_STYLE, (IntPtr)style);
        }

        #endregion
    }
}
