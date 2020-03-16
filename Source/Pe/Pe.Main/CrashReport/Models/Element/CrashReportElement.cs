using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Element
{
    internal class CrashReportElement : ElementBase
    {
        public CrashReportElement(CrashReportOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options;
        }

        #region property

        CrashReportOptions Options { get; }
        public CrashReportSaveData Data { get; set; } = new CrashReportSaveData();

        public bool AutoSend => Options.AutoSend;
        public string CrashReportRawFilePath => Environment.ExpandEnvironmentVariables(Options.CrashReportRawFilePath);


        public IReadOnlyList<ObjectDumpItem> RawProperties { get; private set; } = new List<ObjectDumpItem>();

        #endregion

        #region function

        public void Reboot()
        {
            var systemExecutor = new SystemExecutor();
            Logger.LogInformation("App path: {0}", Options.ExecuteCommand);
            Logger.LogInformation("App args: {0}", Options.ExecuteArgument);
            systemExecutor.ExecuteFile(Options.ExecuteCommand, Options.ExecuteArgument);
        }

        CrashReportRawData LoadRawData()
        {
            using var stream = new FileStream(Options.CrashReportRawFilePath, FileMode.Open);
            var serializer = new BinaryDataContractSerializer();
            return serializer.Load<CrashReportRawData>(stream);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            var rawData = LoadRawData();

            var dumper = new ObjectDumper();
            RawProperties = dumper.Dump(rawData);

            Data = new CrashReportSaveData() {
                UserId = rawData.UserId,
                Timestamp = rawData.Timestamp,
                Exception = rawData.Exception,
                Informations = rawData.Informations,
                LogItems = rawData.LogItems,
            };
        }

        #endregion
    }
}
