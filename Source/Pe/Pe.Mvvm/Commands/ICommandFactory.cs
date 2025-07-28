using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Commands
{
    public interface ICommandFactory
    {
        #region function

        #region DelegateCommand@void

        DelegateCommand Create(Action executeAction);

        DelegateCommand Create(Action executeAction, Func<bool> canExecuteFunc);

        #endregion

        #region DelegateCommand@object

        DelegateCommand Create(Action<object> executeAction);

        DelegateCommand Create(Action<object> executeAction, Func<object, bool> canExecuteFunc);

        #endregion

        #region DelegateCommand@T

        DelegateCommand<T> Create<T>(Action<T> executeAction);

        DelegateCommand<T> Create<T>(Action<T> executeAction, Func<T, bool> canExecuteFunc);

        #endregion

        #region AsyncDelegateCommand@void

        AsyncDelegateCommand Create(Func<Task> executeAction);

        AsyncDelegateCommand Create(Func<Task> executeAction, Func<bool> canExecuteFunc);

        #endregion

        #region AsyncDelegateCommand@object

        AsyncDelegateCommand Create(Func<object, Task> executeAction);

        AsyncDelegateCommand Create(Func<object, Task> executeAction, Func<object, bool> canExecuteFunc);

        #endregion

        #region AsyncDelegateCommand@T

        AsyncDelegateCommand<T> Create<T>(Func<T, Task> executeAction);

        AsyncDelegateCommand<T> Create<T>(Func<T, Task> executeAction, Func<T, bool> canExecuteFunc);

        #endregion

        #endregion

    }
}
