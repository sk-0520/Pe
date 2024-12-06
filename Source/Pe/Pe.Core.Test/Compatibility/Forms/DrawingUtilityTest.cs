using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Compatibility.Forms
{
    public class DrawingUtilityTest
    {
        #region function

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(2, 2, 2.9, 2.9)]
        public void Convert_SW_Size_Test(int expectedWidth, int expectedHeight, double width, double height)
        {
            var input = new System.Windows.Size(width, height);
            var actual = DrawingUtility.Convert(input);
            Assert.Equal(expectedWidth, actual.Width);
            Assert.Equal(expectedHeight, actual.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(2, 2, 2, 2)]
        public void Convert_SD_Size_Test(double expectedWidth, double expectedHeight, int width, int height)
        {
            var input = new System.Drawing.Size(width, height);
            var actual = DrawingUtility.Convert(input);
            Assert.Equal(expectedWidth, actual.Width);
            Assert.Equal(expectedHeight, actual.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(2, 2, 2.9, 2.9)]
        public void Convert_SW_Point_Test(int expectedX, int expectedY, double x, double y)
        {
            var input = new System.Windows.Point(x, y);
            var actual = DrawingUtility.Convert(input);
            Assert.Equal(expectedX, actual.X);
            Assert.Equal(expectedY, actual.Y);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(2, 2, 2, 2)]
        public void Convert_SD_Point_Test(double expectedX, double expectedY, int x, int y)
        {
            var input = new System.Drawing.Point(x, y);
            var actual = DrawingUtility.Convert(input);
            Assert.Equal(expectedX, actual.X);
            Assert.Equal(expectedY, actual.Y);
        }

        public static TheoryData<System.Drawing.Rectangle, System.Windows.Rect> Convert_SW_Rect_Data => new() {
            {
                new (),
                new ()
            },
            {
                new (1, 2, 3, 4),
                new (1.9, 2.1, 3.9, 4.1)
            },
        };

        [Theory]
        [MemberData(nameof(Convert_SW_Rect_Data))]
        public void Convert_SW_Rect_Test(System.Drawing.Rectangle expected, System.Windows.Rect rect)
        {
            var actual = DrawingUtility.Convert(rect);
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.Height, actual.Height);
        }

        public static TheoryData<System.Windows.Rect, System.Drawing.Rectangle> Convert_SD_Rectangle_Data => new() {
            {
                new (),
                new ()
            },
            {
                new (1, 2, 3, 4),
                new (1, 2, 3, 4)
            },
        };

        [Theory]
        [MemberData(nameof(Convert_SD_Rectangle_Data))]
        public void Convert_SD_Rectangle_Test(System.Windows.Rect expected, System.Drawing.Rectangle rectangle)
        {
            var actual = DrawingUtility.Convert(rectangle);
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.Height, actual.Height);
        }


        #endregion
    }
}
