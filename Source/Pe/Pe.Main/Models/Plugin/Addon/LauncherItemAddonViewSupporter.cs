using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemExtension;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonViewSupporterCollection
    {
        public LauncherItemAddonViewSupporterCollection(IOrderManager orderManager, IWindowManager windowManager, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            OrderManager = orderManager;
            WindowManager = windowManager;
            UserTracker = userTracker;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="IOrderManager"/>
        IOrderManager OrderManager { get; }
        /// <inheritdoc cref="IWindowManager"/>
        IWindowManager WindowManager { get; }
        /// <inheritdoc cref="IUserTracker"/>
        IUserTracker UserTracker { get; }
        /// <inheritdoc cref="IDispatcherWrapper"/>
        IDispatcherWrapper DispatcherWrapper { get; }

        ISet<LauncherItemAddonViewSupporter> LauncherItemAddonViewSupporters { get; } = new HashSet<LauncherItemAddonViewSupporter>();

        #endregion

        #region function

        public bool ExistsInformations(Guid launcherItemId)
        {
            var target = LauncherItemAddonViewSupporters.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
            if(target == null) {
                return false;
            }

            if(target.Element == null) {
                return false;
            }

            return target.Element.GetInformations().Any();
        }

        /// <summary>
        /// ???????????????????????????????????????????????????????????????
        /// </summary>
        /// <param name="launcherItemId"></param>
        public void Foreground(Guid launcherItemId)
        {
            // TODO: ???????????????????????????
        }

        public ILauncherItemAddonViewSupporter Create(IPluginInformations pluginInformations, Guid launcherItemId)
        {
            var createdViewSupporter = LauncherItemAddonViewSupporters.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
            if(createdViewSupporter != null) {
                return createdViewSupporter;
            }

            if(ExistsInformations(launcherItemId)) {
                throw new InvalidOperationException($"{nameof(launcherItemId)}: {launcherItemId}");
            }

            var result = new LauncherItemAddonViewSupporter(pluginInformations, launcherItemId, OrderManager, WindowManager, UserTracker, DispatcherWrapper, LoggerFactory);
            LauncherItemAddonViewSupporters.Add(result);
            return result;
        }


        #endregion
    }

    public class LauncherItemAddonViewInformation
    {
        public LauncherItemAddonViewInformation(WindowItem windowItem, Func<bool>? userClosing, Action? closedWindow)
        {
            WindowItem = windowItem;
            UserClosing = userClosing ?? DefaultUserClosing;
            ClosedWindow = closedWindow ?? DefaultClosedWindow;
        }

        #region property

        public WindowItem WindowItem { get; }
        public Func<bool> UserClosing { get; }
        public Action ClosedWindow { get; }

        #endregion

        #region function

        static bool DefaultUserClosing() => false;
        static void DefaultClosedWindow() { }

        #endregion
    }

    internal class LauncherItemAddonViewSupporter: ILauncherItemAddonViewSupporter, ILauncherItemId
    {
        public LauncherItemAddonViewSupporter(IPluginInformations pluginInformations, Guid launcherItemId, IOrderManager orderManager, IWindowManager windowManager, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginInformations = pluginInformations;
            LauncherItemId = launcherItemId;
            OrderManager = orderManager;
            WindowManager = windowManager;
            UserTracker = userTracker;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="IPluginInformations"/>
        IPluginInformations PluginInformations { get; }
        /// <inheritdoc cref="IWindowManager"/>
        IWindowManager WindowManager { get; }
        /// <inheritdoc cref="IOrderManager"/>
        IOrderManager OrderManager { get; }
        /// <inheritdoc cref="IUserTracker"/>
        IUserTracker UserTracker { get; }
        /// <inheritdoc cref="IDispatcherWrapper"/>
        IDispatcherWrapper DispatcherWrapper { get; }

        public LauncherItemExtensionElement? Element { get; private set; }

        #endregion

        #region function

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ILauncherItemAddonViewSupporter

        /// <inheritdoc cref="ILauncherItemAddonViewSupporter.RegisterWindow(Window, Func{bool}?, Action?)"/>
        public bool RegisterWindow(Window window, Func<bool>? userClosing, Action? closedWindow)
        {
            if(window == null) {
                throw new ArgumentNullException(nameof(window));
            }
            if(window.IsVisible) {
                Logger.LogError("????????????????????????????????????????????????????????? Pe ????????????????????????????????????");
                window.Close();
                return false;
            }

            if(Element == null) {
                Element = OrderManager.CreateLauncherItemExtensionElement(PluginInformations, LauncherItemId);
            }
            //NOTE: ?????????????????????????????????????????? IOrderManager ?????????
            var windowItem = new WindowItem(WindowKind.LauncherItemExtension, Element, new LauncherItemExtensionViewModel(Element, UserTracker, DispatcherWrapper, LoggerFactory), window);
            if(WindowManager.Register(windowItem)) {
                var info = new LauncherItemAddonViewInformation(windowItem, userClosing, closedWindow);
                Element.Add(info);
                window.Show();
                return true;
            }

            return false;
        }

        #endregion
    }
}
