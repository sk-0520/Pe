using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// ミドルウェアパイプライン基底。
    /// </summary>
    /// <typeparam name="TMiddleware">ミドルウェア</typeparam>
    /// <typeparam name="THandler">ハンドラー。</typeparam>
    /// <remarks><typeparamref name="TMiddleware"/>, <typeparamref name="THandler"/> に対する制限はないため継承先で制限すること。</remarks>
    public abstract class PipelineBase<TMiddleware, THandler>
    {
        #region property

        /// <summary>
        /// 登録されたミドルウェア。
        /// </summary>
        /// <remarks>
        /// <para>先頭が外側。</para>
        /// </remarks>
        protected List<TMiddleware> Middlewares { get; } = [];

        #endregion

        #region function

        /// <summary>
        /// ミドルウェア登録。
        /// </summary>
        /// <param name="middleware">登録ミドルウェア。</param>
        /// <remarks>外側に登録されるイメージ。</remarks>
        public void Use(TMiddleware middleware)
        {
            Middlewares.Insert(0, middleware);
        }

        /// <summary>
        /// ミドルウェア連続登録。
        /// </summary>
        /// <param name="middlewares">登録ミドルウェア一覧。先頭が外側になる想定。</param>
        public void UseRange(IEnumerable<TMiddleware> middlewares)
        {
            Middlewares.InsertRange(0, middlewares.Reverse());
        }

        #endregion

        #region function

        /// <summary>
        /// ミドルウェアからパイプラインの構築。
        /// </summary>
        /// <param name="process">最終処理。</param>
        /// <returns>パイプラインによる先頭ハンドラー。</returns>
        public virtual THandler Build(THandler process)
        {
            var handler = process;
            foreach(var middleware in Middlewares) {
                handler = CreateChainHandler(middleware, handler);
            }

            return handler;
        }

        /// <summary>
        /// チェーンハンドラの生成。
        /// </summary>
        /// <param name="middleware">ミドルウェア。</param>
        /// <param name="handler">ハンドラー。</param>
        /// <returns>チェーンハンドラ。</returns>
        protected abstract THandler CreateChainHandler(TMiddleware middleware, THandler handler);

        #endregion
    }

    /// <summary>
    /// <see cref="IStatementMiddleware"/> 用パイプライン。
    /// </summary>
    public class StatementPipeline: PipelineBase<IStatementMiddleware, IStatementHandler>
    {
        #region PipelineBase

        protected override IStatementHandler CreateChainHandler(IStatementMiddleware middleware, IStatementHandler handler)
        {
            return new StatementChainHandler(middleware, handler);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="IExecuteNonQueryMiddleware"/> 用パイプライン。
    /// </summary>
    public class ExecuteNonQueryPipeline: PipelineBase<IExecuteNonQueryMiddleware, IExecuteNonQueryHandler>
    {
        #region PipelineBase

        protected override IExecuteNonQueryHandler CreateChainHandler(IExecuteNonQueryMiddleware middleware, IExecuteNonQueryHandler handler)
        {
            return new ExecuteNonQueryChainHandler(middleware, handler);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="IExecuteScalarMiddleware"/> 用パイプライン。
    /// </summary>
    public class ExecuteScalarPipeline: PipelineBase<IExecuteScalarMiddleware, IExecuteScalarHandler>
    {
        #region PipelineBase

        protected override IExecuteScalarHandler CreateChainHandler(IExecuteScalarMiddleware middleware, IExecuteScalarHandler handler)
        {
            return new ExecuteScalarChainHandler(middleware, handler);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="IExecuteDataReaderMiddleware"/> 用パイプライン。
    /// </summary>
    public class ExecuteDataReaderPipeline: PipelineBase<IExecuteDataReaderMiddleware, IExecuteDataReaderHandler>
    {
        #region PipelineBase

        protected override IExecuteDataReaderHandler CreateChainHandler(IExecuteDataReaderMiddleware middleware, IExecuteDataReaderHandler handler)
        {
            return new ExecuteDataReaderChainHandler(middleware, handler);
        }

        #endregion
    }
}
