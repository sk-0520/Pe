using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Handler
{
    internal class AppLoggingExecuteScalarHandler: ExecuteScalarHandlerBase
    {
        public AppLoggingExecuteScalarHandler(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(null!, implementation, loggerFactory)
        { }

        #region ExecuteScalarHandlerBase

        public override object? Next(DbCommand command, object? input)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
