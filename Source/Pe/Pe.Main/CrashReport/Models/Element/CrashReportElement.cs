using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Element
{
    public class CrashReportElement: ElementBase
    {
        public CrashReportElement(CrashReportOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options;
            SendStatus = new RunningStatus(LoggerFactory);
        }

        #region property

        private TimeSpan RetryWaitTime { get; } = TimeSpan.FromSeconds(5);
        private CrashReportOptions Options { get; }
        public CrashReportSaveData Data { get; set; } = new CrashReportSaveData();

        public bool AutoSend => Options.AutoSend;
        public string CrashReportRawFilePath => Environment.ExpandEnvironmentVariables(Options.CrashReportRawFilePath);


        public IReadOnlyList<ObjectDumpItem> RawProperties { get; private set; } = new List<ObjectDumpItem>();

        public RunningStatus SendStatus { get; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public string CrashReportSaveFilePath { get; private set; } = string.Empty;

        #endregion

        #region function

        public void ShowSourceUri()
        {
            var systemExecutor = new SystemExecutor();
            try {
                systemExecutor.OpenUri(new Uri(Options.SourceUri));
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
        }

        public void CancelAutoSend()
        {
            Options.AutoSend = false;
        }

        public async Task SendAsync(CancellationToken cancellationToken)
        {
            ErrorMessage = string.Empty;
            SendStatus.State = RunningState.Running;

            // 入力内容をファイルに保存及び、該当ファイルを転送する
            var path = Environment.ExpandEnvironmentVariables(Options.CrashReportSaveFilePath);
            IOUtility.MakeFileParentDirectory(path);
            var jsonValue = JsonSerializer.Serialize(Data, Data.GetType());
            using(var writer = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), Encoding.UTF8)) {
                await writer.WriteAsync(jsonValue);
            }
            CrashReportSaveFilePath = path;

            //var map = new Dictionary<string, string>() {
            //    ["report"] = jsonValue,
            //};
            //var encoding = Encoding.UTF8;
            //var items = map
            //    .Select(p => new { Key = HttpUtility.UrlEncode(p.Key, encoding), Value = HttpUtility.UrlEncode(p.Value, encoding) })
            //    .Select(i => $"{i.Key}={i.Value}");
            //;
            //var param = string.Join("&", items);
            //var content = new StringContent(param, encoding, "application/x-www-form-urlencoded");
            var content = new StringContent(jsonValue, Encoding.UTF8, "application/json");

            foreach(var counter in new Counter(5)) {
                Logger.LogInformation("post {PostUri}: {CurrentCount}/{MaxCount}", Options.PostUri, counter.CurrentCount, counter.MaxCount);
                // 失敗時にリフレッシュしたいので毎回生成する
                try {
                    using(var httpClient = new HttpClient()) {
                        var result = await httpClient.PostAsync(Options.PostUri, content, cancellationToken);
                        if(result.IsSuccessStatusCode) {
                            Logger.LogInformation("送信完了");
                            var rawResponse = await result.Content.ReadAsStringAsync(cancellationToken);
                            var response = JsonSerializer.Deserialize<CrashReportResponse>(rawResponse);

                            if(response != null && response.Success) {
                                SendStatus.State = RunningState.End;
                                Logger.LogInformation("BODY: {Response}", rawResponse);
                            } else {
                                ErrorMessage = response?.Message ?? Properties.Resources.String_Common_Network_UnknownResponse;
                                SendStatus.State = RunningState.Error;
                            }

                            return;
                        }
                        var s = await result.Content.ReadAsStringAsync(cancellationToken);
                        Logger.LogWarning("HTTP: {StatusCode}, {Content}", result.StatusCode, s);
                        if(!result.IsSuccessStatusCode && counter.IsLast) {
                            ErrorMessage = result.StatusCode.ToString();
                            SendStatus.State = RunningState.Error;
                        }
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    if(!counter.IsLast) {
                        Logger.LogDebug("待機中: {RetryWaitTime}", RetryWaitTime);
                        await Task.Delay(RetryWaitTime, cancellationToken);
                    } else {
                        ErrorMessage = ex.Message;
                        SendStatus.State = RunningState.Error;
                    }
                }
            }
        }

        public void Reboot()
        {
            var systemExecutor = new SystemExecutor();
            Logger.LogInformation("App path: {ExecuteCommand}", Options.ExecuteCommand);
            Logger.LogInformation("App args: {ExecuteArgument}", Options.ExecuteArgument);
            systemExecutor.ExecuteFile(Options.ExecuteCommand, Options.ExecuteArgument);
        }

        public void Cancel()
        {
            SendStatus.State = RunningState.None;
        }

        private CrashReportRawData LoadRawData()
        {
            using var stream = new FileStream(Options.CrashReportRawFilePath, FileMode.Open);
            var serializer = new CrashReportSerializer();
            return serializer.Load<CrashReportRawData>(stream);
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            var rawData = LoadRawData();

            var dumper = new ObjectDumper();
            RawProperties = dumper.Dump(rawData);

            var versionConverter = new VersionConverter();

            Data = new CrashReportSaveData() {
                UserId = rawData.UserId,
                Version = versionConverter.ConvertNormalVersion(rawData.Version),
                Build = rawData.Build,
                Revision = rawData.Revision,
                Exception = rawData.Exception,
                Information = rawData.InformationMap,
                LogItems = rawData.LogItems,
            };

            return Task.CompletedTask;
        }

        #endregion
    }
}
