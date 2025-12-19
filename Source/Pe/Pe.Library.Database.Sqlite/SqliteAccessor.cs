using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Connection.BackupDatabase(destination.Connection, destinationName, sourceName, -1, null, -1);
        }

        #endregion

        #region DatabaseAccessor

        public override IDatabaseTransaction BeginReadOnlyTransaction()
        {
            // SQLite は複数スレッドでトランザクション開くと死ぬのでトランザクション実体なしで仮想的に対応。
            return BeginTransactionCore(null, true);
        }

        public override IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            return BeginReadOnlyTransaction();
        }

        #endregion
    }
}
