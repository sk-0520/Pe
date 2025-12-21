using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public abstract class DatabaseContextBase: DisposerBase, IDatabaseContext
    {
        protected DatabaseContextBase(IDbConnection dbConnection, IDbTransaction? dbTransaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            DbConnection = dbConnection;
            DbTransaction = dbTransaction;
            Implementation = implementation;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected virtual IDbConnection DbConnection { get; private set; }
        protected IDbTransaction? DbTransaction { get; private set; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        protected List<IDatabaseStatementMiddleware> StatementMiddlewares { get; } = [];
        protected List<IDatabaseReaderMiddleware> ReaderMiddlewares { get; } = [];
        protected List<IDatabaseExecutorMiddleware> ExecutorMiddlewares { get; } = [];

        #endregion

        #region function

        /// <summary>
        /// 問い合わせ文をログ出力。
        /// </summary>
        /// <remarks>
        /// <para>あくまで実行するための文をログに出すだけで実際に実行される文ではない。</para>
        /// </remarks>
        /// <param name="statement">問い合わせ文。</param>
        /// <param name="parameter">パラメータ。</param>
        protected virtual void LoggingStatement(string statement, object? parameter)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        protected virtual void LoggingExecuteScalarResult<TResult>(TResult result, TimeSpan elapsedTime)
        { }

        /// <summary>
        /// 単体結果の問い合わせ結果のログ出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        protected virtual void LoggingQueryResult<T>([MaybeNull] T result, TimeSpan elapsedTime)
        { }

        /// <summary>
        /// 複数結果の問い合わせ結果のログ出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="buffered">偽の場合、<paramref name="result"/>に全数は存在しない。</param>
        /// <param name="elapsedTime"></param>
        protected virtual void LoggingQueryResults<T>(IEnumerable<T> result, bool buffered, TimeSpan elapsedTime)
        { }

        /// <summary>
        /// 実行結果のログ出力。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDatabaseExecutor.Execute(string, object?)"/>で使用される。</para>
        /// </remarks>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        protected virtual void LoggingExecuteResult(int result, TimeSpan elapsedTime)
        { }

        /// <summary>
        /// 問い合わせ結果のログ出力。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDatabaseReader.GetDataTable(string, object?)"/>で使用される。</para>
        /// </remarks>
        /// <param name="table"></param>
        /// <param name="elapsedTime"></param>
        protected virtual void LoggingDataTable(DataTable table, TimeSpan elapsedTime)
        { }

        #endregion

        #region IDatabaseContext

        public IDatabaseImplementation Implementation { get; }

        public IDataReader GetDataReader(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var result = DbConnection.ExecuteReader(formattedStatement, parameter, DbTransaction);
            return result;
        }

        public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            var result = DbConnection.ExecuteReaderAsync(command);
            return result;
        }

        public virtual DataTable GetDataTable(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = Stopwatch.GetTimestamp();
            using(var reader = GetDataReader(statement, parameter)) {
                dataTable.Load(reader);
            }
            LoggingDataTable(dataTable, Stopwatch.GetElapsedTime(startTime));

            return dataTable;
        }

        public async virtual Task<DataTable> GetDataTableAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = Stopwatch.GetTimestamp();
            using(var reader = await GetDataReaderAsync(statement, parameter, cancellationToken)) {
                dataTable.Load(reader);
            }
            LoggingDataTable(dataTable, Stopwatch.GetElapsedTime(startTime));

            return dataTable;
        }

        public virtual TResult? GetScalar<TResult>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.ExecuteScalar<TResult>(formattedStatement, parameter, DbTransaction);
            LoggingExecuteScalarResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );
            var result = await DbConnection.ExecuteScalarAsync<TResult>(command);
            LoggingExecuteScalarResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.Query{T}(IDatabaseTransaction?, string, object?, bool)"/>
        public virtual IEnumerable<T> Query<T>(string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.Query<T>(formattedStatement, parameter, DbTransaction, buffered);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QueryAsync<T>(command);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query(string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.Query(formattedStatement, parameter, DbTransaction, buffered);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QueryAsync(command);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryFirst{T}(IDatabaseTransaction?, string, object?)"/>
        public virtual T QueryFirst<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.QueryFirst<T>(formattedStatement, parameter, DbTransaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<T> QueryFirstAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QueryFirstAsync<T>(command);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        [return: MaybeNull]
        public virtual T QueryFirstOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.QueryFirstOrDefault<T>(formattedStatement, parameter, DbTransaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            return DbConnection.QueryFirstOrDefaultAsync<T?>(command).ContinueWith(t => {
                LoggingQueryResult(t.Result, Stopwatch.GetElapsedTime(startTime));
                return t.Result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.QuerySingle<T>(formattedStatement, parameter, DbTransaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<T> QuerySingleAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QuerySingleAsync<T>(command);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.QuerySingleOrDefault<T>(formattedStatement, parameter, DbTransaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );
            var result = await DbConnection.QuerySingleOrDefaultAsync<T>(command);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual int Execute(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = DbConnection.Execute(formattedStatement, parameter, DbTransaction);
            LoggingExecuteResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<int> ExecuteAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );
            var result = await DbConnection.ExecuteAsync(command);
            LoggingExecuteResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                // DatabaseContext 自体に所有権はないため参照だけ外す。
                // DatabaseAccessor, DatabaseTransaction などがそれぞれ責任もって処理する。
                DbTransaction = null;
                DbConnection = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
