using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppUpdateSettingEntityDao: EntityDaoBase
    {
        #region define

        private class AppUpdateSettingEntityDto: CommonDtoBase
        {
            #region property

            public string UpdateKind { get; set; } = string.Empty;
            public Version IgnoreVersion { get; set; } = new Version(0, 0, 0, 0);

            #endregion
        }

        #endregion

        public AppUpdateSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string UpdateKind => "UpdateKind";

            #endregion
        }

        #endregion

        #region function

        public SettingAppUpdateSettingData SelectSettingUpdateSetting()
        {
            var updateKindTransfer = new EnumTransfer<UpdateKind>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppUpdateSettingEntityDto>(statement);
            var result = new SettingAppUpdateSettingData() {
                UpdateKind = updateKindTransfer.ToEnum(dto.UpdateKind),
            };
            return result;
        }


        public void UpdateSettingUpdateSetting(SettingAppUpdateSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var updateKindTransfer = new EnumTransfer<UpdateKind>();

            var statement = LoadStatement();
            var parameter = new AppUpdateSettingEntityDto() {
                UpdateKind = updateKindTransfer.ToString(data.UpdateKind),
            };
            commonStatus.WriteCommon(parameter);
            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateReleaseVersion(UpdateKind updateKind, IDatabaseCommonStatus commonStatus)
        {
            var updateKindTransfer = new EnumTransfer<UpdateKind>();

            var statement = LoadStatement();
            var parameter = new AppUpdateSettingEntityDto() {
                UpdateKind = updateKindTransfer.ToString(updateKind),
            };
            commonStatus.WriteCommon(parameter);
            Context.UpdateByKey(statement, parameter);
        }


        #endregion
    }
}
