using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class ChangeSelfDirectoryCommandTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var command = new ChangeSelfDirectoryCommand();
            var actual = command.GetStatement();
            Assert.Equal("cd /d %~dp0", actual);
        }

        #endregion
    }
}
