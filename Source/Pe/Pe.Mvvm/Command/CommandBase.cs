using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Mvvm.Command
{
    /// <summary>
    /// <see cref="ICommand"/>実装基底クラス。
    /// </summary>
    public abstract class CommandBase: ICommand
    {
        protected CommandBase()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }

        #region property

        protected SynchronizationContext? SynchronizationContext { get; }

        /// <summary>
        /// 同時実行を抑制するか。
        /// <para>基本的に <see langword="init"/> であることを前提としてる。使えんけど。</para>
        /// </summary>
        public bool SuppressCommandWhileExecuting { get; set; } = true;

        #endregion

        #region function

        protected virtual void OnCanExecuteChanged()
        {
            if(SynchronizationContext != null && SynchronizationContext != SynchronizationContext.Current) {
                SynchronizationContext.Post(o => CanExecuteChanged?.Invoke(this, EventArgs.Empty), null);
            } else {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        #endregion

        #region ICommand

        public event EventHandler? CanExecuteChanged;

        public abstract void Execute(object? parameter);

        public abstract bool CanExecute(object? parameter);

        #endregion
    }

}
