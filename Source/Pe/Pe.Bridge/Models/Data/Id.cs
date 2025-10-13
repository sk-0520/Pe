using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Generator.Id;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// ランチャーアイテムID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct LauncherItemId;

    /// <summary>
    /// 資格情報ID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct CredentialId;

    /// <summary>
    /// ランチャーツールバーID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct LauncherToolbarId;

    /// <summary>
    /// フォントID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct FontId;

    /// <summary>
    /// ランチャーグループID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct LauncherGroupId;

    /// <summary>
    /// ノートID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct NoteId;
    /// <summary>
    /// ノートファイルID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct NoteFileId;
    /// <summary>
    /// キーアクションID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct KeyActionId;
    /// <summary>
    /// プラグインID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct PluginId;
    /// <summary>
    /// 通知ログID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct NotifyLogId;
    /// <summary>
    /// スケジュールジョブID。
    /// </summary>
    [GenerateGuidId()]
    public readonly partial record struct ScheduleJobId;
}
