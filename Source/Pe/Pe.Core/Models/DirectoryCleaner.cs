using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class DirectoryCleaner
    {
        public DirectoryCleaner(DirectoryInfo directory, int waitCount, TimeSpan waitTime, ILoggerFactory loggerFactory)
        {
            Directory = directory;
            WaitCount = waitCount;
            WaitTime = waitTime;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        DirectoryInfo Directory { get; }
        int WaitCount { get; }
        TimeSpan WaitTime { get; }
        ILogger Logger { get; }
        #endregion

        #region function

        public bool DeleteDirectory(DirectoryInfo directory, bool directoryIsDelete = true)
        {
            if(!directory.Exists) {
                return false;
            }
            var files = directory.GetFiles();
            foreach(var file in files) {
                if(file.Attributes.HasFlag(FileAttributes.ReadOnly)) {
                    file.Attributes = file.Attributes & ~FileAttributes.ReadOnly;
                }
                file.Delete();
            }
            var dirs = directory.GetDirectories();
            foreach(var dir in dirs) {
                DeleteDirectory(dir, true);
            }

            if(directoryIsDelete) {
                directory.Delete(true);
            }

            return true;
        }

        void CreateDirectory()
        {
            var counter = new Counter(WaitCount);
            foreach(var count in counter) {
                Directory.Create();
                Directory.Refresh();
                if(Directory.Exists) {
                    break;
                } else if(count.IsLast) {
                    Logger.LogError("ディレクトリ作成に失敗: {0}", Directory);
                    return;
                }
                Logger.LogInformation("ディレクトリ作成待機中: {0}/{1} {2}", count.CurrentCount, count.MaxCount, WaitTime);
                Thread.Sleep(WaitTime);
            }
        }

        /// <summary>
        /// <see cref="Directory"/>以下を綺麗にする。
        /// </summary>
        /// <param name="rootDelete">内部的に<see cref="Directory"/>を削除するか</param>
        public void Clear(bool rootDelete = true)
        {
            Directory.Refresh();
            if(Directory.Exists) {
                DeleteDirectory(Directory, rootDelete);
                if(rootDelete) {
                    CreateDirectory();
                }
            } else {
                CreateDirectory();
            }
        }

        #endregion
    }
}
