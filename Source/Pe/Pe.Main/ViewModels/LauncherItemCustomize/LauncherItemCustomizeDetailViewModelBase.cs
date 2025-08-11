using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public abstract class LauncherItemCustomizeDetailViewModelBase: SingleModelViewModelBase<LauncherItemCustomizeEditorElement>, ILauncherItemId
    {
        protected LauncherItemCustomizeDetailViewModelBase(LauncherItemCustomizeEditorElement model, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(null, nameof(model) + "." + nameof(Model.IsInitialized));
            }
            ContextDispatcher = contextDispatcher;

            Kind = Model.Kind;
        }

        #region property

        protected IContextDispatcher ContextDispatcher { get; }

        public LauncherItemKind Kind { get; }

        public bool IsInitialize { get; private set; }

        #endregion

        #region command
        #endregion

        #region function

        protected abstract void InitializeImpl();

        public void Initialize()
        {
            if(IsInitialize) {
                return;
            }
            InitializeImpl();
            if(!Model.IsLazyLoad) {
                IsInitialize = true;
            }
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Model.PropertyChanged -= Model_PropertyChanged;
            }
            base.Dispose(disposing);
        }

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

#if DEBUG

        public override string ToString()
        {
            return GetType().Name!.Replace("LauncherItemCustomize", string.Empty).Replace("ViewModel", string.Empty);
        }

#endif

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId => Model.LauncherItemId;

        #endregion

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Model.IsLazyLoad) && !Model.IsLazyLoad) {
                Model.PropertyChanged -= Model_PropertyChanged;
                Initialize();
            }
        }
    }
}
