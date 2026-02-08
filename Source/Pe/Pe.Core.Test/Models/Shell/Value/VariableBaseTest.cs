using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Value
{
    public class VariableBaseTest
    {
        #region define

        public class TestVariable: VariableBase
        {
            public TestVariable(string name)
                : base(name)
            { }

            #region VariableBase

            public override string Expression => $"Name: {Name}, IsReadOnly: {IsReadOnly}";

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void ConstructorTest()
        {
            var test = new TestVariable("name");
            Assert.Equal("name", test.Name);
            Assert.False(test.IsReadOnly);
        }


        [Fact]
        public void Constructor_Readonly_Test()
        {
            var test = new TestVariable("name") {
                IsReadOnly = true,
            };
            Assert.Equal("name", test.Name);
            Assert.True(test.IsReadOnly);
        }

        #endregion
    }
}
