using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public abstract class DomainDaoBase: ApplicationDatabaseObjectBase
    {
        protected DomainDaoBase(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(context, statementLoader, loggerFactory)
        { }

        #region property
        #endregion
    }
}
