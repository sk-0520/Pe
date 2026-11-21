using System;
using ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test.ConvertHandlers
{
    public class CommandLineTimeSpanConvertHandlerTest
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
