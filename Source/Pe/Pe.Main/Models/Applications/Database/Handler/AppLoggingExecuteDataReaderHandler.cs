using System.Data;
using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Handler
{
    internal class AppLoggingExecuteDataReaderHandler: ExecuteDataReaderHandlerBase
    {
        public AppLoggingExecuteDataReaderHandler(IExecuteDataReaderHandler handler, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(handler, implementation, loggerFactory)
        { }

        #region ExecuteDataReaderHandlerBase

        public override void Next(DbCommand command, CommandBehavior behavior, ref DbDataReader? result)
        {
        }

        #endregion
    }
}
