using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Handler
{
    internal class AppLoggingExecuteNonQueryHandler: ExecuteNonQueryHandlerBase
    {
        public AppLoggingExecuteNonQueryHandler(IExecuteNonQueryHandler handler, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(handler, implementation, loggerFactory)
        { }

        #region ExecuteNonQueryHandlerBase


        public override int Next(IExecuteNonQueryHandler handler, DbCommand command, int input)
        {
            return Handler.Next(handler, command, input);
        }

        #endregion
    }
}
