using System;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Main.Models.Applications.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherIconFactory: ILauncherItemId
    {
        public LauncherIconFactory(LauncherItemId launcherItemId, LauncherItemKind launcherItemKind, ILauncherItemAddonFinder launcherItemAddonFinder, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
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

        private ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        public LauncherItemKind LauncherItemKind { get; }
        private ILauncherItemAddonFinder LauncherItemAddonFinder { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        #endregion

        #region function

        public object CreateIconSource(IContextDispatcher contextDispatcher)
        {
            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, contextDispatcher, LoggerFactory);
                    }

                case LauncherItemKind.Addon: {
                        var pluginId = MainDatabaseBarrier.ReadData(c => {
                            var daoFactory = new AppDaoFactory(c, DatabaseStatementLoader, LoggerFactory);
                            var launcherAddonsEntityDao = daoFactory.Create<LauncherAddonsEntityDao>();
                            return launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);
                        });
                        if(!LauncherItemAddonFinder.Exists(pluginId)) {
                            Logger.LogError("プラグイン取得失敗: ランチャーアイテム = {0}, プラグインID = {1}", LauncherItemId, pluginId);
                            return default!; // null は上流で何とかしてちょ
                        }
                        var addon = LauncherItemAddonFinder.Find(LauncherItemId, pluginId);
                        return addon;
                    }

                case LauncherItemKind.Separator: {
                        var control = new Control() {
                            Template = (ControlTemplate)Application.Current.Resources["App-Image-Separator"],
                        };
                        return control;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public object CreateView(object? iconSource, bool useCache, bool isEnabledBadge, IContextDispatcher contextDispatcher)
        {
            if(iconSource == null) {
                // アイコンがないパターン
                // 読み込み失敗(要調査: IconImageLoaderBase は多分出来上がってる)・アドオンがない
                return null!;
            }

            BadgeData badge;
            if(isEnabledBadge) {
                using(var context = MainDatabaseBarrier.WaitRead()) {
                    var daoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                    var launcherBadgesEntityDao = daoFactory.Create<LauncherBadgesEntityDao>();

                    badge = launcherBadgesEntityDao.SelectLauncherBadge(LauncherItemId) ?? BadgeData.CreateEmpty();
                }
            } else {
                badge = BadgeData.CreateEmpty();
            }

            switch(iconSource) {
                case IconImageLoaderBase iconImageLoader:
                    return new IconViewerViewModel(iconImageLoader, badge, contextDispatcher, LoggerFactory) {
                        UseCache = useCache,
                    };

                case ILauncherItemExtension launcherItemExtension:
                    return new IconViewerViewModel(LauncherItemId, launcherItemExtension, badge, contextDispatcher, LoggerFactory) {
                        UseCache = useCache,
                    };

                case DependencyObject dependencyObject:
                    return new IconViewerViewModel(dependencyObject, badge, contextDispatcher, LoggerFactory) {
                        UseCache = useCache,
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }
}
