using System;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi
{
    /// <summary>
    /// Windows 万歳な GDI 系オブジェクトを扱う。
    /// </summary>
    public abstract class SafeGdiBase: SafeHandle, IMakeBitmapSource
    {
        protected SafeGdiBase(IntPtr hHandle)
            : base(hHandle, true)
        { }

        #region property

        public IntPtr ResourceHandle => this.handle;

        public virtual bool CanMakeImageSource => false;

        #endregion

        #region function

        protected abstract BitmapSource MakeBitmapSourceCore();

        protected void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(IsInvalid, this);
        }

        #endregion

        #region IMakeBitmapSource

        /// <summary>
        /// GDIオブジェクトから<see cref="BitmapSource"/>作成。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><see cref="CanMakeImageSource"/></exception>
        public BitmapSource MakeBitmapSource()
        {
            if(!CanMakeImageSource) {
                throw new NotSupportedException();
            }

            ThrowIfDisposed();

            return MakeBitmapSourceCore();
        }

        #endregion

        #region SafeHandle

        public sealed override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            return NativeMethods.DeleteObject(this.handle);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(!IsInvalid) {
                this.handle = nint.Zero;
            }
        }

        #endregion
    }
}
