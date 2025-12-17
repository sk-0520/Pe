using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// DBアクセスに対してラップする。
    /// </summary>
    /// <remarks>
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </remarks>
    public class DatabaseAccessor: DisposerBase, IDatabaseAccessor
    {
        #region variable

        DatabaseContext? _context;

        #endregion

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            DatabaseFactory = databaseFactory;
            LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            Implementation = DatabaseFactory.CreateImplementation();
        }

        #region property

        private Lazy<IDbConnection> LazyConnection { get; set; }

        protected ILogger Logger { get; }
        protected ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// データベース接続が開いているか。
        /// </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        /// データベース接続が一時的に閉じているか。
        /// </summary>
        public bool ConnectionPausing { get; private set; }

        private DatabaseContext Context
        {
            get
            {
                return this._context ??= new DatabaseContext(OpenConnection(), null, Implementation, LoggerFactory);
            }
        }

        #endregion

        #region function

        /// <summary>
        /// DB接続を開く。
        /// </summary>
        /// <returns></returns>
        private IDbConnection OpenConnection()
        {
            if(ConnectionPausing) {
                throw new InvalidOperationException(nameof(ConnectionPausing));
            }
            if(IsOpened) {
                throw new InvalidOperationException(nameof(IsOpened));
            }
            ThrowIfDisposed();

            var con = DatabaseFactory.CreateConnection();
            con.Open();
            IsOpened = true;
            return con;
        }

        #endregion

        #region IDatabaseAccessor

        public IDatabaseImplementation Implementation { get; }

        /// <inheritdoc cref="IDatabaseAccessor.DatabaseFactory"/>
        public IDatabaseFactory DatabaseFactory { get; }

        /// <inheritdoc cref="IDatabaseAccessor.BaseConnection"/>
        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        /// <inheritdoc cref="IDatabaseAccessor.PauseConnection"/>
        public virtual IDisposable PauseConnection()
        {
            ThrowIfDisposed();

            if(!IsOpened || ConnectionPausing) {
                return ActionDisposerHelper.CreateEmpty();
            }

            BaseConnection.Close();
            IsOpened = false;
            ConnectionPausing = true;
            return new ActionDisposer(d => {
                ConnectionPausing = false;
                LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            });
        }

        public IDataReader GetDataReader(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.GetDataReader(statement, parameter);
        }

        public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            return Context.GetDataReaderAsync(statement, parameter, cancellationToken);
        }

        public virtual DataTable GetDataTable(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.GetDataTable(statement, parameter);
        }

        public virtual Task<DataTable> GetDataTableAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.GetDataTableAsync(statement, parameter, cancellationToken);
        }

        public virtual TResult? GetScalar<TResult>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.GetScalar<TResult>(statement, parameter);
        }

        public virtual Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.GetScalarAsync<TResult?>(statement, parameter, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.Query{T}(string, object?, bool)"/>
        public virtual IEnumerable<T> Query<T>(string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            return Context.Query<T>(statement, parameter, buffered);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query(string statement, object? parameter, bool buffered)
        {
            ThrowIfDisposed();

            return Context.Query(statement, parameter, buffered);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync(string, object?, bool, CancellationToken)"/>
        public virtual Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.QueryAsync(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryFirst{T}(string, object?)"/>
        public virtual T QueryFirst<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.QueryFirst<T>(statement, parameter);
        }

        public virtual Task<T> QueryFirstAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.QueryFirstAsync<T>(statement, parameter, cancellationToken);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.QueryFirstOrDefault<T>(statement, parameter);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.QueryFirstOrDefaultAsync<T>(statement, parameter, cancellationToken);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.QuerySingle<T>(statement, parameter);
        }

        public virtual Task<T> QuerySingleAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.QuerySingleAsync<T>(statement, parameter, cancellationToken);
        }

        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.QuerySingleOrDefault<T>(statement, parameter);
        }

        public virtual Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.QuerySingleOrDefaultAsync<T>(statement, parameter, cancellationToken);
        }

        public virtual int Execute(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return Context.Execute(statement, parameter);
        }

        public virtual Task<int> ExecuteAsync(string statement, object? parameter, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Context.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction()
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(BaseConnection, Implementation, LoggerFactory);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <param name="isolationLevel">トランザクションの分離レベル。</param>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(true, this, isolationLevel);
        }

        public virtual IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(true, this);
        }
        public virtual IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(true, this, isolationLevel);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(LazyConnection.IsValueCreated) {
                        BaseConnection.Dispose();
                    }
                }
                IsOpened = false;
                ConnectionPausing = false;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="DatabaseAccessor"/>の型付き<see cref="DatabaseAccessor.BaseConnection"/>実装。
    /// </summary>
    /// <typeparam name="TDbConnection">具象<see cref="DatabaseAccessor.BaseConnection"/>。</typeparam>
    public class DatabaseAccessor<TDbConnection>: DatabaseAccessor
        where TDbConnection : IDbConnection
    {
        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILoggerFactory loggerFactory)
            : base(connectionFactory, loggerFactory)
        { }

        #region property

        /// <summary>
        /// 接続元。
        /// </summary>
        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
