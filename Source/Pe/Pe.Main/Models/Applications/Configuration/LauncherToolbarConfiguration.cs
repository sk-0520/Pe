using System;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: ランチャーツールバー。
    /// </summary>
    public class LauncherToolbarConfiguration: ConfigurationBase
    {
        public LauncherToolbarConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// 自動的に隠すまでの時間(デフォルト)。
        /// </summary>
        [Configuration]
        public TimeSpan AutoHideShowWaitTime { get; }

        /// <summary>
        /// ランチャーアイテムのD&amp;D開始装飾キー。
        /// </summary>
        [Configuration]
        public ModifierKeys DragModifierKey { get; }

        #endregion
    }
}
