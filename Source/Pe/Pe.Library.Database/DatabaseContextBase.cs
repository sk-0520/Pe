using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    [SuppressMessage("Low Code Smell", "S4136:Method overloads should be grouped together")]
    public abstract class DatabaseContextBase: DisposerBase, IDatabaseContext
    {
        protected DatabaseContextBase(IDbConnection dbConnection, IDbTransaction? dbTransaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            DbConnection = dbConnection;
            DbTransaction = dbTransaction;
            Implementation = implementation;
            MiddlewareCollection = new MiddlewareCollection();
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected virtual IDbConnection DbConnection { get; private set; }
        protected IDbTransaction? DbTransaction { get; private set; }

        /// <summary>
        /// ミドルウェア一覧。
        /// </summary>
        public MiddlewareCollection MiddlewareCollection { get; set; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region function

        protected StatementPipeline CreateStatementPipeline()
        {
            var pipeline = new StatementPipeline();
            pipeline.UseRange(MiddlewareCollection.Statements);
            return pipeline;
        }

        protected virtual IStatementHandler CreateStatementProcess()
        {
            return new DefaultStatementProcess();
        }

        protected string FormatStatement(string statement)
        {
            var pipeline = CreateStatementPipeline();
            var process = CreateStatementProcess();
            var handler = pipeline.Build(process);
            return handler.Invoke(statement);
        }

        #endregion

        #region IDatabaseContext

        public IDatabaseImplementation Implementation { get; }

        public IDataReader GetDataReader(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.ExecuteReader(formattedStatement, parameter, DbTransaction);
            return result;
        }

        public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
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

            var dataTable = new DataTable();
            using(var reader = GetDataReader(statement, parameter)) {
                dataTable.Load(reader);
            }

            return dataTable;
        }

        public async virtual Task<DataTable> GetDataTableAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var dataTable = new DataTable();
            using(var reader = await GetDataReaderAsync(statement, parameter, cancellationToken)) {
                dataTable.Load(reader);
            }

            return dataTable;
        }

        public virtual TResult? GetScalar<TResult>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.ExecuteScalar<TResult>(formattedStatement, parameter, DbTransaction);

            return result;
        }

        public virtual async Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );
            var result = await DbConnection.ExecuteScalarAsync<TResult>(command);

            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.Query{T}(string, object?, bool)"/>
        public virtual IEnumerable<T> Query<T>(string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.Query<T>(formattedStatement, parameter, DbTransaction, buffered);

            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QueryAsync<T>(command);
            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query(string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.Query(formattedStatement, parameter, DbTransaction, buffered);

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QueryAsync(command);
            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryFirst{T}(IDatabaseTransaction?, string, object?)"/>
        public virtual T QueryFirst<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.QueryFirst<T>(formattedStatement, parameter, DbTransaction);

            return result;
        }

        public virtual async Task<T> QueryFirstAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QueryFirstAsync<T>(command);
            return result;
        }

        [return: MaybeNull]
        public virtual T QueryFirstOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.QueryFirstOrDefault<T>(formattedStatement, parameter, DbTransaction);

            return result;
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            return DbConnection.QueryFirstOrDefaultAsync<T?>(command);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.QuerySingle<T>(formattedStatement, parameter, DbTransaction);

            return result;
        }

        public virtual async Task<T> QuerySingleAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );

            var result = await DbConnection.QuerySingleAsync<T>(command);
            return result;
        }

        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.QuerySingleOrDefault<T>(formattedStatement, parameter, DbTransaction);

            return result;
        }

        public virtual async Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );
            var result = await DbConnection.QuerySingleOrDefaultAsync<T>(command);

            return result;
        }

        public virtual int Execute(string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var result = DbConnection.Execute(formattedStatement, parameter, DbTransaction);

            return result;
        }

        public virtual async Task<int> ExecuteAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = FormatStatement(statement);

            var command = new CommandDefinition(
                formattedStatement,
                parameters: parameter,
                transaction: DbTransaction,
                cancellationToken: cancellationToken
            );
            var result = await DbConnection.ExecuteAsync(command);

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
