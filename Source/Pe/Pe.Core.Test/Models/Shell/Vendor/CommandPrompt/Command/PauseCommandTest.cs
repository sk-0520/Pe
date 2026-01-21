using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class PauseCommandTest
    {
        #region function

        [Fact]
        public void GetStatementTest()
        {
            var command = new PauseCommand();
            var actual = command.GetStatement();
            Assert.Equal("pause", actual);
        }

        #endregion
    }
}
