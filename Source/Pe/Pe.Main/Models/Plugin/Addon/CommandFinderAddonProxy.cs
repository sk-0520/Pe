using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal sealed class CommandFinderAddonProxy: AddonsProxyBase<ICommandFinder>, ICommandFinder
    {
        public CommandFinderAddonProxy(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, pluginContextFactory, userAgentFactory, platformTheme, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        #endregion

        #region AddonWrapperBase

        protected override AddonKind AddonKind => AddonKind.CommandFinder;

        protected override ICommandFinder BuildFunctionUnit(IAddon loadedAddon)
        {
            return loadedAddon.BuildCommandFinder(CreateParameter(loadedAddon));
        }


        #endregion


        #region ICommandFinder


        public bool IsInitialized { get; private set; }


        public void Initialize()
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            foreach(var addonFunctions in FunctionUnits) {
                if(addonFunctions.IsInitialized) {
                    continue;
                }
                addonFunctions.Initialize();
            }

            IsInitialized = true;
        }

        public IEnumerable<ICommandItem> EnumerateCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            foreach(var addonFunctions in FunctionUnits) {
                Debug.Assert(addonFunctions.IsInitialized);
                var results = addonFunctions.EnumerateCommandItems(inputValue, inputRegex, hitValuesCreator, cancellationToken);
                foreach(var result in results) {
                    yield return result;
                }
            }
        }

        public void Refresh(IPluginContext pluginContext)
        {
            Debug.Assert(pluginContext.GetType() == typeof(NullPluginContext));

            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            foreach(var functionUnit in FunctionUnits) {
                var addon = GetAddon(functionUnit);
                using(var reader = PluginContextFactory.BarrierRead()) {
                    var context = PluginContextFactory.CreateContext(addon.PluginInformations, reader, true);
                    functionUnit.Refresh(context);
                }
            }
        }

        #endregion
    }
}
