using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// <see cref="DbConnection"/> ラッパー。
    /// </summary>
    /// <remarks><see cref="IDbConnection"/> は知らん。</remarks>
    public class DbConnectionWrapper: DbConnection
    {
        public DbConnectionWrapper(DbConnection dbConnection, MiddlewareCollection middlewareCollection)
        {
            BaseConnection = dbConnection;
            MiddlewareCollection = middlewareCollection;
        }

        #region property

        /// <summary>
        /// 元の <see cref="DbConnection"/>。
        /// </summary>
        public DbConnection BaseConnection { get; private set; }
        /// <inheritdoc cref="ContentTypeTextNet.Pe.Library.Database.Handler.MiddlewareCollection"/>
        public MiddlewareCollection MiddlewareCollection { get; }

        #endregion

        #region DbConnection

        [AllowNull]
        public override string ConnectionString
        {
            get => BaseConnection.ConnectionString;
            set => BaseConnection.ConnectionString = value;
        }

        public override string Database => BaseConnection.Database;

        public override string DataSource => BaseConnection.DataSource;

        public override string ServerVersion => BaseConnection.ServerVersion;

        public override ConnectionState State => BaseConnection.State;

        public override void ChangeDatabase(string databaseName)
        {
            BaseConnection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            BaseConnection.Close();
        }

        public override void Open()
        {
            BaseConnection.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return BaseConnection.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            var command = new DbCommandWrapper(BaseConnection.CreateCommand(), MiddlewareCollection);
            return command;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                BaseConnection?.Dispose();
                BaseConnection = null!;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
