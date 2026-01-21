namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    public abstract class CommandPromptCommandBase: CommandBase
    {
        protected CommandPromptCommandBase(string commandName)
        {
            CommandName = commandName;
        }

        #region property

        public static CommandPromptImplementation Default { get; set; } = new CommandPromptImplementation();

        public CommandPromptImplementation Implementation { get; init; } = Default;

        protected CommandPromptOptions Options => Implementation.Options;

        /// <summary>
        /// コマンド出力を抑制するか。
        /// </summary>
        public bool SuppressCommand { get; set; }


        #endregion

        #region function

        protected string GetStatementCommandName(string commandName)
        {
            var command = Options.CommandNameIsUpper
                ? commandName.ToUpperInvariant()
                : commandName
            ;

            return SuppressCommand
                ? "@" + command
                : command
            ;
        }

        protected virtual string GetStatementCommandName()
        {
            return GetStatementCommandName(CommandName);
        }

        #endregion

        #region CommandBase

        public override string CommandName { get; }

        #endregion
    }
}
