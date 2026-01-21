namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt
{
    public class CommandPromptOptions: ShellOptions
    {
        public CommandPromptOptions()
        {
            Indent = "\t";
        }

        #region property

        public bool CommandNameIsUpper { get; set; }

        #endregion
    }
}
