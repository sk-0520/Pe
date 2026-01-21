using System;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class SetVariableCommandTest
    {
        #region function

        public static TheoryData<string, (string variableName, Express value, bool isExpress)> GetStatementData => new() {
            {
                "set a=1",
                (
                    "a",
                    "1",
                    false
                )
            },
            {
                "set /a a=1",
                (
                    "a",
                    "1",
                    true
                )
            }
        };


        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, (string variableName, Express value, bool isExpress) parameters)
        {
            var (variableName, value, isExpress) = parameters;
            var command = new SetVariableCommand() {
                VariableName = variableName,
                Value = value,
                IsExpress = isExpress
            };
            var actual = command.GetStatement();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetStatement_Throw_Test(string? variableName)
        {
            var command = new SetVariableCommand() {
                VariableName = variableName!,
                Value = "",
            };
            Assert.Throws<InvalidOperationException>(() => command.GetStatement());
        }

        #endregion
    }
}
