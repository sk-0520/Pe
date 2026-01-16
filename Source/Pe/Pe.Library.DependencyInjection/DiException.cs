using System;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Throws;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GenerateExceptionAttribute: System.Attribute
{
    public GenerateExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// DI処理でわっけ分からんことになったら投げられる例外。
    /// </summary>
    /// <remarks>
    /// <para>内部的に <see cref="ArgumentException"/> 等を投げる場合はわざわざラップしないのでこの例外だけ受ければ良いという話ではない。</para>
    /// </remarks>
    [GenerateException]
    public partial class DiException: Exception
    { }

    [GenerateException]
    public partial class DiFunctionMethodNotFoundException: DiException
    { }

    [GenerateException]
    public partial class DiFunctionResultException: DiException
    { }
}
