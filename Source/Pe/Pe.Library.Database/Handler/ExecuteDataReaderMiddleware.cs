using System.Data;
using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteDataReaderHandler
    {
        #region function

        DbDataReader Handle(DbCommand command, CommandBehavior behavior, DbDataReader reader);

        #endregion
    }

    public interface IExecuteDataReaderMiddleware
    {
        #region function

        DbDataReader Next(IExecuteDataReaderHandler handler, DbCommand command, CommandBehavior behavior, DbDataReader reader);

        #endregion
    }

    public abstract class ExecuteDataReaderMiddlewareBase: MiddlewareBase, IExecuteDataReaderMiddleware
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="loggerFactory"></param>
        protected ExecuteDataReaderMiddlewareBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region IExecuteDataReaderMiddleware

        public abstract DbDataReader Next(IExecuteDataReaderHandler handler, DbCommand command, CommandBehavior behavior, DbDataReader reader);

        #endregion
    }

    public class ExecuteDataReaderChainHandler: ChainHandlerBase<IExecuteDataReaderMiddleware, IExecuteDataReaderHandler>, IExecuteDataReaderHandler
    {
        public ExecuteDataReaderChainHandler(IExecuteDataReaderMiddleware middleware, IExecuteDataReaderHandler handler)
            : base(middleware, handler)
        { }

        #region IExecuteDataReaderHandler

        public DbDataReader Handle(DbCommand command, CommandBehavior behavior, DbDataReader reader)
        {
            return Middleware.Next(Handler, command, behavior, reader);
        }

        #endregion
    }
}
