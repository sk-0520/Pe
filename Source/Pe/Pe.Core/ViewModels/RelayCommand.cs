using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public abstract class RelayCommandBase: ICommand
    {
        #region ICommand

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class RelaySyncCommand
    {
        #region property



        #endregion
    }

    public class RelayAsyncCommand
    {
        #region property



        #endregion
    }
}
