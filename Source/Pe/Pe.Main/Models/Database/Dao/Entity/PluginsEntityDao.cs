using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class PluginStateDto: CommonDtoBase
    {
        #region property

        public Guid PluginId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;


        #endregion
    }


    public class PluginsEntityDao: EntityDaoBase
    {
        public PluginsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string Name { get; } = "Name";
            public static string State { get; } = "State";
            public static string LastUseTimestamp { get; } = "LastUseTimestamp";
            public static string LastUsePluginVersion { get; } = "LastUsePluginVersion";
            public static string LastUseAppVersion { get; } = "LastUseAppVersion";
            public static string ExecuteCount { get; } = "ExecuteCount";

            #endregion
        }

        #endregion

        #region function

        PluginStateDto ConvertFromData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginStateTransfer = new EnumTransfer<PluginState>();

            var dto = new PluginStateDto() {
                PluginId = data.PluginId,
                Name = data.Name,
                State = pluginStateTransfer.ToString(data.State),
            };
            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        PluginStateData ConvertFromDto(PluginStateDto dto)
        {
            var pluginStateTransfer = new EnumTransfer<PluginState>();

            var data = new PluginStateData() {
                PluginId = dto.PluginId,
                Name = dto.Name,
                State = pluginStateTransfer.ToEnum(dto.State),
            };

            return data;
        }

        public IEnumerable<PluginStateData> SelectePlguinStateData()
        {
            var statement = LoadStatement();
            return Commander
                .Query<PluginStateDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public bool SelecteExistsPlguin(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Commander.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public bool InsertPluginStateData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);
            return Commander.Execute(statement, dto) == 1;
        }

        public bool UpdatePluginStateData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);
            return Commander.Execute(statement, dto) == 1;
        }

        public bool UpdatePluginRunningState(Guid pluginId, Version pluginVersion, Version applicationVersio, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.LastUseTimestamp] = DateTime.UtcNow; // DAO層でまぁいっかぁ
            parameter[Column.LastUsePluginVersion] = pluginVersion;
            parameter[Column.LastUseAppVersion] = applicationVersio;
            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
