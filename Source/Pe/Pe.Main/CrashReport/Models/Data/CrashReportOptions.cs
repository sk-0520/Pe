using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Args;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    public class CrashReportOptions
    {
        #region property

        /// <summary>
        /// 言語。
        /// </summary>
        [CommandLine(longKey: "language", kind: CommandLineKeyKind.Value)]
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// 自動送信するか。
        /// </summary>
        [CommandLine(longKey: "auto-send", kind: CommandLineKeyKind.Switch)]
        public bool AutoSend { get; set; }

        /// <summary>
        /// 送信先。
        /// </summary>
        [CommandLine(longKey: "post-uri", kind: CommandLineKeyKind.Value)]
        public string PostUri { get; set; } = string.Empty;
        /// <summary>
        /// 送信先。
        /// </summary>
        [CommandLine(longKey: "src-uri", kind: CommandLineKeyKind.Value)]
        public string SourceUri { get; set; } = string.Empty;

        /// <summary>
        /// 生クラッシュレポートのファイルパス。
        /// </summary>
        [CommandLine(longKey: "report-raw-file", kind: CommandLineKeyKind.Value)]
        public string CrashReportRawFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 送信時に保存されるクラッシュレポートのファイルパス。
        /// </summary>
        [CommandLine(longKey: "report-save-file", kind: CommandLineKeyKind.Value)]
        public string CrashReportSaveFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Pe の実行コマンド。
        /// </summary>
        [CommandLine(longKey: "execute-command", kind: CommandLineKeyKind.Value)]
        public string ExecuteCommand { get; set; } = string.Empty;

        /// <summary>
        /// Pe の起動時オプション。
        /// </summary>
        [CommandLine(longKey: "execute-argument", kind: CommandLineKeyKind.Value)]
        public string ExecuteArgument { get; set; } = string.Empty;

        #endregion
    }
}
