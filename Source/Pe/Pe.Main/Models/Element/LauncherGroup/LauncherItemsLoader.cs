using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Applications;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherItemsLoader
    {
        public LauncherItemsLoader(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            Context = context;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
        }

        #region property

        private IDatabaseContext Context { get; }
        private IDatabaseStatementLoader StatementLoader { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }

        #endregion

        #region function

        private IEnumerable<LauncherItemId> LoadNormalIds(LauncherGroupId launcherGroupId)
        {
            var daoFactory = new AppDaoFactory(Context, StatementLoader, LoggerFactory);
            var dao = daoFactory.Create<LauncherGroupItemsEntityDao>();
            return dao.SelectLauncherItemIds(launcherGroupId);
        }

        public IEnumerable<LauncherItemId> LoadLauncherItemIds(LauncherGroupId launcherGroupId, LauncherGroupKind kind)
        {
            switch(kind) {
                case LauncherGroupKind.Normal:
                    return LoadNormalIds(launcherGroupId);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
