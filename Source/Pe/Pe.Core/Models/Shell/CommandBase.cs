using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Redirect;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    public abstract class CommandBase
    {
        #region property

        /// <summary>
        /// コマンド名。
        /// </summary>
        public abstract string CommandName { get; }

        /// <summary>
        /// 標準入力ファイル。
        /// </summary>
        public Express? Input { get; set; }

        /// <summary>
        /// リダイレクト先。
        /// </summary>
        public OutputRedirect? Redirect { get; set; }

        /// <summary>
        /// パイプ先。
        /// </summary>
        public CommandBase? Pipe { get; set; }

        #endregion

        #region function

        /// <summary>
        /// コマンドの出力。
        /// </summary>
        /// <returns>インデントとかない状態。</returns>
        public abstract string GetStatement();

        /// <summary>
        /// 文脈に合わせたコマンドの出力。
        /// </summary>
        /// <param name="indent">インデント。</param>
        /// <returns>インデントされたあれこれ。</returns>
        public virtual string ToStatement(IndentContext indent)
        {
            var sb = new StringBuilder();

            var statement = GetStatement();
            sb.Append(statement);

            if(Input != null) {
                sb.Append(" < ");
                sb.Append(Input.Expression);
            }

            if(Pipe != null) {
                var pipeAction = Pipe.ToStatement(IndentContext.Root);
                sb.Append(" | ");
                sb.Append(pipeAction);
            } else if(Redirect != null) {
                var redirect = Redirect.Expression;
                if(!string.IsNullOrWhiteSpace(redirect)) {
                    sb.Append(' ');
                    sb.Append(redirect);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
