using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Compatibility.Windows
{
    public class HandleUtilityTest
    {
        #region define

        private class TestWindow: Window
        {
            public TestWindow()
                : base()
            {
                Opacity = 0;
                WindowStyle = WindowStyle.None;
                AllowsTransparency = true;
                ShowInTaskbar = false;
            }
        }

        #endregion

        #region function

        [WpfFact]
        public void GetWindowHandleTest()
        {
            var ui = new TestWindow();
            ui.Show();
            var actual = HandleUtility.GetWindowHandle(ui);
            Assert.NotEqual(IntPtr.Zero, actual);
        }

        #endregion
    }
}
