using System;
using ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test.ConvertHandlers
{
    public class CommandLineIntegerConvertHandlerTest
    {
        #region function

        [Theory]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(int))]
        public void IsSupportTypeTest(Type type)
        {
            var test = new CommandLineIntegerConvertHandler();
            Assert.True(test.IsSupportType(type));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(0, "")]
        [InlineData(0, " ")]
        [InlineData(-1, "-1")]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(42, " 42 ")]
        public void ConvertTest(int expected, string? input)
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(int), input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Byte_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(byte), "255");
            Assert.Equal((byte)255, actual);
        }

        [Fact]
        public void Convert_SByte_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(sbyte), "127");
            Assert.Equal((sbyte)127, actual);
        }

        [Fact]
        public void Convert_Int16_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(short), "32767");
            Assert.Equal((short)32767, actual);
        }

        [Fact]
        public void Convert_UInt16_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(ushort), "65535");
            Assert.Equal((ushort)65535, actual);
        }

        [Fact]
        public void Convert_Int32_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(int), "2147483647");
            Assert.Equal((int)2147483647, actual);
        }

        [Fact]
        public void Convert_UInt32_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            var actual = test.Convert(typeof(uint), "4294967295");
            Assert.Equal((uint)4294967295, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(null!, "a"));
        }

        [Fact]
        public void Convert_Throw_TypeError_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(typeof(string), "a"));
        }

        #endregion
    }
}
