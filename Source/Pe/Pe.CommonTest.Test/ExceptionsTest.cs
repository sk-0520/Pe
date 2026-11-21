using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest.Test
{
    public class TestExceptionTest
    {
        #region function

        [Fact]
        public void ThrowIfNullTest()
        {
            var actual = TestException.ThrowIfNull("a");
            Assert.Equal("a", actual);
        }

        [Theory]
        [InlineData("value is null (null)", null)]
        [InlineData("MESSAGE (null)", "MESSAGE")]
        public void ThrowIfNullTest_Null_Throw(string expectedMessage, string? message)
        {
            var exception = Assert.Throws<TestException>(() => TestException.ThrowIfNull<string>(null, message));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("value is null (variable)", null)]
        [InlineData("MESSAGE (variable)", "MESSAGE")]
        public void ThrowIfNullTest_Variable_Throw(string expectedMessage, string? message)
        {
            var variable = (string?)null;
            var exception = Assert.Throws<TestException>(() => TestException.ThrowIfNull<string>(variable, message));
            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion
    }
}
