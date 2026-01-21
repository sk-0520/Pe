using System;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command
{
    /// <summary>
    /// copy
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/windows-server/administration/windows-commands/copy" />
    public class CopyCommand: CommandPromptCommandBase
    {
        #region define

        internal const string Name = "copy";

        #endregion

        public CopyCommand()
            : base(Name)
        { }

        #region property

        /// <summary>
        /// 暗号化されたファイルをコピー先で暗号化解除されたファイルとして保存できるようにします。
        /// </summary>
        /// <remarks>
        /// <para>/d</para>
        /// </remarks>
        public bool IsDecryption { get; set; }
        /// <summary>
        /// 新しいファイルが正しく書き込まれたことを確認します。
        /// </summary>
        /// <remarks>
        /// <para>/v</para>
        /// </remarks>
        public bool IsVerify { get; set; }
        /// <summary>
        /// プロンプト状態。
        /// </summary>
        /// <remarks>
        /// <para>[<see cref="PromptMode.Silent"/>] /y: 既存の宛先ファイルを上書きするかどうかを確認するプロンプトを抑制します。</para>
        /// <para>[<see cref="PromptMode.Confirm"/>] /-y: 既存のリンク先ファイルを上書きするかどうかを確認するプロンプトを表示します。</para>
        /// <para>[<see cref="PromptMode.Default"/>] デフォルト。</para>
        /// </remarks>
        public PromptMode PromptMode { get; set; }

        /// <summary>
        /// ファイルまたは一連のファイルをコピーする場所を指定します。 ソース は、ドライブ文字とコロン、ディレクトリ名、ファイル名、またはこれらの組み合わせで構成できます。
        /// </summary>
        public required Express Source { get; set; }
        /// <summary>
        /// ファイルまたは一連のファイルをコピーする場所を指定します。 宛先は 、ドライブ文字とコロン、ディレクトリ名、ファイル名、またはこれらの組み合わせで構成できます。
        /// </summary>
        public required Express Destination { get; set; }

        #endregion

        #region function
        #endregion

        #region CommandPromptCommandBase

        public override string GetStatement()
        {
            var src = Source?.Expression;
            var dst = Destination?.Expression;
            if(string.IsNullOrWhiteSpace(src)) {
                throw new InvalidOperationException(nameof(Source));
            }
            if(string.IsNullOrWhiteSpace(dst)) {
                throw new InvalidOperationException(nameof(Destination));
            }

            var sb = new StringBuilder();

            sb.Append(GetStatementCommandName());
            sb.Append(' ');

            if(IsDecryption) {
                sb.Append("/d ");
            }

            if(IsVerify) {
                sb.Append("/v ");
            }

            switch(PromptMode) {
                case PromptMode.Default:
                    break;

                case PromptMode.Confirm:
                    sb.Append("/-y ");
                    break;

                case PromptMode.Silent:
                    sb.Append("/y ");
                    break;

                default:
                    throw new NotImplementedException();
            }

            sb.Append(Implementation.EscapeValue(src));
            sb.Append(' ');
            sb.Append(Implementation.EscapeValue(dst));

            return sb.ToString();
        }

        #endregion
    }
}
