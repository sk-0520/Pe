using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Value;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Value
{
    public class CommandPromptVariableTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var test = new CommandPromptVariable("name");
            Assert.Equal("name", test.Name);
            Assert.Equal("%name%", test.Expression);
        }

        [Fact]
        public void DelayedExpansionTest()
        {
            var test = new CommandPromptVariable("name") {
                DelayedExpansion = true,
            };
            Assert.Equal("name", test.Name);
            Assert.Equal("!name!", test.Expression);
        }

        #endregion
    }
}
