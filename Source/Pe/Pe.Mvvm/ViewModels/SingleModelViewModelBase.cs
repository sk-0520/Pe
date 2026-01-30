using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    /// <summary>
    /// モデルとビューモデルを一対一で紐づける。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class SingleModelViewModelBase<TModel>: SimpleModelViewModelBase<TModel>
        where TModel : INotifyPropertyChanged
    {
        protected SingleModelViewModelBase(TModel model, PropertyMode propertyMode, ILoggerFactory loggerFactory)
            : base(model, propertyMode, DefaultPropertyChanged, DefaultDisposing, loggerFactory)
        { }

        protected SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : this(model, DefaultPropertyMode, loggerFactory)
        { }

        #region property

        #endregion
    }
}
