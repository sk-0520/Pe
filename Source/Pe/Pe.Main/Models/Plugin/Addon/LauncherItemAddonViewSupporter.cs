using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemExtension;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonViewSupporterCollection
    {
        public LauncherItemAddonViewSupporterCollection(IOrderManager orderManager, IWindowManager windowManager, IUserTracker userTracker, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            OrderManager = orderManager;
            WindowManager = windowManager;
            UserTracker = userTracker;
            ContextDispatcher = contextDispatcher;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="IOrderManager"/>
        private IOrderManager OrderManager { get; }
        /// <inheritdoc cref="IWindowManager"/>
        private IWindowManager WindowManager { get; }
        /// <inheritdoc cref="IUserTracker"/>
        private IUserTracker UserTracker { get; }
        /// <inheritdoc cref="IContextDispatcher"/>
        private IContextDispatcher ContextDispatcher { get; }

        private ISet<LauncherItemAddonViewSupporter> LauncherItemAddonViewSupporters { get; } = new HashSet<LauncherItemAddonViewSupporter>();

        #endregion

        #region function

        public bool ExistsInformation(LauncherItemId launcherItemId)
        {
            var target = LauncherItemAddonViewSupporters.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
            if(target == null) {
                return false;
            }

            if(target.Element == null) {
                return false;
            }

            return target.Element.GetInformationItems().Any();
        }

        /// <summary>
        /// 対象のランチャーアイテムアドオンを活性化。
        /// </summary>
        /// <param name="launcherItemId"></param>
        public void Foreground(LauncherItemId launcherItemId)
        {
            var target = LauncherItemAddonViewSupporters.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
            if(target is null || target.Element is null) {
                Logger.LogDebug("[{LauncherItemId}] 対象が存在しない", launcherItemId);
                return;
            }

            var windowItems = WindowManager.GetWindowItems(WindowKind.LauncherItemExtension);
            var windowItem = windowItems.FirstOrDefault(a => a.Element == target.Element);
            if(windowItem is null) {
                Logger.LogDebug("[{LauncherItemId}] ウィンドウアイテム未登録", launcherItemId);
                return;
            }
            ContextDispatcher.BeginAsync(() => {
                Logger.LogDebug("[{LauncherItemId}] 最前面化", launcherItemId);
                var hWnd = HandleUtility.GetWindowHandle(windowItem.Window);
                WindowsUtility.ShowActiveForeground(hWnd);
            });
        }

        public ILauncherItemAddonViewSupporter Create(IPluginInformation pluginInformation, LauncherItemId launcherItemId)
        {
            var createdViewSupporter = LauncherItemAddonViewSupporters.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
            if(createdViewSupporter != null) {
                return createdViewSupporter;
            }

            if(ExistsInformation(launcherItemId)) {
                throw new InvalidOperationException($"{nameof(launcherItemId)}: {launcherItemId}");
            }

            var result = new LauncherItemAddonViewSupporter(pluginInformation, launcherItemId, OrderManager, WindowManager, UserTracker, ContextDispatcher, LoggerFactory);
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

        private static bool DefaultUserClosing() => false;
        private static void DefaultClosedWindow() { }

        #endregion
    }

    public class LauncherItemAddonViewSupporter: ILauncherItemAddonViewSupporter, ILauncherItemId
    {
        public LauncherItemAddonViewSupporter(IPluginInformation pluginInformation, LauncherItemId launcherItemId, IOrderManager orderManager, IWindowManager windowManager, IUserTracker userTracker, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginInformation = pluginInformation;
            LauncherItemId = launcherItemId;
            OrderManager = orderManager;
            WindowManager = windowManager;
            UserTracker = userTracker;
            ContextDispatcher = contextDispatcher;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="IPluginInformation"/>
        private IPluginInformation PluginInformation { get; }
        /// <inheritdoc cref="IWindowManager"/>
        private IWindowManager WindowManager { get; }
        /// <inheritdoc cref="IOrderManager"/>
        private IOrderManager OrderManager { get; }
        /// <inheritdoc cref="IUserTracker"/>
        private IUserTracker UserTracker { get; }
        /// <inheritdoc cref="IContextDispatcher"/>
        private IContextDispatcher ContextDispatcher { get; }

        public LauncherItemExtensionElement? Element { get; private set; }

        #endregion

        #region function

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion

        #region ILauncherItemAddonViewSupporter

        /// <inheritdoc cref="ILauncherItemAddonViewSupporter.RegisterWindowAsync(Window, Func{bool}?, Action?, CancellationToken)"/>
        public async Task<bool> RegisterWindowAsync(Window window, Func<bool>? userClosing, Action? closedWindow, CancellationToken cancellationToken)
        {
            if(window == null) {
                throw new ArgumentNullException(nameof(window));
            }
            if(window.IsVisible) {
                Logger.LogError("ランチャーアイテムアドオンの初期表示は Pe 側で処理される必要がある");
                window.Close();
                return false;
            }

            if(Element == null) {
                Element = await OrderManager.CreateLauncherItemExtensionElementAsync(PluginInformation, LauncherItemId, cancellationToken);
            }
            //NOTE: 引数がどんどこ増えるようなら IOrderManager に移す
            var windowItem = new WindowItem(Manager.WindowKind.LauncherItemExtension, Element, new LauncherItemExtensionViewModel(Element, UserTracker, ContextDispatcher, LoggerFactory), window);
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
