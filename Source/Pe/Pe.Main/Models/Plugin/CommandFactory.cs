using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    sealed internal class CommandFactory: ICommandFactory
    {
        #region ICommandFactory

        public ICommand CreateCommand(Action execute)
        {
            return new DelegateCommand(execute);
        }

        public ICommand CreateCommand<TParameter>(Action<TParameter> execute)
        {
            return new DelegateCommand<TParameter>(execute);
        }

        public ICommand CreateCommand(Action execute, Func<bool> canExecute)
        {
            return new DelegateCommand(execute, canExecute);
        }

        public ICommand CreateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new DelegateCommand<TParameter>(execute, canExecute);
        }

        #endregion
    }
}
