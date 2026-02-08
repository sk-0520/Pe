namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// pause
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/pause" />
    public class PauseCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "pause";

        #endregion

        public PauseCommand()
            : base(Name)
        { }

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            return $"{GetStatementCommandName()}";
        }

        #endregion
    }

}
