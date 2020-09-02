using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherAddonsEntityDao: EntityDaoBase
    {
        public LauncherAddonsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region proeprty

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string PluginId { get; } = "PluginId";

            #endregion
        }

        #endregion

        #region function

        public Guid SelectAddonPluginId(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.QueryFirst<Guid>(statement, parameter);
        }

        public bool InsertAddonPluginId(Guid launcherItemId, Guid pluginId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.PluginId] = pluginId;

            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}