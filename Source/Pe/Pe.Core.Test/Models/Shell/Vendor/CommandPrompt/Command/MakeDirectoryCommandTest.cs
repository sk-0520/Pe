using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class MakeDirectoryCommandTest
    {
        #region function


        public static TheoryData<string, Express> GetStatementData => new() {
            {
                "mkdir \"\"",
                ""
            },
            {
                "mkdir abc",
                "abc"
            },
            {
                "mkdir \"a z\"",
                "a z"
            },
        };

        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, Express path)
        {
            var test = new MakeDirectoryCommand() {
                Path = path,
            };
            var actual = test.GetStatement();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
