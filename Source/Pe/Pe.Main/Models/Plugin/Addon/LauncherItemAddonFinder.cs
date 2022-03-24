using System;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public interface ILauncherItemAddonFinder
    {
        #region function

        bool Exists(Guid pluginId);

        IPlugin GetPlugin(Guid pluginId);

        ILauncherItemExtension Find(LauncherItemId launcherItemId, Guid pluginId);

        #endregion
    }

    internal class LauncherItemAddonFinder: ILauncherItemAddonFinder
    {
        public LauncherItemAddonFinder(AddonContainer addonContainer, ILoggerFactory loggerFactory)
        {
            AddonContainer = addonContainer;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private AddonContainer AddonContainer { get; }
        private ILogger Logger { get; }

        #endregion

        #region ILauncherItemAddonFinder

        public bool Exists(Guid pluginId)
        {
            return AddonContainer.Plugins.Any(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginId);
        }

        public IPlugin GetPlugin(Guid pluginId)
        {
            return AddonContainer.Plugins.First(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginId);
        }

        public LauncherItemAddonProxy Find(LauncherItemId launcherItemId, Guid pluginId)
        {
            return AddonContainer.GetLauncherItemAddon(launcherItemId, pluginId);
        }

        ILauncherItemExtension ILauncherItemAddonFinder.Find(LauncherItemId launcherItemId, Guid pluginId) => Find(launcherItemId, pluginId);

        #endregion
    }
}
