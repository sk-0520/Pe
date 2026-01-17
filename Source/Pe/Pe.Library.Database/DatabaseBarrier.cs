using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    file class DatabaseBarrierTransaction: DatabaseTransaction
    {
        public DatabaseBarrierTransaction(IDisposable locker, IDbConnection connection, IDatabaseTransaction transaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(connection, transaction.DbTransaction, implementation, loggerFactory)
        {
            Locker = locker;
            BaseTransaction = transaction;
        }

        #region property

        private IDisposable Locker { get; [Unused(UnusedKinds.Dispose)] set; }
        // トランザクションは参照だけ保持しておく
        private IDatabaseTransaction BaseTransaction { get; set; }

        #endregion

        #region function
        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            // 解放順序としてトランザクション処理後にロック解除が必要なためトランザクション処理を担う基底を先に呼ぶ
            var isDisposed = IsDisposed;
            base.Dispose(disposing);
            // BaseTransaction の解放すべき対象は基底で解放されているので参照だけ解除する
            BaseTransaction = null!;

            if(!isDisposed) {
                if(disposing) {
                    Locker.Dispose();
                }
                Locker = null!;
            }
        }

        #endregion
    }

    file sealed class ReadOnlyDatabaseBarrierTransaction: DatabaseBarrierTransaction
    {
        public ReadOnlyDatabaseBarrierTransaction(IDisposable locker, IDbConnection connection, IDatabaseTransaction transaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(locker, connection, transaction, implementation, loggerFactory)
        {
            //NOP
        }

        #region DatabaseBarrierTransaction

        public override void Commit() => throw new NotSupportedException();

        public override int Execute(string statement, object? parameter) => throw new NotSupportedException();

        public override Task<int> ExecuteAsync(string statement, object? parameter, CancellationToken cancellationToken) => throw new NotSupportedException();

        #endregion
    }

    /// <inheritdoc cref="IDatabaseBarrier" />
    public class DatabaseBarrier: IDatabaseBarrier
    {
        public DatabaseBarrier(IDatabaseAccessor accessor, IReadWriteLockHelper locker, ILoggerFactory loggerFactory)
        {
            Accessor = accessor;
            Locker = locker;
            LoggerFactory = loggerFactory;
        }

        #region property

        protected IDatabaseAccessor Accessor { get; }
        protected IReadWriteLockHelper Locker { get; }
        protected ILoggerFactory LoggerFactory { get; }
        #endregion

        #region IDatabaseBarrier

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite" />
        /// </summary>
        /// <remarks>
        /// <para><see cref="ReadWriteLockHelper.WaitWriteByDefaultTimeout()"/>が規定時間。</para>
        /// </remarks>
        public virtual IDatabaseTransaction WaitWrite()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, Accessor.BaseDbConnection, transaction, Accessor.Implementation, LoggerFactory);
            return result;
        }

        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite(TimeSpan)" />
        public virtual IDatabaseTransaction WaitWrite(TimeSpan timeout)
        {
            var locker = Locker.WaitWrite(timeout);
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, Accessor.BaseDbConnection, transaction, Accessor.Implementation, LoggerFactory);
            return result;
        }

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitRead" />
        /// </summary>
        /// <remarks>
        /// <para><see cref="ReadWriteLockHelper.WaitReadByDefaultTimeout()"/>が規定時間。</para>
        /// </remarks>
        /// <returns></returns>
        public virtual IDatabaseTransaction WaitRead()
        {
            var locker = Locker.WaitReadByDefaultTimeout();
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new ReadOnlyDatabaseBarrierTransaction(locker, Accessor.BaseDbConnection, transaction, Accessor.Implementation, LoggerFactory);
            return result;
        }

        /// <inheritdoc cref="IDatabaseBarrier.WaitRead(TimeSpan)" />
        public virtual IDatabaseTransaction WaitRead(TimeSpan timeout)
        {
            var locker = Locker.WaitRead(timeout);
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new ReadOnlyDatabaseBarrierTransaction(locker, Accessor.BaseDbConnection, transaction, Accessor.Implementation, LoggerFactory);
            return result;
        }

        #endregion
    }
}
