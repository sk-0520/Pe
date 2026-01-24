using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public abstract class StatementHandlerBase
    {
        /// <summary>
        /// データベース問い合わせ文の実行前ハンドラー基底。
        /// </summary>
        /// <param name="implementation"></param>
        protected StatementHandlerBase(IDatabaseImplementation implementation)
        {
            Implementation = implementation;
        }

        #region property

        protected IDatabaseImplementation Implementation { get; }

        #endregion

        #region function

        /// <summary>
        /// 問い合わせ分の加工。
        /// </summary>
        /// <param name="statement">問い合わせ文。</param>
        /// <returns>加工後の問い合わせ文</returns>
        public abstract string Handle(string statement);

        #endregion
    }

    /// <summary>
    /// 特に何もしない加工処理。
    /// </summary>
    public sealed class RawStatementHandler: StatementHandlerBase
    {
        public RawStatementHandler(IDatabaseImplementation databaseImplementation)
            : base(databaseImplementation)
        { }

        #region function

        public override string Handle(string statement)
        {
            return statement;
        }

        #endregion
    }
}
