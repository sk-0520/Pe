using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware
{
    internal class AppStatementMiddleware: StatementMiddlewareBase
    {
        public AppStatementMiddleware(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region IStatementMiddleware

        public override string Next(IStatementHandler handler, string input)
        {
            return handler.Invoke(input);
        }

        #endregion
    }
}
