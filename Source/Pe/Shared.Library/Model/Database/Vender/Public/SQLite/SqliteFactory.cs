#define USE_DB_SQLITE
#if USE_DB_SQLITE

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Dapper;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database.Vender.Public.SQLite
{
    public abstract class SqliteFactory : IDatabaseFactory
    {
        #region IDatabaseFactory

        public abstract IDbConnection CreateConnection();

        public virtual IDbDataAdapter CreateDataAdapter() => new SQLiteDataAdapter();

        public virtual IDatabaseImplementation CreateImplementation() => new SqliteImplementation();

        #endregion

    }
}

#endif
