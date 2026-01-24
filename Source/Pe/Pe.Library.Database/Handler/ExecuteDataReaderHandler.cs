using System.Data;
using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteDataReaderHandler
    {
        #region function

        void Next(DbCommand command, CommandBehavior behavior, ref DbDataReader? result);

        #endregion
    }

    public sealed class ExecuteDataReaderAction: IExecuteDataReaderHandler
    {
        public ExecuteDataReaderAction()
        { }

        #region function

        public void Next(DbCommand command, CommandBehavior behavior, ref DbDataReader? result)
        {
            result = command.ExecuteReader(behavior);
        }

        #endregion
    }

    public abstract class ExecuteDataReaderHandlerBase: IExecuteDataReaderHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteDataReaderHandlerBase(IExecuteDataReaderHandler handler, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Handler = handler;
            Implementation = implementation;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IExecuteDataReaderHandler Handler { get; }
        protected IDatabaseImplementation Implementation { get; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region IExecuteDataReaderHandler

        public abstract void Next(DbCommand command, CommandBehavior behavior, ref DbDataReader? result);

        #endregion
    }

}
