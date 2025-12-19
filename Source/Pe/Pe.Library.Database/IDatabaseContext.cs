namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベースとの会話用インターフェイス。
    /// </summary>
    /// <remarks>
    /// <para><see cref="IDatabaseReader"/>, <see cref="IDatabaseExecutor"/>による明確な分離状態で処理するのは現実的でないため本IFで統合して扱う。</para>
    /// </remarks>
    public interface IDatabaseContext: IDatabaseReader, IDatabaseExecutor
    {
        #region property

        IDatabaseImplementation Implementation { get; }

        #endregion
    }
}
