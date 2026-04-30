using System;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Value
{
    file sealed class TestValue: ValueBase
    {
        #region property

        public required string Value { get; init; }

        #endregion

        #region ValueBase  

        public override string Expression => throw new NotImplementedException();

        #endregion
    }

    public class ValueBaseTest
    {
        #region function

        [Fact]
        public void OperatorAddTest()
        {
            var a = new TestValue() {
                Value = "a",
            };
            var b = new TestValue() {
                Value = "b",
            };
            var c = a + b;
            Assert.Equal(a, c.Values[0]);
            Assert.Equal(b, c.Values[1]);
        }

        #endregion

    }
}
