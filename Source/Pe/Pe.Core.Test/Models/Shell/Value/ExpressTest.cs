using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Value
{
    public class ExpressTest
    {
        #region function

        [Fact]
        public void Constructor_Test()
        {
            var actual = new Express();
            Assert.Empty(actual.Values);
        }

        [Fact]
        public void implicit_operator_Test()
        {
            string text = "sample";
            Express actual = text;
            Assert.Single(actual.Values);
            Assert.IsType<Text>(actual.Values[0]);
            Assert.Equal(text, actual.Values[0].Expression);
        }

        [Fact]
        public void ExpressionTest()
        {
            Express test = "first";
            test.Values.Add(new Text("second"));
            var actrual = test.Expression;
            Assert.Equal("firstsecond", actrual);
        }

        #endregion
    }
}
