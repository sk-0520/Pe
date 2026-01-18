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

namespace ContentTypeTextNet.Pe.Library.Database
{
    [GenerateException]
    public partial class DatabaseException: Exception
    { }

    [GenerateException]
    public partial class DatabaseStatementException: DatabaseException
    { }

    [GenerateException]
    public partial class DatabaseManipulationException: DatabaseException
    { }

    [GenerateException]
    public partial class DatabaseFactoryException: DatabaseException
    { }
}
