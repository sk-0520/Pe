using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 基本設定。
    /// </summary>
    public class SettingAppGeneralSettingData: DataBase
    {
        public SettingAppGeneralSettingData()
        { }

        #region property

        /// <summary>
        /// 使用言語。
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// ユーザー設定ディレクトリパス。
        /// </summary>

        public string UserBackupDirectoryPath { get; set; } = string.Empty;

        /// <summary>
        /// 使用テーマのプラグインID。
        /// </summary>
        public Guid ThemePluginId { get; set; }

        #endregion
    }

    /// <summary>
    /// 初回実行データ。
    /// </summary>
    public class AppGeneralFirstData: DataBase
    {
        #region property

        /// <summary>
        /// 初回実行バージョン。
        /// </summary>
        public Version FirstExecuteVersion { get; set; } = new Version();

        /// <summary>
        /// 初回実行日時。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime FirstExecuteTimestamp { get; set; }


        #endregion
    }
}
