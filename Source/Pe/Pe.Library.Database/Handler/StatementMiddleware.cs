using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IStatementHandler
    {
        #region function

        /// <summary>
        /// 問い合わせ文の加工。
        /// </summary>
        /// <param name="input">問い合わせ文。</param>
        /// <returns>加工後の問い合わせ文</returns>
        string Handle(string input);

        #endregion
    }

    public interface IStatementMiddleware
    {
        #region function

        /// <summary>
        /// 問い合わせ分の加工。
        /// </summary>
        /// <param name="handler">次の処理。</param>
        /// <param name="input">問い合わせ文。</param>
        /// <returns>加工後の問い合わせ文</returns>
        string Next(IStatementHandler handler, string input);

        #endregion
    }

    /// <summary>
    /// データベース問い合わせ文の実行前ハンドラー基底。
    /// </summary>
    public abstract class StatementMiddlewareBase: IStatementMiddleware
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected StatementMiddlewareBase(IDatabaseImplementation implementation)
        {
            Implementation = implementation;
        }

        #region property

        protected IDatabaseImplementation Implementation { get; }

        #endregion

        #region IStatementHandler

        public abstract string Next(IStatementHandler handler, string input);

        #endregion
    }

    public class StatementChainHandler: IStatementHandler
    {
        public StatementChainHandler(IStatementMiddleware middleware, IStatementHandler handler)
        {
            Middleware = middleware;
            Handler = handler;
        }

        private IStatementMiddleware Middleware { get; }
        private IStatementHandler Handler { get; }

        #region IStatementHandler

        public string Handle(string input)
        {
            return Middleware.Next(Handler, input);
        }

        #endregion
    }
}
