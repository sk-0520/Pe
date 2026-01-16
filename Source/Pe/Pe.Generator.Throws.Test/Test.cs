namespace ContentTypeTextNet.Pe.Generator.Throws.Test
{
    [GenerateException]
    public partial class AbcException: System.Exception
    { }

    [GenerateException]
    public partial class DefException: AbcException
    { }

    public abstract class GhiException: System.Exception
    {
        public enum GhiCode
        {
            A,
            B,
            C,
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarAnalyzer.CSharp", "S3442:Abstract methods should not be static", Justification = "<Pending>")]
        public GhiException(GhiCode code)
            : base()
        {
            Code = code;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarAnalyzer.CSharp", "S3442:Abstract methods should not be static", Justification = "<Pending>")]
        public GhiException(GhiCode code, string message)
            : base(message)
        {
            Code = code;
        }

        public GhiCode Code { get; }
    }

    [GenerateException]
    public partial class JklException: GhiException
    { }
}
