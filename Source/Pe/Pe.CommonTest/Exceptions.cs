using System.Runtime.CompilerServices;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Throws;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GeneratedExceptionAttribute: System.Attribute
{
    public GeneratedExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// テストインフラ系例外。
    /// </summary>
    /// <remarks>こいつを捕まえてどうこうすることはない。</remarks>
    [GeneratedException]
    public partial class TestException: System.Exception
    {
        public static T ThrowIfNull<T>(T? value, string? message = null, [CallerArgumentExpression(nameof(value))] string valueExpression = "")
            where T : class
        {
            if(value is null) {
                var msg = (message ?? $"{nameof(value)} is null") + $" ({valueExpression})";

                throw new TestException(msg);
            }
            return value;
        }
    }

    #region PrivateObject

    [GeneratedException]
    public partial class PrivateObjectException: TestException
    { }

    [GeneratedException]
    public partial class PrivateObjectFieldException: PrivateObjectException
    { }

    [GeneratedException]
    public partial class PrivateObjectPropertyException: PrivateObjectException
    { }

    [GeneratedException]
    public partial class PrivateObjectMethodException: PrivateObjectException
    { }

    [GeneratedException]
    public partial class PrivateObjectMethodParametersException: PrivateObjectException
    { }

    #endregion

    #region TestIO

    [GeneratedException]
    public partial class TestIOException: TestException
    { }

    [GeneratedException]
    public partial class TestIOInitializedException: TestIOException
    { }

    #endregion
}
