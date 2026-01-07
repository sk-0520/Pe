using System;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: ノート。
    /// </summary>
    public class NoteConfiguration: ConfigurationBase
    {
        public NoteConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// 移動時の透明度。
        /// </summary>
        [Configuration]
        public double MovingOrResizingOpacity { get; }

        /// <summary>
        /// 絶対座標での標準サイズ。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutAbsoluteSize { get; }
        /// <summary>
        /// 相対座標での標準サイズ。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutRelativeSize { get; }
        /// <summary>
        /// フォント最小最大サイズ。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertMinMaxDefault))]
        public MinMaxDefault<double> FontSize { get; }

        /// <summary>
        /// 自動的に最小化するまでの時間。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertMinMax))]
        public MinMax<TimeSpan> HiddenCompactWaitTime { get; }
        /// <summary>
        /// 自動的に隠すまでの時間。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertMinMax))]
        public MinMax<TimeSpan> HiddenBlindWaitTime { get; }
        /// <summary>
        /// モニタのみ(<see cref="PInvoke.Windows.WDA.WDA_MONITOR"/>)を除外対象とするか。
        /// </summary>
        /// <remarks>
        /// <see langword="false"/> の場合は、<see cref="PInvoke.Windows.WDA.WDA_EXCLUDEFROMCAPTURE"/>が使用される。
        /// </remarks>
        [Configuration]
        public bool ExcludeScreenCaptureMonitorOnly { get; }

        #endregion
    }
}
