using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Common.Linq;

namespace ContentTypeTextNet.Pe.Library.Common.Linq
{
    /// <summary>
    /// ふわふわ LINQ。
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region function

        /// <summary>
        /// シーケンスの内容を固定化し、以降変更不可な読み取り専用リストとして返す。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="source">元となるシーケンス。</param>
        /// <returns>
        /// シーケンスの内容を固定化した <see cref="IReadOnlyList{T}"/>。
        /// 返却されるリストは要素の追加・削除ができませんが、要素自体が参照型の場合はその内容が変更される可能性があります。
        /// </returns>
        /// <remarks>
        /// <para>
        /// LINQ の遅延評価を明示的に切り、列挙時の副作用やパフォーマンス問題を回避したい場合に有効です。
        /// コレクションの構造（要素数・順序）は不変となりますが、要素自体のイミュータビリティは保証しません。
        /// </para>
        /// </remarks>
        public static IReadOnlyList<T> FrozenSequence<T>(this IEnumerable<T> source)
        {
            return Array.AsReadOnly(source.ToArray());
        }

        /// <summary>
        /// 0基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<CountingItem<int, TElement>> Counting<TElement>(this IEnumerable<TElement> source)
        {
            return source.Select((v, i) => new CountingItem<int, TElement>(i, v));
        }
        /// <summary>
        /// 独自基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="baseNumber">基点。</param>
        /// <returns></returns>
        public static IEnumerable<CountingItem<int, TElement>> Counting<TElement>(this IEnumerable<TElement> source, int baseNumber)
        {
            return source.Select((v, i) => new CountingItem<int, TElement>(i + baseNumber, v));
        }

        /// <summary>
        /// 入力シーケンスを結合した文字列を返す。
        /// </summary>
        /// <remarks>
        /// <para><see cref="string.Join"/>してるだけだけど、 linq でふわっと使いたい。</para>
        /// </remarks>
        /// <inheritdoc cref="string.Join"/>
        public static string JoinString<T>(this IEnumerable<T> source, string? separator) => string.Join(separator, source);

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector)
        {
            return order switch {
                Order.Ascending => source.OrderBy(keySelector),
                Order.Descending => source.OrderByDescending(keySelector),
                _ => throw new NotImplementedException(),
            };
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return order switch {
                Order.Ascending => source.OrderBy(keySelector, comparer),
                Order.Descending => source.OrderByDescending(keySelector, comparer),
                _ => throw new NotImplementedException(),
            };
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector)
        {
            return order switch {
                Order.Ascending => source.ThenBy(keySelector),
                Order.Descending => source.ThenByDescending(keySelector),
                _ => throw new NotImplementedException(),
            };
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return order switch {
                Order.Ascending => source.ThenBy(keySelector, comparer),
                Order.Descending => source.ThenByDescending(keySelector, comparer),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// シーケンスの要素が全て同じか。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns>同じか。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool AllEquals<TSource>(this IEnumerable<TSource> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            using var enumerator = source.GetEnumerator();
            if(!enumerator.MoveNext()) {
                return true;
            }

            var firstElement = enumerator.Current;
            while(enumerator.MoveNext()) {
                var currentElement = enumerator.Current;
                if(!EqualityComparer<TSource>.Default.Equals(firstElement, currentElement)) {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
