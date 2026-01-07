using System;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineConverterTest
    {
        #region function

        private sealed class ClassDefault
        {
            #region property

            [CommandLineOption("string", CommandLineOptionKind.Value)]
            public string String1 { get; set; } = string.Empty;
            public string String2 { get; set; } = string.Empty;
            public string? String3 { get; set; }

            [CommandLineOption("int32", CommandLineOptionKind.Value)]
            public int Int32_1 { get; set; }
            public int Int32_2 { get; set; }

            [CommandLineOption("bool-value1", CommandLineOptionKind.Value)]
            public bool BoolValue1 { get; set; }
            [CommandLineOption("bool-value2", CommandLineOptionKind.Value)]
            public bool BoolValue2 { get; set; }
            [CommandLineOption("bool-switch1", CommandLineOptionKind.Switch)]
            public bool BoolSwitch1 { get; set; }
            [CommandLineOption("bool-switch2", CommandLineOptionKind.Switch)]
            public bool BoolSwitch2 { get; set; }

            [CommandLineOption("datetime", CommandLineOptionKind.Value)]
            public DateTime DateTime1 { get; set; }
            public DateTime DateTime2 { get; set; }


            [CommandLineOption("timespan", CommandLineOptionKind.Value)]
            public TimeSpan TimeSpan1 { get; set; }
            public TimeSpan TimeSpan2 { get; set; }

            #endregion
        }

        [Fact]
        public void Converter_Default_Test()
        {
            var converter = new CommandLineConverter(NullLoggerFactory.Instance);
            var actual = converter.Convert<ClassDefault>(
                new CommandLineParser(),
                [
                    "--string", "text",
                    "--int32", "42",
                    "--bool-value1", "true",
                    "--bool-value2", "false",
                    "--bool-switch1",
                    "--datetime=2026-01-02T03:04:05",
                    "--timespan", "1.2:3:4.005",
                ]
            );

            Assert.Equal("text", actual.String1);
            Assert.Empty(actual.String2);
            Assert.Null(actual.String3);

            Assert.Equal(42, actual.Int32_1);
            Assert.Equal(0, actual.Int32_2);

            Assert.True(actual.BoolValue1);
            Assert.False(actual.BoolValue2);

            Assert.True(actual.BoolSwitch1);
            Assert.False(actual.BoolSwitch2);

            Assert.Equal(new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc), actual.DateTime1);
            Assert.Equal(DateTime.MinValue, actual.DateTime2);

            Assert.Equal(new TimeSpan(1, 2, 3, 4, 5), actual.TimeSpan1);
            Assert.Equal(TimeSpan.Zero, actual.TimeSpan2);
        }

        private sealed class ClassConvertError
        {
            #region property

            [CommandLineOption("int32", CommandLineOptionKind.Value)]
            public int Int32_1 { get; set; }
            [CommandLineOption("datetime", CommandLineOptionKind.Value)]
            public DateTime DateTime1 { get; set; } = DateTime.MaxValue;

            #endregion
        }

        [Fact]
        public void Converter_ConvertError_Test()
        {
            var mockLog = MockLog.Create();

            var converter = new CommandLineConverter(mockLog.Factory.Object);
            var actual = converter.Convert<ClassConvertError>(
                new CommandLineParser(),
                [
                    "--int32", "str",
                    "--int32", "-100",
                    "--datetime", "昨日"
                ]
            );

            Assert.Equal(-100, actual.Int32_1);
            Assert.Equal(DateTime.MaxValue, actual.DateTime1);

            mockLog.VerifyLog(Microsoft.Extensions.Logging.LogLevel.Warning, Moq.Times.Exactly(2));
        }

        private sealed class ClassCustomHandler
        {
            #region property

            [CommandLineOption("color", CommandLineOptionKind.Value)]
            public byte[]? Color { get; set; }

            #endregion
        }

        private sealed class ColorConvertHandler: CommandLineConvertHandlerBase<byte[]?>
        {
            #region ArgConverterHandlerBase
            public override byte[]? ConvertCore(string? value)
            {
                if(string.IsNullOrWhiteSpace(value)) {
                    return null;
                }
                var v = value.Trim();
                if(v.Length != 7 || v[0] != '#') {
                    throw new CommandLineConverterException();
                }
                var r = System.Convert.ToByte(v.Substring(1, 2), 16);
                var g = System.Convert.ToByte(v.Substring(3, 2), 16);
                var b = System.Convert.ToByte(v.Substring(5, 2), 16);
                return new byte[] { r, g, b };
            }
            #endregion
        }

        [Fact]
        public void Converter_ClassCustomHandler_Test()
        {
            var converter = new CommandLineConverter(NullLoggerFactory.Instance);
            converter.Handlers.Add(new ColorConvertHandler());
            var actual = converter.Convert<ClassCustomHandler>(
                new CommandLineParser(),
                [
                    "--color", "#ff0088",
                ]
            );

            Assert.NotNull(actual.Color);
            Assert.Equal(0xff, actual.Color[0]);
            Assert.Equal(0x00, actual.Color[1]);
            Assert.Equal(0x88, actual.Color[2]);
        }

        [Fact]
        public void Converter_ClassCustomHandler_Throw_Test()
        {
            var converter = new CommandLineConverter(NullLoggerFactory.Instance);
            // ColorConvertHandler の指定忘れ
            var exception = Assert.Throws<CommandLineTypeConvertException>(() => converter.Convert<ClassCustomHandler>(
                new CommandLineParser(),
                [
                    "--color", "#ff0088",
                ]
            ));
            Assert.Equal(typeof(byte[]), exception.Type);
        }

        #endregion
    }
}
