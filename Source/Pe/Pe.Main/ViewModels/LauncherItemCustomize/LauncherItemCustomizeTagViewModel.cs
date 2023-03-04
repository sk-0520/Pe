using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeTagViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        //TextDocument? _tagDocument;

        #endregion

        public LauncherItemCustomizeTagViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            TagLazyChanger = new LazyAction("タグ編集編集:" + Model.LauncherItemId.ToString(), TimeSpan.FromSeconds(3), LoggerFactory);
            TagDocument = new TextDocument(string.Join(Environment.NewLine, Model.TagItems));
            TagDocument.TextChanged += TagDocument_TextChanged;
        }

        #region property

        private LazyAction TagLazyChanger { get; }

        public TextDocument TagDocument { get; }

        #endregion

        #region command
        #endregion

        #region function

        public IReadOnlyCollection<string> GetTagItems()
        {
            return TextUtility.ReadLines(TagDocument!.Text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Trim())
                .ToList()
            ;
        }

        void ChangedTag()
        {
            var tagItems = TextUtility.ReadLines(DispatcherWrapper.Get(() => TagDocument.Text))
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Trim())
                .ToList()
            ;
            Model.TagItems.SetRange(tagItems);
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    TagLazyChanger.Dispose();
                }
                TagDocument.TextChanged -= TagDocument_TextChanged;
            }
            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            //var tags = Model.LoadTags();
            //TagDocument = new TextDocument(string.Join(Environment.NewLine, tags));
        }

        #endregion

        #region IFlushable
        public void Flush()
        {
            TagLazyChanger.SafeFlush();
        }

        #endregion

        private void TagDocument_TextChanged(object? sender, EventArgs e)
        {
            TagLazyChanger.DelayAction(ChangedTag);
        }
    }
}
