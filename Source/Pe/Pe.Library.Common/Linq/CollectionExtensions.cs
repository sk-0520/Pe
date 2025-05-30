using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Common.Linq
{
    /// <summary>
    /// コレクション LINQ。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 全要素を削除してから指定コレクションを追加。
        /// </summary>
        /// <remarks>
        /// <para><see cref="ICollection{T}.Clear"/>からの<see cref="ICollection{T}.Add"/></para>
        /// <para><see cref="ICollection{T}"/>が<see cref="List{T}"/>なら<see cref="List{T}.AddRange(IEnumerable{T})"/>する</para>
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="collection"></param>
        public static void SetRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            source.Clear();
            if(source is List<T> list) {
                list.AddRange(collection);
            } else {
                foreach(var item in collection) {
                    source.Add(item);
                }
            }
        }

        /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            foreach(var item in collection) {
                source.Add(item);
            }
        }

        /// <summary>
        /// 次のインデックスを取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="startIndex">開始インデックス。</param>
        /// <param name="distance">進める距離。</param>
        /// <returns></returns>
        public static int GetNextIndex<T>(this IReadOnlyCollection<T> source, int startIndex, int distance)
        {
            ArgumentNullException.ThrowIfNull(source);
            // startIndex < 0 
            ArgumentOutOfRangeException.ThrowIfLessThan(startIndex, 0);
            // startIndex > source.Count-1
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, source.Count - 1);

            if(distance == 0) {
                return startIndex;
            }

            var result = (startIndex + distance) % source.Count;
            if(result < 0) {
                result += source.Count;
            }

            return result;
        }
    }
}
