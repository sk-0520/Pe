using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Mvvm.Commands
{
    public static class CommandExtensions
    {
        #region function

        public static void Invoke(this ICommand command, object? parameter = null)
        {
            if(command.CanExecute(parameter)) {
                command.Execute(parameter);
            }
        }

        public static void Invoke<TParameter>(this DelegateCommandBase<TParameter> command, TParameter parameter)
        {
            if(command.CanExecute(parameter)) {
                command.Execute(parameter);
            }
        }

        public static Task Invoke<TParameter>(this AsyncDelegateCommandBase<TParameter> command, TParameter parameter)
        {
            if(command.CanExecute(parameter)) {
                return Task.Run(() => command.Execute(parameter));
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
