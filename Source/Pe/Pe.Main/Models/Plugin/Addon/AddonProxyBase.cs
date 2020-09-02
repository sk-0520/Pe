using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal abstract class CommonAddonProxyBase<TFunctionUnit>: DisposerBase
        where TFunctionUnit : notnull
    {
        protected CommonAddonProxyBase(PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PluginContextFactory = pluginContextFactory;

            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
        protected PluginContextFactory PluginContextFactory { get; }
        protected IHttpUserAgentFactory UserAgentFactory { get; }
        protected IPlatformTheme PlatformTheme { get; }
        protected IImageLoader ImageLoader { get; }
        protected IMediaConverter MediaConverter { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// 担当する処理単位。
        /// </summary>
        protected abstract AddonKind AddonKind { get; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="AddonParameter"/> を普通に作成する。
        /// </summary>
        /// <returns></returns>
        protected virtual AddonParameter CreateParameter(IPlugin plugin) => new AddonParameter(new SkeletonImplements(), plugin.PluginInformations, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);

        protected abstract TFunctionUnit BuildFunctionUnit(IAddon loadedAddon);

        #endregion

    }

    /// <summary>
    /// 特定に機能単位による単独アドオン機能のラッパー。
    /// </summary>
    /// <typeparam name="TFunctionUnit"></typeparam>
    internal abstract class AddonProxyBase<TFunctionUnit>: CommonAddonProxyBase<TFunctionUnit>
        where TFunctionUnit : class
    {
        #region variable

        TFunctionUnit? _functionUnit;

        #endregion

        protected AddonProxyBase(IAddon addon, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginContextFactory, userAgentFactory, platformTheme, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            Addon = addon;
        }

        #region property

        /// <summary>
        /// 対象アドオン。
        /// </summary>
        public IAddon Addon { get; }

        protected TFunctionUnit FunctionUnit
        {
            get
            {
                if(this._functionUnit == null) {
                    Debug.Assert(Addon.IsSupported(AddonKind));

                    if(!Addon.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                        using(var reader = PluginContextFactory.BarrierRead()) {
                            using var loadContext = PluginContextFactory.CreateLoadContex(Addon.PluginInformations, reader);
                            Addon.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                        }
                    }

                    this._functionUnit = BuildFunctionUnit(Addon);
                }

                return this._functionUnit;
            }
        }

        #endregion

        #region function
        #endregion
    }

    /// <summary>
    /// 特定に機能単位による複数アドオン機能のラッパー。
    /// </summary>
    /// <typeparam name="TFunctionUnit"></typeparam>
    internal abstract class AddonsProxyBase<TFunctionUnit>: CommonAddonProxyBase<TFunctionUnit>
        where TFunctionUnit : notnull
    {
        #region variable

        IReadOnlyList<TFunctionUnit>? _functionUnits;
        IReadOnlyDictionary<TFunctionUnit, IAddon>? _functionAddonMap;

        #endregion

        protected AddonsProxyBase(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginContextFactory, userAgentFactory, platformTheme, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            Addons = addons;
        }

        #region property


        /// <summary>
        /// 対象アドオン一覧。
        /// </summary>
        protected IReadOnlyList<IAddon> Addons { get; }

        /// <summary>
        /// 処理単位一覧。
        /// <para>初回参照時に読み込まれる。</para>
        /// </summary>
        protected IReadOnlyList<TFunctionUnit> FunctionUnits
        {
            get
            {
                if(this._functionUnits == null) {
                    var result = LoadFunctionUnits();
                    this._functionUnits = result.units;
                    this._functionAddonMap = result.map;
                }

                return this._functionUnits;
            }
        }

        #endregion

        #region function

        protected (IReadOnlyList<TFunctionUnit> units, IReadOnlyDictionary<TFunctionUnit, IAddon> map) LoadFunctionUnits()
        {
            var map = new Dictionary<TFunctionUnit, IAddon>();
            var list = new List<TFunctionUnit>(Addons.Count);
            foreach(var addon in Addons) {
                Debug.Assert(addon.IsSupported(AddonKind));

                if(!addon.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                    using(var reader = PluginContextFactory.BarrierRead()) {
                        using var loadContext = PluginContextFactory.CreateLoadContex(addon.PluginInformations, reader);
                        addon.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                    }
                }
                var functionUnit = BuildFunctionUnit(addon);
                list.Add(functionUnit);
                map.Add(functionUnit, addon);
            }
            return (list, map);
        }

        public IAddon GetAddon(TFunctionUnit functionUnit)
        {
            if(this._functionAddonMap == null) {
                throw new InvalidOperationException(nameof(FunctionUnits));
            }

            return this._functionAddonMap[functionUnit];
        }

        #endregion
    }
}