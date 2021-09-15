using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region property

        #endregion

        #region function


        /// <summary>
        /// アプリケーション新バージョン遅延チェック。
        /// </summary>
        /// <returns></returns>
        public async Task DelayCheckApplicationNewVersionAsync()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            UpdateKind updateKind;
            using(var context = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppUpdateSettingEntityDao>(context, context.Implementation);
                updateKind = dao.SelectSettingUpdateSetting().UpdateKind;
            }

            if(updateKind == UpdateKind.None) {
                return;
            }

            var updateWait = ApplicationDiContainer.Build<ApplicationConfiguration>().General.UpdateWait;
            await Task.Delay(updateWait).ConfigureAwait(false);
            var updateCheckKind = updateKind switch {
                UpdateKind.Notify => UpdateCheckKind.CheckOnly,
                _ => UpdateCheckKind.Update,
            };
            await CheckApplicationNewVersionAsync(updateCheckKind).ConfigureAwait(false);
        }

        /// <summary>
        /// アプリケーション新バージョンチェック。
        /// </summary>
        /// <param name="updateCheckKind"></param>
        /// <returns></returns>
        public async Task CheckApplicationNewVersionAsync(UpdateCheckKind updateCheckKind)
        {
            if(ApplicationUpdateInfo.State == NewVersionState.None || ApplicationUpdateInfo.State == NewVersionState.Error) {
                if(ApplicationUpdateInfo.State == NewVersionState.Error) {
                    Logger.LogInformation("エラーありのため再実施");
                }
            } else {
                if(ApplicationUpdateInfo.IsReady) {
                    Logger.LogInformation("アップデート準備完了");
                } else {
                    Logger.LogInformation("アップデート排他制御中");
                }

                if(ApplicationUpdateInfo.NewVersionItem != null) {
                    ShowNewVersionReleaseNote(ApplicationUpdateInfo.NewVersionItem, false);
                } else {
                    Logger.LogInformation("アップデート情報未取得");
                }

                return;
            }

            var newVersionChecker = ApplicationDiContainer.Build<NewVersionChecker>();

            ApplicationUpdateInfo.State = NewVersionState.Checking;
            {
                var appVersion = await newVersionChecker.CheckApplicationNewVersionAsync().ConfigureAwait(false);
                if(appVersion == null) {
                    Logger.LogInformation("アップデートなし");
                    ApplicationUpdateInfo.State = NewVersionState.None;
                    return;
                }
                ApplicationUpdateInfo.NewVersionItem = appVersion;
            }

            Logger.LogInformation("アップデートあり: {0}", ApplicationUpdateInfo.NewVersionItem.Version);

            // CheckApplicationUpdateAsync で弾いてる
            //if(BuildStatus.Version < ApplicationUpdateInfo.UpdateItem.MinimumVersion) {
            //    Logger.LogWarning("最低バージョン未満であるためバージョンアップ不可: 現在 = {0}, 要求 = {1}", BuildStatus.Version, appVersion.MinimumVersion);
            //    UpdateInfo.State = UpdateState.None;
            //    return;
            //}

            Logger.LogInformation("アップデート可能");

            var newVersionDownloader = ApplicationDiContainer.Build<NewVersionDownloader>();

            if(updateCheckKind != UpdateCheckKind.ForceUpdate) {
                try {
                    ShowNewVersionReleaseNote(ApplicationUpdateInfo.NewVersionItem, updateCheckKind == UpdateCheckKind.CheckOnly);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    ApplicationUpdateInfo.SetError(ex.Message);
                    return;
                }
            }
            if(updateCheckKind == UpdateCheckKind.CheckOnly) {
                ApplicationUpdateInfo.State = NewVersionState.None;
                return;
            }

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var versionConverter = new VersionConverter();
            var downloadFileName = versionConverter.ConvertFileName(BuildStatus.Name, ApplicationUpdateInfo.NewVersionItem.Version, ApplicationUpdateInfo.NewVersionItem.Platform, ApplicationUpdateInfo.NewVersionItem.ArchiveKind);
            var downloadFilePath = Path.Combine(environmentParameters.MachineUpdateArchiveDirectory.FullName, downloadFileName);
            var downloadFile = new FileInfo(downloadFilePath);
            try {
                var skipDownload = false;
                downloadFile.Refresh();
                if(downloadFile.Exists) {
                    ApplicationUpdateInfo.State = NewVersionState.Checksumming;
                    skipDownload = await newVersionDownloader.ChecksumAsync(ApplicationUpdateInfo.NewVersionItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.ChecksumProgress, ApplicationUpdateInfo.CurrentLogProgress));
                    if(!skipDownload) {
                        downloadFile.Delete(); // ゴミは消しとく
                    }
                }
                if(skipDownload) {
                    Logger.LogInformation("チェックサムの結果ダウンロード不要");
                    IProgress<double> progress = ApplicationUpdateInfo.DownloadProgress;
                    progress.Report(1);
                } else {
                    ApplicationUpdateInfo.State = NewVersionState.Downloading;
                    await newVersionDownloader.DownloadArchiveAsync(ApplicationUpdateInfo.NewVersionItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.DownloadProgress, ApplicationUpdateInfo.CurrentLogProgress)).ConfigureAwait(false);

                    // ここで更新しないとチェックサムでファイル無し判定を食らう
                    downloadFile.Refresh();

                    ApplicationUpdateInfo.State = NewVersionState.Checksumming;
                    var checksumOk = await newVersionDownloader.ChecksumAsync(ApplicationUpdateInfo.NewVersionItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.ChecksumProgress, ApplicationUpdateInfo.CurrentLogProgress));
                    if(!checksumOk) {
                        Logger.LogError("チェックサム異常あり");
                        ApplicationUpdateInfo.SetError(Properties.Resources.String_Download_ChecksumError);
                        return;
                    }
                }
            } catch(Exception ex) {
                ApplicationUpdateInfo.SetError(ex.Message);
                return;
            }

            Logger.LogInformation("アップデートファイル展開");
            ApplicationUpdateInfo.State = NewVersionState.Extracting;
            try {
                var directoryCleaner = new DirectoryCleaner(environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, LoggerFactory);
                directoryCleaner.Clear(false);

                var archiveExtractor = ApplicationDiContainer.Build<ArchiveExtractor>();
                archiveExtractor.Extract(downloadFile, environmentParameters.TemporaryApplicationExtractDirectory, ApplicationUpdateInfo.NewVersionItem.ArchiveKind, new UserNotifyProgress(ApplicationUpdateInfo.ExtractProgress, ApplicationUpdateInfo.CurrentLogProgress));

                // #766ここの例外はきちんと対応した際にはなくなる
                var newAppPath = Path.Join(environmentParameters.TemporaryApplicationExtractDirectory.FullName, EnvironmentParameters.RootApplicationName);
                Logger.LogInformation("アプリケーション試走: {0}", newAppPath);
                using var process = new Process();
                process.StartInfo.FileName = newAppPath;
                process.StartInfo.Arguments = "-_mode dry-run";
                try {
                    if(process.Start()) {
                        process.WaitForExit();
                        if(process.ExitCode != 0) {
                            Logger.LogError("アプリケーション試走失敗: {0}", process.ExitCode);
                            //ApplicationUpdateInfo.SetError(#766-exec);
                            //return;
                        }
                    } else {
                        Logger.LogError("アプリケーション試走実行失敗");
                        //ApplicationUpdateInfo.SetError(#766-run);
                        //return;
                    }
                } catch(Exception innerEx) {
                    Logger.LogError(innerEx, innerEx.Message);
                }

                var scriptFactory = ApplicationDiContainer.Build<ApplicationUpdateScriptFactory>();
                var exeutePathParameter = scriptFactory.CreateUpdateExecutePathParameter(environmentParameters.EtcUpdateScriptFile, environmentParameters.TemporaryDirectory, environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.RootDirectory);
                ApplicationUpdateInfo.Path = exeutePathParameter;
                ApplicationUpdateInfo.State = NewVersionState.Ready;

                // アップデートアーカイブのローテート
                var fileRotation = new FileRotation();
                fileRotation.ExecuteExtensions(
                    environmentParameters.MachineUpdateArchiveDirectory,
                    new[] { "zip", "7z" },
                    environmentParameters.ApplicationConfiguration.Backup.ArchiveCount,
                    ex => {
                        Logger.LogWarning(ex, ex.Message);
                        return true;
                    }
                );

                // リリースノート再表示
                ShowNewVersionReleaseNote(ApplicationUpdateInfo.NewVersionItem, false);

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                ApplicationUpdateInfo.SetError(ex.Message);
            }
        }

        PluginInstaller CreatePluginInstaller(EnvironmentParameters environmentParameters)
        {
            var pluginInstaller = new PluginInstaller(
                ApplicationDiContainer.Build<PluginContainer>(),
                ApplicationDiContainer.Build<PluginConstructorContext>(),
                Logging.PauseReceiveLog,
                environmentParameters,
                ApplicationDiContainer.Build<IDatabaseStatementLoader>(),
                ApplicationDiContainer.Build<LoggerFactory>()
            );

            return pluginInstaller;
        }

        async Task<bool> CheckPluginNewVersionAsync(Guid pluginId, string pluginName, Version pluginVersion)
        {
            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var newVersionChecker = ApplicationDiContainer.Build<NewVersionChecker>();
            var mainDatabaseBarrier = ApplicationDiContainer.Get<IMainDatabaseBarrier>();
            var temporaryDatabaseBarrier = ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            // 新バージョンチェック
            IReadOnlyList<string> urls;
            using(var context = mainDatabaseBarrier.WaitWrite()) {
                var pluginVersionChecksEntityDao = new PluginVersionChecksEntityDao(context, statementLoader, context.Implementation, LoggerFactory);
                urls = pluginVersionChecksEntityDao.SelectPluginVersionCheckUrls(pluginId).ToList();
            }

            var newVersionItem = await newVersionChecker.CheckPluginNewVersionAsync(pluginId, pluginVersion, urls);
            if(newVersionItem is null) {
                return false;
            }

            // 最新バージョンアーカイブ取得
            var newVersionDownloader = ApplicationDiContainer.Build<NewVersionDownloader>();
            var notifyProgress = ApplicationDiContainer.Build<NullNotifyProgress>();
            var versionConverter = ApplicationDiContainer.Build<VersionConverter>();

            var pluginDownloadDirectoryPath = Path.Join(environmentParameters.MachineUpdatePluginDirectory.FullName, PluginUtility.ConvertDirectoryName(pluginId));
            var pluginDownloadFileName = PathUtility.AddExtension(versionConverter.ToFileString(newVersionItem.Version), newVersionChecker.GetExtension(newVersionItem));
            var pluginArchivePath = Path.Join(pluginDownloadDirectoryPath, pluginDownloadFileName);
            var pluginArchiveFile = new FileInfo(pluginArchivePath);
            pluginArchiveFile.Refresh();
            var skipDownload = false;
            if(pluginArchiveFile.Exists) {
                skipDownload = await newVersionDownloader.ChecksumAsync(newVersionItem, pluginArchiveFile, notifyProgress);
                if(skipDownload) {
                    Logger.LogInformation("[{0}] 既存ファイルのチェックサムは正常", pluginId);
                } else {
                    pluginArchiveFile.Delete();
                }
            }
            if(!skipDownload) {
                FileUtility.MakeFileParentDirectory(pluginArchiveFile);
                await newVersionDownloader.DownloadArchiveAsync(newVersionItem, pluginArchiveFile, notifyProgress);

                // ここで更新しないとチェックサムでファイル無し判定を食らう(知らんけど CheckApplicationNewVersionAsync でそう言ってる)
                pluginArchiveFile.Refresh();

                var checksumOk = await newVersionDownloader.ChecksumAsync(newVersionItem, pluginArchiveFile, notifyProgress);
                if(!checksumOk) {
                    Logger.LogError("[{0}] チェックサム異常あり", pluginId);
                    return false;
                }
            }

            // ファイル展開
            IReadOnlyList<PluginInstallData> installItems;
            using(var context = temporaryDatabaseBarrier.WaitRead()) {
                var installPluginsEntityDao = ApplicationDiContainer.Build<InstallPluginsEntityDao>(context, context.Implementation);
                installItems = installPluginsEntityDao.SelectInstallPlugins().ToList();
            }

            var pluginInstaller = CreatePluginInstaller(environmentParameters);
            var pluginInstallData = await pluginInstaller.InstallPluginArchiveAsync(pluginName, pluginArchiveFile, newVersionItem.ArchiveKind, false, installItems, ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>());

            Logger.LogInformation("{0}", pluginInstallData);

            return true;
        }

        async Task<bool> CheckPluginsNewVersionAsync()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Get<IMainDatabaseBarrier>();
            IList<PluginLastUsedData> lastUsedPlugins;
            using(var context = mainDatabaseBarrier.WaitRead()) {
                var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(context, context.Implementation);
                lastUsedPlugins = pluginsEntityDao.SelectAllLastUsedPlugins().ToList();
            }
            var newVersionPlugins = new Dictionary<Guid, bool>(lastUsedPlugins.Count);
            foreach(var lastUsePlugin in lastUsedPlugins) {
                try {
                    var newVersion = await CheckPluginNewVersionAsync(lastUsePlugin.PluginId, lastUsePlugin.Name, lastUsePlugin.Version);
                    newVersionPlugins[lastUsePlugin.PluginId] = newVersion;
                } catch(Exception ex) {
                    Logger.LogError(ex, $"[{lastUsePlugin.PluginId}] {ex.Message}");
                    newVersionPlugins[lastUsePlugin.PluginId] = false;
                }
            }
            foreach(var pair in newVersionPlugins) {
                Logger.LogInformation("{0}: {1}", pair.Key, pair.Value);
            }

            return newVersionPlugins.Any(i => i.Value);
        }


        #endregion
    }
}
