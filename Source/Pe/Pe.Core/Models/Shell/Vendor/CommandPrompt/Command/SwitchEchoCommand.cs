namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    public sealed class SwitchEchoCommand: EchoCommand
    {
        public SwitchEchoCommand()
        {
            SuppressCommand = true;
        }

        #region property

        public required bool On { get; set; }

        #endregion

        #region Echo

        public override string GetStatement()
        {
            var value = On ? "on" : "off";
            return $"{GetStatementCommandName()} {value}";
        }

        #endregion
    }
}
