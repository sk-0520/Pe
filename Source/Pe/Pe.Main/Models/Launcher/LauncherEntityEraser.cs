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
            var appDaoFactory = new AppDaoFactory(context, statementLoader, LoggerFactory);
            var launcherBadgesEntityDao = appDaoFactory.Create<LauncherBadgesEntityDao>();
            var launcherEnvVarsEntityDao = appDaoFactory.Create<LauncherEnvVarsEntityDao>();

            launcherBadgesEntityDao.DeleteLauncherBadge(LauncherItemId);
            launcherEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(LauncherItemId);

            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        var launcherFilesEntityDao = appDaoFactory.Create<LauncherFilesEntityDao>();
                        launcherFilesEntityDao.DeleteFileByLauncherItemId(LauncherItemId);
                    }
                    break;

                case LauncherItemKind.Addon:
                    break;

                case LauncherItemKind.Separator: {
                        var launcherSeparatorsEntityDao = appDaoFactory.Create<LauncherSeparatorsEntityDao>();
                        launcherSeparatorsEntityDao.DeleteSeparatorByLauncherItemId(LauncherItemId);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            var launcherGroupItemsEntityDao = appDaoFactory.Create<LauncherGroupItemsEntityDao>();
            launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherItemId(LauncherItemId);

            var launcherItemHistoriesEntityDao = appDaoFactory.Create<LauncherItemHistoriesEntityDao>();
            launcherItemHistoriesEntityDao.DeleteHistoriesByLauncherItemId(LauncherItemId);

            var launcherTagsEntityDao = appDaoFactory.Create<LauncherTagsEntityDao>();
            launcherTagsEntityDao.DeleteTagByLauncherItemId(LauncherItemId);

            var launcherRedoItemsEntityDao = appDaoFactory.Create<LauncherRedoItemsEntityDao>();
            launcherRedoItemsEntityDao.DeleteRedoItemByLauncherItemId(LauncherItemId);

            var launcherRedoSuccessExitCodesEntityDao = appDaoFactory.Create<LauncherRedoSuccessExitCodesEntityDao>();
            launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(LauncherItemId);

            var launcherItemsEntityDao = appDaoFactory.Create<LauncherItemsEntityDao>();
            launcherItemsEntityDao.DeleteLauncherItem(LauncherItemId);
        }

        protected override void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var appDaoFactory = new AppDaoFactory(context, statementLoader, LoggerFactory);
            var launcherItemIconsEntityDao = appDaoFactory.Create<LauncherItemIconsEntityDao>();
            launcherItemIconsEntityDao.DeleteAllSizeImageBinary(LauncherItemId);
        }

        protected override void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        { }

        #endregion
    }
}
