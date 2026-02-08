using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// rmdir
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/rmdir" />
    public class DeleteDirectoryCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "rmdir";

        #endregion

        public DeleteDirectoryCommand()
            : base(Name)
        { }

        #region property

        /// <summary>
        /// ディレクトリ ツリー (指定したディレクトリとそのすべてのサブディレクトリ (すべてのファイルを含む) を削除します。
        /// </summary>
        /// <remarks>
        /// <para>/s</para>
        /// </remarks>
        public bool Recursion { get; set; }

        /// <summary>
        /// 静音モードを指定します。 ディレクトリ ツリーを削除するときに確認を求めません。 /q パラメーターは、/s も指定されている場合にのみ機能します。
        /// </summary>
        /// <remarks>
        /// <para>/q</para>
        /// <para>注意： クワイエットモードで実行すると、ディレクトリツリー全体が確認なしで削除されます。 /q コマンド ライン オプションを使用する前に、重要なファイルが移動またはバックアップされていることを確認してください。</para>
        /// </remarks>
        public bool Quiet { get; set; }

        /// <summary>
        /// 削除するディレクトリの場所と名前を指定します。 パスは 必須です。 指定した パスの先頭に円記号 () を含めると、 パス はルートディレクトリから始まります (現在のディレクトリに関係なく)。
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
