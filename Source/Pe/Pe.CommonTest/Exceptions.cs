#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Exception;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GenerateExceptionAttribute: System.Attribute
{
    public GenerateExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// テストインフラ系例外。
    /// </summary>
    /// <remarks>こいつを捕まえてどうこうすることはない。</remarks>
    [GenerateException]
    public partial class TestException: System.Exception
    { }

    #region PrivateObject

    [GenerateException]
    public partial class PrivateObjectException: TestException
    { }

    [GenerateException]
    public partial class PrivateObjectFieldException: PrivateObjectException
    { }

    [GenerateException]
    public partial class PrivateObjectPropertyException: PrivateObjectException
    { }

    [GenerateException]
    public partial class PrivateObjectMethodException: PrivateObjectException
    { }

    [GenerateException]
    public partial class PrivateObjectMethodParametersException: PrivateObjectException
    { }

    #endregion

    #region TestIO

    [GenerateException]
    public partial class TestIOException: TestException
    { }

    [GenerateException]
    public partial class TestIOInitializedException: TestIOException
    { }

    #endregion
}
