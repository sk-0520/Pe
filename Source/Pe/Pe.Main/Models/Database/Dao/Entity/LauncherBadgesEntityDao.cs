using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherBadgesEntityDao: EntityDaoBase
    {
        #region define

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        private sealed class LauncherBadgeSelectDto: RowDtoBase
        {
            #region property

            public bool IsVisible { get; set; } = false;
            public string Display { get; set; } = string.Empty;
            public string Shape { get; set; } = string.Empty;
            public string Background { get; set; } = string.Empty;

            #endregion
        }

        private sealed class LauncherBadgeRegisterDto: CommonDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public bool IsVisible { get; set; }
            public string Display { get; set; } = string.Empty;
            public string Shape { get; set; } = string.Empty;
            public string Background { get; set; } = string.Empty;

            #endregion
        }


        #endregion

        public LauncherBadgesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private BadgeData ConvertFromDto(LauncherBadgeSelectDto dto)
        {
            var badgeShapeTransfer = new EnumTransfer<BadgeShape>();

            return new BadgeData() {
                IsVisible = dto.IsVisible,
                Display = dto.Display,
                BadgeShape = badgeShapeTransfer.ToEnum(dto.Shape),
                Background = ToColor(dto.Background),
            };
        }

        private LauncherBadgeRegisterDto ConvertFromData(LauncherItemId launcherItemId, IReadOnlyBadgeData data, IDatabaseCommonStatus commonStatus)
        {
            var badgeShapeTransfer = new EnumTransfer<BadgeShape>();

            var dto = new LauncherBadgeRegisterDto() {
                LauncherItemId = launcherItemId,
                IsVisible = data.IsVisible,
                Display = data.Display,
                Shape = badgeShapeTransfer.ToString(data.BadgeShape),
                Background = FromColor(data.Background),
            };
            commonStatus.WriteCommonTo(dto);

            return dto;
        }


        public BadgeData? SelectLauncherBadge(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new Dictionary<string, object?> {
                [Column.LauncherItemId] = launcherItemId,
            };

            var dto = Context.QueryFirstOrDefault<LauncherBadgeSelectDto>(statement, parameter);
            if(dto is null) {
                return null;
            }

            return ConvertFromDto(dto);
        }

        public void InsertLauncherBadge(LauncherItemId launcherItemId, IReadOnlyBadgeData badgeData, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(launcherItemId, badgeData, commonStatus);

            Context.InsertSingle(statement, parameter);
        }

        public void UpdateLauncherBadge(LauncherItemId launcherItemId, IReadOnlyBadgeData badgeData, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(launcherItemId, badgeData, commonStatus);

            Context.UpdateByKey(statement, parameter);
        }


        public bool DeleteLauncherBadge(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new Dictionary<string, object?> {
                [Column.LauncherItemId] = launcherItemId,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }
        #endregion
    }
}
