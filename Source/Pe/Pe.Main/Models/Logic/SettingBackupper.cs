using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class SettingBackupper
    {
        public SettingBackupper(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        public void BackupUserSetting(DirectoryInfo userDirectory, DirectoryInfo targetDirectory, string backupFileBaseName, int enabledCount)
        {
            if(enabledCount < 1) {
                Logger.LogInformation("バックアップ世代が無効値のため処理終了");
                return;
            }

            var backupFilePath = Path.Combine(targetDirectory.FullName, backupFileBaseName + ".zip");

            Logger.LogDebug("バックアップ処理 開始: {0}", backupFilePath);
            using(var stream = new BufferedStream(new FileStream(backupFilePath, FileMode.Create, FileAccess.Write))) {
                var archive = new ZipArchive(stream, ZipArchiveMode.Create);
                var files = userDirectory.EnumerateFiles("*", SearchOption.AllDirectories);
                foreach(var file in files) {
                    var entryPath = file.FullName.Substring(userDirectory.FullName.Length);
                    while(entryPath.StartsWith(Path.DirectorySeparatorChar)) {
                        entryPath = entryPath.Substring(1);
                    }
                    var entry = archive.CreateEntry(entryPath, CompressionLevel.Optimal);
                    using var entryStream = entry.Open();
                    using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    fileStream.CopyTo(entryStream);
                }
            }
            //ZipFile.CreateFromDirectory(userDirectory.FullName, backupFilePath);
            Logger.LogDebug("バックアップ処理 終了: {0}", backupFilePath);

            //FileUtility.RotateFiles(targetDirectory.FullName, "*.zip", enabledCount, ex => {
            //    Logger.LogWarning(ex, ex.Message);
            //    return true;
            //});
            var fileRotation = new FileRotation();
            fileRotation.ExecuteWildcard(targetDirectory, "*.zip", enabledCount, ex => {
                Logger.LogWarning(ex, ex.Message);
                return true;
            });
        }

        public void BackupUserSettingToCustomDirectory(DirectoryInfo userDirectory, DirectoryInfo targetDirectory)
        {
            Logger.LogInformation("ユーザー設定バックアップディレクトリへバックアップ: {0}", targetDirectory.FullName);

            targetDirectory.Refresh();
            if(!targetDirectory.Exists) {
                Logger.LogDebug("ユーザー設定バックアップディレクトリ作成: {0}", targetDirectory.FullName);
                targetDirectory.Create();
            }

            var createdDirs = new HashSet<string>();
            var copiedFiles = new List<string>();
            //TODO: 差分とって不要なファイルは破棄するようにしたい
            var files = userDirectory
                .EnumerateFiles("*", SearchOption.AllDirectories)
            ;
            foreach(var file in files) {
                var relPath = Path.GetRelativePath(userDirectory.FullName, file.FullName);
                var dirPath = Path.GetDirectoryName(relPath) ?? string.Empty;
                var targetParentDirPath = Path.Combine(targetDirectory.FullName, dirPath);

                var cahcheDirPath = targetParentDirPath.ToLowerInvariant();
                if(!createdDirs.Contains(cahcheDirPath)) {
                    Logger.LogDebug("ユーザー設定バックアップ サブディレクトリ作成: {0}", targetParentDirPath);
                    Directory.CreateDirectory(targetParentDirPath);
                    createdDirs.Add(cahcheDirPath);
                }

                var targetFilePath = Path.Combine(targetDirectory.FullName, relPath);
                File.Copy(file.FullName, targetFilePath, true);
                copiedFiles.Add(relPath);
            }
        }

        #endregion
    }
}
