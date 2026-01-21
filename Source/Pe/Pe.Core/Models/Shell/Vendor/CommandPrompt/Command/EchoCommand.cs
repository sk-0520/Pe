using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// copy
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/echo" />
    public class EchoCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "echo";

        #endregion

        public EchoCommand()
            : base(Name)
        { }

        #region property

        public ValueBase Value { get; set; } = Express.Empty;

        #endregion

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            var value = Value.Expression;
            if(string.IsNullOrEmpty(value)) {
                return $"{GetStatementCommandName()}.";
            }

            return $"{GetStatementCommandName()} {Implementation.EscapeValue(value)}";
        }

        #endregion
    }
}
