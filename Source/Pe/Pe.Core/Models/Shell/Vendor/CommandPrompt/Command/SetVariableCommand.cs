using System;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// set
    /// </summary>
    /// <remarks>
    /// <para>/p はサポートしない: するなら別処理</para>
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/set" />
    public class SetVariableCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "set";

        #endregion

        public SetVariableCommand()
            : base(Name)
        { }

        #region property

        public required string VariableName { get; set; }

        public required Express Value { get; set; }

        public bool IsExpress { get; set; }

        public CommandPromptVariable Variable
        {
            get
            {
                var variable = new CommandPromptVariable(VariableName);
                return variable;
            }
        }

        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            if(string.IsNullOrWhiteSpace(VariableName)) {
                throw new InvalidOperationException(nameof(VariableName));
            }

            var sb = new StringBuilder();

            sb.Append(GetStatementCommandName());
            sb.Append(' ');

            if(IsExpress) {
                sb.Append("/a ");
            }

            sb.Append(Variable.Name);
            sb.Append('=');
            sb.Append(Value.Expression);

            return sb.ToString();
        }

        #endregion
    }
}
