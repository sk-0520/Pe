namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt
{
    public class CommandPromptEditor: ShellEditorBase<CommandPromptImplementation, CommandPromptOptions>
    {
        public CommandPromptEditor(CommandPromptOptions options)
        {
            Implementation = new CommandPromptImplementation() {
                Options = options,
            };
        }

        public CommandPromptEditor()
            : this(new CommandPromptOptions())
        { }

        #region ShellEditorBase

        public override CommandPromptImplementation Implementation { get; }

        #endregion
    }
}
