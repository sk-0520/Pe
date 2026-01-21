namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    public abstract class ShellImplementationBase<TShellOptions>
        where TShellOptions : ShellOptions, new()
    {
        #region property

        public virtual string Indent { get; } = "\t";

        public TShellOptions Options { get; set; } = new();

        #endregion

        #region function

        public abstract string EscapeValue(string value);

        public abstract string ToSafeVariableName(string name);

        #endregion

    }
}
