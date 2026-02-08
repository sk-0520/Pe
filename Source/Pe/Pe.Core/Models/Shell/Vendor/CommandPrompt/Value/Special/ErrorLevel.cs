namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Value.Special
{
    public sealed class ErrorLevel: CommandPromptVariable
    {
        public ErrorLevel()
            : base("ERRORLEVEL")
        {
            IsReadOnly = true;
        }

        #region property

        public static ErrorLevel Instance => new ErrorLevel();

        #endregion
    }
}
