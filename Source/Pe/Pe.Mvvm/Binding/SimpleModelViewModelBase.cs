using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Binding
{
    /// <summary>
    /// モデルとビューモデルを一対一で紐づける。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class SimpleModelViewModelBase<TModel>: ViewModelBase
    {
        protected SimpleModelViewModelBase(TModel model)
            : base()
        {
            Model = model;
        }

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
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
            var type = Model.GetType();
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
            var prop = type.GetProperty(modelPropertyName);

#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            return ChangePropertyValue(Model, value, prop, notifyPropertyName);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
        }

        // 互換用
        protected bool SetModelValue<T>(T value, [CallerMemberName] string modelPropertyName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetModel(value, modelPropertyName, notifyPropertyName);
        }

        /// <summary>
        /// モデルを取り込んだ際に一度だけ呼び出される処理。
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </summary>
        protected virtual void AttachModelEventsImpl()
        {
            ThrowIfDisposed();
        }

        protected void AttachModelEvents()
        {
            if(Model != null) {
                ThrowIfDisposed();

                AttachModelEventsImpl();
            }
        }

        /// <summary>
        /// モデルとサヨナラするとき(<see cref="Dispose(bool)"/>とか)するときに一度だけ呼び出される。
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </summary>
        protected virtual void DetachModelEventsImpl()
        { }

        protected void DetachModelEvents()
        {
            if(Model != null) {
                DetachModelEventsImpl();
            }
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Model = default!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
