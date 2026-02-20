using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:空のインターフェイスは使用しません", Justification = "実装方針が決まってない")]
    public interface IBackgroundAddonRunStartupContext
    { }

    public interface IBackgroundAddonRunPauseContext
    {
        #region property

        /// <summary>
        /// 処理を停止中か。
        /// </summary>
        bool IsPausing { get; }

        #endregion
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:空のインターフェイスは使用しません", Justification = "実装方針が決まってない")]
    public interface IBackgroundAddonRunShutdownContext
    { }

    /// <summary>
    /// 実行処理コンテキスト。
    /// </summary>
    public interface IBackgroundAddonRunExecuteContext
    {
        #region property

        RunExecuteKind RunExecuteKind { get; }
        object? Parameter { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

    public interface IBackgroundAddonKeyboardContext
    {
        #region property

        Key Key { get; }
        bool IsDown { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseMoveContext
    {
        #region property

        /// <summary>
        /// マウスカーソルの物理座標。
        /// </summary>
        [PixelKind(Px.Device)]
        Point Location { get; }
        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseButtonContext
    {
        #region property

        MouseButton Button { get; }
        MouseButtonState State { get; }
        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

}
