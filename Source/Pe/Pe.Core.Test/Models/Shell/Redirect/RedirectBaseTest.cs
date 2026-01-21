using ContentTypeTextNet.Pe.Core.Models.Shell.Redirect;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Redirect
{
    public class RedirectBaseTest
    {
        #region function

        private sealed class Expression_null: RedirectBase
        { }

        [Fact]
        public void Expression_null_Test()
        {
            var test = new Expression_null();
            var actual = test.Expression;
            Assert.Empty(actual);
        }

        private sealed class Expression_Target: RedirectBase
        {
            //public Expression_Target(bool append)
            //{
            //    var target = new Express();
            //    if(append) {
            //        target.Values.Add("append");
            //    } else {
            //        target.Values.Add("not append");
            //    }
            //    Target = target;
            //}
        }

        [Fact]
        public void Expression_Append_false_Test()
        {
            var express = new Express();
            express.Values.Add(new Text("not append"));
            var test = new Expression_Target() {
                Append = false,
                Target = express,
            };
            var actual = test.Expression;
            Assert.Equal("> not append", actual);
        }

        [Fact]
        public void Expression_Append_true_Test()
        {
            var express = new Express();
            express.Values.Add(new Text("append"));
            var test = new Expression_Target() {
                Append = true,
                Target = express,
            };
            var actual = test.Expression;
            Assert.Equal(">> append", actual);
        }

        #endregion
    }
}
