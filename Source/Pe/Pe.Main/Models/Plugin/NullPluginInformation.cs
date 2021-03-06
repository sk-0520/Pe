using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class NullPluginInformation: PluginInformations
    {
        public NullPluginInformation()
            : base(
                  new PluginIdentifiers(Guid.Empty, "NullPlugin"),
                  new PluginVersions(new Version(), new Version(), new Version()),
                  new PluginAuthors(new Author("Pe"), PluginLicense.Unknown),
                  new PluginCategory(PluginCategories.Others)
            )
        { }
    }
}
