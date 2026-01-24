using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteNonQueryHandler
    {
        int Next(IExecuteNonQueryHandler handler, DbCommand command, int input);
    }

    public sealed class ExecuteNonQueryAction: IExecuteNonQueryHandler
    {
        public ExecuteNonQueryAction()
        { }

        #region function

        public int Next(IExecuteNonQueryHandler _, DbCommand command, int input)
        {
            return command.ExecuteNonQuery();
        }

        #endregion
    }

    public abstract class ExecuteNonQueryHandlerBase: IExecuteNonQueryHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteNonQueryHandlerBase(IExecuteNonQueryHandler handler, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Implementation = implementation;
            Handler = handler;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IExecuteNonQueryHandler Handler { get; }
        protected IDatabaseImplementation Implementation { get; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region IExecuteNonQueryHandler

        public abstract int Next(IExecuteNonQueryHandler handler, DbCommand command, int input);

        #endregion
    }
}
