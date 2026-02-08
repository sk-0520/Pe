using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// ランチャーアイテム種別。
    /// </summary>
    public enum LauncherItemKind
    {
        /// <summary>
        /// 💩。
        /// </summary>
        [EnumResource]
        Unknown,
        /// <summary>
        /// ファイルアイテム。
        /// </summary>
        /// <remarks>
        /// <para>可能な限りPATHを考慮するので旧来のコマンドに近い挙動も可能。</para>
        /// </remarks>
        [EnumResource]
        File,
        /// <summary>
        /// ストアアプリ。
        /// </summary>
        /// <remarks>
        /// <para>プロトコルとかエイリアスであれこれ。</para>
        /// <para><see cref="File"/>と違って小難しい処理は無理。</para>
        /// </remarks>
        [EnumResource]
        StoreApp,
        /// <summary>
        /// プラグインアイテム。
        /// </summary>
        /// <remarks>
        /// <para>プラグインのみぞ知る機能。</para>
        /// </remarks>
        [EnumResource]
        Addon,
        /// <summary>
        /// セパレータ。
        /// </summary>
        /// <remarks>
        /// <para>いる、これ？</para>
        /// </remarks>
        [EnumResource]
        Separator,
    }


    /// <summary>
    /// ランチャーツールバーのアイコン(ボタン)位置。
    /// </summary>
    public enum LauncherToolbarIconDirection
    {
        /// <summary>
        /// 水平: 左から, 垂直: 上から。
        /// </summary>
        [EnumResource]
        LeftTop,
        /// <summary>
        /// 中央から。
        /// </summary>
        [EnumResource]
        Center,
        /// <summary>
        /// 水平: 右から, 垂直: 下から。
        /// </summary>
        [EnumResource]
        RightBottom,
    }

    /// <summary>
    /// ランチャーツールバーへのD&amp;D処理。
    /// </summary>
    public enum LauncherToolbarContentDropMode
    {
        /// <summary>
        /// 指定して実行。
        /// </summary>
        [EnumResource]
        ExtendsExecute,
        /// <summary>
        /// D&amp;Dデータをパラメータとして直接実行。
        /// </summary>
        [EnumResource]
        DirectExecute,
    }

    public enum LauncherToolbarShortcutDropMode
    {
        /// <summary>
        /// 登録方法を確認する。
        /// </summary>
        [EnumResource]
        Confirm,
        /// <summary>
        /// ショートカットのリンク先を登録する。
        /// </summary>
        [EnumResource]
        Target,
        /// <summary>
        /// ショートカット自体を登録する。
        /// </summary>
        [EnumResource]
        Shortcut,
    }

    /// <summary>
    /// 重複ファイルアイテム登録方法。
    /// </summary>
    public enum LauncherToolbarDuplicatedFileRegisterMode
    {
        /// <summary>
        /// 重複を考慮しない。
        /// </summary>
        [EnumResource]
        None,
        /// <summary>
        /// ファイルパスが重複している場合に重複元アイテムを使用する。
        /// </summary>
        [EnumResource]
        FilePathOnly,
        /// <summary>
        /// ファイルパスとオプションが重複している場合に重複元アイテムを使用する。
        /// </summary>
        [EnumResource]
        FilePathWithOption,
    }



    public interface ILauncherExecutePathParameter
    {
        #region property

        string Path { get; }
        string Option { get; }
        string WorkDirectoryPath { get; }

        #endregion
    }

    public class LauncherExecutePathParameter: ILauncherExecutePathParameter
    {
        public LauncherExecutePathParameter(string path, string option, string workDirectoryPath)
        {
            Path = path;
            Option = option;
            WorkDirectoryPath = workDirectoryPath;
        }

        #region ILauncherExecutePathParameter

        public string Path { get; }
        public string Option { get; }
        public string WorkDirectoryPath { get; }

        #endregion
    }

    public interface ILauncherExecuteCustomParameter
    {
        #region property

        string Caption { get; }
        ShowMode ShowMode { get; }
        bool IsEnabledCustomEnvironmentVariable { get; }
        bool IsEnabledStandardInputOutput { get; }
        Encoding StandardInputOutputEncoding { get; }
        bool RunAdministrator { get; }
        #endregion
    }

    [Serializable, DataContract]
    public class LauncherExecutePathData: ILauncherExecutePathParameter
    {
        #region ILauncherExecutePathParameter

        public string Path { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectoryPath { get; set; } = string.Empty;

        #endregion
    }

    public class LauncherFileData: LauncherExecutePathData, ILauncherExecuteCustomParameter
    {
        #region ILauncherExecuteCustomParameter
        public string Caption { get; set; } = string.Empty;
        public ShowMode ShowMode { get; set; } = ShowMode.Normal;
        public bool IsEnabledCustomEnvironmentVariable { get; set; }
        public bool IsEnabledStandardInputOutput { get; set; }
        public Encoding StandardInputOutputEncoding { get; set; } = EncodingUtility.GetDefaultEncoding();
        public bool RunAdministrator { get; set; }
        #endregion
    }

    public class LauncherStoreAppData
    {
        #region property

        public string ProtocolAlias { get; init; } = string.Empty;
        public string Option { get; init; } = string.Empty;
        #endregion
    }

    public record class LauncherSeparatorData
    {
        #region property

        public required LauncherSeparatorKind Kind { get; init; }

        public required int Width { get; init; }

        #endregion
    }

    /// <summary>
    /// 環境変数設定データ。
    /// </summary>
    public record class LauncherEnvironmentVariableData
    {
        /// <summary>
        /// 値あり生成。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <remarks>編集対象。</remarks>
        public LauncherEnvironmentVariableData(string name, string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNull(value);

            Name = name;
            Value = value;
        }

        /// <summary>
        /// 値無し生成。
        /// </summary>
        /// <param name="name"></param>
        /// <remarks>削除対象。</remarks>
        public LauncherEnvironmentVariableData(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
            Value = string.Empty;
        }

        #region property

        /// <summary>
        /// 環境変数名。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 環境変数の値。
        /// </summary>
        /// <remarks>
        /// <para>空文字列(or <see langword="null"/> は NE)の場合は削除対象。</para>
        /// <para><see cref="IsRemove"/>で判定する。</para>
        /// </remarks>
        public string Value { get; }

        /// <summary>
        /// 削除対象か。
        /// </summary>
        public bool IsRemove => string.IsNullOrEmpty(Value);

        #endregion
    }

    public class LauncherIconData
    {
        #region property

        public LauncherItemKind Kind { get; set; }

        public IconData Path { get; set; } = new IconData();
        public IconData Icon { get; set; } = new IconData();

        #endregion
    }

    [Obsolete("なんだこれは")]
    public class StandardStreamData
    {
        #region property

        public bool IsEnabledStandardOutput { get; set; }
        public bool IsEnabledStandardInput { get; set; }

        #endregion
    }

    public enum LauncherGroupKind
    {
        [EnumResource]
        Normal,
    }

    public enum LauncherGroupImageName
    {
        DirectoryNormal,
        DirectoryOpen,
        File,
        Gear,
        Config,
        Builder,
        Bookmark,
        Book,
        Light,
        Shortcut,
        Storage,
        Cloud,
        User,
    }

    /// <summary>
    /// グループメニューの表示位置。
    /// </summary>
    public enum LauncherGroupPosition
    {
        [EnumResource]
        Top,
        [EnumResource]
        Bottom,
    }


    public class LauncherGroupData: ILauncherGroupId
    {
        #region property

        public string Name { get; init; } = string.Empty;
        public LauncherGroupKind Kind { get; init; }
        public LauncherGroupImageName ImageName { get; init; }
        public Color ImageColor { get; init; }
        public long Sequence { get; set; }

        #endregion

        #region ILauncherGroupId

        public LauncherGroupId LauncherGroupId { get; set; }

        #endregion
    }
    public interface ILauncherGroupId
    {
        #region property

        LauncherGroupId LauncherGroupId { get; }

        #endregion
    }

    public class LauncherItemData
    {
        #region property

        public LauncherItemId LauncherItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual LauncherItemKind Kind { get; set; }

        public IconData Icon { get; set; } = new IconData();

        public bool IsEnabledCommandLauncher { get; set; }

        public string Comment { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// ランチャーアイテム履歴データ種別。
    /// </summary>
    public enum LauncherHistoryKind
    {
        /// <summary>
        /// コマンドラインオプション。
        /// </summary>
        Option,
        /// <summary>
        /// 作業ディレクトリ。
        /// </summary>
        WorkDirectory,
    }

    public class LauncherHistoryData
    {
        #region property

        public LauncherHistoryKind Kind { get; set; }
        public string Value { get; set; } = string.Empty;
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastExecuteTimestamp { get; set; }
        #endregion

    }

    #region LauncherItemDetailData

    public abstract class LauncherDetailDataBase
    { }

    public class LauncherFileDetailData: LauncherDetailDataBase
    {
        #region property

        public LauncherExecutePathData PathData { get; set; } = new LauncherExecutePathData();
        public string FullPath { get; set; } = string.Empty;

        #endregion
    }

    public class LauncherAddonDetailData: LauncherDetailDataBase
    {
        #region property

        public bool IsEnabled { get; set; }

        /// <summary>
        /// <see cref="IsEnabled"/> が有効な場合は非<see langword="null" />となる。
        /// </summary>
        public ILauncherItemExtension? Extension { get; set; }

        #endregion
    }

    #endregion

    public interface ILauncherToolbarId
    {
        #region property

        LauncherToolbarId LauncherToolbarId { get; }

        #endregion
    }

    public class LauncherToolbarsScreenData: ILauncherToolbarId, IScreenData
    {
        #region ILauncherToolbarId

        public required LauncherToolbarId LauncherToolbarId { get; init; }

        #endregion

        #region IScreenData

        public string ScreenName { get; set; } = string.Empty;
        [PixelKind(Px.Device)]
        public long X { get; set; }
        [PixelKind(Px.Device)]
        public long Y { get; set; }
        [PixelKind(Px.Device)]
        public long Width { get; set; }
        [PixelKind(Px.Device)]
        public long Height { get; set; }

        #endregion
    }

    public class LauncherToolbarsDisplayData: ILauncherToolbarId
    {
        #region property

        public required LauncherGroupId LauncherGroupId { get; init; }
        public required AppDesktopToolbarPosition ToolbarPosition { get; init; }
        public required LauncherToolbarIconDirection IconDirection { get; init; }
        public required IconBox IconBox { get; init; }
        public required FontId FontId { get; init; }
        public required TimeSpan DisplayDelayTime { get; init; }
        public required TimeSpan AutoHideTime { get; init; }
        public required int TextWidth { get; init; }
        public required bool IsVisible { get; init; }
        public required bool IsTopmost { get; init; }
        public required bool IsAutoHide { get; init; }
        public required bool IsIconOnly { get; init; }

        #endregion

        #region ILauncherToolbarId

        public required LauncherToolbarId LauncherToolbarId { get; init; }

        #endregion
    }

    internal record class LauncherFileItemData
    {
        public LauncherFileItemData(LauncherItemData item, LauncherFileData file)
        {
            Item = item;
            File = file;
        }

        #region property
        public LauncherItemData Item { get; }
        public LauncherFileData File { get; }

        #endregion
    }

    /// <summary>
    /// 再実施待機方法。
    /// </summary>
    public enum RedoMode
    {
        /// <summary>
        /// 再実施しない。
        /// </summary>
        [EnumResource]
        None,
        /// <summary>
        /// 一定時間繰り返す。
        /// </summary>
        [EnumResource]
        Timeout,
        /// <summary>
        /// 指定回数繰り返す。
        /// </summary>
        [EnumResource]
        Count,
        /// <summary>
        /// 一定時間内で指定回数繰り返す。
        /// </summary>
        [EnumResource]
        TimeoutOrCount,
    }

    public interface IReadOnlyLauncherRedoData
    {
        #region property

        RedoMode RedoMode { get; }
        TimeSpan WaitTime { get; }
        int RetryCount { get; }
        IReadOnlyCollection<int> SuccessExitCodes { get; }

        #endregion
    }

    public class LauncherRedoData: IReadOnlyLauncherRedoData
    {
        #region IReadOnlyLauncherRedoData

        public RedoMode RedoMode { get; set; }
        public TimeSpan WaitTime { get; init; }
        public int RetryCount { get; init; }
        public List<int> SuccessExitCodes { get; init; } = new List<int>();
        IReadOnlyCollection<int> IReadOnlyLauncherRedoData.SuccessExitCodes => SuccessExitCodes;

        #endregion

        #region function

        public static LauncherRedoData GetDisable() => new LauncherRedoData() {
            RedoMode = RedoMode.None,
            RetryCount = 1,
            WaitTime = TimeSpan.FromSeconds(1),
        };

        #endregion
    }



    public record class LauncherIconStatus
    {
        public LauncherIconStatus(IconBox iconBox, Point dpiScale, [DateTimeKind(DateTimeKind.Utc)] DateTime lastUpdatedTimestamp)
        {
            IconScale = new IconScale(iconBox, dpiScale);
            LastUpdatedTimestamp = lastUpdatedTimestamp;
        }

        #region property

        public IconScale IconScale { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastUpdatedTimestamp { get; }

        #endregion
    }

    public class LauncherSettingCommonData: LauncherItemData
    {
        #region property

        public IList<string> Tags { get; set; } = new List<string>();

        #endregion
    }

    public enum BadgeShape
    {
        /// <summary>
        /// ふわっとした四角形。
        /// </summary>
        [EnumResource]
        RoundedSquare,
        /// <summary>
        /// かっちりした四角形。
        /// </summary>
        [EnumResource]
        SolidSquare,
        /// <summary>
        /// 円。
        /// </summary>
        [EnumResource]
        Circle
    }

    public interface IReadOnlyBadgeData
    {
        #region property

        /// <summary>
        /// バッジが有効か。
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// バッジ表記。
        /// </summary>
        /// <remarks>
        /// <para>いわゆるSJIS2バイトくらいの長さを想定。</para>
        /// </remarks>
        string Display { get; }

        /// <summary>
        /// バッジ種別。
        /// </summary>
        BadgeShape BadgeShape { get; }

        /// <summary>
        /// 背景色。
        /// </summary>
        Color Background { get; }

        #endregion
    }

    public record class BadgeData: IReadOnlyBadgeData
    {
        #region IReadOnlyBadgeData

        public bool IsVisible { get; init; }
        public required string Display { get; init; }
        public BadgeShape BadgeShape { get; init; }
        public Color Background { get; init; }

        #endregion

        #region function

        /// <summary>
        /// バッジ非表示データ。
        /// </summary>
        public static BadgeData CreateEmpty() => new BadgeData() {
            IsVisible = false,
            Display = string.Empty,
            BadgeShape = default(BadgeShape),
            Background = Colors.Black,
        };

        #endregion
    }

    public enum LauncherExecuteSource
    {
        LauncherToolbarButton,
        LauncherToolbarMenu,
        LauncherToolbarExtends,
        CommandLineNormal,
        CommandLineExtends,
        KeyGesture,
    }
}
