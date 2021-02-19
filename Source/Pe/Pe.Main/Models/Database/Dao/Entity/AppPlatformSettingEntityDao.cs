using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppPlatformSettingEntityDao: EntityDaoBase
    {
        #region define

        private class AppPlatformSettingEntityDto: CommonDtoBase
        {
            #region property

            public bool SuppressSystemIdle { get; set; }
            public bool SupportExplorer { get; set; }


            #endregion
        }

        #endregion

        public AppPlatformSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string SuppressSystemIdle => "SuppressSystemIdle";
            public static string SupportExplorer => "SupportExplorer";

            #endregion
        }

        #endregion

        #region function

        public SettingAppPlatformSettingData SelectSettingPlatformSetting()
        {
            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppPlatformSettingEntityDto>(statement);
            var data = new SettingAppPlatformSettingData() {
                SupportExplorer = dto.SupportExplorer,
                SuppressSystemIdle = dto.SuppressSystemIdle,
            };
            return data;
        }

        public void UpdateSuppressSystemIdle(bool isEnabled, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.SuppressSystemIdle] = isEnabled;
            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateSupportExplorer(bool isEnabled, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.SupportExplorer] = isEnabled;
            Context.UpdateByKey(statement, parameter);
        }



        #endregion

    }
}
