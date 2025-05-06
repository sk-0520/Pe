using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// <see cref="IDatabaseExecutor"/>の各種メソッドデフォルト引数追加と書き込み処理の安全のしおり。
    /// </summary>
    /// <remarks>
    /// <para>問い合わせ文として非ユーザー入力でデバッグ中に検証可能なものを想定している。</para>
    /// <para>そのため問い合わせ文の確認自体もデバッグ時のみ有効となる。</para>
    /// <para>なお確認自体はただの文字列比較であるため該当ワードをコメントアウトしても通過する点に注意。</para>
    /// </remarks>
    public static class IDatabaseExecutorExtensions
    {
        #region function

        /// <inheritdoc cref="IDatabaseExecutor.Execute(string, object?)"/>
        public static int Execute(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            return executor.Execute(statement, parameter);
        }

        /// <inheritdoc cref="IDatabaseExecutor.ExecuteAsync(string, object?, CancellationToken)"/>
        public static Task<int> ExecuteAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return executor.ExecuteAsync(statement, parameter, cancellationToken);
        }


        [Conditional("DEBUG")]
        private static void ThrowIfNotUpdate(string statement)
        {
            if(!Regex.IsMatch(statement, @"\b update \b", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("update");
            }
        }

        /// <summary>
        /// 更新処理を強制。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>更新件数。</returns>
        public static int Update(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotUpdate(statement);

            return executor.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Update(IDatabaseExecutor, string, object?)"/>
        public static Task<int> UpdateAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotUpdate(statement);

            return executor.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一更新を強制。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">未更新。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void UpdateByKey(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotUpdate(statement);

            var result = executor.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseManipulationException($"update -> {result}");
            }
        }

        /// <inheritdoc cref="UpdateByKey(IDatabaseExecutor, string, object?)"/>
        public static async Task UpdateByKeyAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotUpdate(statement);

            var result = await executor.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseManipulationException($"update -> {result}");
            }
        }

        /// <summary>
        /// 単一更新か未更新を強制。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">複数更新。</exception>
        /// <returns>真: 単一更新、偽: 未更新。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static bool UpdateByKeyOrNothing(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotUpdate(statement);

            var result = executor.Execute(statement, parameter);
            if(1 < result) {
                throw new DatabaseManipulationException($"update -> {result}");
            }

            return result == 1;
        }

        /// <inheritdoc cref="UpdateByKeyOrNothing(IDatabaseExecutor, string, object)"/>
        public static async Task<bool> UpdateByKeyOrNothingAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotUpdate(statement);

            var result = await executor.ExecuteAsync(statement, parameter, cancellationToken);
            if(1 < result) {
                throw new DatabaseManipulationException($"update -> {result}");
            }

            return result == 1;
        }

        [Conditional("DEBUG")]
        private static void ThrowIfNotInsert(string statement)
        {
            if(!Regex.IsMatch(statement, @"\b insert \b", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("insert");
            }
        }

        /// <summary>
        /// 挿入を強制。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>挿入件数。</returns>
        public static int Insert(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotInsert(statement);

            return executor.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Insert(IDatabaseExecutor, string, object?)"/>
        public static Task<int> InsertAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotInsert(statement);

            return executor.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一挿入。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">未挿入か複数挿入。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void InsertSingle(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotInsert(statement);

            var result = executor.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseManipulationException($"insert -> {result}");
            }
        }

        /// <inheritdoc cref="InsertSingle(IDatabaseExecutor, string, object?)"/>
        public static async Task InsertSingleAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotInsert(statement);

            var result = await executor.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseManipulationException($"insert -> {result}");
            }
        }

        [Conditional("DEBUG")]
        private static void ThrowIfNotDelete(string statement)
        {
            if(!Regex.IsMatch(statement, @"\b delete \b", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("delete");
            }
        }

        /// <summary>
        /// 削除を強制。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>削除件数。</returns>
        public static int Delete(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotDelete(statement);

            return executor.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Delete(IDatabaseExecutor, string, object?)"/>
        public static Task<int> DeleteAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotDelete(statement);

            return executor.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一削除。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">未削除。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void DeleteByKey(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotDelete(statement);

            var result = executor.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }
        }
        /// <inheritdoc cref="DeleteByKey(IDatabaseExecutor, string, object?)"/>
        public static async Task DeleteByKeyAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotDelete(statement);

            var result = await executor.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }
        }

        /// <summary>
        /// 単一更新か未削除を強制。
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        /// <exception cref="DatabaseStatementException">複数削除。</exception>
        /// <returns>真: 単一削除、偽: 未削除。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static bool DeleteByKeyOrNothing(this IDatabaseExecutor executor, string statement, object? parameter = null)
        {
            ThrowIfNotDelete(statement);

            var result = executor.Execute(statement, parameter);
            if(1 < result) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }

            return result == 1;
        }

        /// <inheritdoc cref="DeleteByKeyOrNothing(IDatabaseExecutor, string, object?)"/>
        public static async Task<bool> DeleteByKeyOrNothingAsync(this IDatabaseExecutor executor, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotDelete(statement);

            var result = await executor.ExecuteAsync(statement, parameter, cancellationToken);
            if(1 < result) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }

            return result == 1;
        }

        #endregion
    }
}
