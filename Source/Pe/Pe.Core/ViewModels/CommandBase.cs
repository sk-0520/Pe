using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public abstract class CommandBase: ICommand
    {
        #region function

        protected void OnCanExecuteChanged()
        {
            var canExecuteChanged = CanExecuteChanged;
            if(canExecuteChanged is not null) {
                canExecuteChanged(this, EventArgs.Empty);
            }
        }

        protected abstract bool CanExecuteImpl(object? parameter);

        protected abstract void ExecuteImpl(object? parameter);

        #endregion

        #region ICommand

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return CanExecuteImpl(parameter);
        }

        public void Execute(object? parameter)
        {
            ExecuteImpl(parameter);
        }

        #endregion
    }
}
