using System;
using ContentTypeTextNet.Pe.Generator.Throws;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    [GeneratedException]
    public partial class SetupException: Exception;

    [GeneratedException]
    public partial class SetupFormatException: SetupException;

}
