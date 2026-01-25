using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware
{
    internal class AppStatementMiddleware: StatementMiddlewareBase
    {
        public AppStatementMiddleware(IDatabaseImplementation implementation)
            : base(implementation)
        { }

        #region IStatementMiddleware

        public override string Next(IStatementHandler handler, string input)
        {
            return handler.Handle(input);
        }

        #endregion
    }
}
