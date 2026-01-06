using ContentTypeTextNet.Pe.Library.Args;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    public class CrashReportOptions
    {
        #region property

        /// <summary>
        /// 言語。
        /// </summary>
        [CommandLineOption("language", kind: CommandLineOptionKind.Value)]
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// 自動送信するか。
        /// </summary>
        [CommandLineOption("auto-send", kind: CommandLineOptionKind.Switch)]
        public bool AutoSend { get; set; }

        /// <summary>
        /// 送信先。
        /// </summary>
        [CommandLineOption("post-uri", kind: CommandLineOptionKind.Value)]
        public string PostUri { get; set; } = string.Empty;
        /// <summary>
        /// 送信先。
        /// </summary>
        [CommandLineOption("src-uri", kind: CommandLineOptionKind.Value)]
        public string SourceUri { get; set; } = string.Empty;

        /// <summary>
        /// 生クラッシュレポートのファイルパス。
        /// </summary>
        [CommandLineOption("report-raw-file", kind: CommandLineOptionKind.Value)]
        public string CrashReportRawFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 送信時に保存されるクラッシュレポートのファイルパス。
        /// </summary>
        [CommandLineOption("report-save-file", kind: CommandLineOptionKind.Value)]
        public string CrashReportSaveFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Pe の実行コマンド。
        /// </summary>
        [CommandLineOption("execute-command", kind: CommandLineOptionKind.Value)]
        public string ExecuteCommand { get; set; } = string.Empty;

        /// <summary>
        /// Pe の起動時オプション。
        /// </summary>
        [CommandLineOption("execute-argument", kind: CommandLineOptionKind.Value)]
        public string ExecuteArgument { get; set; } = string.Empty;

        #endregion
    }
}
