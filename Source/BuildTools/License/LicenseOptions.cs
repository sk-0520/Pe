using ContentTypeTextNet.Pe.Library.Args;

namespace License
{
    public class LicenseOptions
    {
        #region property

        [CommandLineOption("central-package", CommandLineOptionKind.Value, description: "中央管理している Directory.Packages.props を指定", required: true)]
        public string CentralPackage { get; init; } = string.Empty;

        #endregion
    }
}
