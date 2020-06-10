using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.FileFinder.Addon;
using ContentTypeTextNet.Pe.Plugins.FileFinder.Preferences;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder
{
    public class FileFinder: PluginBase, IAddon, IPreferences
    {
        #region variable

        FileFinderAddonImpl _addon;

        #endregion

        public FileFinder(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new FileFinderAddonImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        protected override IPreferences CreatePreferences() => new FileFinderPreferences(this);

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }


        #endregion
    }
}
