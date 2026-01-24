using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteNonQueryHandler
    {
        void Next(DbCommand command, ref int result);
    }

    public sealed class ExecuteNonQueryAction: IExecuteNonQueryHandler
    {
        public ExecuteNonQueryAction()
        { }

        #region function

        public void Next(DbCommand command, ref int result)
        {
            result = command.ExecuteNonQuery();
        }

        #endregion
    }

    public abstract class ExecuteNonQueryHandlerBase: IExecuteNonQueryHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteNonQueryHandlerBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        #region IExecuteNonQueryHandler

        public abstract void Next(DbCommand command, ref int result);

        #endregion
    }
}
