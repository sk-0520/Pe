using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteEntityEraser: EntityEraserBase
    {
        public NoteEntityEraser(NoteId noteId, IDatabaseContextPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(contextsPack, statementLoader, loggerFactory)
        {
            NoteId = noteId;
        }

        public NoteEntityEraser(NoteId noteId, IDatabaseContext mainContexts, IDatabaseContext fileContexts, IDatabaseContext temporaryContexts, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainContexts, fileContexts, temporaryContexts, statementLoader, loggerFactory)
        {
            NoteId = noteId;
        }

        #region property

        private NoteId NoteId { get; }

        #endregion

        #region EntityEraserBase

        protected override void ExecuteMainImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var appDaoFactory = new AppDaoFactory(context, statementLoader, LoggerFactory);
            var noteViewOffsetsEntityDao = appDaoFactory.Create< NoteViewOffsetsEntityDao>();
            var noteContentsEntityDao = appDaoFactory.Create<NoteContentsEntityDao>();
            var noteLayoutsEntityDao = appDaoFactory.Create<NoteLayoutsEntityDao>();
            var notesEntityDao = appDaoFactory.Create<NotesEntityDao>();

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
