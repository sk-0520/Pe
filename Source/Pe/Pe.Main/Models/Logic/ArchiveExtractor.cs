using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;
using SevenZipExtractor;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 圧縮ファイル展開処理。
    /// </summary>
    public class ArchiveExtractor
    {
        public ArchiveExtractor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region function

        private void ExtractZip(FileInfo archiveFile, DirectoryInfo extractDirectory, UserNotifyProgress userNotifyProgress)
        {
            var createdDirs = new HashSet<string>();
            var dirSpes = new char[] { '/', '\\' };

            // 使い方間違えてんのか知らんけど ZipFile.ExtractToDirectory ってやたら例外吐かん？
            using(var zipArchive = ZipFile.OpenRead(archiveFile.FullName)) {
                var dirEntries = zipArchive.Entries.Where(a => a.Name.Length == 0 && a.FullName.Length > 0 && dirSpes.Contains(a.FullName[^1])).ToArray();
                var fileEntries = zipArchive.Entries.Where(a => a.Name.Length > 0).Except(dirEntries).ToArray();

                var totalExtractItemCount = dirEntries.Length + fileEntries.Length;
                var extractedItemCount = 0;

                userNotifyProgress.Start();

                foreach(var entry in dirEntries) {
                    var dirPath = PathUtility.SafeCombine(extractDirectory.FullName, entry.FullName);
                    if(!createdDirs.Contains(dirPath) && !Directory.Exists(dirPath)) {
                        Logger.LogTrace("作成: {0}", dirPath);
                        Directory.CreateDirectory(dirPath);
                        createdDirs.Add(dirPath);

                        extractedItemCount += 1;
                        userNotifyProgress.Report(extractedItemCount / (double)totalExtractItemCount, entry.FullName);
                    }
                }

                foreach(var entry in fileEntries) {
                    var expandPath = PathUtility.SafeCombine(extractDirectory.FullName, entry.FullName);
                    Logger.LogTrace("展開: {0}", expandPath);
                    entry.ExtractToFile(expandPath, true);
                    extractedItemCount += 1;
                    userNotifyProgress.Report(extractedItemCount / (double)totalExtractItemCount, entry.FullName);
                }

                userNotifyProgress.End();
            }
        }

        private void Extract7z(FileInfo archiveFile, DirectoryInfo extractDirectory, UserNotifyProgress userNotifyProgress)
        {
            using(var archive = new ArchiveFile(archiveFile.FullName)) {
                var totalExtractItemCount = archive.Entries.Count;
                var extractedItemCount = 0;
                archive.Extract(e => {
                    var expandPath = PathUtility.SafeCombine(extractDirectory.FullName, e.FileName);

                    Logger.LogTrace("展開: {0}", expandPath);
                    extractedItemCount += 1;
                    userNotifyProgress.Report(extractedItemCount / (double)totalExtractItemCount, expandPath);

                    return expandPath;
                });
            }

        }

        /// <summary>
        /// 展開処理。
        /// </summary>
        /// <param name="archiveFile">アーカイブファイル。</param>
        /// <param name="extractDirectory">展開ディレクトリ。</param>
        /// <param name="archive">アーカイブ種別。TODO: <see langword="enum" />に置き換える。</param>
        /// <param name="userNotifyProgress">展開状況進捗報告。</param>
        public void Extract(FileInfo archiveFile, DirectoryInfo extractDirectory, string archive, UserNotifyProgress userNotifyProgress)
        {
            void Extract(string archiveKind)
            {
                switch(archiveKind) {
                    case "zip":
                        ExtractZip(archiveFile, extractDirectory, userNotifyProgress);
                        break;

                    case "7z":
                        Extract7z(archiveFile, extractDirectory, userNotifyProgress);
                        break;

                    default:
                        throw new NotSupportedException($"kind: {archiveKind}");
                }
            }

            if(!string.IsNullOrWhiteSpace(archive)) {
                Extract(archive);
            } else {
                var dotExt = Path.GetExtension(archiveFile.Name);
                if(string.IsNullOrEmpty(dotExt) || dotExt.Length < 2) {
                    throw new Exception("not found extension: " + archiveFile);
                }
                Extract(dotExt.Substring(1).ToLowerInvariant());
            }
        }

        #endregion
    }
}
