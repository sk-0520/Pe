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
using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    internal class LauncherEntityEraser: EntityEraserBase
    {
        public LauncherEntityEraser(LauncherItemId launcherItemId, LauncherItemKind launcherItemKind, IDatabaseContextPack contextPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(contextPack, statementLoader, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemKind = launcherItemKind;
        }

        public LauncherEntityEraser(LauncherItemId launcherItemId, LauncherItemKind launcherItemKind, IDatabaseContext mainContext, IDatabaseContext fileContext, IDatabaseContext temporaryContext, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainContext, fileContext, temporaryContext, statementLoader, loggerFactory)
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
            var daoFactory = new AppDaoFactory(context, statementLoader, LoggerFactory);
            var launcherBadgesEntityDao = daoFactory.Create<LauncherBadgesEntityDao>();
            var launcherEnvVarsEntityDao = daoFactory.Create<LauncherEnvVarsEntityDao>();

            launcherBadgesEntityDao.DeleteLauncherBadge(LauncherItemId);
            launcherEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(LauncherItemId);

            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        var launcherFilesEntityDao = daoFactory.Create<LauncherFilesEntityDao>();
                        launcherFilesEntityDao.DeleteFileByLauncherItemId(LauncherItemId);
                    }
                    break;

                case LauncherItemKind.Addon:
                    break;

                case LauncherItemKind.Separator: {
                        var launcherSeparatorsEntityDao = daoFactory.Create<LauncherSeparatorsEntityDao>();
                        launcherSeparatorsEntityDao.DeleteSeparatorByLauncherItemId(LauncherItemId);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            var launcherGroupItemsEntityDao = daoFactory.Create<LauncherGroupItemsEntityDao>();
            launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherItemId(LauncherItemId);

            var launcherItemHistoriesEntityDao = daoFactory.Create<LauncherItemHistoriesEntityDao>();
            launcherItemHistoriesEntityDao.DeleteHistoriesByLauncherItemId(LauncherItemId);

            var launcherTagsEntityDao = daoFactory.Create<LauncherTagsEntityDao>();
            launcherTagsEntityDao.DeleteTagByLauncherItemId(LauncherItemId);

            var launcherRedoItemsEntityDao = daoFactory.Create<LauncherRedoItemsEntityDao>();
            launcherRedoItemsEntityDao.DeleteRedoItemByLauncherItemId(LauncherItemId);

            var launcherRedoSuccessExitCodesEntityDao = daoFactory.Create<LauncherRedoSuccessExitCodesEntityDao>();
            launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(LauncherItemId);

            var launcherItemsEntityDao = daoFactory.Create<LauncherItemsEntityDao>();
            launcherItemsEntityDao.DeleteLauncherItem(LauncherItemId);
        }

        protected override void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var daoFactory = new AppDaoFactory(context, statementLoader, LoggerFactory);
            var launcherItemIconsEntityDao = daoFactory.Create<LauncherItemIconsEntityDao>();
            launcherItemIconsEntityDao.DeleteAllSizeImageBinary(LauncherItemId);
        }

        protected override void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        { }

        #endregion
    }
}
