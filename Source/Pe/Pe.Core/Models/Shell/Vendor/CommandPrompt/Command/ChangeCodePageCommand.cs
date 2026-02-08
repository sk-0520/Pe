using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    public class ChangeCodePageCommand: CommandPromptCommandBase
    {
        #region define

        public const string Name = "chcp";

        #endregion

        public ChangeCodePageCommand()
            : base(Name)
        { }

        #region property


        public required Encoding Encoding { get; set; }

        #endregion

        #region function

        public override string GetStatement()
        {
            return $"{GetStatementCommandName()} {Encoding.CodePage}";
        }

        #endregion
    }

}
