using System;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings
{
    /// <summary>
    /// MVVM で使用するモデル基底。
    /// </summary>
    public abstract class BindModelBase: NotifyPropertyBase, IDisposed, IDisposeObservable
    {
        #region define

        /// <summary>
        /// 未指定時の <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> の参照方法。
        /// </summary>
        public const EventReference DefaultPropertyChanged = EventReference.Weak;
        /// <summary>
        /// 未指定時の <see cref="IDisposeObservable.Disposing"/> の参照方法。
        /// </summary>
        public const EventReference DefaultDisposing = EventReference.Weak;

        #endregion

        #region event

        private event EventHandler<EventArgs>? StrongDisposing;

        #endregion

        protected BindModelBase(EventReference propertyChangedEventReference, EventReference disposingEventReference)
            : base(propertyChangedEventReference)
        {
            if(disposingEventReference == EventReference.Weak) {
                DisposingWeakEvent = new WeakEvent<EventArgs>(nameof(Disposing));
            }
        }

        protected BindModelBase()
            : this(DefaultPropertyChanged, DefaultDisposing)
        { }

        ~BindModelBase()
        {
            Dispose(false);
        }

        #region property

        private WeakEvent<EventArgs>? DisposingWeakEvent { get; }

        #endregion

        #region function


        /// <summary>
        /// 既に破棄済みの場合は処理を中断する。
        /// </summary>
        /// <exception cref="ObjectDisposedException">破棄済み。</exception>
        /// <seealso cref="IDisposed"/>
        protected void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
        }

        #endregion

        #region IDisposeObservable

        public event EventHandler<EventArgs>? Disposing
        {
            add
            {
                if(DisposingWeakEvent is null) {
                    StrongDisposing += value;
                } else {
                    DisposingWeakEvent.Add(value);
                }
            }
            remove
            {
                if(DisposingWeakEvent is null) {
                    StrongDisposing -= value;
                } else {
                    DisposingWeakEvent.Remove(value);
                }
            }
        }

        #endregion

        #region IDisposed

        private bool _isDisposed;

        public bool IsDisposed => this._isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if(this._isDisposed) {
                return;
            }

            if(DisposingWeakEvent is null) {
                StrongDisposing?.Invoke(this, EventArgs.Empty);
            } else {
                DisposingWeakEvent.Raise(this, EventArgs.Empty);
            }

            this._isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
