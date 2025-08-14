using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 要素の属する(?)DPIを出力。
    /// </summary>
    /// <remarks>
    /// <para>ウィンドウのいるディスプレイのDPIを出力する感じ。</para>
    /// </remarks>
    public interface IDpiScaleContext
    {
        #region function

        /// <summary>
        /// 96 px に対する現在 DPI スケール。
        /// </summary>
        /// <returns></returns>
        Point GetDpiScale();
        /// <summary>
        /// 所属しているディスプレイの取得。
        /// </summary>
        /// <returns></returns>
        IScreen GetOwnerScreen();

        #endregion
    }

    /// <summary>
    /// 空の <see cref="IDpiScaleContext"/>。
    /// </summary>
    /// <remarks>
    /// <para>固定値を取得する。</para>
    /// </remarks>
    public sealed class EmptyDpiScaleContext : IDpiScaleContext
    {
        static EmptyDpiScaleContext()
        {
            Default = new EmptyDpiScaleContext();
        }

        #region property

        public static EmptyDpiScaleContext Default { get; }

        #endregion

        #region IDpiScaleContext

        /// <inheritdoc cref="IDpiScaleContext.GetDpiScale"/>
        public Point GetDpiScale() => new Point(1, 1);
        /// <inheritdoc cref="IDpiScaleContext.GetOwnerScreen"/>
        public IScreen GetOwnerScreen() => Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen is null");

        #endregion
    }
}
