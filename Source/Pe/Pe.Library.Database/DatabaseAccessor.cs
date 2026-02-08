using System;
using System.Data;
using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// DBアクセスに対してラップする。
    /// </summary>
    /// <remarks>
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </remarks>
    public class DatabaseAccessor: DatabaseContextBase, IDatabaseAccessor
    {
        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            // 第一引数は本クラスの DbConnection が使用されるので null で問題ない
            : base(null!, null, databaseFactory.CreateImplementation(), loggerFactory)
        {
            DatabaseFactory = databaseFactory;
            LazyConnection = new Lazy<DbConnectionWrapper>(OpenConnection);
        }

        #region property

        private Lazy<DbConnectionWrapper> LazyConnection { get; set; }

        /// <summary>
        /// データベース接続が開いているか。
        /// </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        /// データベース接続が一時的に閉じているか。
        /// </summary>
        public bool ConnectionPausing { get; private set; }

        #endregion

        #region function

        private DbConnectionWrapper CreateDbConnectionWrapper(IDbConnection connection)
        {
            if(connection is DbConnection dbConnection) {
                return new DbConnectionWrapper(dbConnection, MiddlewareCollection);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// DB接続を開く。
        /// </summary>
        /// <returns></returns>
        private DbConnectionWrapper OpenConnection()
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
            return CreateDbConnectionWrapper(con);
        }

        private IDatabaseTransaction BeginTransactionCore(IDbTransaction? transaction, bool isReadonly)
        {
            if(isReadonly) {
                return new ReadOnlyDatabaseTransaction(BaseDbConnection, transaction, Implementation, LoggerFactory);
            }
            return new DatabaseTransaction(BaseDbConnection, transaction, Implementation, LoggerFactory);
        }


        #endregion

        #region IDatabaseAccessor

        protected override IDbConnection DbConnection
        {
            get
            {
                ThrowIfDisposed();

                return LazyConnection.Value;
            }
        }


        /// <inheritdoc cref="IDatabaseAccessor.DatabaseFactory"/>
        public IDatabaseFactory DatabaseFactory { get; }

        /// <inheritdoc cref="IDatabaseAccessor.BaseDbConnection"/>
        public virtual DbConnectionWrapper BaseDbConnection => LazyConnection.Value;
        IDbConnection IDatabaseAccessor.BaseDbConnection => BaseDbConnection;


        /// <inheritdoc cref="IDatabaseAccessor.PauseConnection"/>
        public virtual IDisposable PauseConnection()
        {
            ThrowIfDisposed();

            if(!IsOpened || ConnectionPausing) {
                return ActionDisposerHelper.CreateEmpty();
            }

            BaseDbConnection.Close();
            IsOpened = false;
            ConnectionPausing = true;
            return new ActionDisposer(d => {
                ConnectionPausing = false;
                LazyConnection = new Lazy<DbConnectionWrapper>(OpenConnection);
            });
        }


        /// <inheritdoc cref="IDatabaseAccessor.BeginTransaction"/>
        public virtual IDatabaseTransaction BeginTransaction()
        {
            ThrowIfDisposed();

            var transaction = BaseDbConnection.BeginTransaction();
            return BeginTransactionCore(transaction, false);
        }

        /// <inheritdoc cref="IDatabaseAccessor.BeginTransaction(IsolationLevel)"/>
        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            var transaction = BaseDbConnection.BeginTransaction(isolationLevel);
            return BeginTransactionCore(transaction, false);
        }

        /// <inheritdoc cref="IDatabaseAccessor.BeginReadOnlyTransaction"/>
        public virtual IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            var transaction = BaseDbConnection.BeginTransaction();
            return BeginTransactionCore(transaction, true);
        }
        /// <inheritdoc cref="IDatabaseAccessor.BeginReadOnlyTransaction(IsolationLevel)"/>
        public virtual IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            var transaction = BaseDbConnection.BeginTransaction(isolationLevel);
            return BeginTransactionCore(transaction, true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(LazyConnection.IsValueCreated) {
                        BaseDbConnection.Dispose();
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
    /// <see cref="DatabaseAccessor"/>の型付き<see cref="DatabaseAccessor.BaseDbConnection"/>実装。
    /// </summary>
    /// <typeparam name="TDbConnection">具象<see cref="DatabaseAccessor.BaseDbConnection"/>。</typeparam>
    public class DatabaseAccessor<TDbConnection>: DatabaseAccessor
        where TDbConnection : DbConnection
    {
        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILoggerFactory loggerFactory)
            : base(connectionFactory, loggerFactory)
        { }

        #region property

        /// <summary>
        /// 接続元。
        /// </summary>
        public new TDbConnection DbConnection => (TDbConnection)BaseDbConnection.BaseConnection;

        #endregion
    }
}
