using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ウィジェットビュー種別。
    /// </summary>
    public enum WidgetViewType
    {
        /// <summary>
        /// 通常の<see cref="System.Windows.Window"/>をウィジェット側で生成。
        /// </summary>
        Window,
        /// <summary>
        /// HTML的な。
        /// <para>考え中だけど、Pe側で透明ウィンドウとCEFぶっこんでウィジェット側からそこにHTMLを流し込むイメージ。</para>
        /// <para>IMEはあきらめ。</para>
        /// </summary>
        WebView,
    }

    /// <summary>
    /// ウィジェット！
    /// <para>1アドオンにつき1ウィンドウのみ生成可能な限定的UIとして扱う。ユーザー通常操作の妨げにならず、簡単な機能を提供するUIを目標とする。</para>
    /// <para>プライマリスクリーンの左上を原点とし、表示制御は Pe 側で処理される。各種パラメータの一部は Pe 側で制御され、ウィンドウスタイルは WS_EX_TOOLWINDOW が強制され最大化最小化も不可となる。</para>
    /// </summary>
    public interface IWidget
    {
        #region property

        /// <summary>
        /// このウィジェットのビュー種別。
        /// </summary>
        WidgetViewType ViewType { get; }

        #endregion

        #region function

        /// <summary>
        /// 通知領域ウィジェットメニューアイコン。
        /// </summary>
        /// <returns></returns>
        DependencyObject? GetMenuIcon(IPluginContext pluginContext);
        /// <summary>
        /// 通知領域ウィジェットメニューヘッダ。
        /// </summary>
        /// <returns></returns>
        string GetMenuHeader(IPluginContext pluginContext);

        /// <summary>
        /// ウィンドウウィジェットを生成。
        /// <para>ウィンドウの表示・非表示制御は Pe 側にて処理するため <see cref="Window.Show"/>/<see cref="UIElement.Visibility"/> はウィジェット側で操作しないこと。</para>
        /// </summary>
        /// <returns></returns>
        Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext);

        /// <summary>
        /// WebViewウィジェットを生成。
        /// </summary>
        /// <returns>WebViewウィジェット生成方法。</returns>
        IWebViewSeed CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext);

        /// <summary>
        /// ウィジェットが開かれる際に呼ばれる。
        /// </summary>
        void OpeningWidget(IPluginContext pluginContext);

        /// <summary>
        /// ウィジェットが開かれた際に呼ばれる。
        /// </summary>
        void OpenedWidget(IPluginContext pluginContext);

        /// <summary>
        /// ウィジェットが閉じられた際に呼ばれる。
        /// </summary>
        void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext);

        #endregion
    }

    public interface IWebViewSeed
    {
        #region property

        IHtmlSource HtmlSource { get; }

        /// <summary>
        /// 生成ウィジェットのウィンドウスタイル。
        /// <para><see cref="WindowStyle.None"/>の場合に<see cref="Window.AllowsTransparency"/>が有効になる。</para>
        /// </summary>
        WindowStyle WindowStyle { get; }
        /// <summary>
        /// 生成ウィジェットのリサイズモード。
        /// </summary>
        ResizeMode ResizeMode { get; }
        /// <summary>
        /// 生成される初期サイズ。
        /// </summary>
        Size ViewSize { get; }
        /// <summary>
        /// 生成ウィジェットの背景。
        /// </summary>
        Brush Background { get; }

        /// <summary>
        /// 生成後に各種パラメータを受けとるコールバック。
        /// </summary>
        Action<IWebViewGrass>? SoilCallback { get; }

        #endregion
    }

    /// <inheritdoc cref="IWebViewSeed"/>
    public class WebViewSeed: IWebViewSeed
    {
        public WebViewSeed(IHtmlAddress htmlAddress)
        {
            HtmlSource = htmlAddress;
        }

        public WebViewSeed(IHtmlSourceCode htmlSourceCode)
        {
            HtmlSource = htmlSourceCode;
        }

        #region IWebViewSeed

        /// <inheritdoc cref="IWebViewSeed.HtmlSource"/>
        public IHtmlSource HtmlSource { get; }

        /// <inheritdoc cref="IWebViewSeed.WindowStyle"/>
        public WindowStyle WindowStyle { get; set; } = WindowStyle.SingleBorderWindow;
        /// <inheritdoc cref="IWebViewSeed.ResizeMode"/>
        public ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        /// <inheritdoc cref="IWebViewSeed.ViewSize"/>
        public Size ViewSize { get; set; } = new Size(400, 400);
        /// <inheritdoc cref="IWebViewSeed.Background"/>
        public Brush Background { get; set; } = SystemColors.WindowBrush;

        /// <inheritdoc cref="IWebViewSeed.SoilCallback"/>
        public Action<IWebViewGrass>? SoilCallback { get; set; }

        #endregion
    }

    /// <summary>
    /// WevView生成後に返される。
    /// </summary>
    public interface IWebViewGrass
    {
        #region property

        object WebView { get; }

        #endregion
    }
}
