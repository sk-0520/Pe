using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
{
    public class ImportProgramsViewModel : SingleModelViewModelBase<ImportProgramsElement>
    {
        public ImportProgramsViewModel(ImportProgramsElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ProgramCollection = new ActionModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel>(Model.ProgramItems, LoggerFactory) {
                ToViewModel = m => new ProgramViewModel(m, LoggerFactory),
            };
            CloseRequest = new RequestSender(dispatcherWapper);
        }

        #region property

        public RequestSender CloseRequest { get; }

        ActionModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel> ProgramCollection { get; }
        public ObservableCollection<ProgramViewModel> ProgramItems => ProgramCollection.ViewModels;



        #endregion

        #region command

        public ICommand ViewLoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.LoadProgramsAsync().ConfigureAwait(false);
            }
        ));

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Send();
            }
        ));

        public ICommand ImportCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ImportAsync().ConfigureAwait(false);
            }
        ));

        #endregion

    }
}
