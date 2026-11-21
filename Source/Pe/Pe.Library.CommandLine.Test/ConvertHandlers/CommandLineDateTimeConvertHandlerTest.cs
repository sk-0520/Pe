using System;
using ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test.ConvertHandlers
{
    public class CommandLineDateTimeConvertHandlerTest
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
}
