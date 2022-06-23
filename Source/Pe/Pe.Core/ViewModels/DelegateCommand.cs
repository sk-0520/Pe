using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public class _DelegateCommand: CommandBase
    {
        public _DelegateCommand(Action executeAction, Func<bool>? canExecuteFunc = null)
        {
            ExecuteAction = executeAction;
            CanExecuteFunc = canExecuteFunc;
        }

        #region property

        private Func<bool>? CanExecuteFunc { get; init; }
        private Action ExecuteAction { get; init; }

        #endregion

        #region CommandBase

        protected override bool CanExecuteImpl(object? parameter)
        {
            if(CanExecuteFunc is null) {
                return true;
            }

            return CanExecuteFunc();
        }

        protected override void ExecuteImpl(object? parameter)
        {
            ExecuteAction();
        }

        #endregion
    }

    public class _DelegateCommand<TParameter>: CommandBase
        where TParameter : class
    {
        public _DelegateCommand(Action<TParameter> executeAction, Func<TParameter, bool>? canExecuteFunc = null)
        {
            ExecuteAction = executeAction;
            CanExecuteFunc = canExecuteFunc;
        }

        #region property

        private Func<TParameter, bool>? CanExecuteFunc { get; init; }
        private Action<TParameter> ExecuteAction { get; init; }

        #endregion

        #region CommandBase

        protected override bool CanExecuteImpl(object? parameter)
        {
            if(CanExecuteFunc is null) {
                return true;
            }

            var param = (TParameter?)parameter;

            return CanExecuteFunc(param!);
        }

        protected override void ExecuteImpl(object? parameter)
        {
            var param = (TParameter?)parameter;
            ExecuteAction(param!);
        }

        #endregion
    }

}
