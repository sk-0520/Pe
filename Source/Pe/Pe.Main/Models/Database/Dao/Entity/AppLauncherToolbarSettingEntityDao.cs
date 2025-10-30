using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppLauncherToolbarSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppLauncherToolbarSettingEntityDto: CommonDtoBase
        {
            #region property
            public string ContentDropMode { get; set; } = string.Empty;
            public string ShortcutDropMode { get; set; } = string.Empty;
            public string GroupMenuPosition { get; set; } = string.Empty;
            public string DuplicatedFileRegisterMode { get; set; } = string.Empty;
            #endregion
        }

        private static class Column
        {
            #region property


            #endregion
        }

        #endregion

        public AppLauncherToolbarSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public FontId SelectAppLauncherToolbarSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<FontId>(statement);
        }

        public AppLauncherToolbarSettingData SelectSettingLauncherToolbarSetting()
        {
            var launcherToolbarContentDropModeTransfer = new EnumTransfer<LauncherToolbarContentDropMode>();
            var launcherToolbarShortcutDropModeTransfer = new EnumTransfer<LauncherToolbarShortcutDropMode>();
            var launcherGroupPositionTransfer = new EnumTransfer<LauncherGroupPosition>();
            var launcherToolbarDuplicatedFileRegisterModeTransfer = new EnumTransfer<LauncherToolbarDuplicatedFileRegisterMode>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppLauncherToolbarSettingEntityDto>(statement);
            var result = new AppLauncherToolbarSettingData() {
                ContentDropMode = launcherToolbarContentDropModeTransfer.ToEnum(dto.ContentDropMode),
                ShortcutDropMode = launcherToolbarShortcutDropModeTransfer.ToEnum(dto.ShortcutDropMode),
                GroupMenuPosition = launcherGroupPositionTransfer.ToEnum(dto.GroupMenuPosition),
                DuplicatedFileRegisterMode = launcherToolbarDuplicatedFileRegisterModeTransfer.ToEnum(dto.DuplicatedFileRegisterMode),
            };
            return result;
        }

        public void UpdateSettingLauncherToolbarSetting(AppLauncherToolbarSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var launcherToolbarContentDropModeTransfer = new EnumTransfer<LauncherToolbarContentDropMode>();
            var launcherToolbarShortcutDropModeTransfer = new EnumTransfer<LauncherToolbarShortcutDropMode>();
            var launcherGroupPositionTransfer = new EnumTransfer<LauncherGroupPosition>();
            var launcherToolbarDuplicatedFileRegisterModeTransfer = new EnumTransfer<LauncherToolbarDuplicatedFileRegisterMode>();

            var statement = LoadStatement();
            var dto = new AppLauncherToolbarSettingEntityDto() {
                ContentDropMode = launcherToolbarContentDropModeTransfer.ToString(data.ContentDropMode),
                ShortcutDropMode = launcherToolbarShortcutDropModeTransfer.ToString(data.ShortcutDropMode),
                GroupMenuPosition = launcherGroupPositionTransfer.ToString(data.GroupMenuPosition),
                DuplicatedFileRegisterMode = launcherToolbarDuplicatedFileRegisterModeTransfer.ToString(data.DuplicatedFileRegisterMode),
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }


        #endregion
    }
}
