using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Mvvm.Commands
{
    /// <summary>
    /// <see cref="ICommand"/> に非同期処理を追加。"/>
    /// </summary>
    public interface IAsyncCommand: ICommand
    {
        #region property

        /// <summary>
        /// 実行中か。
        /// </summary>
        bool IsExecuting { get; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="ICommand.Execute(object?)"/>の非同期版。
        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        /// </summary>
        /// <param name="parameter"><inheritdoc cref="ICommand.Execute(object?)"/></param>
        /// <returns>タスク。</returns>
        Task ExecuteAsync(object? parameter);

        /// <summary>
        /// 実行中コマンドを取り消す。
        /// </summary>
        /// <returns>キャンセルできたか。</returns>
        bool CancelExecution();

        #endregion
    }


    public abstract class AsyncDelegateCommandBase<TParameter>: CommandBase, IAsyncCommand
    {
        protected AsyncDelegateCommandBase(Func<TParameter, CancellationToken, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        protected AsyncDelegateCommandBase(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ArgumentNullException.ThrowIfNull(executeAction);

            ExecuteAction = (o, _) => executeAction(o);
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        protected AsyncDelegateCommandBase(Func<TParameter, Task> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        /// <summary>
        /// <see cref="OperationCanceledException"/>を再スローするか。
        /// </summary>
        public bool RethrowOperationCanceledException { get; init; } = false;

        protected CancellationTokenSource? CancellationTokenSource { get; set; }

        private Func<TParameter, CancellationToken, Task> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override async void Execute(object? parameter)
        {
            await ExecuteAsync(parameter);
        }

        public override bool CanExecute(object? parameter)
        {
            return !IsExecuting && CanExecuteFunc((TParameter)parameter!);
        }

        #endregion

        #region IAsyncCommand

        public bool IsExecuting => CancellationTokenSource != null;

        public async Task ExecuteAsync(object? parameter)
        {
            CancellationTokenSource = new CancellationTokenSource();
            try {
                RaiseCanExecuteChanged();
                await ExecuteAction((TParameter)parameter!, CancellationTokenSource.Token);
            } catch(OperationCanceledException ex) when(ex.CancellationToken == CancellationTokenSource.Token) {
                if(RethrowOperationCanceledException) {
                    throw;
                }
                Debug.Write(ex);
            } finally {
                CancellationTokenSource.Dispose();
                CancellationTokenSource = null;
                RaiseCanExecuteChanged();
            }
        }

        public bool CancelExecution()
        {
            if(!IsExecuting) {
                return false;
            }
            var c = CancellationTokenSource;
            c?.Cancel();
            return c is not null;
        }


        #endregion
    }

    public class AsyncDelegateCommand: AsyncDelegateCommandBase<object>
    {
        public AsyncDelegateCommand(Func<object, CancellationToken, Task> executeAction, Func<object, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }

        public AsyncDelegateCommand(Func<object, Task> executeAction)
            : base(executeAction)
        { }

        public AsyncDelegateCommand(Func<object, Task> executeAction, Func<object, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }

        public AsyncDelegateCommand(Func<Task> executeAction)
            : this(_ => executeAction())
        {
            ArgumentNullException.ThrowIfNull(executeAction);
        }

        public AsyncDelegateCommand(Func<Task> executeAction, Func<bool> canExecuteFunc)
            : this(_ => executeAction(), _ => canExecuteFunc())
        {
            ArgumentNullException.ThrowIfNull(executeAction);
            ArgumentNullException.ThrowIfNull(canExecuteFunc);
        }
    }

    public class AsyncDelegateCommand<TParameter>: AsyncDelegateCommandBase<TParameter>
    {
        public AsyncDelegateCommand(Func<TParameter, CancellationToken, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }

        public AsyncDelegateCommand(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }
}
