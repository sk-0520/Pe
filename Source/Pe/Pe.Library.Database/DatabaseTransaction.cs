using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// トランザクション中の処理をサポート。
    /// </summary>
    /// <remarks>
    /// <para>コミットと <see cref="IDatabaseExecutor"/> を見る感じなのでトランザクションの実体(<see cref="IDbTransaction"/>)は <see langword="null"/> でも構わない。</para>
    /// <para>基本的にはユーザーコードで登場せず <see cref="IDatabaseContext"/>がすべて上位から良しなに対応する。</para>
    /// </remarks>
    public class DatabaseTransaction: DatabaseContext, IDatabaseTransaction
    {
        public DatabaseTransaction(IDbConnection connection, IDbTransaction? transaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(connection, transaction, implementation, loggerFactory)
        {
            //NOP
        }

        #region property

        public bool Committed { get; private set; }

        #endregion

        #region IDatabaseTransaction

        public virtual void Commit()
        {
            Debug.Assert(Transaction is not null);

            ThrowIfDisposed();

            Transaction.Commit();
            Committed = true;
        }

        public virtual void Rollback()
        {
            ThrowIfDisposed();

            if(Transaction is not null && Transaction.Connection is not null) {
                Transaction.Rollback();
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(!Committed) {
                        Rollback();
                    }
                    if(Transaction is not null) {
                        Transaction.Dispose();
                    }
                    Transaction = null;
                    Connection = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 読み込み専用トランザクション。
    /// </summary>
    public sealed class ReadOnlyDatabaseTransaction: DatabaseTransaction
    {
        public ReadOnlyDatabaseTransaction(IDbConnection connection, IDbTransaction? transaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(connection, transaction, implementation, loggerFactory)
        {
            //NOP
        }

        #region DatabaseTransaction

        public override void Commit() => throw new NotSupportedException();

        public override int Execute(string statement, object? parameter) => throw new NotSupportedException();

        public override Task<int> ExecuteAsync(string statement, object? parameter, CancellationToken cancellationToken) => throw new NotSupportedException();

        #endregion
    }
}
