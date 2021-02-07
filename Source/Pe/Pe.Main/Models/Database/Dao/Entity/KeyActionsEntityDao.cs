using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyActionsEntityDao: EntityDaoBase
    {
        #region define

        private class KeyActionsEntityDto: CommonDtoBase
        {
            #region property

            public Guid KeyActionId { get; set; }
            public string KeyActionKind { get; set; } = string.Empty;
            public string KeyActionContent { get; set; } = string.Empty;
            public string Comment { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public KeyActionsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string KeyActionId { get; } = "KeyActionId";
            public static string KeyActionKind { get; } = "ActionKind";
            public static string KeyActionContent { get; } = "KeyActionContent";
            public static string KeyActionOption { get; } = "KeyActionOption";
            public static string Comment { get; } = "Comment";

            #endregion
        }

        #endregion

        #region function

        private KeyActionData ConvertFromDto(KeyActionsEntityDto dto)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var result = new KeyActionData() {
                KeyActionId = dto.KeyActionId,
                KeyActionContent = dto.KeyActionContent,
                KeyActionKind = keyActionKindTransfer.ToEnum(dto.KeyActionKind),
                Comment = dto.Comment,
            };

            return result;
        }

        private KeyActionsEntityDto ConvertFromData(KeyActionData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var dto = new KeyActionsEntityDto() {
                KeyActionId = data.KeyActionId,
                KeyActionKind = keyActionKindTransfer.ToString(data.KeyActionKind),
                KeyActionContent = data.KeyActionContent,
                Comment = data.Comment,
            };
            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public IEnumerable<KeyActionData> SelectAllKeyActionsFromKind(KeyActionKind keyActionKind)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = LoadStatement();
            var parameter = new { KeyActionKind = keyActionKindTransfer.ToString(keyActionKind) };
            return Context.Query<KeyActionsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public IEnumerable<KeyActionData> SelectAllKeyActionsIgnoreKinds(IReadOnlyCollection<KeyActionKind> ignoreKinds)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = LoadStatement();
            var parameter = new { IgnoreKinds = ignoreKinds.Select(i => keyActionKindTransfer.ToString(i)) };
            return Context.Query<KeyActionsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public bool InsertKeyAction(KeyActionData keyActionData, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(keyActionData, databaseCommonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public bool UpdateKeyAction(KeyActionData keyActionData, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(keyActionData, databaseCommonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public bool UpdateUsageCountIncrement(Guid keyActionId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.KeyActionId] = keyActionId;
            return Context.Execute(statement, parameter) == 1;
        }

        public bool DeleteKeyAciton(Guid keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new { KeyActionId = keyActionId };
            return Context.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
