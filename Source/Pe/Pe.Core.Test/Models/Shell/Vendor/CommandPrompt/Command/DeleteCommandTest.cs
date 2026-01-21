using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class DeleteCommandTest
    {
        #region function

        public static TheoryData<string, (Express path, bool pause, bool force, bool recursion, bool quiet)> GetStatementData => new() {
            {
                "del path",
                (
                    "path",
                    false,
                    false,
                    false,
                    false
                )
            },
            {
                "del /p path",
                (
                    "path",
                    true,
                    false,
                    false,
                    false
                )
            },
            {
                "del /f path",
                (
                    "path",
                    false,
                    true,
                    false,
                    false
                )
            },
            {
                "del /s path",
                (
                    "path",
                    false,
                    false,
                    true,
                    false
                )
            },
            {
                "del /q path",
                (
                    "path",
                    false,
                    false,
                    false,
                    true
                )
            },
        };

        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, (Express path, bool pause, bool force, bool recursion, bool quiet) parameters)
        {
            var (path, pause, force, recursion, quiet) = parameters;
            var test = new DeleteCommand() {
                Path = path,
                Pause = pause,
                Force = force,
                Recursion = recursion,
                Quiet = quiet,
            };
            var acutual = test.GetStatement();
            Assert.Equal(expected, acutual);
        }


        #endregion
    }
}
