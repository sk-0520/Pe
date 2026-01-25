using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware
{
    internal class AppExecuteScalarMiddleware: ExecuteScalarMiddlewareBase
    {
        public AppExecuteScalarMiddleware(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region ExecuteScalarMiddlewareBase

        public override object? Next(IExecuteScalarHandler handler, DbCommand command, object? input)
        {
            return handler.Invoke(command, input);
        }

        #endregion
    }
}
