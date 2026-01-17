using Xunit;

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

    public class ExceptionSourceGeneratorTest
    {
        #region function

        [Fact]
        public void AbcExceptionTest()
        {
            var type = typeof(AbcException);

            var actualPublic = type.GetConstructors();
            Assert.Equal(3, actualPublic.Length);

            var actualPrivate = type.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            Assert.Equal(4, actualPrivate.Length);
        }

        [Fact]
        public void DefExceptionTest()
        {
            var type = typeof(DefException);

            var actualPublic = type.GetConstructors();
            Assert.Equal(3, actualPublic.Length);

            var actualPrivate = type.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            Assert.Equal(4, actualPrivate.Length);
        }

        [Fact]
        public void GhiExceptionTest()
        {
            var type = typeof(GhiException);

            var actualPublic = type.GetConstructors();
            Assert.Equal(2, actualPublic.Length);

            var actualPrivate = type.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            Assert.Equal(2, actualPrivate.Length);
        }

        [Fact]
        public void JklExceptionTest()
        {
            var type = typeof(JklException);

            var actualPublic = type.GetConstructors();
            Assert.Equal(2, actualPublic.Length);

            var actualPrivate = type.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            Assert.Equal(2, actualPrivate.Length);
        }

        #endregion
    }
}
