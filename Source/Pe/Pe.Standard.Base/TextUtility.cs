using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// 文字列適当操作処理。
    /// </summary>
    public static class TextUtility
    {
        #region function

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="seq">集合</param>
        /// <param name="comparisonType">比較処理。</param>
        /// <param name="converter"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used")]
        public static string ToUnique(string target, IReadOnlyCollection<string> seq, StringComparison comparisonType, Func<string, int, string> converter)
        {
            if(target == null) {
                throw new ArgumentNullException(nameof(target));
            }
            if(seq == null) {
                throw new ArgumentNullException(nameof(seq));
            }
            if(converter == null) {
                throw new ArgumentNullException(nameof(converter));
            }

            var changeName = target;

            int n = 1;
            RETRY:
            foreach(var value in seq) {
                if(string.Equals(value, changeName, comparisonType)) {
                    changeName = converter(target, ++n);
                    goto RETRY;
                }
            }

            return changeName;
        }

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="seq"></param>
        /// <param name="comparisonType"></param>
        /// <returns>集合の中に同じものがなければ<paramref name="target"/>, 存在すれば<paramref name="target"/>(n)。</returns>
        public static string ToUniqueDefault(string target, IReadOnlyCollection<string> seq, StringComparison comparisonType)
        {
            return ToUnique(target, seq, comparisonType, (source, index) => string.Format(CultureInfo.InvariantCulture, "{0}({1})", source, index));
        }

#if false
        /// <summary>
        /// 指定文字列集合を<see cref="StringCollection"/>に変換する。
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static StringCollection ToStringCollection(IEnumerable<string> seq)
        {
            var sc = new StringCollection();
            sc.AddRange(seq.ToArray());

            return sc;
        }
#endif

        /// <summary>
        /// 指定範囲の値を指定処理で置き換える。
        /// </summary>
        /// <param name="source">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="dg">処理。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplacePlaceholder(string source, string head, string tail, Func<string, string> dg, char headEscape)
        {
            var isEnabledHeadEscape = headEscape != '\0';

            var escHead = Regex.Escape(head);
            var escTail = Regex.Escape(tail);
            var pattern = escHead + "(.+?)" + escTail;
            var replacedText = Regex.Replace(source, pattern, (m) => dg(m.Groups[1].Value));
            return replacedText;
        }

        /// <summary>
        /// 指定範囲の値を指定のコレクションで置き換える。
        /// </summary>
        /// <param name="source">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="map">置き換え対象文字列と置き換え後文字列のペアであるコレクション。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplacePlaceholderFromDictionary(string source, string head, string tail, IReadOnlyDictionary<string, string> map, char headEscape = '\0')
        {
            return ReplacePlaceholder(source, head, tail, s => map.ContainsKey(s) ? map[s] : head + s + tail, headEscape);
        }
        /// <summary>
        /// 文字列中の<c>${key}</c>を<see cref="IReadOnlyDictionary{string, string}"/>の対応で置き換える。
        /// </summary>
        /// <param name="source">対象文字列。</param>
        /// <param name="map">マップ。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplaceFromDictionary(string source, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholderFromDictionary(source, "${", "}", map, '$');
        }

        /// <summary>
        /// 文字列から行毎に分割する。
        /// </summary>
        /// <param name="s">対象文字列。</param>
        /// <returns>分割文字列。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4456:Parameter validation in yielding methods should be wrapped")]
        public static IEnumerable<string> ReadLines(string s)
        {
            if(s == null) {
                throw new ArgumentNullException(nameof(s));
            }

            using var reader = new StringReader(s);
            string? line;
            while((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }


        /// <summary>
        /// リーダーから行毎に分割する。
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4456:Parameter validation in yielding methods should be wrapped")]
        public static IEnumerable<string> ReadLines(TextReader reader)
        {
            if(reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            string? line;
            while((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }

        /// <summary>
        /// 文字のなんちゃってな長さを取得。
        /// </summary>
        /// <param name="s">対象文字列。</param>
        /// <returns>A: 1, ｱ: 1, あ: 1, 🐙: 1。<para><see cref="GetCharacters"/>も参照のこと。</para></returns>
        public static int TextWidth(string s)
        {
            if(s == null) {
                return 0;
            }

            var si = new StringInfo(s);
            return si.LengthInTextElements;
        }

        /// <summary>
        /// 文字列をなんちゃって一文字単位に分解。
        /// </summary>
        /// <param name="s">対象文字列。</param>
        /// <returns>文字列としての一文字で分解された集合。<para><see cref="TextWidth"/>も参照のこと。</para></returns>
        public static IEnumerable<string> GetCharacters(string s)
        {
            var textElements = StringInfo.GetTextElementEnumerator(s);
            while(textElements.MoveNext()) {
                yield return (string)textElements.Current;
            }
        }

        /// <summary>
        /// 安全に<see cref="string.Trim"/>を行う。
        /// </summary>
        /// <inheritdoc cref="string.Trim"/>
        /// <param name="s">対象文字列。</param>
        /// <returns><paramref name="s"/>が<see langword="null" />の場合は空文字列、それ以外はトリムされた文字列。</returns>
        public static string SafeTrim(string? s)
        {
            if(s == null) {
                return string.Empty;
            }

            return s.Trim();
        }

        /// <summary>
        /// 複数行を指定文字列で結合。
        /// </summary>
        /// <param name="lines">行分割された文字列。</param>
        /// <param name="separator">結合文字列。</param>
        /// <returns><paramref name="separator"/> で結合された文字列。</returns>
        public static string JoinLines(string lines, string separator) => string.Join(separator, ReadLines(lines));
        /// <summary>
        /// 複数行データを半角スペースで結合。
        /// </summary>
        /// <param name="lines">行分割された文字列。</param>
        /// <returns>半角スペースで結合された文字列。</returns>
        public static string JoinLines(string lines) => JoinLines(lines, " ");

        /// <summary>
        /// 指定文字を破棄。
        /// </summary>
        /// <param name="target">対象文字列。</param>
        /// <param name="characters">削除対象文字。</param>
        /// <returns>削除後文字列。</returns>
        public static string RemoveCharacters(string target, IReadOnlyCollection<char> characters)
        {
            if(characters.Count == 0) {
                return target;
            }

            if(target.IndexOfAny(characters.ToArray()) == -1) {
                return target;
            }

            var sb = new StringBuilder(target.Length);
            foreach(var c in target) {
                if(characters.Contains(c)) {
                    continue;
                }
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 文字列の特定の文字を置き換える。
        /// </summary>
        /// <param name="target">対象文字列。</param>
        /// <param name="characters"><see cref="IReadOnlyDictionary{char, char}.Keys"/>に対して<see cref="IReadOnlyDictionary{char, char}.Values"/>に置き換える。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplaceCharacters(string target, IReadOnlyDictionary<char, char> characters)
        {
            if(characters.Count == 0) {
                return target;
            }

            if(target.IndexOfAny(characters.Keys.ToArray()) == -1) {
                return target;
            }

            var sb = new StringBuilder(target.Length);
            foreach(var c in target) {
                if(characters.TryGetValue(c, out var newChar)) {
                    sb.Append(newChar);
                } else {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
