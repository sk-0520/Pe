using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class DeleteDirectoryCommandTest
    {
        #region function

        public static TheoryData<string, (Express path, bool recursion, bool quiet)> GetStatementData => new() {
            {
                "rmdir path",
                (
                    "path",
                    false,
                    false
                )
            },
            {
                "rmdir /s path",
                (
                    "path",
                    true,
                    false
                )
            },
            {
                "rmdir /q path",
                (
                    "path",
                    false,
                    true
                )
            },
            {
                "rmdir /s /q path",
                (
                    "path",
                    true,
                    true
                )
            },
        };

        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, (Express path, bool recursion, bool quiet) parameters)
        {
            var (path, recursion, quiet) = parameters;
            var test = new DeleteDirectoryCommand() {
                Path = path,
                Recursion = recursion,
                Quiet = quiet,
            };
            var acutual = test.GetStatement();
            Assert.Equal(expected, acutual);
        }

        #endregion
    }
}
