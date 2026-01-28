using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class MediaUtilityTest
    {
        #region function

        public static TheoryData<uint, Color> ConvertRawColorFromColorData => new() {
            {
                0xff000000,
                Color.FromArgb(0xff, 0x00, 0x00, 0x00)
            },
            {
                0x00ff0000,
                Color.FromArgb(0x00, 0xff, 0x00, 0x00)
            },
            {
                0x0000ff00,
                Color.FromArgb(0x00, 0x00, 0xff, 0x00)
            },
            {
                0x000000ff,
                Color.FromArgb(0x00, 0x00, 0x00, 0xff)
            },
        };

        [Theory]
        [MemberData(nameof(ConvertRawColorFromColorData))]
        public void ConvertRawColorFromColorTest(uint expected, Color color)
        {
            var actual = MediaUtility.ConvertRawColorFromColor(color);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<Color, uint> ConvertColorFromRawColorData => new() {
            {
                Color.FromArgb(0xff, 0x00, 0x00, 0x00),
                0xff000000
            },
            {
                Color.FromArgb(0x00, 0xff, 0x00, 0x00),
                0x00ff0000
            },
            {
                Color.FromArgb(0x00, 0x00, 0xff, 0x00),
                0x0000ff00
            },
            {
                Color.FromArgb(0x00, 0x00, 0x00, 0xff),
                0x000000ff
            },
        };

        [Theory]
        [MemberData(nameof(ConvertColorFromRawColorData))]
        public void ConvertColorFromRawColorTest(Color expected, uint rawColor)
        {
            var actual = MediaUtility.ConvertColorFromRawColor(rawColor);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<Color, Color> GetNegativeColorData => new() {
            {
                Color.FromArgb(0x00, 0x00, 0x00, 0x00),
                Color.FromArgb(0x00, 0xff, 0xff, 0xff)
            },
            {
                Color.FromArgb(0x00, 0xff, 0xff, 0xff),
                Color.FromArgb(0x00, 0x00, 0x00, 0x00)
            },
            {
                Color.FromArgb(0x00, 0x10, 0xff, 0xff),
                Color.FromArgb(0x00, 0xef, 0x00, 0x00)
            },
        };

        [Theory]
        [MemberData(nameof(GetNegativeColorData))]
        public void GetNegativeColorTest(Color expected, Color color)
        {
            var actual = MediaUtility.GetNegativeColor(color);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<Color, Color> GetComplementaryColorData => new() {
            {
                Color.FromArgb(0x00, 0x01, 0x02, 0x03),
                Color.FromArgb(0x00, 0x03, 0x02, 0x01)
            },
            {
                Color.FromArgb(0x80, 0x10, 0x20, 0x30),
                Color.FromArgb(0x80, 0x30, 0x20, 0x10)
            },
            {
                Color.FromArgb(0xff, 0x10, 0x10, 0x10),
                Color.FromArgb(0xff, 0x10, 0x10, 0x10)
            },
        };

        [Theory]
        [MemberData(nameof(GetComplementaryColorData))]
        public void GetComplementaryColorTest(Color expected, Color color)
        {
            var actual = MediaUtility.GetComplementaryColor(color);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<Color, Color> GetAutoColorData => new() {
            {
                Color.FromArgb(MediaUtility.Opaque, 0xff, 0xff, 0xff),
                Color.FromArgb(MediaUtility.Opaque, 0x00, 0x00, 0x00)
            },
            {
                Color.FromArgb(MediaUtility.Opaque, 0x00, 0x00, 0x00),
                Color.FromArgb(MediaUtility.Opaque, 0xff, 0xff, 0xff)
            },
        };

        [Theory]
        [MemberData(nameof(GetAutoColorData))]
        public void GetAutoColorTest(Color expected, Color color)
        {
            var actual = MediaUtility.GetAutoColor(color);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(MediaUtility.Opaque, 0xff, 0xff, 0xff)]
        [InlineData(0xE7, 0xff, 0xff, 0xff)]
        [InlineData(0x00, 0xff, 0xff, 0xff)]
        public void GetOpaqueColorTest(byte a, byte r, byte g, byte b)
        {
            var color = Color.FromArgb(a, r, g, b);

            var actual = MediaUtility.GetOpaqueColor(color);

            Assert.Equal(r, actual.R);
            Assert.Equal(g, actual.G);
            Assert.Equal(b, actual.B);
            Assert.Equal(MediaUtility.Opaque, actual.A);
        }


        #endregion
    }
}
