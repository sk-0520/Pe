using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherIconFactory: ILauncherItemId
    {
        public LauncherIconFactory(Guid launcherItemId, LauncherItemKind launcherItemKind, ILauncherItemAddonFinder launcherItemAddonFinder, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            LauncherItemId = launcherItemId;
            LauncherItemKind = launcherItemKind;
            LauncherItemAddonFinder = launcherItemAddonFinder;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        public LauncherItemKind LauncherItemKind { get; }
        ILauncherItemAddonFinder LauncherItemAddonFinder { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        #endregion

        #region function

        private IconImageLoaderBase CreateFileIconLoader(IDispatcherWrapper dispatcherWrapper)
        {
            return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
        }

        public object CreateIconSource(IDispatcherWrapper dispatcherWrapper)
        {
            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
                    }

                case LauncherItemKind.Addon: {
                        var pluginId = MainDatabaseBarrier.ReadData(c => {
                            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            return launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);
                        });
                        if(!LauncherItemAddonFinder.Exists(pluginId)) {
                            Logger.LogError("???????????????????????????: ??????????????????????????? = {0}, ???????????????ID = {1}", LauncherItemId, pluginId);
                            return default!; // null ?????????????????????????????????
                        }
                        var addon = LauncherItemAddonFinder.Find(LauncherItemId, pluginId);
                        return addon;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public object CreateView(object? iconSource, bool useCache, IDispatcherWrapper dispatcherWrapper)
        {
            if(iconSource == null) {
                // ?????????????????????????????????
                // ??????????????????(?????????: IconImageLoaderBase ??????????????????????????????)????????????????????????
                return null!;
            }

            switch(iconSource) {
                case IconImageLoaderBase iconImageLoader:
                    return new IconViewerViewModel(iconImageLoader, dispatcherWrapper, LoggerFactory) {
                        UseCache = useCache,
                    };

                case ILauncherItemExtension launcherItemExtension:
                    return new IconViewerViewModel(LauncherItemId, launcherItemExtension, dispatcherWrapper, LoggerFactory) {
                        UseCache = useCache,
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion
    }
}
