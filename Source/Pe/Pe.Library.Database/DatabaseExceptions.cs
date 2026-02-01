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

namespace ContentTypeTextNet.Pe.Library.Database
{
    [GeneratedException]
    public partial class DatabaseException: Exception
    { }

    [GeneratedException]
    public partial class DatabaseStatementException: DatabaseException
    { }

    [GeneratedException]
    public partial class DatabaseManipulationException: DatabaseException
    { }

    [GeneratedException]
    public partial class DatabaseFactoryException: DatabaseException
    { }
}
