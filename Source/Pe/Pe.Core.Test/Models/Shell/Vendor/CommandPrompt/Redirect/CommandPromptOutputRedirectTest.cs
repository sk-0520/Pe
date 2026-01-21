using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Redirect;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Redirect
{
    public class CommandPromptOutputRedirectTest
    {
        #region function

        [Fact]
        public void NullTest()
        {
            var test = new CommandPromptOutputRedirect();
            Assert.Equal("NUL", test.Null.Target!.Expression);
        }

        [Fact]
        public void NullWithErrorTest()
        {
            var test = new CommandPromptOutputRedirect();
            Assert.Equal("NUL", test.NullWithError.Target!.Expression);
        }

        #endregion
    }
}
