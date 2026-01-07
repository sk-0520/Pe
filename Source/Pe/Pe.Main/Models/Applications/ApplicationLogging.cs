using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;
using NLog;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public delegate IDisposable PauseReceiveLogDelegate();

    internal class ApplicationLogging
    {
        #region define

        private const string InternalLogName = "APPLOG";

        #endregion

        public ApplicationLogging(int internalLogSize, string loggingConfigFilePath, string outputPath, string withLog, bool createDirectory, bool isFullTrace, TimeProvider timeProvider)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(internalLogSize);

            Factory = new LoggerFactory();
            LogManager.Setup().LoadConfigurationFromFile(loggingConfigFilePath);

            var po = new NLog.Extensions.Logging.NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true };
            var prov = new NLog.Extensions.Logging.NLogLoggerProvider(po, LogManager.LogFactory);
            Factory.AddProvider(prov);

            var internalTarget = new NLog.Targets.MethodCallTarget(InternalLogName, ReceiveLog);
            var isEnabledInternalLog = 0 < internalLogSize;

            var logger = Factory.CreateLogger(GetType());
            logger.LogInformation("ログ構築開始: LogLimit = {LogLimit}", internalLogSize);
            
            var enabledLogNames = new HashSet<string>();
            if(isEnabledInternalLog) {
                logger.LogInformation("内部ログあり");
                LogManager.Configuration?.AddTarget(internalTarget);
                InternalLogItems = new ConcurrentFixedQueue<LogEventInfo>(internalLogSize);
                enabledLogNames.Add(InternalLogName);
            } else {
                logger.LogInformation("内部ログなし");
                InternalLogItems = new FixedQueue<LogEventInfo>(1);
            }

            // ログ出力(ファイル・ディレクトリが存在しなければ終了で構わない)
            if(!string.IsNullOrWhiteSpace(outputPath)) {
                var expandedOutputPath = Environment.ExpandEnvironmentVariables(outputPath);
                if(createDirectory) {
                    var fileName = Path.GetFileName(expandedOutputPath);
                    if(!string.IsNullOrEmpty(fileName) && fileName.IndexOf('.') == -1) {
                        // 拡張子がなければディレクトリ指定と決めつけ
                        Directory.CreateDirectory(expandedOutputPath);
                    } else {
                        var parentDir = Path.GetDirectoryName(expandedOutputPath);
                        if(!string.IsNullOrEmpty(parentDir)) {
                            Directory.CreateDirectory(parentDir);
                        }
                    }
                }

                // ディレクトリ指定であればタイムスタンプ付きでファイル生成(プレーンログ)
                var filePath = expandedOutputPath;
                if(Directory.Exists(expandedOutputPath)) {
                    var fileName = PathUtility.AddExtension(timeProvider.GetLocalNow().DateTime.ToString("yyyy-MM-dd_HHmmss", CultureInfo.InvariantCulture), "log");
                    filePath = Path.Combine(expandedOutputPath, fileName);
                }

                //TODO: なんかうまいことする
                switch(Path.GetExtension(filePath)?.ToLowerInvariant() ?? string.Empty) {
                    case ".log":
                        LogManager.LogFactory.Configuration?.Variables.Add("logPath", filePath);
                        enabledLogNames.Add("log");
                        switch(withLog) {
                            case "xml":
                                LogManager.LogFactory.Configuration?.Variables.Add("xmlPath", Path.ChangeExtension(filePath, "xml"));
                                enabledLogNames.Add("xml");
                                break;
                        }
                        break;

                    case ".xml":
                        LogManager.LogFactory.Configuration?.Variables.Add("xmlPath", filePath);
                        enabledLogNames.Add("xml");
                        switch(withLog) {
                            case "log":
                                LogManager.LogFactory.Configuration?.Variables.Add("logPath", Path.ChangeExtension(filePath, "log"));
                                enabledLogNames.Add("log");
                                break;
                        }
                        break;
                }
                LogManager.LogFactory.Configuration?.Variables.Add("dirPath", Path.GetDirectoryName(filePath) ?? string.Empty);
            }

            foreach(var enabledLogName in enabledLogNames) {
                logger.LogInformation("有効ログ: {EnabledLogName}", enabledLogName);
            }

            var traceTargets = enabledLogNames
                .Select(i => LogManager.Configuration?.FindTargetByName(i)!)
                .ToArray()!
            ;

            foreach(var loggingRule in LogManager.Configuration?.LoggingRules ?? []) {
                if(isFullTrace) {
                    if(loggingRule.RuleName == "fulltrace") {
                        foreach(var traceTarget in traceTargets) {
                            loggingRule.Targets.Add(traceTarget);
                        }
                    }
                } else {
                    if(loggingRule.RuleName != "fulltrace") {
                        foreach(var traceTarget in traceTargets) {
                            loggingRule.Targets.Add(traceTarget);
                        }
                    }
                }
            }
            if(isEnabledInternalLog) {
                foreach(var loggingRule in LogManager.Configuration?.LoggingRules.Where(i => i.RuleName == "fulltrace") ?? []) {
                    loggingRule.Targets.Insert(0, internalTarget);
                }
            }

            if(traceTargets.Length != 0) {
                var stopwatch = Stopwatch.StartNew();
                LogManager.ReconfigExistingLoggers();
                LogManager.Flush();
                logger = Factory.CreateLogger(GetType());
                if(isFullTrace) {
                    logger.LogInformation("全データ出力: {Elapsed}", stopwatch.Elapsed);
                } else {
                    logger.LogInformation("データ出力: {Elapsed}", stopwatch.Elapsed);
                }
                foreach(var traceTarget in traceTargets) {
                    logger.LogInformation("{0}", traceTarget);
                }
            }
        }

        #region property

        private IFixedQueue<LogEventInfo> InternalLogItems { get; }

        public LoggerFactory Factory { get; }

        private bool ReceivePausing { get; set; } = false;


        #endregion

        #region function

        public void ReceiveLog(LogEventInfo logEventInfo, object?[] parameters)
        {
            if(ReceivePausing) {
                return;
            }

            InternalLogItems.Enqueue(logEventInfo);
        }

        internal IDisposable PauseReceiveLog()
        {
            ReceivePausing = true;
            return new ActionDisposer(d => ReceivePausing = false);
        }

        public IReadOnlyList<LogEventInfo> GetInternalLogItems() => InternalLogItems.ToArray();

        #endregion
    }
}
