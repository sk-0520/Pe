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

        public abstract THandler Build(THandler process);

        #endregion
    }

    public class StatementPipeline: PipelineBase<IStatementMiddleware, IStatementHandler>
    {
        #region PipelineBase

        public override IStatementHandler Build(IStatementHandler process)
        {
            var handler = process;
            foreach(var middleware in Middlewares) {
                handler = new StatementChainHandler(middleware, handler);
            }

            return handler;
        }

        #endregion
    }

    //public class Pipeline
    //{
    //    public Pipeline() { }

    //    #region property

    //    private List<IExecuteNonQueryMiddleware> Middlewares { get; } = [];

    //    #endregion

    //    #region function

    //    public void Add(IExecuteNonQueryMiddleware middleware)
    //    {
    //        Middlewares.Insert(0, middleware);
    //    }

    //    public void AddRange(IEnumerable<IExecuteNonQueryMiddleware> middlewares)
    //    {
    //        Middlewares.InsertRange(0, middlewares.Reverse());
    //    }

    //    #endregion

    //    #region function

    //    #endregion
    //}

    //public class Pipeline<THandler>
    //where THandler : IExecuteScalarHandler
    //{
    //}
}
