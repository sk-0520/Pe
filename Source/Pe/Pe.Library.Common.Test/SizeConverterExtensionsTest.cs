using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class SizeConverterExtensionsTest
    {
        #region function

        [Theory]
        [InlineData("0.00 byte", 0, new[] { "byte" })]
        [InlineData("1023.00 byte", 1023, new[] { "byte", "KB" })]
        [InlineData("1.00 KB", 1024, new[] { "byte", "KB" })]
        public void ConvertHumanReadableByte_2_1_Test(string expected, long byteSize, IReadOnlyList<string> units)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, units);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0.00 byte", 0, "{0:0.00} {1}")]
        [InlineData("1023.00 byte", 1023, "{0:0.00} {1}")]
        [InlineData("1.00 KB", 1024, "{0:0.00} {1}")]
        public void ConvertHumanReadableByte_2_2_Test(string expected, long byteSize, string sizeFormat)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, sizeFormat);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0.00 byte", 0)]
        [InlineData("1023.00 byte", 1023)]
        [InlineData("1.00 KB", 1024)]
        public void ConvertHumanReadableByte_1_Test(string expected, long byteSize)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize);
            Assert.Equal(expected, actual);
        }


        #endregion
    }
}
