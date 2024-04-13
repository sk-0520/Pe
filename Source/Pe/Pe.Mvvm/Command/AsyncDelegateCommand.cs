using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Command
{
    public abstract class AsyncDelegateCommandBase<TParameter>: CommandBase
    {
        #region variable

        private int _executingCount;

        #endregion

        public AsyncDelegateCommandBase(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        public AsyncDelegateCommandBase(Func<TParameter, Task> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        /// <summary>
        /// 現在実行数。
        /// </summary>
        public int ExecutingCount => this._executingCount;

        /// <summary>
        /// 同時実行を抑制するか。
        /// </summary>
        /// <remarks>
        /// <para>基本的に <see langword="init"/> であることを前提としてる。使えんけど。</para>
        /// </remarks>
        public bool SuppressCommandWhileExecuting { get; set; } = true;

        private Func<TParameter, Task> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override async void Execute(object? parameter)
        {
            Interlocked.Increment(ref this._executingCount);
            try {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                await ExecuteAction((TParameter)parameter);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
            } finally {
                Interlocked.Decrement(ref this._executingCount);
                RaiseCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            if(SuppressCommandWhileExecuting) {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                return ExecutingCount == 0 && CanExecuteFunc((TParameter)parameter);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
            }

#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            return CanExecuteFunc((TParameter)parameter);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
        }

        #endregion
    }

    public class AsyncDelegateCommand: AsyncDelegateCommandBase<object>
    {
        public AsyncDelegateCommand(Func<object?, Task> executeAction)
            : base(executeAction)
        { }

        public AsyncDelegateCommand(Func<object?, Task> executeAction, Func<object?, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }

    public class AsyncDelegateCommand<TParameter>: AsyncDelegateCommandBase<TParameter>
    {
        public AsyncDelegateCommand(Func<TParameter, Task> executeAction)
            : base(executeAction)
        { }

        public AsyncDelegateCommand(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }
}
