using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public class DbConnectionWrapper: DisposerBase, IDbConnection
    {
        public DbConnectionWrapper(IDbConnection connection)
        {
            Connection = connection;
        }

        #region property

        public IDbConnection Connection { get; private set; }

        #endregion

        #region IDbConnection

        [AllowNull]
        public string ConnectionString
        {
            get => Connection.ConnectionString;
            set => throw new NotSupportedException();
        }

        public int ConnectionTimeout => Connection.ConnectionTimeout;

        public string Database => Connection.Database;

        public ConnectionState State => Connection.State;

        public IDbTransaction BeginTransaction() => Connection.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => Connection.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => Connection.ChangeDatabase(databaseName);

        public IDbCommand CreateCommand() => new DbCommandWrapper(Connection.CreateCommand());
        public void Open() => Connection.Open();
        public void Close() => Connection.Close();

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Connection.Dispose();
                    Connection = null!;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    public class DbCommandWrapper: DisposerBase, IDbCommand
    {
        public DbCommandWrapper(IDbCommand command)
        {
            Command = command;
        }

        #region property

        private IDbCommand Command { get; set; }

        #endregion

        #region IDbCommand

        public IDbConnection? Connection
        {
            get => Command.Connection;
            set => Command.Connection = value;
        }

        public IDbTransaction? Transaction
        {
            get => Command.Transaction;
            set => Command.Transaction = value;
        }

        [AllowNull]
        public string CommandText
        {
            get => Command.CommandText;
            set => Command.CommandText = value;
        }

        public int CommandTimeout
        {
            get => Command.CommandTimeout;
            set => Command.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => Command.CommandType;
            set => Command.CommandType = value;
        }

        public IDataParameterCollection Parameters => Command.Parameters;

        public UpdateRowSource UpdatedRowSource
        {
            get => Command.UpdatedRowSource;
            set => Command.UpdatedRowSource = value;
        }

        public void Cancel() => Command.Cancel();

        public IDbDataParameter CreateParameter() => Command.CreateParameter();

        public int ExecuteNonQuery() => Command.ExecuteNonQuery();

        public IDataReader ExecuteReader() => Command.ExecuteReader();

        public IDataReader ExecuteReader(CommandBehavior behavior) => Command.ExecuteReader(behavior);

        public object? ExecuteScalar() => Command.ExecuteScalar();

        public void Prepare() => Command.Prepare();

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Command.Dispose();
                    Command = null!;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}
