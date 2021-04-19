using System;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// Formsの<see cref="WinForms.Form"/>をウィンドウとして扱う。
    /// <para>要はウィンドウハンドル欲しい。</para>
    /// </summary>
    public class CompatibleFormWindow: WinForms.IWin32Window, IWindowsHandle
    {
        public CompatibleFormWindow(Window window)
        {
            Handle = HandleUtility.GetWindowHandle(window);
        }

        #region IWin32Window, IWindowsHandle

        /// <summary>
        /// ウィンドウハンドル。
        /// </summary>
        public IntPtr Handle { get; }

        #endregion
    }
}
