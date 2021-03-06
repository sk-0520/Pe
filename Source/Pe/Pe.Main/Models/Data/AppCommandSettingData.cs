using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// コマンド設定。
    /// </summary>
    public class SettingAppCommandSettingData: DataBase
    {
        public SettingAppCommandSettingData()
        { }

        #region property

        /// <summary>
        /// フォントID。
        /// </summary>
        public Guid FontId { get; set; }
        /// <summary>
        /// アイコンサイズ。
        /// </summary>
        public IconBox IconBox { get; set; }
        /// <summary>
        /// テキスト状態の横幅。
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 非表示までの時間。
        /// </summary>
        public TimeSpan HideWaitTime { get; set; }
        /// <summary>
        /// タグ検索を行うか。
        /// </summary>
        public bool FindTag { get; set; }

        #endregion
    }
}
