using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Core.Models.Command
{
    public abstract class DelegateCommandBase: ICommand
    {
        protected DelegateCommandBase()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }

        #region property

        private SynchronizationContext? SynchronizationContext { get; }

        #endregion

        #region function

        protected virtual void OnCanExecuteChanged()
        {
            var canExecuteChanged = CanExecuteChanged;
            if(canExecuteChanged is null) {
                return;
            }

            if(SynchronizationContext is not null && SynchronizationContext != SynchronizationContext.Current) {
                SynchronizationContext.Post((o) => canExecuteChanged.Invoke(this, EventArgs.Empty), null);
            } else {
                canExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        #endregion

        #region ICommand

        public event EventHandler? CanExecuteChanged;

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);

        #endregion
    }
}
