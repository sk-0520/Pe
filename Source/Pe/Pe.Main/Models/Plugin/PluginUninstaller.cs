using System;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグインアンインストール処理。
    /// </summary>
    public class PluginUninstaller
    {
        public PluginUninstaller(IDatabaseContextPack databaseContextPack, IDatabaseStatementLoader statementLoader, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            DatabaseContextPack = databaseContextPack;

            StatementLoader = statementLoader;
            EnvironmentParameters = environmentParameters;
        }

        #region property

        private IDatabaseContextPack DatabaseContextPack { get; }
        private IDatabaseStatementLoader StatementLoader { get; }
        private EnvironmentParameters EnvironmentParameters { get; }

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private IDatabaseContext MainContext => DatabaseContextPack.Main;
        private IDatabaseContext LargeContext => DatabaseContextPack.Large;
        private IDatabaseContext TemporaryContext => DatabaseContextPack.Temporary;

        #endregion

        #region function

        private void UninstallFiles(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginDirManager = new PluginDirectoryManager(EnvironmentParameters);
            var dataDirItems = new[] {
                new { Target = "一時", Directory = pluginDirManager.GetTemporaryDirectory(pluginIdentifiers) },
                new { Target = "端末", Directory = pluginDirManager.GetMachineDirectory(pluginIdentifiers) },
                new { Target = "ユーザー", Directory = pluginDirManager.GetUserDirectory(pluginIdentifiers) },
            };
            foreach(var dataDirItem in dataDirItems) {
                dataDirItem.Directory.Refresh();
                if(dataDirItem.Directory.Exists) {
                    Logger.LogDebug("プラグインデータディレクトリ削除: {0}, {1}", dataDirItem.Target, dataDirItem.Directory);
                    dataDirItem.Directory.Delete(true);
                } else {
                    Logger.LogDebug("プラグインデータディレクトリなし: {0}, {1}", dataDirItem.Target, dataDirItem.Directory);
                }
            }

            var moduleDirectory = pluginDirManager.GetModuleDirectory(pluginIdentifiers);
            moduleDirectory.Delete(true);
        }

        private void UninstallPersistence(IPluginIdentifiers pluginIdentifiers)
        {
            var mainDaoFactory = new AppDaoFactory(MainContext, StatementLoader, LoggerFactory);
            var largeDaoFactory = new AppDaoFactory(LargeContext, StatementLoader, LoggerFactory);

            // デカいデータ破棄
            var pluginValuesEntityDao = largeDaoFactory.Create<PluginValuesEntityDao>();
            pluginValuesEntityDao.DeletePluginValuesByPluginId(pluginIdentifiers.PluginId);

            // ウィジェットデータ破棄
            var pluginWidgetSettingsEntityDao = mainDaoFactory.Create<PluginWidgetSettingsEntityDao>();
            pluginWidgetSettingsEntityDao.DeletePluginWidgetSettingsByPluginId(pluginIdentifiers.PluginId);

            // ランチャーアイテム系列の対象データを連鎖的に破棄(キー設定はきつない？)
            var launcherAddonsEntityDao = mainDaoFactory.Create<LauncherAddonsEntityDao>();
            var deleteTargetLauncherItemIds = launcherAddonsEntityDao.SelectLauncherItemIdsByPluginId(pluginIdentifiers.PluginId).ToArray();
            launcherAddonsEntityDao.DeleteLauncherAddonsByPluginId(pluginIdentifiers.PluginId);

            var pluginVersionChecksEntityDao = mainDaoFactory.Create<PluginVersionChecksEntityDao>();
            pluginVersionChecksEntityDao.DeletePluginVersionChecks(pluginIdentifiers.PluginId);

            var pluginLauncherItemSettingsEntityDao = mainDaoFactory.Create<PluginLauncherItemSettingsEntityDao>();
            pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSettingsByPluginId(pluginIdentifiers.PluginId);

            foreach(var deleteTargetLauncherItemId in deleteTargetLauncherItemIds) {
                var launcherEntityEraser = new LauncherEntityEraser(deleteTargetLauncherItemId, LauncherItemKind.Addon, MainContext, LargeContext, TemporaryContext, StatementLoader, LoggerFactory);
                launcherEntityEraser.Execute();
            }

            var pluginSettingsEntityDao = mainDaoFactory.Create<PluginSettingsEntityDao>();
            pluginSettingsEntityDao.DeleteAllPluginSettings(pluginIdentifiers.PluginId);

            var pluginsEntityDao = mainDaoFactory.Create<PluginsEntityDao>();
            pluginsEntityDao.DeletePlugin(pluginIdentifiers.PluginId);
        }

        public void Uninstall(IPluginIdentifiers pluginIdentifiers)
        {
            Logger.LogInformation("プラグインアンインストール: {0}", pluginIdentifiers);

            UninstallPersistence(pluginIdentifiers);
            UninstallFiles(pluginIdentifiers);
        }

        #endregion
    }
}
