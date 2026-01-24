using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Handler
{
    internal class AppLoggingExecuteNonQueryHandler: ExecuteNonQueryHandlerBase
    {
        public AppLoggingExecuteNonQueryHandler(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region ExecuteNonQueryHandlerBase

        public override void Next(DbCommand command, ref int result)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
