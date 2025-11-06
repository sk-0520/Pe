using System;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi
{
    public class IconHandle: GdiBase
    {
        public IconHandle(IntPtr hIcon)
            : base(hIcon)
        { }

        #region GdiBase

        public override bool CanMakeImageSource => true;

        protected override BitmapSource MakeBitmapSourceCore()
        {
            var result = Imaging.CreateBitmapSourceFromHIcon(
                this.handle,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return result;
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.DestroyIcon(this.handle);
        }

        #endregion
    }
}
