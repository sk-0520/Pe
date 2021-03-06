using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Core.Models
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
        /// <returns>集合の中に同じものがなければtarget, 存在すれば<paramref name="target"/>(n)。</returns>
        public static string ToUniqueDefault(string target, IReadOnlyCollection<string> seq, StringComparison comparisonType)
        {
            return ToUnique(target, seq, comparisonType, (string source, int index) => string.Format("{0}({1})", source, index));
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
        /// <returns></returns>
        public static string ReplacePlaceholder(string source, string head, string tail, Func<string, string> dg)
        {
            var escHead = Regex.Escape(head);
            var escTail = Regex.Escape(tail);
            var pattern = escHead + "(.+?)" + escTail;
            var replacedText = Regex.Replace(source, pattern, (Match m) => dg(m.Groups[1].Value));
            return replacedText;
        }

        /// <summary>
        /// 指定範囲の値を指定のコレクションで置き換える。
        /// </summary>
        /// <param name="source">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="map">置き換え対象文字列と置き換え後文字列のペアであるコレクション。</param>
        /// <returns></returns>
        public static string ReplacePlaceholderFromDictionary(string source, string head, string tail, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholder(source, head, tail, s => map.ContainsKey(s) ? map[s] : head + s + tail);
        }
        /// <summary>
        /// ${key}をvalueに置き変える。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static string ReplaceFromDictionary(string source, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholderFromDictionary(source, "${", "}", map);
        }

        /// <summary>
        /// 文字列から行毎に分割する。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4456:Parameter validation in yielding methods should be wrapped")]
        public static IEnumerable<string> ReadLines(string text)
        {
            if(text == null) {
                throw new ArgumentNullException(nameof(text));
            }

            using var reader = new StringReader(text);
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
        /// <param name="s"></param>
        /// <returns>A: 1, ｱ: 1, あ: 1, 🐙: 1</returns>
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
        /// <param name="s"></param>
        /// <returns></returns>
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
        /// <param name="s"></param>
        /// <returns></returns>
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
        /// <param name="lines"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinLines(string lines, string separator) => string.Join(separator, ReadLines(lines));
        /// <summary>
        /// 複数行データを半角スペースで結合。
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string JoinLines(string lines) => JoinLines(lines, " ");

        /// <summary>
        /// 指定文字を破棄。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string RemoveCharacters(string input, ISet<char> characters)
        {
            if(characters.Count == 0) {
                return input;
            }

            if(input.IndexOfAny(characters.ToArray()) == -1) {
                return input;
            }

            var sb = new StringBuilder(input.Length);
            foreach(var c in input) {
                if(characters.Contains(c)) {
                    continue;
                }
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static string ReplaceCharacters(string input, IReadOnlyDictionary<char, char> characters)
        {
            if(characters.Count == 0) {
                return input;
            }

            if(input.IndexOfAny(characters.Keys.ToArray()) == -1) {
                return input;
            }

            var sb = new StringBuilder(input.Length);
            foreach(var c in input) {
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
