using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// mkdir
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/mkdir" />
    public class MakeDirectoryCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "mkdir";

        #endregion


        public MakeDirectoryCommand()
            : base(Name)
        { }

        #region property

        public required Express Path { get; set; }

        #endregion

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            var path = Implementation.EscapeValue(Path.Expression);
            return $"{GetStatementCommandName()} {path}";
        }

        #endregion
    }

}
