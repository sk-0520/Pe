using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineHelperTest
    {
        #region function

        [Fact]
        public void ThrowIfInvalidLongKeyTest()
        {
            Assert.Throws<ArgumentNullException>(() => CommandLineHelper.ThrowIfInvalidLongKey(null!));
            Assert.Throws<ArgumentException>(() => CommandLineHelper.ThrowIfInvalidLongKey(""));
            Assert.Throws<ArgumentException>(() => CommandLineHelper.ThrowIfInvalidLongKey(" "));

            var actual = CommandLineHelper.ThrowIfInvalidLongKey(" key ");
            Assert.Equal(" key ", actual);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("\"", "\"")]
        [InlineData("\"a", "\"a")]
        [InlineData("a\"", "a\"")]
        [InlineData("\"\"", "\"\"")]
        [InlineData("a", "\"a\"")]
        [InlineData(" \"a\" ", " \"a\" ")]
        public void StripDoubleQuotesTest(string expected, string input)
        {
            var actual =  CommandLineHelper.StripDoubleQuotes(input);
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
            var actual = CommandLineHelper.Escape(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToCommandLineArgumentsTest()
        {
            var input = new Dictionary<string, string>() {
                ["key"] = "value",
                ["space"] = "abc xyz",
                ["empty"] = "",
                ["white"] = " ",
            };
            var actual1 = CommandLineHelper.ToCommandLineArguments(input);
            Assert.Contains("--key=value", actual1);
            Assert.Contains("--space=\"abc xyz\"", actual1);
            Assert.Contains("--empty=\"\"", actual1);
            Assert.Contains("--white=\" \"", actual1);

            var actual2 = CommandLineHelper.ToCommandLineArguments(input, "@@");
            Assert.Contains("@@key=value", actual2);
            Assert.Contains("@@space=\"abc xyz\"", actual2);
            Assert.Contains("@@empty=\"\"", actual2);
            Assert.Contains("@@white=\" \"", actual2);

            var actual3 = CommandLineHelper.ToCommandLineArguments(input, "**", ' ');
            Assert.Contains("**key value", actual3);
            Assert.Contains("**space \"abc xyz\"", actual3);
            Assert.Contains("**empty \"\"", actual3);
            Assert.Contains("**white \" \"", actual3);
        }

        #endregion
    }
}
