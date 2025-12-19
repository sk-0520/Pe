using System;
using System.Data;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベース実装におけるトランザクション処理。
    /// </summary>
    /// <remarks>
    /// <para><see cref="IDatabaseAccessor.BeginReadOnlyTransaction"/>でも説明のある通りデータベース実装のトランザクションではない可能性あり。</para>
    /// <para>コミット・ロールバックは一回だけ実行される使用を想定している。</para>
    /// </remarks>
    public interface IDatabaseTransaction: IDatabaseContext, IDisposable
    {
        #region property

        IDbTransaction? DbTransaction { get; }

        #endregion

        #region function

        /// <summary>
        /// コミット！
        /// </summary>
        void Commit();

        /// <summary>
        /// なかったことにしたい人生の一部。
        /// </summary>
        void Rollback();

        #endregion
    }

}
