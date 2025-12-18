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
    public class NoteViewOffsetsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class NotesEntityDto: CreateDtoBase
        {
            #region property

            public NoteId NoteId { get; set; }
            public double X { get; set; }
            public double Y { get; set; }

            #endregion
        }


        #endregion

        public NoteViewOffsetsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(context, statementLoader, loggerFactory)
        { }

        #region function

        public NoteViewOffsetData? SelectNoteViewOffset(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
            };

            return Context.QueryFirstOrDefault<NoteViewOffsetData>(statement, parameter);
        }

        public void InsertNoteViewOffset(NoteId noteId, NoteViewOffsetData offset, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var dto = new NotesEntityDto() {
                NoteId = noteId,
                X = offset.X,
                Y = offset.Y,
            };
            commonStatus.WriteCreateTo(dto);

            Context.InsertSingle(statement, dto);
        }

        public bool DeleteNoteViewOffset(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }

        #endregion
    }
}
