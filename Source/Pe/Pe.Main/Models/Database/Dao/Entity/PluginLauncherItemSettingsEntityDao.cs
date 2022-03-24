using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginLauncherItemSettingsEntityDao: EntityDaoBase
    {
        #region define

        private class PluginLauncherItemSettingDto: CommonDtoBase
        {
            #region property

            public Guid PluginId { get; set; }
            public LauncherItemId LauncherItemId { get; set; }
            public string PluginSettingKey { get; set; } = string.Empty;
            public string DataType { get; set; } = string.Empty;
            public string DataValue { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string DataType { get; } = "DataType";
            public static string DataValue { get; } = "DataValue";

            #endregion
        }

        #endregion

        public PluginLauncherItemSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginSettingRawValue ConvertFromDto(PluginLauncherItemSettingDto dto)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var data = new PluginSettingRawValue(
                pluginPersistentFormatTransfer.ToEnum(dto.DataType),
                dto.DataValue
            );
            return data;
        }

        private PluginLauncherItemSettingDto ConvertFromData(Guid pluginId, LauncherItemId launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var dto = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
                DataType = pluginPersistentFormatTransfer.ToString(data.Format),
                DataValue = data.Value,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public bool SelecteExistsPluginLauncherItemSetting(Guid pluginId, LauncherItemId launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginLauncherItemValue(Guid pluginId, LauncherItemId launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            var dto = Context.QueryFirstOrDefault<PluginLauncherItemSettingDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            var data = ConvertFromDto(dto);
            return data;
        }

        internal bool InsertPluginLauncherItemSetting(Guid pluginId, LauncherItemId launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, launcherItemId, key, data, databaseCommonStatus);

            return Context.Execute(statement, parameter) == 1;
        }

        internal bool UpdatePluginLauncherItemSetting(Guid pluginId, LauncherItemId launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, launcherItemId, key, data, databaseCommonStatus);

            return Context.Execute(statement, parameter) == 1;
        }

        public bool DeletePluginLauncherItemSetting(Guid pluginId, LauncherItemId launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            return Context.Execute(statement, parameter) == 1;
        }

        public int DeletePluginLauncherItemSettingsByPluginId(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
