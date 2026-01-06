using System;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineStringConverterHandlerTest
    {
        #region function

        [Fact]
        public void TypeTest()
        {
            var test = new CommandLineStringConvertHandler();
            Assert.Equal(typeof(string), test.Type);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void ConvertTest(string expected, string? input)
        {
            var test = new CommandLineStringConvertHandler();
            var actual = test.Convert(input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }

    public class CommandLineInt32ConverterHandlerTest
    {
        #region function

        [Fact]
        public void TypeTest()
        {
            var test = new CommandLineInt32ConvertHandler();
            Assert.Equal(typeof(int), test.Type);
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
            var test = new CommandLineInt32ConvertHandler();
            var actual = test.Convert(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineInt32ConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert("a"));
        }

        #endregion
    }

    public class CommandLineBooleanConverterHandlerTest
    {
        #region function

        [Fact]
        public void TypeTest()
        {
            var test = new CommandLineBooleanConvertHandler();
            Assert.Equal(typeof(bool), test.Type);
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
            var actual = test.Convert(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineBooleanConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert("yes"));
        }

        #endregion
    }

    public class CommandLineDateTimeConverterHandlerTest
    {
        #region function

        [Fact]
        public void TypeTest()
        {
            var test = new CommandLineDateTimeConvertHandler();
            Assert.Equal(typeof(DateTime), test.Type);
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
            var actual = test.Convert(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineDateTimeConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert("令和！"));
        }

        #endregion
    }

    public class CommandLineTimeSpanConverterHandlerTest
    {
        #region function

        [Fact]
        public void TypeTest()
        {
            var test = new CommandLineTimeSpanConvertHandler();
            Assert.Equal(typeof(TimeSpan), test.Type);
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
            var actual = test.Convert(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_Test()
        {
            var test = new CommandLineTimeSpanConvertHandler();
            Assert.Throws<CommandLineConverterException>(() => test.Convert("💣"));
        }

        #endregion
    }
}
