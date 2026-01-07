using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineHelperTest
    {
        #region function

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowIfInvalidKey_Throw_Test(string? s)
        {
            var test = new CommandLineHelper();
            Assert.Throws<CommandLineInvalidKeyException>(() => test.ThrowIfInvalidKey(s!));
        }

        [Fact]
        public void ThrowIfInvalidKeyTest()
        {
            var test = new CommandLineHelper();
            var actual = Record.Exception(() => test.ThrowIfInvalidKey("key"));
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("\"", "\"")]
        [InlineData("\"a", "\"a")]
        [InlineData("a\"", "a\"")]
        [InlineData("\"\"", "\"\"")]
        [InlineData("a", "\"a\"")]
        [InlineData(" \"a\" ", " \"a\" ")]
        public void StripEnclosingTest(string expected, string input)
        {
            var test = new CommandLineHelper();
            var actual = test.StripEnclosing(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("\"\"", "")]
        [InlineData("\" \"", " ")]
        [InlineData("a", "a")]
        [InlineData("a", " a")]
        [InlineData("a", "a ")]
        [InlineData("a", " a ")]
        [InlineData("\"a a\"", "a a")]
        [InlineData("a\"\"b", "a\"b")]
        [InlineData("a\"\"\"\"\"\"b", "a\"\"\"b")]
        [InlineData("\"a \"\"\"\"\"\" b\"", "a \"\"\" b")]
        public void EscapeTest(string expected, string input)
        {
            var test = new CommandLineHelper();
            var actual = test.Escape(input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
