using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteFileViewModel: ElementViewModelBase<NoteFileElement>
    {
        public NoteFileViewModel(NoteFileElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            IconViewer = new IconViewerViewModel(Model.IconImageLoader, DispatcherWrapper, LoggerFactory);
        }

        #region property

        public NoteFileId NoteFileId => Model.NoteFileId;

        public string FilePath => Model.NoteFilePath;

        public string FileName
        {
            get
            {
                var dirName = Path.GetDirectoryName(FilePath);
                var fileName = Path.GetFileName(FilePath);
                return FilePath;
            }
        }

        public IconViewerViewModel IconViewer { get; set; }

        #endregion

        #region command

        public ICommand OpenFileCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenFile();
            }
        ));

        #endregion
    }
}
