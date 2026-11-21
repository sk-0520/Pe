using System;
using ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test.ConvertHandlers
{
    public class CommandLineStringConvertHandlerTest
    {
        #region function

        [Theory]
        [InlineData(typeof(string))]
        public void IsSupportTypeTest(Type type)
        {
            var test = new CommandLineStringConvertHandler();
            Assert.True(test.IsSupportType(type));
        }

        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void ConvertTest(string expected, string? input)
        {
            var test = new CommandLineStringConvertHandler();
            var actual = test.Convert(typeof(string), input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
