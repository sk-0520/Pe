using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteEntityEraser: EntityEraserBase
    {
        public NoteEntityEraser(NoteId noteId, IDatabaseContextPack contextPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(contextPack, statementLoader, loggerFactory)
        {
            NoteId = noteId;
        }

        public NoteEntityEraser(NoteId noteId, IDatabaseContext mainContext, IDatabaseContext fileContext, IDatabaseContext temporaryContext, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainContext, fileContext, temporaryContext, statementLoader, loggerFactory)
        {
            NoteId = noteId;
        }

        #region property

        private NoteId NoteId { get; }

        #endregion

        #region EntityEraserBase

        protected override void ExecuteMainImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var daoFactory = new AppDaoFactory(context, statementLoader, LoggerFactory);
            var noteViewOffsetsEntityDao = daoFactory.Create<NoteViewOffsetsEntityDao>();
            var noteContentsEntityDao = daoFactory.Create<NoteContentsEntityDao>();
            var noteLayoutsEntityDao = daoFactory.Create<NoteLayoutsEntityDao>();
            var notesEntityDao = daoFactory.Create<NotesEntityDao>();

            noteViewOffsetsEntityDao.DeleteNoteViewOffset(NoteId);
            noteContentsEntityDao.DeleteContents(NoteId);
            noteLayoutsEntityDao.DeleteLayouts(NoteId);
            notesEntityDao.DeleteNote(NoteId);
        }

        protected override void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            //TODO: 添付ファイル(そもそも添付ファイル自体実装してない)
        }

        protected override void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        { }

        #endregion
    }
}
