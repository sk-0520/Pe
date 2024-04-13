using System;
using System.Threading;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Mvvm.Command
{
    /// <summary>
    /// <see cref="ICommand"/>実装基底クラス。
    /// </summary>
    public abstract class CommandBase: ICommand
    {
        protected CommandBase()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }

        #region property

        protected SynchronizationContext? SynchronizationContext { get; }

        #endregion

        #region function

        protected virtual void OnCanExecuteChanged()
        {
            if(SynchronizationContext != null && SynchronizationContext != SynchronizationContext.Current) {
                SynchronizationContext.Post(o => CanExecuteChanged?.Invoke(this, EventArgs.Empty), null);
            } else {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        #endregion

        #region ICommand

        public event EventHandler? CanExecuteChanged;

        public abstract void Execute(object? parameter);

        public abstract bool CanExecute(object? parameter);

        #endregion
    }
}
