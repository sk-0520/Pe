﻿using System;
using System.Drawing;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Unmanaged.Gdi
{
    public class IconHandleWrapperTest
    {
        #region function

        [UIFact]
        public void Test()
        {
            using var gdiBitmap = new Bitmap(32, 32);
            var test = new IconHandleWrapper(gdiBitmap.GetHicon());
            Assert.True(test.CanMakeImageSource);
            Assert.False(test.IsInvalid);

            var image = test.MakeBitmapSource();
            Assert.Equal(gdiBitmap.Width, image.Width);
            Assert.Equal(gdiBitmap.Height, image.Height);

            test.Dispose();
            Assert.Throws<ObjectDisposedException>(() => test.MakeBitmapSource());
        }

        #endregion
    }
}
