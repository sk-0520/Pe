using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{

    /// <summary>
    /// ランチャーアイテムアドオンファイル操作処理。
    /// <para>Pe の管理下で処理する。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonFileStorage
    {
        #region function

        /// <summary>
        /// 指定ファイル名が存在するか。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="name">ファイル名として有効な名前。</param>
        /// <returns>存在すれば真。</returns>
        bool Exists(Guid launcherItemId, string name);

        /// <summary>
        /// 指定ファイル名を変更する。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="sourceName">元ファイル名。</param>
        /// <param name="destinationName">変更ファイル名。</param>
        /// <param name="overwrite">上書きを行うか。</param>
        void Rename(Guid launcherItemId, string sourceName, string destinationName, bool overwrite);

        /// <summary>
        /// 指定ファイル名を複製する。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="sourceName">元ファイル名。</param>
        /// <param name="destinationName">コピー先ファイル名。</param>
        /// <param name="overwrite">上書きを行うか。</param>
        void Copy(Guid launcherItemId, string sourceName, string destinationName, bool overwrite);

        /// <summary>
        /// 指定ファイル名を削除する。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="name">ファイル名として有効な名前。</param>
        void Delete(Guid launcherItemId, string name);

        /// <summary>
        /// 指定ファイル名を開く。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="name">ファイル名として有効な名前。</param>
        /// <param name="fileMode"><see cref="System.IO.FileMode"/></param>
        /// <returns>ストリーム。</returns>
        Stream Open(Guid launcherItemId, string name, FileMode fileMode);

        #endregion
    }

    /// <summary>
    /// ランチャーアイテムアドオン永続データ操作処理。
    /// <para>Pe の管理下で処理する。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonPersistentStorage
    {
        #region property

        /// <summary>
        /// 永続データアクセスは読み取り専用か。
        /// <para>読み取り専用の場合、書き込み処理実行で例外発生。</para>
        /// </summary>
        bool IsReadOnly { get; }

        #endregion

        #region function

        /// <summary>
        /// 指定データは存在するか。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="key">キー</param>
        /// <returns></returns>
        bool Exists(Guid launcherItemId, string key);

        /// <summary>
        /// 指定データを取得する。
        /// </summary>
        /// <typeparam name="TValue">格納データ型。</typeparam>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="key">キー</param>
        /// <param name="value">取得・変換できた場合に格納。</param>
        /// <returns>取得・変換できたか。</returns>
        bool TryGet<TValue>(Guid launcherItemId, string key, [MaybeNullWhen(returnValue: false)] out TValue value);

        /// <summary>
        /// 指定データを保存する。
        /// <para><see cref="PluginPersistentFormat.Text"/>を使用する以外は原則使用せず<see cref="Set{TValue}(string, TValue)"/>を用いること。</para>
        /// </summary>
        /// <typeparam name="TValue">保存データ。</typeparam>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="key">キー。</param>
        /// <param name="value">値。</param>
        /// <param name="format">変換種別。</param>
        /// <returns>保存成功・失敗。</returns>
        bool Set<TValue>(Guid launcherItemId, string key, TValue value, PluginPersistentFormat format);
        /// <summary>
        /// 現行バージョンにおける最適な型を使用して指定データを保存する。
        /// </summary>
        /// <typeparam name="TValue"><inheritdoc cref="SetValue{TValue}(string, TValue, PluginPersistentFormat)"/></typeparam>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="key">キー。</param>
        /// <param name="value">値。</param>
        /// <returns>保存成功・失敗。</returns>
        bool Set<TValue>(Guid launcherItemId, string key, TValue value);

        /// <summary>
        /// 指定データを破棄する。
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="key"></param>
        /// <returns>破棄成功。</returns>
        bool Delete(Guid launcherItemId, string key);

        #endregion
    }

    /// <summary>
    /// ランチャーアイテムアドオンファイル操作処理グループ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonFiles
    {
        #region property

        /// <summary>
        /// バックアップ対象。
        /// </summary>
        ILauncherItemAddonFileStorage User { get; }
        /// <summary>
        /// 非バックアップ対象。
        /// <para>次回起動時に存在する可能性あり。</para>
        /// </summary>
        ILauncherItemAddonFileStorage Machine { get; }
        /// <summary>
        /// 非バックアップ対象。
        /// <para>次回起動時に存在しない。</para>
        /// </summary>
        ILauncherItemAddonFileStorage Temporary { get; }

        #endregion
    }

    /// <summary>
    /// ランチャーアイテムアドオン永続データ操作処理グループ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonPersistents
    {
        #region property

        /// <summary>
        /// 通常データ。
        /// </summary>
        ILauncherItemAddonPersistentStorage Normal { get; }
        /// <summary>
        /// 大きめのデータ。
        /// </summary>
        ILauncherItemAddonPersistentStorage Large { get; }
        /// <summary>
        /// お好きに。
        /// </summary>
        ILauncherItemAddonPersistentStorage Temporary { get; }

        #endregion
    }

    /// <summary>
    /// ランチャーアイテムアドオンからのストレージ操作処理。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonStorage
    {
        #region property

        /// <summary>
        /// ファイル処理。
        /// </summary>
        ILauncherItemAddonFiles File { get; }
        /// <summary>
        /// 永続データ処理。
        /// </summary>
        ILauncherItemAddonPersistents Persistent { get; }

        #endregion
    }
}
