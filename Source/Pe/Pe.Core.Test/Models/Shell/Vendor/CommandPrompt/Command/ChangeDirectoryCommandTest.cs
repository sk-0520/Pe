using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class ChangeDirectoryCommandTest
    {
        #region function

        [Fact]
        public void CommandNameTest()
        {
            var command = new ChangeDirectoryCommand() {
                Path = "foo",
            };
            Assert.Equal("cd", command.CommandName);
        }

        [Theory]
        [InlineData("cd a", "a", false)]
        [InlineData("cd \"a b\"", "a b", false)]
        [InlineData("cd /d a", "a", true)]
        [InlineData("cd /d \"a b\"", "a b", true)]
        public void GetStatementTest(string expected, string path, bool withDrive)
        {
            var command = new ChangeDirectoryCommand() {
                Path = path,
                WithDrive = withDrive,
            };
            var actual = command.GetStatement();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
