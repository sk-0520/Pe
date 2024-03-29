using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using Drawing = System.Drawing;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Drawing.Icon"/>の簡易ラッパー。
    /// <para><see cref="System.IDisposable"/>は実装されていないため使用側で<see cref="System.Drawing.Icon"/>の面倒を見ること。</para>
    /// </summary>
    public class IconWrapper: IMakeBitmapSource
    {
        public IconWrapper(Stream stream, IconSize iconSize)
        {
            Icon = new Drawing.Icon(stream, DrawingUtility.Convert(iconSize.ToSize()));
        }

        public IconWrapper(StreamResourceInfo streamInfo, IconSize iconSize)
            : this(streamInfo.Stream, iconSize)
        { }


        #region property

        public Drawing.Icon Icon { get; }

        #endregion

        #region IMakeBitmapSource

        public BitmapSource MakeBitmapSource()
        {
            using(var hIcon = new IconHandleWrapper(Icon.Handle)) {
                return hIcon.MakeBitmapSource();
            }
        }

        #endregion
    }
}
