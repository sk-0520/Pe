#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Id;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Struct)]
file sealed class GenerateGuidIdAttribute: System.Attribute
{
    public GenerateGuidIdAttribute()
    { }
}
#endif


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
