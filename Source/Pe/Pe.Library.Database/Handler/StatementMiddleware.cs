using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

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

    public abstract class StatementMiddlewareBase: MiddlewareBase, IStatementMiddleware
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="loggerFactory"></param>
        protected StatementMiddlewareBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region IStatementMiddleware

        public abstract string Next(IStatementHandler handler, string input);

        #endregion
    }

    public class StatementChainHandler: ChainHandlerBase<IStatementMiddleware, IStatementHandler>, IStatementHandler
    {
        public StatementChainHandler(IStatementMiddleware middleware, IStatementHandler handler)
            : base(middleware, handler)
        { }

        #region IStatementHandler

        public string Handle(string input)
        {
            return Middleware.Next(Handler, input);
        }

        #endregion
    }

    public sealed class DefaultStatementProcess: IStatementHandler
    {
        #region IStatementHandler

        public string Handle(string input)
        {
            return input;
        }

        #endregion
    }

}
