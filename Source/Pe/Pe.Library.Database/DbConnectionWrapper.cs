using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

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
            set => throw new NotSupportedException();
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
        public DbCommandWrapper(IDbCommand command)
        {
            Raw = command;
            //nameof(DbCommand.CommandText);
        }

        #region property

        public IDbCommand Raw { get; }

        #endregion

        #region DbCommand

        [AllowNull]
        public override string CommandText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CommandType CommandType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool DesignTimeVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override DbConnection? DbConnection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override DbParameterCollection DbParameterCollection => throw new NotImplementedException();

        protected override DbTransaction? DbTransaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object? ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
