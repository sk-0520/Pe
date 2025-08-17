using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEditorViewModel: SingleModelViewModelBase<LauncherItemCustomizeEditorElement>, ILauncherItemId, IFlushable
    {
        #region variable

        //List<LauncherItemCustomizeDetailViewModelBase>? _customizeItems;
        private bool _isChanged;

        #endregion

        public LauncherItemCustomizeEditorViewModel(LauncherItemCustomizeEditorElement model, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            IsCloned = false;

            ContextDispatcher = contextDispatcher;

            Common = new LauncherItemCustomizeCommonViewModel(Model, IconSelectRequest, ImageSelectRequest, ContextDispatcher, LoggerFactory);

            var items = new List<LauncherItemCustomizeDetailViewModelBase>();
            items.Add(Common);

            switch(Model.Kind) {
                case LauncherItemKind.File: {
                        var file = new LauncherItemCustomizeFileViewModel(Model, FileSelectRequest, ContextDispatcher, LoggerFactory);
                        var env = new LauncherItemCustomizeEnvironmentVariableViewModel(Model, ContextDispatcher, LoggerFactory);
                        var redo = new LauncherItemCustomizeRedoViewModel(Model, ContextDispatcher, LoggerFactory);

                        items.Add(file);
                        items.Add(env);
                        items.Add(redo);
                    }
                    break;

                case LauncherItemKind.StoreApp: {
                        var storeApp = new LauncherItemCustomizeStoreAppViewModel(Model, ContextDispatcher, LoggerFactory);
                        items.Add(storeApp);
                    }
                    break;

                case LauncherItemKind.Addon: {
                        var addon = new LauncherItemCustomizeAddonViewModel(Model, ContextDispatcher, LoggerFactory);
                        items.Add(addon);
                    }
                    break;

                case LauncherItemKind.Separator: {
                        var separator= new LauncherItemCustomizeSeparatorViewModel(Model, ContextDispatcher, LoggerFactory);
                        items.Add(separator);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(Model.Kind != LauncherItemKind.Separator) {
                Tag = new LauncherItemCustomizeTagViewModel(Model, ContextDispatcher, LoggerFactory);
                items.Add(Tag);
            }

            Comment = new LauncherItemCustomizeCommentViewModel(Model, ContextDispatcher, LoggerFactory);
            items.Add(Comment);

            CustomizeItems = items;

            foreach(var item in CustomizeItems) {
                if(!IsCloned) {
                    item.Initialize();
                }
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        internal LauncherItemCustomizeEditorViewModel(LauncherItemCustomizeEditorViewModel source)
            : base(source.Model, source.LoggerFactory)
        {
            IsCloned = true;

            ContextDispatcher = source.ContextDispatcher;
            Common = source.Common;
            Tag = source.Tag;
            Comment = source.Comment;
            CustomizeItems = source.CustomizeItems;
        }

        #region property

        protected IContextDispatcher ContextDispatcher { get; }

        public bool IsCloned { get; }

        public RequestSender IconSelectRequest { get; } = new RequestSender();
        public RequestSender ImageSelectRequest { get; } = new RequestSender();
        public RequestSender FileSelectRequest { get; } = new RequestSender();

        public List<LauncherItemCustomizeDetailViewModelBase> CustomizeItems { get; }

        public LauncherItemCustomizeCommonViewModel Common { get; }
        public LauncherItemCustomizeTagViewModel? Tag { get; }
        public LauncherItemCustomizeCommentViewModel Comment { get; }

        public bool IsChanged
        {
            get => this._isChanged;
            set => SetProperty(ref this._isChanged, value);
        }

        #endregion

        #region function


        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                foreach(var item in CustomizeItems) {
                    item.PropertyChanged -= Item_PropertyChanged;
                    if(disposing) {
                        item.Dispose();
                    }
                }
                IsChanged = false;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId => Model.LauncherItemId;

        #endregion

        #region IFlushable

        public void Flush()
        {
            foreach(var flushable in CustomizeItems.OfType<IFlushable>()) {
                flushable.Flush();
            }
        }

        #endregion

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsChanged = true;
        }
    }
}
