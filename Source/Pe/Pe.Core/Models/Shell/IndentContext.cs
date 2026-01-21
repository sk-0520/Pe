using System.Linq;

namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    /// <summary>
    /// 現在インデント。
    /// </summary>
    /// <remarks>
    /// <para>まぁきちんと動いてないけど使うこともないのでどうでもよさげ</para>
    /// </remarks>
    public class IndentContext
    {
        public IndentContext(string indent, int level)
        {
            Indent = indent;
            Level = level;
        }

        #region property

        public static IndentContext Root { get; } = new IndentContext("    ", 0);

        /// <summary>
        /// インデントに使用する文字列。
        /// </summary>
        public string Indent { get; }

        /// <summary>
        /// インデントレベル。
        /// </summary>
        /// <remarks>
        /// <para>0 が最上位。</para>
        /// </remarks>
        public int Level { get; }

        #endregion

        #region function

        /// <summary>
        /// ネスト。
        /// </summary>
        /// <returns>ネスト後のインデント。</returns>
        public IndentContext Nest()
        {
            return new IndentContext(Indent, Level + 1);
        }

        /// <summary>
        /// ネストされたインデントの取得。
        /// </summary>
        /// <returns></returns>
        public string GetIndent()
        {
            return string.Concat(Enumerable.Repeat(Indent, Level));
        }

        #endregion
    }
}
