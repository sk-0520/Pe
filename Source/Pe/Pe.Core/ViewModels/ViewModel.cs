using System.ComponentModel;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Mvvm.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    /// <summary>
    /// model と対になる ViewModel の基底クラス。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class SingleModelViewModelBase<TModel>: SimpleModelViewModelBase<TModel>
        where TModel : INotifyPropertyChanged
    {
        protected SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
        }




        #region function

        /// <summary>
        /// <see cref="Model"/>のプロパティに対して値設定。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="targetMemberName"></param>
        /// <param name="notifyPropertyName"></param>
        /// <returns></returns>
        protected bool SetModelValue<T>(T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetPropertyValue(Model, value, targetMemberName, notifyPropertyName);
        }

        #endregion

        #region ViewModelBase



        #endregion
    }
}
