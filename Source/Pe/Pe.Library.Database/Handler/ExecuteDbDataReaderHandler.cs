using System.Data;
using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteDbDataReaderHandler
    {
        #region function

        DbDataReader Next(DbCommand command, CommandBehavior behavior);

        #endregion
    }

    public sealed class ExecuteDbDataReaderAction: IExecuteDbDataReaderHandler
    {
        public ExecuteDbDataReaderAction()
        { }

        #region function

        public DbDataReader Next(DbCommand command, CommandBehavior behavior)
        {
            return command.ExecuteReader(behavior);
        }

        #endregion
    }

    public abstract class ExecuteDbDataReaderHandlerBase: IExecuteDbDataReaderHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteDbDataReaderHandlerBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        #region IExecuteDbDataReaderHandler

        public abstract DbDataReader Next(DbCommand command, CommandBehavior behavior);

        #endregion
    }

}
