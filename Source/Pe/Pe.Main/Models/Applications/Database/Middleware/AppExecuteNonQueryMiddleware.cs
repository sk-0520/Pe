using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware
{
    internal class AppExecuteNonQueryMiddleware: ExecuteNonQueryMiddlewareBase
    {
        public AppExecuteNonQueryMiddleware(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        {
        }

        #region ExecuteNonQueryMiddlewareBase

        public override int Next(IExecuteNonQueryHandler handler, DbCommand command, int input)
        {
            return handler.Invoke(command, input);
        }

        #endregion
    }
}
