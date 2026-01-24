using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteScalarHandler
    {
        #region function

        void Next(DbCommand command, ref object? result);

        #endregion
    }

    public sealed class ExecuteScalarAction: IExecuteScalarHandler
    {
        public ExecuteScalarAction()
        { }

        #region function

        public void Next(DbCommand command, ref object? result)
        {
            result = command.ExecuteScalar();
        }

        #endregion
    }

    public abstract class ExecuteScalarHandlerBase: IExecuteScalarHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteScalarHandlerBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        #region IExecuteScalarHandler

        public abstract void Next(DbCommand command, ref object? result);

        #endregion
    }
}
