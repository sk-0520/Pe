using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public interface IExecuteScalarHandler
    {
        #region function

        object? Next(DbCommand command, object? input);

        #endregion
    }

    public sealed class ExecuteScalarAction: IExecuteScalarHandler
    {
        public ExecuteScalarAction()
        { }

        #region function

        public object? Next(DbCommand command, object? input)
        {
            return command.ExecuteScalar();
        }

        #endregion
    }

    public abstract class ExecuteScalarHandlerBase: IExecuteScalarHandler
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="implementation"></param>
        protected ExecuteScalarHandlerBase(IExecuteScalarHandler handler, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Handler = handler;
            Implementation = implementation;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IExecuteScalarHandler Handler { get; }
        protected IDatabaseImplementation Implementation { get; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region IExecuteScalarHandler

        public abstract object? Next(DbCommand command, object? input);

        #endregion
    }
}
