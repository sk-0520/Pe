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
    public class DatabaseContext: DisposerBase, IDatabaseContext
    {
        public DatabaseContext(IDbConnection connection, IDbTransaction? transaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Connection = connection;
            Transaction = transaction;
            Implementation = implementation;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDbConnection Connection { get; set; }
        protected IDbTransaction? Transaction { get; set; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get;}

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

        public IDataReader GetDataReader( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var result = Connection.ExecuteReader(formattedStatement, parameter, Transaction);
            return result;
        }

        public Task<IDataReader> GetDataReaderAsync( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );

            var result = Connection.ExecuteReaderAsync(command);
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

        public async virtual Task<DataTable> GetDataTableAsync( string statement, object? parameter, CancellationToken cancellationToken)
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

        public virtual TResult? GetScalar<TResult>( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.ExecuteScalar<TResult>(formattedStatement, parameter, Transaction);
            LoggingExecuteScalarResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<TResult?> GetScalarAsync<TResult>( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );
            var result = await Connection.ExecuteScalarAsync<TResult>(command);
            LoggingExecuteScalarResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.Query{T}(IDatabaseTransaction?, string, object?, bool)"/>
        public virtual IEnumerable<T> Query<T>( string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.Query<T>(formattedStatement, parameter, Transaction, buffered);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>( string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );

            var result = await Connection.QueryAsync<T>(command);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query( string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.Query(formattedStatement, parameter, Transaction, buffered);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<dynamic>> QueryAsync( string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );

            var result = await Connection.QueryAsync(command);
            LoggingQueryResults(result, buffered, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryFirst{T}(IDatabaseTransaction?, string, object?)"/>
        public virtual T QueryFirst<T>( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.QueryFirst<T>(formattedStatement, parameter, Transaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<T> QueryFirstAsync<T>( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );

            var result = await Connection.QueryFirstAsync<T>(command);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        [return: MaybeNull]
        public virtual T QueryFirstOrDefault<T>( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.QueryFirstOrDefault<T>(formattedStatement, parameter, Transaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );

            return Connection.QueryFirstOrDefaultAsync<T?>(command).ContinueWith(t => {
                LoggingQueryResult(t.Result, Stopwatch.GetElapsedTime(startTime));
                return t.Result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public virtual T QuerySingle<T>( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.QuerySingle<T>(formattedStatement, parameter, Transaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<T> QuerySingleAsync<T>( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );

            var result = await Connection.QuerySingleAsync<T>(command);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));
            return result;
        }

        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.QuerySingleOrDefault<T>(formattedStatement, parameter, Transaction);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<T?> QuerySingleOrDefaultAsync<T>( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );
            var result = await Connection.QuerySingleOrDefaultAsync<T>(command);
            LoggingQueryResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual int Execute( string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var result = Connection.Execute(formattedStatement, parameter, Transaction);
            LoggingExecuteResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        public virtual async Task<int> ExecuteAsync( string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = Stopwatch.GetTimestamp();
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );
            var result = await Connection.ExecuteAsync(command);
            LoggingExecuteResult(result, Stopwatch.GetElapsedTime(startTime));

            return result;
        }

        #endregion
    }
}
