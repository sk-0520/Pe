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
    /// <para>基本的にはユーザーコードで登場せず <see cref="IDatabaseContext"/>がすべて上位から良しなに対応する。</para>
    /// </remarks>
    public class DatabaseTransaction: DatabaseContext, IDatabaseTransaction
    {
        public DatabaseTransaction(IDbConnection connection, bool beginTransaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(connection, beginTransaction ? connection.BeginTransaction(): null, implementation, loggerFactory)
        {
            //NOP
        }

        public DatabaseTransaction(IDbConnection connection, bool beginTransaction, IDatabaseImplementation implementation, IsolationLevel isolationLevel, ILoggerFactory loggerFactory)
            : base(connection, beginTransaction ? connection.BeginTransaction(isolationLevel) : null, implementation, loggerFactory)
        {
            //NOP
        }

        #region property

        public bool Committed { get; private set; }

        #endregion

        #region IDatabaseTransaction

        /// <summary>
        /// <see cref="IDatabaseContext"/>としての自身を返す。
        /// </summary>
        public IDatabaseContext Context => this;

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
}
