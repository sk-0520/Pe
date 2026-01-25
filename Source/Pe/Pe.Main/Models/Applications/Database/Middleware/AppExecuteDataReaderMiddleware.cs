using System.Data;
using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware
{
    internal class AppExecuteDataReaderMiddleware: ExecuteDataReaderMiddlewareBase
    {
        public AppExecuteDataReaderMiddleware(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region ExecuteDataReaderMiddlewareBase

        public override DbDataReader Next(IExecuteDataReaderHandler handler, DbCommand command, CommandBehavior behavior, DbDataReader reader)
        {
            return handler.Invoke(command, behavior, reader);
        }

        #endregion
    }
}
