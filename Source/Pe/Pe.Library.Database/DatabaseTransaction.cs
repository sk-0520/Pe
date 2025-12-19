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
    public class DatabaseTransaction: DatabaseContextBase, IDatabaseTransaction
    {
        public DatabaseTransaction(IDbConnection dbConnection, IDbTransaction? dbTransaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(dbConnection, dbTransaction, implementation, loggerFactory)
        {
            //NOP
        }

        #region property

        public bool Committed { get; private set; }

        #endregion

        #region IDatabaseTransaction

        public new IDbTransaction? DbTransaction => base.DbTransaction;

        public virtual void Commit()
        {
            if(DbTransaction is null) {
                throw new InvalidOperationException($"{nameof(DbTransaction)} is null");
            }

            ThrowIfDisposed();

            DbTransaction.Commit();
            Committed = true;
        }

        public virtual void Rollback()
        {
            ThrowIfDisposed();

            if(DbTransaction is not null && DbTransaction.Connection is not null) {
                DbTransaction.Rollback();
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
                    if(DbTransaction is not null) {
                        DbTransaction.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 読み込み専用トランザクション。
    /// </summary>
    /// <remarks>
    /// <para><see cref="IDatabaseTransaction.Rollback"/>は実装ミス・不具合により書き込みが実施されていた場合も考慮し、最後の防衛線として無効化しない。</para>
    /// </remarks>
    public sealed class ReadOnlyDatabaseTransaction: DatabaseTransaction
    {
        public ReadOnlyDatabaseTransaction(IDbConnection dbConnection, IDbTransaction? dbTransaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(dbConnection, dbTransaction, implementation, loggerFactory)
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
