using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Library.Common
{
    /// <summary>
    /// 文字列適当操作処理。
    /// </summary>
    public static class TextUtility
    {
        #region define

        /// <summary>
        /// <see cref="ReplacePlaceholder"/> 書き込み処理。
        /// </summary>
        /// <param name="placeholder">プレースホルダー。</param>
        /// <param name="writer">書き込み処理。</param>
        public delegate void ReplacePlaceholderDelegate(ReadOnlySpan<char> placeholder, Action<ReadOnlySpan<char>> writer);

        #endregion

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
        /// <param name="writer">処理。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplacePlaceholder(string source, ReadOnlySpan<char> head, ReadOnlySpan<char> tail, ReplacePlaceholderDelegate writer)
        {
            if(string.IsNullOrEmpty(source) || head.IsEmpty || tail.IsEmpty) {
                return source;
            }
            ArgumentNullException.ThrowIfNull(writer);

            var sourceSpan = source.AsSpan();
            var headLength = head.Length;
            var tailLength = tail.Length;

            var sb = new StringBuilder(source.Length);
            int startIndex = 0;

            while(true) {
                // 現在の startIndex 以降で head を検索
                int relativeHeadIndex = sourceSpan.Slice(startIndex).IndexOf(head);
                if(relativeHeadIndex < 0) {
                    sb.Append(sourceSpan.Slice(startIndex));
                    break;
                }
                var headIndex = startIndex + relativeHeadIndex;

                //TODO: 同じ head が続く場合はエスケープ扱いしたい、けど、テスト書くのだるい

                // head の直後から tail を検索
                int relativeTailIndex = sourceSpan.Slice(headIndex + headLength).IndexOf(tail);
                if(relativeTailIndex < 0) {
                    var rest = sourceSpan.Slice(startIndex);
                    sb.Append(rest);
                    break;
                }
                int tailIndex = headIndex + headLength + relativeTailIndex;

                // head と tail の間が空の場合は、プレースホルダー部分をそのまま出力
                if(tailIndex == headIndex + headLength) {
                    var onlyPlaceholder = sourceSpan.Slice(startIndex, tailIndex + tailLength - startIndex);
                    sb.Append(onlyPlaceholder);
                    startIndex = tailIndex + tailLength;
                    continue;
                }

                // プレースホルダーの直前部分を追加
                var prev = sourceSpan.Slice(startIndex, headIndex - startIndex);
                sb.Append(prev);

                // プレースホルダーをもとに呼び出し側で指定された書き込み処理実施
                var placeholder = sourceSpan.Slice(headIndex + headLength, tailIndex - (headIndex + headLength));
                writer(placeholder, a => sb.Append(a));

                startIndex = tailIndex + tailLength;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 指定範囲の値を指定のコレクションで置き換える。
        /// </summary>
        /// <param name="source">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="map">置き換え対象文字列と置き換え後文字列のペアであるコレクション。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplacePlaceholderFromDictionary(string source, string head, string tail, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholder(source, head, tail, (placeholder, writer) => {
                var value = map.TryGetValue(new(placeholder), out var r)
                    ? r
                    : string.Concat(head, placeholder, tail)
                ;
                writer(value);
            });
        }
        /// <summary>
        /// 文字列中の<c>${key}</c>を<see cref="IReadOnlyDictionary{string, string}"/>の対応で置き換える。
        /// </summary>
        /// <param name="source">対象文字列。</param>
        /// <param name="map">マップ。</param>
        /// <returns>置き換え後文字列。</returns>
        public static string ReplaceFromDictionary(string source, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholderFromDictionary(source, "${", "}", map);
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
        public static string RemoveCharacters(string target, IReadOnlySet<char> characters)
        {
            if(characters.Count == 0) {
                return target;
            }

            bool isRemoved = false;
            char[]? buffer = null;
            int index = 0;
            foreach(var c in target) {
                if(characters.Contains(c)) {
                    isRemoved = true;
                    continue;
                }
                if(buffer is null) {
                    buffer = ArrayPool<char>.Shared.Rent(target.Length);
                }
                buffer[index++] = c;
            }

            if(buffer is not null) {
                var result = new string(buffer, 0, index);
                ArrayPool<char>.Shared.Return(buffer);
                return result;
            }

            return isRemoved ? string.Empty : target;
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
