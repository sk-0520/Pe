using System;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt
{
    public class CommandPromptImplementationTest
    {
        #region function

        [Theory]
        [InlineData("a", "a")]
        [InlineData("&", "&")]
        [InlineData("\"\"", "")]
        [InlineData("\"a b\"", "a b")]
        [InlineData("\" a b \"", " a b ")]
        [InlineData("^^", "^")]
        [InlineData("^>", ">")]
        [InlineData("^<", "<")]
        [InlineData("\" ^^ \"", " ^ ")]
        [InlineData("\" ^> \"", " > ")]
        [InlineData("\" ^< \"", " < ")]
        public void EscapeValueTest(string expected, string value)
        {
            var test = new CommandPromptImplementation();
            var actual = test.EscapeValue(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("_", "%")]
        public void ToSafeVariableNameTest(string expected, string name)
        {
            var test = new CommandPromptImplementation();
            var actual = test.ToSafeVariableName(name);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ToSafeVariableName_Throw_Test(string? name)
        {
            var test = new CommandPromptImplementation();
            Assert.Throws<ArgumentException>(() => test.ToSafeVariableName(name!));
        }

        #endregion
    }
}
