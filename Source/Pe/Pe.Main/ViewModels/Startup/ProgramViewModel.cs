using System.IO;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
{
    public class ProgramViewModel: ElementViewModelBase<ProgramElement>
    {
        public ProgramViewModel(ProgramElement model, IUserTracker userTracker, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, userTracker, contextDispatcher, loggerFactory)
        {
            IconViewer = new IconViewerViewModel(Model.IconImageLoader, BadgeData.CreateEmpty(), ContextDispatcher, LoggerFactory) {
                UseCache = true,
            };
        }

        #region property

        public string? FileName => Path.GetFileNameWithoutExtension(Model.FileInfo.Name);
        public bool IsImport
        {
            get => Model.IsImport;
            set => SetModelValue(value);
        }
        public IconViewerViewModel IconViewer { get; }

        #endregion

        #region ElementViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    IconViewer.Dispose();
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}
