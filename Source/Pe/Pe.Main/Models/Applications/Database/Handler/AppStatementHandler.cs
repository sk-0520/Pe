using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Handler
{
    internal class AppStatementHandler: StatementHandlerBase
    {
        public AppStatementHandler(IDatabaseImplementation implementation)
            : base(implementation)
        { }

        #region StatementHandlerBase

        public override string Handle(string statement)
        {
            return statement;
        }

        #endregion
    }
}
