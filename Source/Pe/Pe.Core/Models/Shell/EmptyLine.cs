namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    public class EmptyLine: CommandBase
    {
        #region CommandBase

        public override string CommandName => string.Empty;

        public override string GetStatement()
        {
            return string.Empty;
        }

        override public void WriteStatement(IShellWriter writer)
        {
            writer.WriteLine();
        }

        #endregion

    }
}
