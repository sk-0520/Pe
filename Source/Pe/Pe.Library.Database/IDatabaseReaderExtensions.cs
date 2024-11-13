using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// 読み込み処理の安全のしおり。
    /// </summary>
    /// <remarks>
    /// <para>問い合わせ文として非ユーザー入力でデバッグ中に検証可能なものを想定している。</para>
    /// <para>そのため問い合わせ文の確認自体もデバッグ時のみ有効となる。</para>
    /// <para>なお確認自体はただの文字列比較であるため該当ワードをコメントアウトしても通過する点に注意。</para>
    /// </remarks>
    public static class IDatabaseReaderExtensions
    {
        #region function

        /// <inheritdoc cref="IDatabaseReader.GetDataReader(string, object?)"/>
        public static IDataReader GetDataReader(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            return reader.GetDataReader(statement, parameter);
        }

        /// <inheritdoc cref="IDatabaseReader.GetDataReaderAsync(string, object?, CancellationToken)"/>
        public static Task<IDataReader> GetDataReaderAsync(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return reader.GetDataReaderAsync(statement, parameter, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.GetDataTable(string, object?)"/>
        public static DataTable GetDataTable(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            return reader.GetDataTable(statement, parameter);
        }

        /// <inheritdoc cref="IDatabaseReader.GetDataTableAsync(string, object?, CancellationToken)"/>
        public static Task<DataTable> GetDataTableAsync(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return reader.GetDataTableAsync(statement, parameter, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.GetScalar{TResult}(string, object?)"/>
        public static TResult? GetScalar<TResult>(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            return reader.GetScalar<TResult>(statement, parameter);
        }

        /// <inheritdoc cref="IDatabaseReader.GetScalarAsync{TResult}(string, object?, CancellationToken)"/>
        public static Task<TResult?> GetScalarAsync<TResult>(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return reader.GetScalarAsync<TResult>(statement, parameter, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.Query{T}(string, object?, bool)"/>
        public static IEnumerable<T> Query<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            return reader.Query<T>(statement, parameter, buffered);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            return reader.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public static IEnumerable<dynamic> Query(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            return reader.Query(statement, parameter, buffered);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync(string, object?, bool, CancellationToken)"/>
        public static Task<IEnumerable<dynamic>> QueryAsync(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            return reader.QueryAsync(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryFirst{T}(string, object?)"/>
        public static T QueryFirst<T>(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            return reader.QueryFirst<T>(statement, parameter);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryFirstAsync{T}(string, object?, CancellationToken)"/>
        public static Task<T> QueryFirstAsync<T>(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return reader.QueryFirstAsync<T>(statement, parameter, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryFirstOrDefault{T}(string, object?)"/>
        [return: MaybeNull]
        public static T QueryFirstOrDefault<T>(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            return reader.QueryFirstOrDefault<T>(statement, parameter);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryFirstOrDefaultAsync{T}(string, object?, CancellationToken)"/>
        public static Task<T?> QueryFirstOrDefaultAsync<T>(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return reader.QueryFirstOrDefaultAsync<T>(statement, parameter, cancellationToken);
        }


        [Conditional("DEBUG")]
        private static void ThrowIfNotOrderBy(string statement)
        {
            if(!Regex.IsMatch(statement, @"\border\s+by\b", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("order by");
            }
        }

        /// <summary>
        /// 検索処理時に順序指定を強制する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectOrdered<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfNotOrderBy(statement);

            return reader.Query<T>(statement, parameter, buffered);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static IEnumerable<dynamic> SelectOrdered(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfNotOrderBy(statement);

            return reader.Query(statement, parameter, buffered);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static Task<IEnumerable<T>> SelectOrderedAsync<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfNotOrderBy(statement);

            return reader.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static Task<IEnumerable<dynamic>> SelectOrderedAsync(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfNotOrderBy(statement);

            return reader.QueryAsync(statement, parameter, buffered, cancellationToken);
        }


        [Conditional("DEBUG")]
        private static void ThrowIfNotSingleCount(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bselect\s+count\s*\(", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("select count()");
            }
        }

        /// <summary>
        /// 単一の件数取得を強制。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        public static long SelectSingleCount(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            ThrowIfNotSingleCount(statement);

            return reader.QueryFirst<long>(statement, parameter);
        }

        /// <inheritdoc cref="SelectSingleCount(IDatabaseReader, string, object?)"/>
        public static Task<long> SelectSingleCountAsync(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotSingleCount(statement);

            return reader.QueryFirstAsync<long>(statement, parameter, cancellationToken);
        }

        #endregion
    }
}
