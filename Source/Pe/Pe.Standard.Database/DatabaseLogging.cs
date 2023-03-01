using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    internal sealed class LogDbConnection: IDbConnection
    {
        public LogDbConnection(IDbConnection connection, LogLevel logLevel, ILogger logger)
        {
            Connection = connection;
            LogLevel = logLevel;
            Logger = logger;
        }

        #region property
        internal IDbConnection Connection { get; }
        private LogLevel LogLevel { get; }
        private ILogger Logger { get; }

        #endregion

        #region IDbConnection

        public string ConnectionString
        {
            get => Connection.ConnectionString;
            set => Connection.ConnectionString = value;
        }

        public int ConnectionTimeout => Connection.ConnectionTimeout;

        public string Database => Connection.Database;

        public ConnectionState State => Connection.State;


        public IDbTransaction BeginTransaction() => Connection.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => Connection.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => Connection.ChangeDatabase(databaseName);

        public void Close() => Connection.Close();

        public IDbCommand CreateCommand() => new LogDbCommand(Connection.CreateCommand(), LogLevel, Logger);

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Open() => Connection.Open();

        #endregion
    }

    internal sealed class LogDbCommand: IDbCommand
    {
        public LogDbCommand(IDbCommand command, LogLevel logLevel, ILogger logger)
        {
            Command = command;
            LogLevel = logLevel;
            Logger = logger;
        }

        #region property

        private IDbCommand Command { get; }
        private LogLevel LogLevel { get; }
        private ILogger Logger { get; }

        #endregion

        #region IDbCommand

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
        public IDbConnection Connection
        {
            get => Command.Connection;
            set => Command.Connection = value;
        }

        public IDataParameterCollection Parameters => Command.Parameters;

        public IDbTransaction Transaction
        {
            get => Command.Transaction;
            set => Command.Transaction = value;
        }
        public UpdateRowSource UpdatedRowSource
        {
            get => Command.UpdatedRowSource;
            set => Command.UpdatedRowSource = value;
        }

        public void Cancel() => Command.Cancel();

        public IDbDataParameter CreateParameter() => Command.CreateParameter();

        public void Dispose()
        {
            Command.Dispose();
            GC.SuppressFinalize(this);
        }

        public int ExecuteNonQuery()
        {
            return Command.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader()
        {
            return Command.ExecuteReader();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return Command.ExecuteReader(behavior);
        }

        public object ExecuteScalar()
        {
            return Command.ExecuteScalar();
        }

        public void Prepare()
        {
            Command.Prepare();
        }

        #endregion
    }
}
