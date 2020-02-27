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
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        public void Initialize(EnvironmentParameters environmentParameters)
        {
            var settings = new CefSharp.Wpf.CefSettings();

            settings.Locale = CultureService.Current.Culture.Parent.ToString();
            settings.AcceptLanguageList = CultureService.Current.Culture.Name;

            settings.CachePath = environmentParameters.TemporaryWebViewCacheDirectory.FullName;
            settings.UserDataPath = environmentParameters.MachineWebViewUserDirectory.FullName;

            settings.UserAgent = environmentParameters.Configuration.Web.ViewUserAgent;

            settings.PersistSessionCookies = true;

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