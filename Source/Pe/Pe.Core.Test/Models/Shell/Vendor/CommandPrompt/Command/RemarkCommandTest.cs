using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class RemarkCommandTest
    {
        #region function

        [Theory]
        [InlineData("rem", "")]
        [InlineData("rem abc", "abc")]
        [InlineData("rem a b c", "a b c")]
        public void GetStatementTest(string expected, Express comment)
        {
            var command = new RemarkCommand() {
                Comment = comment
            };
            var actual = command.GetStatement();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
