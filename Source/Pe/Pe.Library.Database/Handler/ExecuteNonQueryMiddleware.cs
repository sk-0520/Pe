using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteNonQueryMiddleware
    {
        int Next(IExecuteNonQueryMiddleware next, DbCommand command, int input);
    }

    public sealed class ExecuteNonQueryAction: IExecuteNonQueryMiddleware
    {
        public ExecuteNonQueryAction()
        { }

        #region function

        public int Next(IExecuteNonQueryMiddleware _, DbCommand command, int input)
        {
            return command.ExecuteNonQuery();
        }

        #endregion
    }

    public abstract class ExecuteNonQueryMiddlewareBase: IExecuteNonQueryMiddleware
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteNonQueryMiddlewareBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Implementation = implementation;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDatabaseImplementation Implementation { get; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region IExecuteNonQueryMiddleware

        public abstract int Next(IExecuteNonQueryMiddleware next, DbCommand command, int input);

        #endregion
    }
}
