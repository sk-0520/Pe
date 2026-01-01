using System;
using ContentTypeTextNet.Pe.Generator.Exception;

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
