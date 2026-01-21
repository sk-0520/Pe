using ContentTypeTextNet.Pe.Core.Models.Shell;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell
{
    public class EmptyLineTest
    {
        #region function

        [Fact]
        public void CommandNameTest()
        {
            var test = new EmptyLine();
            var actual = test.CommandName;
            Assert.Empty(actual);
        }

        [Fact]
        public void GetStatementTest()
        {
            var test = new EmptyLine();
            var actual = test.GetStatement();
            Assert.Empty(actual);
        }

        #endregion
    }
}
