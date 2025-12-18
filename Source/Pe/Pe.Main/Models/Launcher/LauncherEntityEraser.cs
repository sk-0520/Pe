using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    internal class LauncherEntityEraser: EntityEraserBase
    {
        public LauncherEntityEraser(LauncherItemId launcherItemId, LauncherItemKind launcherItemKind, IDatabaseContextPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(contextsPack, statementLoader, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemKind = launcherItemKind;
        }

        public LauncherEntityEraser(LauncherItemId launcherItemId, LauncherItemKind launcherItemKind, IDatabaseContext mainContexts, IDatabaseContext fileContexts, IDatabaseContext temporaryContexts, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainContexts, fileContexts, temporaryContexts, statementLoader, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemKind = launcherItemKind;
        }

        #region property

        private LauncherItemId LauncherItemId { get; }
        private LauncherItemKind LauncherItemKind { get; }

        #endregion

        #region EntityEraserBase

        protected override void ExecuteMainImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var launcherBadgesEntityDao = new LauncherBadgesEntityDao(context, statementLoader, LoggerFactory);
            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(context, statementLoader, LoggerFactory);

            launcherBadgesEntityDao.DeleteLauncherBadge(LauncherItemId);
            launcherEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(LauncherItemId);

            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        var launcherFilesEntityDao = new LauncherFilesEntityDao(context, statementLoader, LoggerFactory);
                        launcherFilesEntityDao.DeleteFileByLauncherItemId(LauncherItemId);
                    }
                    break;

                case LauncherItemKind.Addon:
                    break;

                case LauncherItemKind.Separator: {
                        var launcherSeparatorsEntityDao = new LauncherSeparatorsEntityDao(context, statementLoader, LoggerFactory);
                        launcherSeparatorsEntityDao.DeleteSeparatorByLauncherItemId(LauncherItemId);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(context, statementLoader, LoggerFactory);
            launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherItemId(LauncherItemId);

            var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(context, statementLoader, LoggerFactory);
            launcherItemHistoriesEntityDao.DeleteHistoriesByLauncherItemId(LauncherItemId);

            var launcherTagsEntityDao = new LauncherTagsEntityDao(context, statementLoader, LoggerFactory);
            launcherTagsEntityDao.DeleteTagByLauncherItemId(LauncherItemId);

            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, statementLoader, LoggerFactory);
            launcherRedoItemsEntityDao.DeleteRedoItemByLauncherItemId(LauncherItemId);

            var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(context, statementLoader, LoggerFactory);
            launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(LauncherItemId);

            var launcherItemsEntityDao = new LauncherItemsEntityDao(context, statementLoader, LoggerFactory);
            launcherItemsEntityDao.DeleteLauncherItem(LauncherItemId);
        }

        protected override void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(context, statementLoader, LoggerFactory);
            launcherItemIconsEntityDao.DeleteAllSizeImageBinary(LauncherItemId);
        }

        protected override void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        { }

        #endregion
    }
}
