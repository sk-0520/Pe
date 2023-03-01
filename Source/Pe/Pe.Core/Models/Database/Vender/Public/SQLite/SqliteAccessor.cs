using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite
{
    public interface ISqliteAccessOptions: IDatabaseAccessOptions
    { }

    public class SqliteAccessOptions: DatabaseAccessOptions, ISqliteAccessOptions
    { }

    public class SqliteAccessor: DatabaseAccessor<SQLiteConnection>
    {
        public SqliteAccessor(IDatabaseFactory databaseFactory, ISqliteAccessOptions options, ILogger logger)
            : base(databaseFactory, options, logger)
        { }

        public SqliteAccessor(IDatabaseFactory databaseFactory, ISqliteAccessOptions options, ILoggerFactory loggerFactory)
            : base(databaseFactory, options, loggerFactory)
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
            Connection.BackupDatabase(destination.Connection, destinationName, sourceName, -1, null, -1);
        }

        #endregion

        #region DatabaseAccessor

        public override IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(false, this);
        }
        public override IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(false, this, isolationLevel);
        }

        #endregion
    }
}
