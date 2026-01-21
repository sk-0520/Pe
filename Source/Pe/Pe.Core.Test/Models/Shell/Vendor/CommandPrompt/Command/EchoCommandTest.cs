using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class EchoCommandTest
    {
        #region function

        public static TheoryData<string, Express> GetStatementData => new() {
            {
                "echo.",
                ""
            },
            {
                "echo \" \"",
                " "
            },
            {
                "echo value",
                "value"
            },
        };

        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, Express value)
        {
            var command = new EchoCommand() {
                Value = value,
            };
            var actual = command.GetStatement();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
