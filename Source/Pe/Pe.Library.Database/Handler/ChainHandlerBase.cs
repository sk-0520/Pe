namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// チェーン処理基底。
    /// </summary>
    /// <typeparam name="TMiddleware">ミドルウェア。</typeparam>
    /// <typeparam name="THandler">ハンドラー。</typeparam>
    /// <remarks><typeparamref name="TMiddleware"/>, <typeparamref name="THandler"/> に対する制限はないため継承先で制限すること。</remarks>
    public abstract class ChainHandlerBase<TMiddleware, THandler>
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="middleware">実行ミドルウェア。</param>
        /// <param name="nextHandler">次のハンドラー。</param>
        protected ChainHandlerBase(TMiddleware middleware, THandler nextHandler)
        {
            Middleware = middleware;
            NextHandler = nextHandler;
        }

        #region property

        /// <summary>
        /// 実行ミドルウェア。
        /// </summary>
        protected TMiddleware Middleware { get; }
        /// <summary>
        /// 次のハンドラー。
        /// </summary>
        protected THandler NextHandler { get; }

        #endregion
    }
}
