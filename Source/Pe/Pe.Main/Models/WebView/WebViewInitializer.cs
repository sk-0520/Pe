using System;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public interface IWebViewInitializer
    {
        #region property

        /// <summary>
        /// WebView が初期化済みか。
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region function

        /// <summary>
        /// WevView 初期化が完了するまで待機。
        /// </summary>
        /// <remarks>初期化完了済みでも呼び出し可能。</remarks>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WaitInitializeAsync(CancellationToken cancellationToken);

        #endregion
    }

    public class WebViewInitializer: DisposerBase, IWebViewInitializer
    {
        public WebViewInitializer(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private ManualResetEventSlim InitializeCompleted { get; } = new ManualResetEventSlim(false);

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        public async Task InitializeAsync(WebView2 webView, EnvironmentParameters environmentParameters, ICultureService cultureService)
        {
            ////NOTE: プラグイン開発等においてここで死ぬ場合はリビルドを。
            //var settings = new CefSharp.Wpf.CefSettings();

            //settings.Locale = cultureService.Culture.TwoLetterISOLanguageName;
            //settings.AcceptLanguageList = cultureService.Culture.Name;

            //settings.CachePath = environmentParameters.TemporaryWebViewCacheDirectory.FullName;

            //settings.UserAgent = ApplicationStringFormats.GetHttpUserAgentWebViewValue(environmentParameters.ApplicationConfiguration.Web.ViewUserAgentFormat);

            //settings.PersistSessionCookies = true;

            //settings.RegisterScheme(
            //    new CefSharp.CefCustomScheme() {
            //        SchemeName = ApplicationStorageSchemeHandlerFactory.SchemeName,
            //        DomainName = ApplicationStorageSchemeHandlerFactory.DomainName,
            //        SchemeHandlerFactory = new ApplicationStorageSchemeHandlerFactory(environmentParameters, LoggerFactory)
            //    }
            //);

            //CefSharp.Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            var userDataDirectoryPath = environmentParameters.MachineWebViewDirectory.FullName;
            var webViewEnvironment = await CoreWebView2Environment.CreateAsync(null, userDataDirectoryPath);

            await webView.EnsureCoreWebView2Async(webViewEnvironment);
            webView.CoreWebView2.Settings.AreDevToolsEnabled = environmentParameters.ApplicationConfiguration.Web.DeveloperTools;
            webView.CoreWebView2.Settings.UserAgent = ApplicationStringFormats.GetHttpUserAgentWebViewValue(environmentParameters.ApplicationConfiguration.Web.ViewUserAgentFormat, webView);
            InitializeCompleted.Set();
        }

        //public void AddVisualCppRuntimeRedist(EnvironmentParameters environmentParameters)
        //{
        //    Logger.LogInformation("Microsoft Visual C++ 再頒布可能パッケージの独自読み込み");

        //    var binDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    if(binDirPath == null) {
        //        throw new Exception($"{nameof(binDirPath)} is null");
        //    }
        //    var crtDir = Path.Combine(binDirPath, "lib", "Redist.MSVC.CRT", ProcessArchitecture.ApplicationArchitecture);
        //    var paths = Directory.GetFiles(crtDir, "*.dll");
        //    Logger.LogInformation("パッケージをディレクトリに移送開始: {0}", binDirPath);
        //    foreach(var path in paths) {
        //        var name = Path.GetFileName(path);
        //        var dstPath = Path.Combine(binDirPath, name);
        //        Logger.LogInformation("移送パッケージ: {0}", dstPath);
        //        File.Copy(path, dstPath);
        //    }
        //}

        #endregion

        #region IWebViewInitializer

        public bool IsInitialized { get; private set; }

        public Task WaitInitializeAsync(CancellationToken cancellationToken)
        {
            if(IsInitialized) {
                return Task.FromResult(IsInitialized);
            }

            return Task.Run(() => {
                try {
                    InitializeCompleted.Wait(cancellationToken);
                    IsInitialized = true;
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }, cancellationToken);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                InitializeCompleted.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
