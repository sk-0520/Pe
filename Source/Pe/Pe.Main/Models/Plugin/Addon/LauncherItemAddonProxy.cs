using System;
using System.ComponentModel;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonProxy: AddonProxyBase<ILauncherItemExtension>, ILauncherItemExtension, ILauncherItemId
    {
        public LauncherItemAddonProxy(LauncherItemId launcherItemId, IAddon addon, PluginContextFactory pluginContextFactory, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IHashAlgorithmGenerator hashAlgorithmGenerator, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(addon, pluginContextFactory, userAgentFactory, viewManager, hashAlgorithmGenerator, platformTheme, imageLoader, mediaConverter, policy, contextDispatcher, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
        }

        #region property

        private LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }

        #endregion

        #region function

        //void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        #endregion

        #region AddonProxyBase

        protected override AddonKind AddonKind => AddonKind.LauncherItem;

        protected override AddonParameter CreateParameter(IPlugin plugin)
        {
            var worker = LauncherItemAddonContextFactory.CreateWorker(plugin.PluginInformation, LauncherItemId);
            return new LauncherItemExtensionCreateParameter(LauncherItemId, worker, new SkeletonImplements(), plugin.PluginInformation, UserAgentFactory, ViewManager, HashAlgorithmGenerator, PlatformTheme, ImageLoader, MediaConverter, Policy, ContextDispatcher, LoggerFactory);
        }

        protected override ILauncherItemExtension BuildFunctionUnit(IAddon loadedAddon)
        {
            var parameter = (ILauncherItemExtensionCreateParameter)CreateParameter(loadedAddon);
            return loadedAddon.CreateLauncherItemExtension(parameter);
        }

        #endregion

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => FunctionUnit.PropertyChanged += value;
            remove => FunctionUnit.PropertyChanged -= value;
        }

        public bool CustomDisplayText => FunctionUnit.CustomDisplayText;

        public string DisplayText => FunctionUnit.DisplayText;

        public bool CustomLauncherIcon => FunctionUnit.CustomLauncherIcon;

        public bool SupportedPreferences => FunctionUnit.SupportedPreferences;

        public void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible, object callerObject)
        {
            FunctionUnit.ChangeDisplay(iconMode, isVisible, callerObject);
        }

        public ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext)
        {
            return FunctionUnit.CreatePreferences(launcherItemAddonContext);
        }

        public void Execute(string? argument, ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            FunctionUnit.Execute(argument, commandExecuteParameter, launcherItemExtensionExecuteParameter, launcherItemAddonContext);
        }

        public object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale)
        {
            return FunctionUnit.GetIcon(iconMode, iconScale);
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion

        //private void FunctionUnit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    OnPropertyChanged(e.PropertyName);
        //}
    }
}
