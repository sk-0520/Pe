using System;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Compatibility.Forms
{
    public class CompatibleFormWindowTest
    {
        #region function

        [UIFact]
        public void Test()
        {
            var wpfWindow = new Window() {
                Opacity = 0,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                ShowInTaskbar = false,
            };
            wpfWindow.Show();
            var fromWindow = new CompatibleFormWindow(wpfWindow);
            Assert.NotEqual(IntPtr.Zero, fromWindow.Handle);
        }

        #endregion
    }
}
