using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IStatementHandler
    {
        #region function

        /// <summary>
        /// 問い合わせ分の加工。
        /// </summary>
        /// <param name="statement">問い合わせ文。</param>
        /// <returns>加工後の問い合わせ文</returns>
        string Handle(string statement);

        #endregion
    }

    /// <summary>
    /// データベース問い合わせ文の実行前ハンドラー基底。
    /// </summary>
    public abstract class StatementHandlerBase: IStatementHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected StatementHandlerBase(IDatabaseImplementation implementation)
        {
            Implementation = implementation;
        }

        #region property

        protected IDatabaseImplementation Implementation { get; }

        #endregion

        #region IStatementHandler

        public abstract string Handle(string statement);

        #endregion
    }
}
