using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public class DbConnectionWrapper: DbConnection
    {
        public DbConnectionWrapper(DbConnection dbConnection)
        {
            Raw = dbConnection;
        }


        #region property

        public DbConnection Raw { get; private set; }

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
            var command = new DbCommandWrapper(Raw.CreateCommand());
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

    public class DbCommandWrapper: DbCommand
    {
        static PropertyInfo DbParameterCollectionPropertyInfo;
        static DbCommandWrapper()
        {
            var type = typeof(DbCommand);
            var property = type.GetProperty("DbParameterCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            DbParameterCollectionPropertyInfo = property!;
        }

        public DbCommandWrapper(IDbCommand command)
        {
            Raw = command;
        }

        #region property

        public IDbCommand Raw { get; private set; }

        #endregion

        #region DbCommand

        [AllowNull]
        public override string CommandText
        {
            get => Raw.CommandText;
            set => Raw.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => Raw.CommandTimeout;
            set => Raw.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => Raw.CommandType;
            set => Raw.CommandType = value;
        }

        public override bool DesignTimeVisible
        {
            get => ((DbCommand)Raw).DesignTimeVisible;
            set => ((DbCommand)Raw).DesignTimeVisible = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => Raw.UpdatedRowSource;
            set => Raw.UpdatedRowSource = value;
        }

        protected override DbConnection? DbConnection
        {
            get => (DbConnection?)Raw.Connection;
            set => Raw.Connection = value;
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return (DbParameterCollection)DbParameterCollectionPropertyInfo.GetValue(Raw)!;
            }
        }

        protected override DbTransaction? DbTransaction
        {
            get => (DbTransaction?)Raw.Transaction;
            set => Raw.Transaction = value;
        }

        public override void Cancel()
        {
            Raw.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            return Raw.ExecuteNonQuery();
        }

        public override object? ExecuteScalar()
        {
            return Raw.ExecuteScalar();
        }

        public override void Prepare()
        {
            Raw.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return (DbParameter)Raw.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return (DbDataReader)Raw.ExecuteReader(behavior);
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
