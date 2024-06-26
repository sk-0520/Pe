using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 特定の <see cref="System.Windows.Threading.Dispatcher"/> で実行する。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IDispatcherWrapper
    {
        #region property

        /// <summary>
        /// ラップ中の<see cref="System.Windows.Threading.Dispatcher"/>
        /// </summary>
        Dispatcher Dispatcher { get; }


        #endregion

        #region function

        /// <inheritdoc cref="Dispatcher.CheckAccess"/>
        bool CheckAccess();
        /// <inheritdoc cref="Dispatcher.VerifyAccess"/>
        void VerifyAccess();

        /// <inheritdoc cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/>
        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="InvokeAsync(Action, DispatcherPriority, CancellationToken)"/>
        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="InvokeAsync(Action, DispatcherPriority, CancellationToken)"/>
        Task InvokeAsync(Action action);
        /// <inheritdoc cref="Dispatcher.InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func);

        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        TResult Get<TArgument, TResult>(Func<TArgument, TResult> func, TArgument argument, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        TResult Get<TArgument, TResult>(Func<TArgument, TResult> func, TArgument argument, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        TResult Get<TArgument, TResult>(Func<TArgument, TResult> func, TArgument argument);

        /// <summary>
        /// 対象 <see cref="Dispatcher"/> でなんかした結果を取得する。
        /// </summary>
        /// <remarks>
        /// <para>内部的には停止状態。</para>
        /// <para>癌。</para>
        /// </remarks>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="dispatcherPriority"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        TResult Get<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        TResult Get<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        TResult Get<TResult>(Func<TResult> func);

        /// <summary>
        /// <inheritdoc cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])"/>
        /// </summary>
        /// <param name="action">実施する処理。</param>
        /// <param name="dispatcherPriority"></param>
        /// <param name="argument">パラメータ。</param>
        Task BeginAsync<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="BeginAsync{TArgument}(Action{TArgument}, TArgument, DispatcherPriority)"/>
        Task BeginAsync<TArgument>(Action<TArgument> action, TArgument argument);
        /// <inheritdoc cref="BeginAsync{TArgument}(Action{TArgument}, TArgument, DispatcherPriority)"/>
        Task BeginAsync(Action action, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="BeginAsync{TArgument}(Action{TArgument}, TArgument, DispatcherPriority)"/>
        Task BeginAsync(Action action);

        #endregion
    }
}
