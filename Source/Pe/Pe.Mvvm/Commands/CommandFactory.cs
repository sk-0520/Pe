using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Commands
{
    public class CommandFactory: ICommandFactory
    {
        #region function
        private static bool EmptyCanExecuteFunc() => true;
        private static bool EmptyCanExecuteFunc<TParameter>(TParameter parameter) => true;

        #endregion

        #region ICommandFactory

        public DelegateCommand Create(Action executeAction)
        {
            return new DelegateCommand(executeAction, EmptyCanExecuteFunc);
        }

        public DelegateCommand Create(Action executeAction, Func<bool> canExecuteFunc)
        {
            return new DelegateCommand(executeAction, canExecuteFunc);
        }

        public DelegateCommand Create(Action<object> executeAction)
        {
            return new DelegateCommand(executeAction, EmptyCanExecuteFunc<object>);
        }

        public DelegateCommand Create(Action<object> executeAction, Func<object, bool> canExecuteFunc)
        {
            return new DelegateCommand(executeAction, canExecuteFunc);
        }

        public DelegateCommand<T> Create<T>(Action<T> executeAction)
        {
            return new DelegateCommand<T>(executeAction, EmptyCanExecuteFunc<T>);
        }

        public DelegateCommand<T> Create<T>(Action<T> executeAction, Func<T, bool> canExecuteFunc)
        {
            return new DelegateCommand<T>(executeAction, canExecuteFunc);
        }

        public AsyncDelegateCommand Create(Func<Task> executeAction)
        {
            return new AsyncDelegateCommand(executeAction, EmptyCanExecuteFunc);
        }

        public AsyncDelegateCommand Create(Func<Task> executeAction, Func<bool> canExecuteFunc)
        {
            return new AsyncDelegateCommand(executeAction, canExecuteFunc);
        }

        public AsyncDelegateCommand Create(Func<object, Task> executeAction)
        {
            return new AsyncDelegateCommand(executeAction, EmptyCanExecuteFunc);
        }

        public AsyncDelegateCommand Create(Func<object, Task> executeAction, Func<object, bool> canExecuteFunc)
        {
            return new AsyncDelegateCommand(executeAction, canExecuteFunc);
        }

        public AsyncDelegateCommand<T> Create<T>(Func<T, Task> executeAction)
        {
            return new AsyncDelegateCommand<T>(executeAction, EmptyCanExecuteFunc<T>);
        }

        public AsyncDelegateCommand<T> Create<T>(Func<T, Task> executeAction, Func<T, bool> canExecuteFunc)
        {
            return new AsyncDelegateCommand<T>(executeAction, canExecuteFunc);
        }

        #endregion
    }
}
