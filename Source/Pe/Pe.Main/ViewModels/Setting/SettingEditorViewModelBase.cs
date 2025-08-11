using System;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface ISettingEditorViewModel: IFlushable, IDisposable
    {
        #region property

        string Header { get; }
        bool IsSelected { get; set; }

        #endregion

        #region function

        /// <summary>
        /// データ読み込み。
        /// </summary>
        Task LoadAsync(CancellationToken cancellationToken);
        /// <summary>
        /// 情報更新。
        /// </summary>
        void Refresh();

        #endregion
    }

    public abstract class SettingEditorViewModelBase<TSettingEditorElement>: SingleModelViewModelBase<TSettingEditorElement>, ISettingEditorViewModel
        where TSettingEditorElement : SettingEditorElementBase
    {
        #region variable

        private bool _isSelected;

        #endregion

        protected SettingEditorViewModelBase(TSettingEditorElement model, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized), nameof(model));
            }

            ContextDispatcher = contextDispatcher;
        }

        #region property

        protected IContextDispatcher ContextDispatcher { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region ISettingEditorViewModel

        public abstract string Header { get; }
        public bool IsSelected
        {
            get => this._isSelected;
            set => SetProperty(ref this._isSelected, value);
        }

        public virtual async Task LoadAsync(CancellationToken cancellationToken)
        {
            if(Model.IsLoaded) {
                Refresh();
                return;
            }
            await Model.LoadAsync(cancellationToken);
        }

        public abstract void Flush();

        public abstract void Refresh();

        #endregion
    }
}
