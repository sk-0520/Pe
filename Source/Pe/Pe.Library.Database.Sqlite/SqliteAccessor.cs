using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Sqlite
{
    public class SqliteAccessor: DatabaseAccessor<SQLiteConnection>
    {
        public SqliteAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            : base(databaseFactory, loggerFactory)
        { }

        #region function

        /// <summary>
        /// データベースのコピー。
        /// </summary>
        /// <param name="sourceName">コピー元DB名。</param>
        /// <param name="destination">コピー先の<see cref="SqliteAccessor"/></param>
        /// <param name="destinationName">コピー先DB名。</param>
        public void CopyTo(string sourceName, SqliteAccessor destination, string destinationName)
        {
            DbConnection.BackupDatabase(destination.DbConnection, destinationName, sourceName, -1, null, -1);
        }

        #endregion

        #region DatabaseAccessor

        public override IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            // SQLite は複数スレッドでトランザクション開くと死ぬのでトランザクション実体なしで仮想的に対応。
            return new ReadOnlyDatabaseTransaction(BaseDbConnection, null, Implementation, LoggerFactory);
        }

        public override IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            // `SqliteAccessor.BeginReadOnlyTransaction()` でトランザクションは仮想的に扱うことにより、
            // 実際のトランザクションは開かないため isolationLevel は意味を持たない。
            return BeginReadOnlyTransaction();
        }

        #endregion
    }
}
