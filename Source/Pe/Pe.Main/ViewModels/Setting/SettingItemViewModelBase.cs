using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public abstract class SettingItemViewModelBase<TModel> : SingleModelViewModelBase<TModel>
        where TModel : ElementBase
    {
        #region variable

        bool _isInitialized;

        #endregion

        protected SettingItemViewModelBase(TModel model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(Model.IsInitialized), OnInitialized);
            if(Model.IsInitialized) {
                OnInitialized();
            }
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected PropertyChangedHooker PropertyChangedHooker { get; }

        public bool IsInitialized
        {
            get => this._isInitialized;
            private set => SetProperty(ref this._isInitialized, value);
        }

        #endregion

        #region command
        #endregion

        #region function

        protected virtual void BuildChildren()
        { }
        protected virtual void RaiseChildren()
        { }

        private void OnInitialized()
        {
            if(!Model.IsInitialized) {
                return;
            }

            BuildChildren();

            var properties = GetType().GetProperties()
                .Where(i => i.CanRead)
            ;
            foreach(var property in properties) {
                RaisePropertyChanged(property.Name);
            }

            RaiseChildren();

            IsInitialized = true;
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            Model.PropertyChanged -= Model_PropertyChanged;

            base.DetachModelEventsImpl();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

    }
}
