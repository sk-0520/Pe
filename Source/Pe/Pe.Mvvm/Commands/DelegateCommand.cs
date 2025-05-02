using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Commands
{
    public abstract class DelegateCommandBase<TParameter>: CommandBase
    {
        protected DelegateCommandBase(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        protected DelegateCommandBase(Action<TParameter> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        private Action<TParameter> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override void Execute(object? parameter)
        {
            ExecuteAction((TParameter)parameter!);
        }

        public override bool CanExecute(object? parameter)
        {
            return CanExecuteFunc((TParameter)parameter!);
        }

        #endregion
    }


    public class DelegateCommand: DelegateCommandBase<object>
    {
        public DelegateCommand(Action<object> executeAction)
            : base(executeAction)
        { }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }

        public DelegateCommand(Action executeAction)
            : this(_ => executeAction())
        {
            ArgumentNullException.ThrowIfNull(executeAction);
        }

        public DelegateCommand(Action executeAction, Func<bool> canExecuteFunc)
            : this(_ => executeAction(), _ => canExecuteFunc())
        {
            ArgumentNullException.ThrowIfNull(executeAction);
            ArgumentNullException.ThrowIfNull(canExecuteFunc);
        }
    }

    public class DelegateCommand<TParameter>: DelegateCommandBase<TParameter>
    {
        public DelegateCommand(Action<TParameter> executeAction)
            : base(executeAction)
        { }

        public DelegateCommand(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }
}
