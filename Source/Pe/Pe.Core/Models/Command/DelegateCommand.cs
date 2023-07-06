using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models.Command
{
    public class DelegateCommand: DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, NullCanExecuteMethod)
        { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            ExecuteMethod = executeMethod;
            CanExecuteMethod = canExecuteMethod;
        }

        #region property

        private Func<bool> CanExecuteMethod { get; }
        private Action ExecuteMethod { get; }

        #endregion

        #region function

        private static bool NullCanExecuteMethod() => true;

        #endregion

        #region DelegateCommandBase

        public override bool CanExecute(object? parameter)
        {
            return CanExecuteMethod();
        }

        public override void Execute(object? parameter)
        {
            ExecuteMethod();
        }

        #endregion
    }

    public class DelegateCommand<TParameter>: DelegateCommandBase
    {
        public DelegateCommand(Action<TParameter> executeMethod)
            : this(executeMethod, NullCanExecuteMethod)
        { }

        public DelegateCommand(Action<TParameter> executeMethod, Func<TParameter, bool> canExecuteMethod)
        {
            ExecuteMethod = executeMethod;
            CanExecuteMethod = canExecuteMethod;
        }

        #region property

        private Func<TParameter, bool> CanExecuteMethod { get; }
        private Action<TParameter> ExecuteMethod { get; }

        #endregion

        #region function

        private static bool NullCanExecuteMethod(TParameter _) => true;

        #endregion

        #region DelegateCommandBase

        public override bool CanExecute(object? parameter)
        {
            return CanExecuteMethod((TParameter)parameter!);
        }

        public override void Execute(object? parameter)
        {
            ExecuteMethod((TParameter)parameter!);
        }

        #endregion
    }
}
