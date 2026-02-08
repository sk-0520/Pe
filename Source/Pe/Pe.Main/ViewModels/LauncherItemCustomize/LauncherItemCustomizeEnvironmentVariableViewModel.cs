using System;
using System.Collections.ObjectModel;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEnvironmentVariableViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        //TextDocument? _mergeTextDocument;
        //TextDocument? _removeTextDocument;

        #endregion

        public LauncherItemCustomizeEnvironmentVariableViewModel(LauncherItemCustomizeEditorElement model, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, contextDispatcher, loggerFactory)
        {
            EnvironmentVariableDelayChanger = new DelayAction("環境変数編集:" + Model.LauncherItemId.ToString(), TimeSpan.FromSeconds(5), LoggerFactory);
        }

        #region property
        private DelayAction EnvironmentVariableDelayChanger { get; }
        public TextDocument? MergeTextDocument { get; private set; }
        public TextDocument? RemoveTextDocument { get; private set; }

        #endregion

        #region command

        public ObservableCollection<string> MergeErrors { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> RemoveErrors { get; } = new ObservableCollection<string>();

        #endregion

        #region function

        void ChangedEnvironmentVariable()
        {
            var envEditor = new EnvironmentVariableEditor(LoggerFactory);

            var (envMergeText, envRemoveText) = ContextDispatcher.Get(() => (MergeTextDocument!.Text, RemoveTextDocument!.Text));

            var envMergeItems = envEditor.ParseMergeItems(envMergeText);
            var envRemoveItems = envEditor.ParseRemoveItems(envRemoveText);
            var envVarItems = envEditor.Join(envMergeItems, envRemoveItems);

            Model.EnvironmentVariableItems!.SetRange(envVarItems);
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    EnvironmentVariableDelayChanger.Dispose();
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
            var envEditor = new EnvironmentVariableEditor(LoggerFactory);
            MergeTextDocument = new TextDocument(envEditor.ConvertMergeText(envItems));
            RemoveTextDocument = new TextDocument(envEditor.ConvertRemoveText(envItems));

            MergeTextDocument.TextChanged += TextDocument_TextChanged;
            RemoveTextDocument.TextChanged += TextDocument_TextChanged;
        }

        protected override void ValidateDomain()
        {
            var envEditor = new EnvironmentVariableEditor(LoggerFactory);

            envEditor.SetValidateCommon(MergeTextDocument!, envEditor.ValidateMergeDocument, seq => AddErrors(seq, nameof(MergeTextDocument)), MergeErrors);
            envEditor.SetValidateCommon(RemoveTextDocument!, envEditor.ValidateRemoveDocument, seq => AddErrors(seq, nameof(RemoveTextDocument)), RemoveErrors);
        }

        #endregion

        #region IFlushable

        public void Flush()
        {
            EnvironmentVariableDelayChanger.SafeFlush();
        }

        #endregion

        private void TextDocument_TextChanged(object? sender, EventArgs e)
        {
            if(Model.IsLazyLoad) {
                return;
            }

            EnvironmentVariableDelayChanger.Callback(ChangedEnvironmentVariable);
        }
    }
}
