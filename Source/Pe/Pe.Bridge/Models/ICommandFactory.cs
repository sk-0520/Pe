using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// コマンド生成処理。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ICommandFactory
    {
        #region function

        public ICommand CreateCommand(Action action);
        public ICommand CreateCommand<TParameter>(Action<TParameter> action);
        public ICommand CreateCommand(Action action, Func<bool> canExecute);
        public ICommand CreateCommand<TParameter>(Action<TParameter> action, Func<TParameter, bool> canExecute);

        #endregion
    }
}
