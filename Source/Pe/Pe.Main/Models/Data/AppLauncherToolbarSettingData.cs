using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// ランチャーツールバー設定。
    /// </summary>
    [Serializable, DataContract]
    public class AppLauncherToolbarSettingData
    {
        #region property

        /// <summary>
        /// ボタンへのD&amp;D実行時の処理方法。
        /// </summary>
        public required LauncherToolbarContentDropMode ContentDropMode { get; init; }
        /// <summary>
        /// D&amp;Dファイルがショートカットの場合の処理方法。
        /// </summary>
        public required LauncherToolbarShortcutDropMode ShortcutDropMode { get; init; }
        /// <summary>
        /// グループメニューの位置。
        /// </summary>
        public required LauncherGroupPosition GroupMenuPosition { get; init; }
        /// <summary>
        /// アイテムの重複確認方法。
        /// </summary>
        public required LauncherToolbarDuplicatedFileRegisterMode DuplicatedFileRegisterMode { get; init; }

        #endregion
    }
}
