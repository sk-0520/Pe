using System;
using ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test.ConvertHandlers
{
    public class CommandLineBooleanConvertHandlerTest
    {
        #region function

        [Theory]
        [InlineData(typeof(bool))]
        public void IsSupportTypeTest(Type type)
        {
            var test = new CommandLineBooleanConvertHandler();
            Assert.True(test.IsSupportType(type));
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(false, "")]
        [InlineData(false, " ")]
        [InlineData(false, "false")]
        [InlineData(true, "true")]
        [InlineData(true, " true ")]
        public void ConvertTest(bool expected, string? input)
        {
            var test = new CommandLineBooleanConvertHandler();
            var actual = test.Convert(typeof(bool), input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineBooleanConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(null!, "yes"));
        }

        #endregion
    }
}
