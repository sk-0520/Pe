using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// ミドルウェア基底。
    /// </summary>
    public abstract class MiddlewareBase
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation">データベース実装依存処理。</param>
        /// <param name="loggerFactory">ロガー生成処理。</param>
        protected MiddlewareBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Implementation = implementation;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <summary>
        /// データベース実装依存処理。
        /// </summary>
        protected IDatabaseImplementation Implementation { get; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

    }
}
