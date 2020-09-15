using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public class WebViewinItializer
    {
        public WebViewinItializer(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        public void Initialize(EnvironmentParameters environmentParameters, CultureService cultureService)
        {
            var settings = new CefSharp.Wpf.CefSettings();

            settings.Locale = cultureService.Culture.TwoLetterISOLanguageName;
            settings.AcceptLanguageList = cultureService.Culture.Name;

            settings.CachePath = environmentParameters.TemporaryWebViewCacheDirectory.FullName;
            settings.UserDataPath = environmentParameters.MachineWebViewUserDirectory.FullName;

            settings.UserAgent = environmentParameters.ApplicationConfiguration.Web.ViewUserAgent;

            settings.PersistSessionCookies = true;

            settings.RegisterScheme(
                new CefSharp.CefCustomScheme() {
                    SchemeName = ApplicationStorageSchemeHandlerFactory.SchemeName,
                    DomainName = ApplicationStorageSchemeHandlerFactory.DomainName,
                    SchemeHandlerFactory = new ApplicationStorageSchemeHandlerFactory(environmentParameters, LoggerFactory)
                }
            );

            CefSharp.Cef.Initialize(settings);
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
    }
}
