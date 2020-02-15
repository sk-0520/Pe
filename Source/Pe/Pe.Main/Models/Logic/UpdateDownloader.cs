using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UpdateDownloader
    {
        public UpdateDownloader(CustomConfiguration configuration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Configuration = configuration;
            UserAgentManager = userAgentManager;
        }

        #region property

        ILogger Logger { get; }

        CustomConfiguration Configuration { get; }
        IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public async Task<bool> ChecksumAsync(UpdateItemData updateItem, FileInfo targetFile, IProgress<double> progress)
        {
            await Task.Delay(1);
            return false;
        }

        public async Task DownloadApplicationArchiveAsync(UpdateItemData updateItem, FileInfo donwloadFile, IProgress<double> progress)
        {
            Logger.LogInformation("アップデートファイルダウンロード: {0}, {1}", updateItem.ArchiveUri, donwloadFile);
            progress.Report(0);

            using(var userAgent = UserAgentManager.CreateAppUserAgent()) {
                var content = await userAgent.GetAsync(updateItem.ArchiveUri);

                //NOTE: long が使えない！
                int downloadedSize = 0;
                int downloadChunkSize = 1024 * 4;

                //TODO: ダウンロード進捗のあれこれ
                using(var networkStream = await content.Content.ReadAsStreamAsync()) {
                    using(var localStream = donwloadFile.Create()) {
                        var downloadChunk = new byte[downloadChunkSize];

                        while(true) {
                            var readSize = networkStream.Read(downloadChunk, 0, downloadChunk.Length);
                            if(0 < readSize) {
                                localStream.Write(downloadChunk, downloadedSize, readSize);
                                downloadChunkSize += readSize;
                                progress.Report(downloadChunkSize / (double)updateItem.ArchiveSize);
                            } else {
                                progress.Report(1);
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

    }
}
