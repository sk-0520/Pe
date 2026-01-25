using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteScalarHandler
    {
        #region function

        object? Handle(DbCommand command, object? input);

        #endregion
    }

    public interface IExecuteScalarMiddleware
    {
        #region function

        object? Next(IExecuteScalarHandler handler, DbCommand command, object? input);

        #endregion
    }

    public abstract class ExecuteScalarMiddlewareBase: MiddlewareBase, IExecuteScalarMiddleware
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="loggerFactory"></param>
        protected ExecuteScalarMiddlewareBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region IExecuteScalarMiddleware

        public abstract object? Next(IExecuteScalarHandler handler, DbCommand command, object? input);

        #endregion
    }

    public class ExecuteScalarChainHandler: ChainHandlerBase<IExecuteScalarMiddleware, IExecuteScalarHandler>, IExecuteScalarHandler
    {
        public ExecuteScalarChainHandler(IExecuteScalarMiddleware middleware, IExecuteScalarHandler handler)
            : base(middleware, handler)
        { }

        #region IExecuteScalarHandler

        public object? Handle(DbCommand command, object? input)
        {
            return Middleware.Next(Handler, command, input);
        }

        #endregion
    }

}
