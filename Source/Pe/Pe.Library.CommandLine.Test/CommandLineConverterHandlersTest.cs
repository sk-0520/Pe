using System;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test
{
    public class CommandLineStringConverterHandlerTest
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
        public void Convert_Throw_Test()
        {
            var test = new CommandLineIntegerConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(null!, "a"));
        }

        #endregion
    }

    public class CommandLineBooleanConverterHandlerTest
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

    public class CommandLineDateTimeConverterHandlerTest
    {
        #region function

        [Theory]
        [InlineData(typeof(DateTime))]
        public void IsSupportTypeTest(Type type)
        {
            var test = new CommandLineDateTimeConvertHandler();
            Assert.True(test.IsSupportType(type));
        }

        public static TheoryData<DateTime, string?> ConvertData => new() {
            {
                DateTime.MinValue,
                null
            },
            {
                DateTime.MinValue,
                ""
            },
            {
                DateTime.MinValue,
                " "
            },
            {
                new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                "2024/01/02"
            },
            {
                new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                "2024-01-02"
            },
            {
                new DateTime(2024, 1, 2, 4, 5, 6, DateTimeKind.Utc),
                "2024/01/02 04:05:06"
            },
            {
                new DateTime(2024, 1, 2, 4, 5, 6, DateTimeKind.Utc),
                "2024-01-02T04:05:06"
            },
               {
                new DateTime(2024, 1, 2, 4, 5, 6, 789, DateTimeKind.Utc),
                "2024/01/02 04:05:06.789"
            },
            {
                new DateTime(2024, 1, 2, 4, 5, 6, 789, DateTimeKind.Utc),
                "2024-01-02T04:05:06.789"
            },
        };

        [Theory]
        [MemberData(nameof(ConvertData))]
        public void ConvertTest(DateTime expected, string? input)
        {
            var test = new CommandLineDateTimeConvertHandler();
            var actual = test.Convert(typeof(DateTime), input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineDateTimeConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(null!, "令和！"));
        }

        #endregion
    }

    public class CommandLineTimeSpanConverterHandlerTest
    {
        #region function

        [Theory]
        [InlineData(typeof(TimeSpan))]
        public void IsSupportTypeTest(Type type)
        {
            var test = new CommandLineTimeSpanConvertHandler();
            Assert.True(test.IsSupportType(type));
        }

        public static TheoryData<TimeSpan, string?> ConvertData => new() {
            {
                TimeSpan.Zero,
                null
            },
            {
                TimeSpan.Zero,
                ""
            },
            {
                TimeSpan.Zero,
                " "
            },
            {
                TimeSpan.Zero,
                "0"
            },
            {
                TimeSpan.Zero,
                "0.00:00:00.0"
            },
            {
                new TimeSpan(0, 0, 0),
                "0.00:00:00.0"
            },
            {
                new TimeSpan(1, 2, 3, 4, 5),
                "1.02:03:04.005"
            }
        };

        [Theory]
        [MemberData(nameof(ConvertData))]
        public void ConvertTest(TimeSpan expected, string? input)
        {
            var test = new CommandLineTimeSpanConvertHandler();
            var actual = test.Convert(typeof(TimeSpan), input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineTimeSpanConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert(null!, "💣"));
        }

        #endregion
    }
}
