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
}
