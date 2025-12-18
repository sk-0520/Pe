using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginValuesEntityDao: EntityDaoBase
    {
        #region define

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";

            #endregion
        }

        #endregion
        public PluginValuesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(context, statementLoader, loggerFactory)
        { }

        #region function

        public int DeletePluginValuesByPluginId(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };
            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
