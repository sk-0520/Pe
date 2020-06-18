using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.CodeGenerator.Addon
{
    internal class CodeGeneratorWidget: IWidget
    {
        #region define

        public class Extensions
        {
            #region property

            #endregion

            #region function

            public int Func(int a, int b)
            {
                return a + b;
            }

            #endregion
        }

        #endregion
        public CodeGeneratorWidget(IAddonParameter parameter, IPluginInformations pluginInformations)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformations = pluginInformations;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IAddonExecutor AddonExecutor { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ISkeletonImplements SkeletonImplements { get; }
        IPluginInformations PluginInformations { get; }

        IWebViewGrass? WebViewGrass { get; set; }

        #endregion

        #region function

        void OnInitialized(IWebViewGrass webViewGrass)
        {
            WebViewGrass = webViewGrass;
            WebViewGrass.ExecuteScriptAsync("test", new object[0]);
        }

        #endregion

        #region IWidget

        public WidgetViewType ViewType => WidgetViewType.WebView;

        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return null;
        }

        public string GetMenuHeader(IPluginContext pluginContext)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name!;
        }

        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            throw new NotSupportedException();
        }

        public IWebViewSeed CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            var webViewSeed = new WebViewSeed(new HtmlAddress(new Uri("pe://plugin"))) {
                Background = Brushes.Transparent,
                WindowStyle = WindowStyle.None,
                Extensions = new Extensions(),
                PublicDirectoryName = "www",
                SoilCallback = OnInitialized,
            };

            return webViewSeed;
        }

        /// <inheritdoc cref="IWidget.OpeningWidget(IPluginContext)"/>
        public void OpeningWidget(IPluginContext pluginContext)
        {
        }

        /// <inheritdoc cref="IWidget.OpenedWidget(IPluginContext)"/>
        public void OpenedWidget(IPluginContext pluginContext)
        {
        }

        /// <inheritdoc cref="IWidget.ClosedWidget(IWidgetAddonClosedContext)"/>
        public void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext)
        {
        }

        #endregion
    }
}
