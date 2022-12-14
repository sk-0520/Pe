using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite
{
    internal sealed class SqliteReadOnlyTransaction: IDatabaseTransaction
    {
        public SqliteReadOnlyTransaction(IDatabaseAccessor databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Implementation = DatabaseAccessor.DatabaseFactory.CreateImplementation();
        }

        public SqliteReadOnlyTransaction(IDatabaseAccessor databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = databaseAccessor;
            Implementation = DatabaseAccessor.DatabaseFactory.CreateImplementation();
        }

        #region property

        IDatabaseAccessor DatabaseAccessor { get; }

        #endregion

        #region IDatabaseTransaction

        public IDatabaseContext Context => this;
        public IDbTransaction Transaction => throw new NotSupportedException();

        public IDatabaseImplementation Implementation { get; }

        public void Commit()
        {
            throw new NotSupportedException();
        }

        public void Rollback()
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public int Execute(string statement, object? parameter = null)
        {
            throw new NotSupportedException();
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            return DatabaseAccessor.GetDataTable(statement, parameter);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            return DatabaseAccessor.Query<T>(statement, parameter, buffered);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            return DatabaseAccessor.Query<dynamic>(statement, parameter, buffered);
        }

        public T QueryFirst<T>(string statement, object? parameter = null)
        {
            return DatabaseAccessor.QueryFirst<T>(statement, parameter);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            return DatabaseAccessor.QueryFirstOrDefault<T>(statement, parameter);
        }

        public T QuerySingle<T>(string statement, object? parameter = null)
        {
            return DatabaseAccessor.QuerySingle<T>(statement, parameter);
        }

        [return: MaybeNull]
        public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
        {
            return DatabaseAccessor.QuerySingleOrDefault<T>(statement, parameter, this);
        }

        #endregion
    }

    public class SqliteAccessor: DatabaseAccessor<SQLiteConnection>
    {
        public SqliteAccessor(IDatabaseFactory databaseFactory, ILogger logger)
            : base(databaseFactory, logger)
        { }

        public SqliteAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            : base(databaseFactory, loggerFactory)
        { }

        #region function

        /// <summary>
        /// ?????????????????????????????????
        /// </summary>
        /// <param name="sourceName">????????????DB??????</param>
        /// <param name="destination">???????????????<see cref="SqliteAccessor"/></param>
        /// <param name="destinationName">????????????DB??????</param>
        public void CopyTo(string sourceName, SqliteAccessor destination, string destinationName)
        {
            Connection.BackupDatabase(destination.Connection, destinationName, sourceName, -1, null, -1);
        }

        #endregion

        #region DatabaseAccessor

        public override IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new SqliteReadOnlyTransaction(this);
        }
        public override IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new SqliteReadOnlyTransaction(this, isolationLevel);
        }

        #endregion

    }
}
