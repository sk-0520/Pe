using System;
using ContentTypeTextNet.Pe.Generator.Throws;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    [GeneratedException]
    public partial class ConfigurationException: Exception;

    [GeneratedException]
    public partial class ConfigurationNotFoundMethodException: ConfigurationException;
}
