using ContentTypeTextNet.Pe.Core.Models.Shell.Redirect;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Redirect
{
    public class OutputRedirectTest
    {
        #region function

        [Fact]
        public void Error_Default_Test()
        {
            var test = new OutputRedirect();
            var actual = test.Error;
            Assert.Null(actual);
        }

        [Fact]
        public void Expression_Default_Test()
        {
            var test = new OutputRedirect();
            var actual = test.Expression;
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void Expression_Errort_Test()
        {
            var test = new OutputRedirect() {
                Error = new ErrorRedirect() {
                    StandardOutput = true,
                }
            };
            var actual = test.Expression;
            Assert.Equal("2>&1", actual);
        }

        [Fact]
        public void NullTest()
        {
            var test = new OutputRedirect();
            Assert.Equal("/dev/null", test.Null.Target!.Expression);
        }

        [Fact]
        public void NullWithErrorTest()
        {
            var test = new OutputRedirect();
            Assert.Equal("/dev/null", test.NullWithError.Target!.Expression);
        }

        #endregion
    }
}
