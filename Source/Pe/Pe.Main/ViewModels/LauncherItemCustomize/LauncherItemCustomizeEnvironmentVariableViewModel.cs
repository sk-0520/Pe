using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using System.Collections.ObjectModel;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEnvironmentVariableViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        //TextDocument? _mergeTextDocument;
        //TextDocument? _removeTextDocument;

        #endregion

        public LauncherItemCustomizeEnvironmentVariableViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            EnvironmentVariableLazyChanger = new LazyAction("環境変数編集:" + Model.LauncherItemId.ToString(), TimeSpan.FromSeconds(5), LoggerFactory);
        }

        #region property
        LazyAction EnvironmentVariableLazyChanger { get; }
        public TextDocument? MergeTextDocument { get; private set; }
        public TextDocument? RemoveTextDocument { get; private set; }

        #endregion

        #region command

        public ObservableCollection<string> MergeErros { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> RemoveErros { get; } = new ObservableCollection<string>();

        #endregion

        #region function

        [Obsolete]
        public IReadOnlyCollection<LauncherEnvironmentVariableData> GetEnvironmentVariableItems()
        {
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            var envMergeItems = envConf.GetMergeItems(MergeTextDocument!);
            var envRemoveItems = envConf.GetRemoveItems(RemoveTextDocument!);
            var envVarItems = envConf.Join(envMergeItems, envRemoveItems);

            return envVarItems;
        }

        void ChangedEnvironmentVariable()
        {
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            var envMergeItems = DispatcherWrapper.Get(() => envConf.GetMergeItems(MergeTextDocument!));
            var envRemoveItems = DispatcherWrapper.Get(() => envConf.GetRemoveItems(RemoveTextDocument!));
            var envVarItems = envConf.Join(envMergeItems, envRemoveItems);

            Model.EnvironmentVariableItems!.SetRange(envVarItems);
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    EnvironmentVariableLazyChanger.Dispose();
                }

                if(MergeTextDocument != null) {
                    MergeTextDocument.TextChanged -= TextDocument_TextChanged;
                }
                if(RemoveTextDocument != null) {
                    RemoveTextDocument.TextChanged -= TextDocument_TextChanged;
                }
            }

            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            if(Model.IsLazyLoad) {
                return;
            }

            var envItems = Model.EnvironmentVariableItems!;
            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            MergeTextDocument = envConf.CreateMergeDocument(envItems);
            RemoveTextDocument = envConf.CreateRemoveDocument(envItems);

            MergeTextDocument.TextChanged += TextDocument_TextChanged;
            RemoveTextDocument.TextChanged += TextDocument_TextChanged;
        }

        protected override void ValidateDomain()
        {

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            envConf.SetValidateCommon(MergeTextDocument!, envConf.ValidateMergeDocument, seq => AddErrors(seq, nameof(MergeTextDocument)), MergeErros);
            envConf.SetValidateCommon(RemoveTextDocument!, envConf.ValidateRemoveDocument, seq => AddErrors(seq, nameof(RemoveTextDocument)), RemoveErros);
        }

        #endregion

        #region IFlushable

        public void Flush()
        {
            EnvironmentVariableLazyChanger.SafeFlush();
        }

        #endregion

        private void TextDocument_TextChanged(object? sender, EventArgs e)
        {
            if(Model.IsLazyLoad) {
                return;
            }

            EnvironmentVariableLazyChanger.DelayAction(ChangedEnvironmentVariable);
        }


    }
}
