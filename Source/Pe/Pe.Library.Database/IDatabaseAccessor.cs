using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// DBアクセス処理。
    /// </summary>
    /// <remarks>
    /// <para>使用者側はトランザクション処理を原則使用しない。</para>
    /// </remarks>
    public interface IDatabaseAccessor: IDatabaseContext
    {
        #region property

        /// <summary>
        /// 接続元。
        /// </summary>
        IDbConnection BaseDbConnection { get; }
        /// <summary>
        /// 対象DBに対する生成処理機。
        /// </summary>
        IDatabaseFactory DatabaseFactory { get; }

        #endregion

        #region function

        /// <summary>
        /// 一時的に切断状態へ遷移。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDisposable.Dispose()"/>が完了するまでの間接続できない状態になる。</para>
        /// </remarks>
        /// <returns>切断状態終了のトリガー。 GC 任せにせず明示的に <see cref="IDisposable.Dispose()"/> すること。</returns>
        IDisposable PauseConnection();

        /// <inheritdoc cref="BeginTransaction(IsolationLevel)"/>
        IDatabaseTransaction BeginTransaction();
        /// <summary>
        /// トランザクションの開始。
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <inheritdoc cref="BeginReadOnlyTransaction(IsolationLevel)"/>
        IDatabaseTransaction BeginReadOnlyTransaction();
        /// <summary>
        /// 読み込み専用でトランザクション開始。
        /// </summary>
        /// <remarks>
        /// <para>書き込みを行わないトランザクション処理。DB直接実行とあんまり変わりはないがソース上で読み込みのみを明記する役割。</para>
        /// <para>実際にトランザクションとして実行されるかどうかは未定義。マルチスレッドでトランザクションが開けないデータベースなどの場合は仮想的なトランザクションとして扱われる可能性あり。</para>
        /// </remarks>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel);

        #endregion
    }
}
