using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteNonQueryHandler
    {
        int Invoke(DbCommand command, int input);
    }

    public interface IExecuteNonQueryMiddleware
    {
        int Next(IExecuteNonQueryHandler handler, DbCommand command, int input);
    }

    public abstract class ExecuteNonQueryMiddlewareBase: MiddlewareBase, IExecuteNonQueryMiddleware
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="loggerFactory"></param>
        protected ExecuteNonQueryMiddlewareBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region IExecuteNonQueryMiddleware

        public abstract int Next(IExecuteNonQueryHandler handler, DbCommand command, int input);

        #endregion
    }

    public class ExecuteNonQueryChainHandler: ChainHandlerBase<IExecuteNonQueryMiddleware, IExecuteNonQueryHandler>, IExecuteNonQueryHandler
    {
        public ExecuteNonQueryChainHandler(IExecuteNonQueryMiddleware middleware, IExecuteNonQueryHandler handler)
            : base(middleware, handler)
        { }

        #region IExecuteNonQueryHandler

        public int Invoke(DbCommand command, int input)
        {
            return Middleware.Next(NextHandler, command, input);
        }

        #endregion
    }

    public sealed class DefaultExecuteNonQueryProcess: IExecuteNonQueryHandler
    {
        #region IExecuteNonQueryHandler

        public int Invoke(DbCommand command, int input)
        {
            return command.ExecuteNonQuery();
        }

        #endregion
    }
}
