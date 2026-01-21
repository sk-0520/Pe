using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Value
{
    public class CommandPromptVariable: VariableBase
    {
        public CommandPromptVariable(string name)
            : base(name)
        { }

        #region property

        public bool DelayedExpansion { get; init; }

        #endregion

        #region VariableBase

        public override string Expression
        {
            get
            {
                if(DelayedExpansion) {
                    return $"!{Name}!";
                }

                return $"%{Name}%";
            }
        }

        #endregion
    }
}
