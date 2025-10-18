using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Id;
#endif

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// ランチャーアイテムID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct LauncherItemId;

    /// <summary>
    /// 資格情報ID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct CredentialId;

    /// <summary>
    /// ランチャーツールバーID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct LauncherToolbarId;

    /// <summary>
    /// フォントID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct FontId;

    /// <summary>
    /// ランチャーグループID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct LauncherGroupId;

    /// <summary>
    /// ノートID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct NoteId;
    /// <summary>
    /// ノートファイルID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct NoteFileId;
    /// <summary>
    /// キーアクションID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct KeyActionId;
    /// <summary>
    /// プラグインID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct PluginId;
    /// <summary>
    /// 通知ログID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct NotifyLogId;
    /// <summary>
    /// スケジュールジョブID。
    /// </summary>
#if !DOC_FX
    [GenerateGuidId()]
#endif
    public readonly partial record struct ScheduleJobId;
}
