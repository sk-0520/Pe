using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// cd
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/cd" />
    public class ChangeDirectoryCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "cd";

        #endregion

        public ChangeDirectoryCommand()
            : base(Name)
        { }

        #region property

        /// <summary>
        /// 現在のドライブだけでなく、ドライブの現在のディレクトリも変更します。
        /// </summary>
        /// <remarks>
        /// <para>/d</para>
        /// </remarks>
        public bool WithDrive { get; set; }

        /// <summary>
        /// 表示または変更するディレクトリへのパスを指定します。
        /// </summary>
        /// <remarks>
        /// <para>&lt;path&gt;</para>
        /// </remarks>
        public required Express Path { get; set; }

        #endregion

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            var sb = new StringBuilder();

            sb.Append(GetStatementCommandName());
            sb.Append(' ');

            if(WithDrive) {
                sb.Append("/d ");
            }

            var path = Implementation.EscapeValue(Path.Expression);
            sb.Append(path);

            return sb.ToString();
        }

        #endregion
    }
}
