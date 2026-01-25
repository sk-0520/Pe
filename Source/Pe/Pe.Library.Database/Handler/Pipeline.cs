using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
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

        public virtual THandler Build(THandler process)
        {
            var handler = process;
            foreach(var middleware in Middlewares) {
                handler = CreateChainHandler(middleware, handler);
            }

            return handler;
        }

        protected abstract THandler CreateChainHandler(TMiddleware middleware, THandler handler);

        #endregion
    }

    public class StatementPipeline: PipelineBase<IStatementMiddleware, IStatementHandler>
    {
        #region PipelineBase

        protected override IStatementHandler CreateChainHandler(IStatementMiddleware middleware, IStatementHandler handler)
        {
            return new StatementChainHandler(middleware, handler);
        }

        #endregion
    }

    public class ExecuteNonQueryPipeline: PipelineBase<IExecuteNonQueryMiddleware, IExecuteNonQueryHandler>
    {
        #region PipelineBase

        protected override IExecuteNonQueryHandler CreateChainHandler(IExecuteNonQueryMiddleware middleware, IExecuteNonQueryHandler handler)
        {
            return new ExecuteNonQueryChainHandler(middleware, handler);
        }

        #endregion
    }

    public class ExecuteScalarPipeline: PipelineBase<IExecuteScalarMiddleware, IExecuteScalarHandler>
    {
        #region PipelineBase

        protected override IExecuteScalarHandler CreateChainHandler(IExecuteScalarMiddleware middleware, IExecuteScalarHandler handler)
        {
            return new ExecuteScalarChainHandler(middleware, handler);
        }

        #endregion
    }

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
