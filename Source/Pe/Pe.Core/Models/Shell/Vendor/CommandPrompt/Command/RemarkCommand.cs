using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    public class RemarkCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "rem";

        #endregion

        public RemarkCommand()
            : base(Name)
        { }

        #region property

        public Express Comment { get; set; } = string.Empty;

        #endregion

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            var value = Comment.Expression;
            if(string.IsNullOrEmpty(value)) {
                return $"{GetStatementCommandName()}";
            }

            return $"{GetStatementCommandName()} {value}";
        }

        #endregion
    }
}
