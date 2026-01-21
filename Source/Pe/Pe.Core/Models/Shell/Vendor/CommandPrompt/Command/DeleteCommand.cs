using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// del
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/del" />
    public class DeleteCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "del";

        #endregion

        public DeleteCommand()
            : base(Name)
        { }

        #region property

        /// <summary>
        /// 指定したファイルを削除する前に確認を求めるメッセージが表示されます。
        /// </summary>
        /// <remarks>
        /// <para>/p</para>
        /// </remarks>
        public bool Pause { get; set; }

        /// <summary>
        /// 読み取り専用ファイルを強制的に削除します。
        /// </summary>
        /// <remarks>
        /// <para>/f</para>
        /// </remarks>
        public bool Force { get; set; }

        /// <summary>
        /// 指定したファイルを現在のディレクトリとすべてのサブディレクトリから削除します。 削除中のファイルの名前を表示します。
        /// </summary>
        /// <remarks>
        /// <para>/s</para>
        /// </remarks>
        public bool Recursion { get; set; }

        /// <summary>
        /// 静音モードを指定します。 削除の確認を求めるメッセージは表示されません。
        /// </summary>
        public bool Quiet { get; set; }

        /// <summary>
        /// 1 つ以上のファイルまたはディレクトリの一覧を指定します。 ワイルドカードは、複数のファイルを削除するために使用できます。 ディレクトリを指定すると、ディレクトリ内のすべてのファイルが削除されます。
        /// </summary>
        /// <remarks>
        /// <para>&lt;path&gt;</para>
        /// </remarks>
        public required Express Path { get; set; }

        // attributes は未対応

        #endregion

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            var sb = new StringBuilder();

            sb.Append(GetStatementCommandName());
            sb.Append(' ');


            if(Pause) {
                sb.Append("/p ");
            }

            if(Force) {
                sb.Append("/f ");
            }

            if(Recursion) {
                sb.Append("/s ");
            }

            if(Quiet) {
                sb.Append("/q ");
            }

            var path = Implementation.EscapeValue(Path.Expression);
            sb.Append(path);

            return sb.ToString();
        }

        #endregion
    }
}
