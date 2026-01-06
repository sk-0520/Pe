namespace ContentTypeTextNet.Pe.Library.Args
{
    public class CommandLineHelper
    {
        #region property

        /// <summary>
        /// オプション開始文字列。
        /// </summary>
        public string OptionPrefix { get; init; } = "--";
        /// <summary>
        /// オプションのキー・値の区切り文字列。
        /// </summary>
        /// <remarks>
        /// <para>スペースは自動的に処理(配列として分割)されるため明示的にスペースを設定した場合の動作はあまり考慮しない(参照: CommandLineParserTest.Parse_SingleValues_Test)。</para>
        /// </remarks>
        public char Separator { get; init; } = '=';
        /// <summary>
        /// 説明文のインデント。
        /// </summary>
        public string DescriptionIndent { get; init; } = "    ";

        #endregion

        #region function

        /// <summary>
        /// キーが不正なら落とす。
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="CommandLineInvalidKeyException"></exception>
        /// <remarks>
        /// <para>実装メモ: `<see cref="CommandLineInvalidKeyException"/>.ThrowIfInvalidKey` ではなく <see cref="CommandLineHelper"/> で実装しているのはキー判定処理が変更される可能性があるため継承可能なこちらで実装している。</para>
        /// </remarks>
        public virtual void ThrowIfInvalidKey(string key)
        {
            if(string.IsNullOrWhiteSpace(key)) {
                throw new CommandLineInvalidKeyException();
            }
        }

        /// <summary>
        /// 左右の囲い文字を切り落とす。
        /// </summary>
        /// <param name="s"></param>
        /// <returns>切り落とされた文字列。存在しない場合は<paramref name="s"/>が返る。</returns>
        /// <remarks>本処理では `"` を対象とするため、対象とするシステムに合わせる場合は継承先で対応すること。</remarks>
        public virtual string StripEnclosing(string s)
        {
            if("\"\"".Length < s.Length && s.StartsWith('"') && s.EndsWith('"')) {
                return s.Substring(1, s.Length - 1 - 1);
            }

            return s;
        }

        /// <summary>
        /// 文字列をコマンドライン引数の値に変換する。
        /// </summary>
        /// <param name="input">対象文字列。</param>
        /// <returns>変換後の文字列。</returns>
        /// <remarks>対象とするシステムに合わせる場合は継承先で対応すること。</remarks>
        public virtual string Escape(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) {
                return "\"" + input + "\"";
            }

            var s = input.Trim();
            var result = s.Replace("\"", "\"\"");
            if(s.IndexOf(' ') == -1) {
                return result;
            } else {
                return "\"" + result + "\"";
            }
        }

        #endregion
    }
}
