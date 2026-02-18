using System;
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

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    [GeneratedException]
    public partial class ConfigurationException: Exception;

    [GeneratedException]
    public partial class ConfigurationNotFoundMethodException: ConfigurationException;
}
