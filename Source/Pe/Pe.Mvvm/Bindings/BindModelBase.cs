using System;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings
{
    /// <summary>
    /// MVVM で使用するモデル基底。
    /// </summary>
    public abstract class BindModelBase: NotifyPropertyBase, IDisposed, IDisposeObservable
    {
        #region event

        private event EventHandler<EventArgs>? StrongDisposing;

        #endregion

        protected BindModelBase(EventReference propertyChangedEventType, EventReference disposingEventReference)
            : base(propertyChangedEventType)
        {
            if(disposingEventReference == EventReference.Weak) {
                DisposingWeakEvent = new WeakEvent<EventArgs>(nameof(Disposing));
            }
        }

        protected BindModelBase()
            : this(EventReference.Weak, EventReference.Weak)
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
        /// <param name="callerMemberName"></param>
        /// <exception cref="ObjectDisposedException">破棄済み。</exception>
        /// <seealso cref="IDisposed"/>
        protected void ThrowIfDisposed([CallerMemberName] string callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(callerMemberName);
            }
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

        private bool _isDisposed = false;

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
