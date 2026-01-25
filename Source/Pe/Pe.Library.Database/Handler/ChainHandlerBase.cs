namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public abstract class ChainHandlerBase<TMiddleware, THandler>
    {
        protected ChainHandlerBase(TMiddleware middleware, THandler handler)
        {
            Middleware = middleware;
            Handler = handler;
        }

        #region property

        protected TMiddleware Middleware { get; }
        protected THandler Handler { get; }

        #endregion
    }
}
