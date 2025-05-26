using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Commands
{
    /// <summary>
    /// 見た目おんなじインターフェイスコマンド生成処理。
    /// </summary>
    public static class CommandFactory
    {
        #region function

        #region DelegateCommand@void

        public static DelegateCommand Create(Action executeAction)
        {
            return new DelegateCommand(executeAction);
        }

        public static DelegateCommand Create(Action executeAction, Func<bool> canExecuteFunc)
        {
            return new DelegateCommand(executeAction, canExecuteFunc);
        }

        #endregion

        #region DelegateCommand@object

        public static DelegateCommand Create(Action<object> executeAction)
        {
            return new DelegateCommand(executeAction);
        }

        public static DelegateCommand Create(Action<object> executeAction, Func<object, bool> canExecuteFunc)
        {
            return new DelegateCommand(executeAction, canExecuteFunc);
        }

        #endregion

        #region DelegateCommand@T

        public static DelegateCommand<T> Create<T>(Action<T> executeAction)
        {
            return new DelegateCommand<T>(executeAction);
        }

        public static DelegateCommand<T> Create<T>(Action<T> executeAction, Func<T, bool> canExecuteFunc)
        {
            return new DelegateCommand<T>(executeAction, canExecuteFunc);
        }

        #endregion

        #region DelegateCommand@void

        public static AsyncDelegateCommand Create(Func<Task> executeAction)
        {
            return new AsyncDelegateCommand(executeAction);
        }

        public static AsyncDelegateCommand Create(Func<Task> executeAction, Func<bool> canExecuteFunc)
        {
            return new AsyncDelegateCommand(executeAction, canExecuteFunc);
        }

        #endregion

        #region DelegateCommand@object

        public static AsyncDelegateCommand Create(Func<object, Task> executeAction)
        {
            return new AsyncDelegateCommand(executeAction);
        }

        public static AsyncDelegateCommand Create(Func<object, Task> executeAction, Func<object, bool> canExecuteFunc)
        {
            return new AsyncDelegateCommand(executeAction, canExecuteFunc);
        }

        #endregion

        #region DelegateCommand@T

        public static AsyncDelegateCommand<T> Create<T>(Func<T, Task> executeAction)
        {
            return new AsyncDelegateCommand<T>(executeAction);
        }

        public static AsyncDelegateCommand<T> Create<T>(Func<T, Task> executeAction, Func<T, bool> canExecuteFunc)
        {
            return new AsyncDelegateCommand<T>(executeAction, canExecuteFunc);
        }

        #endregion

        #endregion
    }
}
