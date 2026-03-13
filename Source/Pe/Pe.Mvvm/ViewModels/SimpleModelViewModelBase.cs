using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    /// <summary>
    /// モデルとビューモデルを一対一で紐づける。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class SimpleModelViewModelBase<TModel>: ViewModelBase
        where TModel : notnull, INotifyPropertyChanged
    {
        protected SimpleModelViewModelBase(TModel model, PropertyMode propertyMode, EventReference propertyChangedEventReference, EventReference disposingEventReference, ILoggerFactory loggerFactory)
            : base(propertyMode, propertyChangedEventReference, disposingEventReference, loggerFactory)
        {
            Model = model;

            AttachModelEvents();
        }

        protected SimpleModelViewModelBase(TModel model, PropertyMode propertyMode, ILoggerFactory loggerFactory)
            : this(model, propertyMode, DefaultPropertyChanged, DefaultDisposing, loggerFactory)
        { }

        protected SimpleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : this(model, DefaultPropertyMode, loggerFactory)
        { }

        #region property

        protected TModel Model { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// モデルの値を変更する。
        /// </summary>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="value">設定値。</param>
        /// <param name="modelPropertyName">モデルのプロパティ名。</param>
        /// <param name="notifyPropertyName">変更通知のプロパティ名。基本的に呼び出し側のメンバ名となる想定。</param>
        /// <returns>変更されたか。</returns>
        protected bool SetModel<T>(T value, [CallerMemberName] string modelPropertyName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            var type = Model.GetType();
            var prop = type.GetProperty(modelPropertyName);
            Debug.Assert(prop is not null);

            return SetProperty(Model, value, prop.Name, notifyPropertyName);
        }

        /// <summary>
        /// モデルを取り込んだ際に一度だけ呼び出される処理。
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </summary>
        protected virtual void AttachModelEventsCore()
        {
            ThrowIfDisposed();
        }

        protected void AttachModelEvents()
        {
            if(Model is not null) {
                ThrowIfDisposed();

                AttachModelEventsCore();
            }
        }

        /// <summary>
        /// モデルとサヨナラするとき(<see cref="Dispose(bool)"/>とか)するときに一度だけ呼び出される。
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </summary>
        protected virtual void DetachModelEventsCore()
        { }

        protected void DetachModelEvents()
        {
            if(Model is not null) {
                DetachModelEventsCore();
            }
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DetachModelEvents();

                Model = default!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
