using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class CommandPromptCommandBaseTest
    {
        #region function

        private sealed class CommandPromptCommandBase_Constructor_Command: CommandPromptCommandBase
        {
            public CommandPromptCommandBase_Constructor_Command()
                : base("command_name")
            { }

            #region function

            public string GetStatementCommandName2() => GetStatementCommandName();

            #endregion

            #region CommandPromptCommandBase

            public override string GetStatement()
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        [Fact]
        public void ConstructorTest()
        {
            var command = new CommandPromptCommandBase_Constructor_Command();
            Assert.Equal("command_name", command.CommandName);
        }

        [Fact]
        public void GetStatementCommandName_Default_Test()
        {
            var command = new CommandPromptCommandBase_Constructor_Command();
            var actual = command.GetStatementCommandName2();
            Assert.Equal("command_name", actual);
        }

        [Theory]
        [InlineData("command_name", false, false)]
        [InlineData("COMMAND_NAME", true, false)]
        [InlineData("@command_name", false, true)]
        [InlineData("@COMMAND_NAME", true, true)]
        public void GetStatementCommandName_Custom_Test(string expected, bool commandNameIsUpper, bool suppressCommand)
        {
            var command = new CommandPromptCommandBase_Constructor_Command() {
                Implementation = new Core.Models.Shell.Vendor.CommandPrompt.CommandPromptImplementation() {
                    Options = new Core.Models.Shell.Vendor.CommandPrompt.CommandPromptOptions() {
                        CommandNameIsUpper = commandNameIsUpper,
                    }
                },
                SuppressCommand = suppressCommand,
            };
            var actual = command.GetStatementCommandName2();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
