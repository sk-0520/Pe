using System;

namespace ContentTypeTextNet.Pe.Mvvm.Command
{
    public abstract class DelegateCommandBase<TParameter>: CommandBase
    {
        public DelegateCommandBase(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        public DelegateCommandBase(Action<TParameter> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        /// <summary>
        /// 現在実行数。
        /// </summary>
        public int ExecutingCount { get; private set; }

        private Action<TParameter> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override void Execute(object? parameter)
        {
            ExecutingCount += 1;
            try {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                ExecuteAction((TParameter)parameter);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
            } finally {
                ExecutingCount -= 1;
            }
        }

        public override bool CanExecute(object? parameter)
        {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            return CanExecuteFunc((TParameter)parameter);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
        }

        #endregion
    }

    public class DelegateCommand: DelegateCommandBase<object>
    {
        public DelegateCommand(Action<object?> executeAction)
            : base(executeAction)
        { }

        public DelegateCommand(Action<object?> executeAction, Func<object?, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }

    public class DelegateCommand<TParameter>: DelegateCommandBase<TParameter>
    {
        public DelegateCommand(Action<TParameter> executeAction)
            : base(executeAction)
        { }

        public DelegateCommand(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }
}
