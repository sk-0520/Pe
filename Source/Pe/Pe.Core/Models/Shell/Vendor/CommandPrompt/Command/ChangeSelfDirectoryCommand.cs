namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    public sealed class ChangeSelfDirectoryCommand: CommandPromptCommandBase
    {
        public ChangeSelfDirectoryCommand()
            : base(ChangeDirectoryCommand.Name)
        { }

        #region CommandBase

        public override string GetStatement()
        {
            return $"{GetStatementCommandName()} /d %~dp0";
        }

        #endregion
    }
}
