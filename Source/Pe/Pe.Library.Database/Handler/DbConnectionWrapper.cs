using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public class DbConnectionWrapper: DbConnection
    {
        public DbConnectionWrapper(DbConnection dbConnection, HandlerCollection handlerCollection)
        {
            Raw = dbConnection;
            HandlerCollection = handlerCollection;
        }


        #region property

        public DbConnection Raw { get; private set; }
        public HandlerCollection HandlerCollection { get; }

        #endregion

        #region DbConnection

        [AllowNull]
        public override string ConnectionString
        {
            get => Raw.ConnectionString;
            set => Raw.ConnectionString = value;
        }

        public override string Database => Raw.Database;

        public override string DataSource => Raw.DataSource;

        public override string ServerVersion => Raw.ServerVersion;

        public override ConnectionState State => Raw.State;

        public override void ChangeDatabase(string databaseName)
        {
            Raw.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            Raw.Close();
        }

        public override void Open()
        {
            Raw.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return Raw.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            var command = new DbCommandWrapper(Raw.CreateCommand(), HandlerCollection);
            return command;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                Raw?.Dispose();
                Raw = null!;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
