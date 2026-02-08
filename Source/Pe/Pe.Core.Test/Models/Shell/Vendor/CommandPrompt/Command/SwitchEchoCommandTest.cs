using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class SwitchEchoCommandTest
    {
        #region function

        [Theory]
        [InlineData("@echo on", true)]
        [InlineData("@echo off", false)]
        public void GetStatementTest(string expected, bool on)
        {
            var command = new SwitchEchoCommand() {
                On = on,
            };
            var actual = command.GetStatement();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
