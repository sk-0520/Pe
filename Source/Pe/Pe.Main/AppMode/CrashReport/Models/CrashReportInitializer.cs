using ContentTypeTextNet.Pe.Main.AppMode.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.AppMode.CrashReport.Models.Element;
using ContentTypeTextNet.Pe.Main.AppMode.CrashReport.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;

namespace ContentTypeTextNet.Pe.Main.AppMode.CrashReport.Models
{
    internal class CrashReportInitializer
    {
        public CrashReportInitializer(CrashReportOptions options)
        {
            Options = options;

            var cultureService = new CultureService(EnumResourceManagerFactory.Create());
            if(string.IsNullOrWhiteSpace(Options.Language)) {
                cultureService.ChangeAutoCulture();
            } else {
                cultureService.ChangeCulture(Options.Language);
            }
            CultureService.Initialize(cultureService);
        }

        #region property

        private CrashReportOptions Options { get; }

        #endregion

        #region function
        #endregion
    }
}
