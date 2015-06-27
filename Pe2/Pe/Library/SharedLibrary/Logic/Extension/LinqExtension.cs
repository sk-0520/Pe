﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class LinqExtension
	{
		/// <summary>
		/// シーケンスを真偽値により処理を分岐させる
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="seq">入力シーケンス</param>
		/// <param name="cond">条件</param>
		/// <param name="t">真の場合に返すシーケンス</param>
		/// <param name="f">偽の場合に返すシーケンス</param>
		/// <returns></returns>
		public static IEnumerable<TResult> IfElse<T, TResult>(this IEnumerable<T> seq, bool cond, Func<IEnumerable<T>, IEnumerable<TResult>> t, Func<IEnumerable<T>, IEnumerable<TResult>> f)
		{
			if (cond) {
				return t(seq);
			} else {
				return f(seq);
			}
		}

		/// <summary>
		/// 条件が真の場合にシーケンスを反転させる。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="seq"></param>
		/// <param name="cond"></param>
		/// <returns></returns>
		public static IEnumerable<T> IfRevese<T>(this IEnumerable<T> seq, bool cond)
		{
			return IfElse(seq, cond, s => s.Reverse(), s => s);
		}

		public static IEnumerable<T> IfOrderByAsc<T, TKey>(this IEnumerable<T> seq, Func<T, TKey> keySelector, bool orderByAsc)
		{
			return IfElse(seq, orderByAsc, s => s.OrderBy(keySelector), s => s.OrderByDescending(keySelector));
		}

		/// <summary>
		/// <para>http://stackoverflow.com/questions/13767451/ilistt-findindexint32-predicate-t?answertab=votes#tab-top</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="startIndex"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static int FindIndex<T>(this IList<T> source, int startIndex , Predicate<T> match)
		{
			// TODO: Validation
			for(int i = startIndex; i < source.Count; i++) {
				if(match(source[i])) {
					return i;
				}
			}
			return -1;
		}

		public static int FindIndex<T>(this IList<T> source, Predicate<T> match)
		{
			return FindIndex(source, 0, match);
		}
	}
}
