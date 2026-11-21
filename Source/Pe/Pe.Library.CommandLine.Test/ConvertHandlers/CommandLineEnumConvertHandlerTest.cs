using System;
using ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test.ConvertHandlers
{
    public class CommandLineEnumConvertHandlerTest
    {
        public enum TestEnum
        {
            None = 0,
            First = 1,
            Second = 7,
        }

        private sealed class EnumOptions
        {
            [CommandLineOption("value", CommandLineOptionKind.Value)]
            public TestEnum Value { get; set; }
        }

        #region function

        [Theory]
        [InlineData(typeof(TestEnum), true)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        public void IsSupportTypeTest(Type type, bool expected)
        {
            var test = new CommandLineEnumConvertHandler();
            Assert.Equal(expected, test.IsSupportType(type));
        }

        [Theory]
        [InlineData(TestEnum.None, "None")]
        [InlineData(TestEnum.First, "First")]
        [InlineData(TestEnum.Second, "Second")]
        [InlineData(TestEnum.First, "first")]
        [InlineData(TestEnum.Second, " SECOND ")]
        public void Convert_Name_Test(TestEnum expected, string input)
        {
            var test = new CommandLineEnumConvertHandler();
            var actual = test.Convert(typeof(TestEnum), input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.None, "0")]
        [InlineData(TestEnum.First, "1")]
        [InlineData(TestEnum.Second, "7")]
        public void Convert_Number_Test(Enum expected, string input)
        {
            var targetType = expected.GetType();
            var test = new CommandLineEnumConvertHandler();
            var actual = test.Convert(targetType, input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Unknown")]
        public void Convert_InvalidValue_Throw_Test(string? input)
        {
            var test = new CommandLineEnumConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(typeof(TestEnum), input));
        }

        [Fact]
        public void Converter_DefaultHandler_Test()
        {
            var converter = new CommandLineConverter(NullLoggerFactory.Instance);
            var actual = converter.Convert<EnumOptions>(
                new CommandLineParser(),
                ["--value", "Second"]
            );

            Assert.Equal(TestEnum.Second, actual.Value);
        }

        #endregion
    }
}
