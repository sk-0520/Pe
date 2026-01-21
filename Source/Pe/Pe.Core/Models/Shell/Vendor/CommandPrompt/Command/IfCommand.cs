using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Value.Special;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    public abstract class IfCommandBase: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "if";
        internal const string ElseName = "else";

        #endregion

        protected IfCommandBase()
            : base(Name)
        { }

        #region property

        public bool IsNot { get; set; }

        protected abstract Express Condition { get; }

        public IList<CommandBase> TrueBlock { get; } = new List<CommandBase>();
        public IList<CommandBase> FalseBlock { get; } = new List<CommandBase>();

        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            var sb = new StringBuilder();
            var newLine = Options.NewLine;

            sb.Append(GetStatementCommandName());
            sb.Append(' ');
            if(IsNot) {
                sb.Append("not ");
            }
            sb.Append(Condition.Expression);
            sb.Append(" (");
            sb.Append(newLine);
            foreach(var command in TrueBlock) {
                sb.Append(Options.Indent);
                sb.Append(command.GetStatement());
                sb.Append(newLine);
            }
            if(FalseBlock.Any()) {
                sb.Append(") ");

                sb.Append(GetStatementCommandName(ElseName));
                sb.Append(" (");
                sb.Append(newLine);
                foreach(var command in FalseBlock) {
                    sb.Append(Options.Indent);
                    sb.Append(command.GetStatement());
                    sb.Append(newLine);
                }
                sb.Append(")");
            } else {
                sb.Append(")");
            }

            return sb.ToString();
        }

        #endregion
    }

    public class IfErrorLevelCommand: IfCommandBase
    {
        #region property

        public int Level { get; set; }

        #endregion

        #region IfCommandBase

        protected override Express Condition
        {
            get
            {
                var result = new Express();
                result.Values.Add(new Text(ErrorLevel.Instance.Name));
                result.Values.Add(new Text(" "));
                result.Values.Add(new Text(Level.ToString()));

                return result;
            }
        }

        #endregion
    }

    // あ、これどうしよ
    public enum CompareOperation
    {
        Default,
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
    }

    public class IfExpressCommand: IfCommandBase
    {
        #region property

        public required ValueBase Left { get; set; }
        public required ValueBase Right { get; set; }

        public CompareOperation CompareOperation { get; set; }

        #endregion

        #region IfCommandBase

        protected override Express Condition
        {
            get
            {
                var result = new Express();

                result.Values.Add(new Text(Implementation.EscapeValue(Left.Expression)));
                result.Values.Add(new Text(" == "));
                result.Values.Add(new Text(Implementation.EscapeValue(Right.Expression)));

                return result;
            }
        }

        #endregion
    }

    public class IfExistCommand: IfCommandBase
    {
        #region property

        public required Express Path { get; set; }

        #endregion

        #region IfCommandBase

        protected override Express Condition
        {
            get
            {
                var result = new Express();
                result.Values.Add(new Text("exist "));
                result.Values.Add(new Text(Implementation.EscapeValue(Path.Expression)));

                return result;
            }
        }

        #endregion
    }
}
