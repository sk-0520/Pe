using ContentTypeTextNet.Pe.Core.Models.Shell;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell
{
    public class IndentContextTest
    {
        #region function

        [Fact]
        public void NestTest()
        {
            var root = IndentContext.Root;
            var child = root.Nest();

            Assert.Equal(root.Indent, child.Indent);
            Assert.Equal(0, root.Level);
            Assert.Equal(1, child.Level);

            var actualRoot = root.GetIndent();
            Assert.Equal(string.Empty, actualRoot);

            var actualChild = child.GetIndent();
            Assert.Equal("    ", actualChild);
        }

        #endregion

    }
}
