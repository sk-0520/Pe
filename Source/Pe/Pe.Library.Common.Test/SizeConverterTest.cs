using System;
using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class SizeConverterTest
    {
        #region function

        [Theory]
        [InlineData("0 byte", 0, "{0} {1}", new[] { "byte" })]
        [InlineData("0.00 byte", 0, "{0:0.00} {1}", new[] { "byte" })]
        [InlineData("1023.00 byte", 1023, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1.00 KB", 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1024.00 KB", 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1048576.00 KB", 1024 * 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1.00 MB", 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB", "MB" })]
        [InlineData("1024.00 MB", 1024 * 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB", "MB" })]
        public void ConvertHumanReadableByte_3_Test(string expected, long byteSize, string sizeFormat, IReadOnlyList<string> units)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, sizeFormat, units);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertHumanReadableByte_Throw_Test()
        {
            var sc = new SizeConverter() {
                Units = []
            };
            Assert.Throws<ArgumentException>(() => sc.ConvertHumanReadableByte(1024));
        }

        #endregion
    }
}
