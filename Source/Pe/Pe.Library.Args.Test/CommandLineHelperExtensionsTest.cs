using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineHelperExtensionsTest
    {
        #region function

        private Dictionary<string, string> ToCommandLineArgumentsData { get; } = new() {
            ["key"] = "value",
            ["space"] = "abc xyz",
            ["empty"] = "",
            ["white"] = " ",
        };

        [Fact]
        public void ToCommandLineArguments_Default_Test()
        {
            var test = new CommandLineHelper();
            var actual1 = test.ToCommandLineArguments(ToCommandLineArgumentsData);
            Assert.Contains("--key=value", actual1);
            Assert.Contains("--space=\"abc xyz\"", actual1);
            Assert.Contains("--empty=\"\"", actual1);
            Assert.Contains("--white=\" \"", actual1);
        }

        [Fact]
        public void ToCommandLineArguments_Default_Plus_Test()
        {
            var test = new CommandLineHelper() {
                Separator = '+',
            };
            var actual1 = test.ToCommandLineArguments(ToCommandLineArgumentsData);
            Assert.Contains("--key+value", actual1);
            Assert.Contains("--space+\"abc xyz\"", actual1);
            Assert.Contains("--empty+\"\"", actual1);
            Assert.Contains("--white+\" \"", actual1);
        }

        [Fact]
        public void ToCommandLineArguments_At_Test()
        {
            var test = new CommandLineHelper() {
                OptionPrefix = "@",
            };
            var actual2 = test.ToCommandLineArguments(ToCommandLineArgumentsData);
            Assert.Contains("@key=value", actual2);
            Assert.Contains("@space=\"abc xyz\"", actual2);
            Assert.Contains("@empty=\"\"", actual2);
            Assert.Contains("@white=\" \"", actual2);
        }

        [Fact]
        public void ToCommandLineArguments_Asterisk2_Test()
        {
            var test = new CommandLineHelper() {
                OptionPrefix = "**",
            };
            var actual3 = test.ToCommandLineArguments(ToCommandLineArgumentsData);
            Assert.Contains("**key=value", actual3);
            Assert.Contains("**space=\"abc xyz\"", actual3);
            Assert.Contains("**empty=\"\"", actual3);
            Assert.Contains("**white=\" \"", actual3);
        }

        #endregion
    }
}
