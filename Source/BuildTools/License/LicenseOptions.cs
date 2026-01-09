using ContentTypeTextNet.Pe.Library.Args;

namespace License
{
    public class LicenseOptions
    {
        #region property

        [CommandLineOption("central-package", CommandLineOptionKind.Value, description: "中央管理している Directory.Packages.props を指定", required: true)]
        public string CentralPackage { get; init; } = string.Empty;

        [CommandLineOption("base", CommandLineOptionKind.Value, description: "入出力元 JSON ファイル", required: true)]
        public string BaseJson { get; init; } = string.Empty;

        [CommandLineOption("output", CommandLineOptionKind.Value, description: "出力先 JSON ファイル", required: true)]
        public string OutputJson { get; init; } = string.Empty;

        #endregion
    }
}
