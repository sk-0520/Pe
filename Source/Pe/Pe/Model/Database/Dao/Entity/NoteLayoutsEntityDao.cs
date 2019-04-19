using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Note;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class NoteLayoutsEntityDao : EntityDaoBase
    {
        public NoteLayoutsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string NoteId { get; } = "NoteId";

            #endregion
        }

        #endregion

        #region function

        NoteLayoutsEntityDto ConvertFromData(NoteLayoutData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var dto = new NoteLayoutsEntityDto() {
                NoteId = data.NoteId,
                LayoutKind = noteLayoutKindTransfer.ToText(data.LayoutKind),
                X = data.X,
                Y = data.Y,
                Width = data.Width,
                Height = data.Height,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool InsertNewLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Commander.Execute(sql, param) == 1;
        }

        #endregion
    }
}
