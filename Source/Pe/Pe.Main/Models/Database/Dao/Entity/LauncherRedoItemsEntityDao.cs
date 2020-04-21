using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class LauncherRedoItemsDto: CommonDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string RedoWait { get; set; } = string.Empty;
        public TimeSpan WaitTime { get; set; }
        public long RetryCount { get; set; }

        #endregion
    }

    public class LauncherRedoItemsEntityDao: EntityDaoBase
    {
        public LauncherRedoItemsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        #region function

        LauncherRedoData ConvertFromDto(LauncherRedoItemsDto dto)
        {
            var redoWaitTransfer = new EnumTransfer<RedoWait>();

            return new LauncherRedoData() {
                RedoWait = redoWaitTransfer.ToEnum(dto.RedoWait),
                RetryCount = ToInt(dto.RetryCount),
                WaitTime = dto.WaitTime,
            };
        }

        public bool SelectExistsLauncherRedoItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.QueryFirst<bool>(statement, parameter);
        }

        public LauncherRedoData SelectLauncherRedoItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QueryFirst<LauncherRedoItemsDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        #endregion
    }
}
