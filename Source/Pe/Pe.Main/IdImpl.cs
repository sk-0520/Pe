using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Id;
#endif
using Dapper;
using ContentTypeTextNet.Pe.Main.Models.Applications.Internal;

// 各 IdHandler の構築も自動化したいなぁ

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Internal
{
    /// <inheritdoc  cref="LauncherItemId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(LauncherItemId))]
#endif
    internal partial class LauncherItemIdHandler;

    /// <inheritdoc  cref="CredentialId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(CredentialId))]
#endif
    internal partial class CredentialIdHandler;

    /// <inheritdoc  cref="LauncherToolbarId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(LauncherToolbarId))]
#endif
    internal partial class LauncherToolbarIdHandler;

    /// <inheritdoc  cref="FontId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(FontId))]
#endif
    internal partial class FontIdHandler;

    /// <inheritdoc  cref="LauncherGroupId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(LauncherGroupId))]
#endif
    internal partial class LauncherGroupIdHandler;

    /// <inheritdoc  cref="NoteId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(NoteId))]
#endif
    internal partial class NoteIdHandler;

    /// <inheritdoc  cref="NoteFileId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(NoteFileId))]
#endif
    internal partial class NoteFileIdHandler;

    /// <inheritdoc  cref="KeyActionId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(KeyActionId))]
#endif
    internal partial class KeyActionIdHandler;

    /// <inheritdoc  cref="PluginId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(PluginId))]
#endif
    internal partial class PluginIdHandler;

    /// <inheritdoc  cref="NotifyLogId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(NotifyLogId))]
#endif
    internal partial class NotifyLogIdHandler;

    /// <inheritdoc  cref="ScheduleJobId" />
#if !DOC_FX
    [GenerateGuidIdTypeHandler(typeof(ScheduleJobId))]
#endif
    internal partial class ScheduleJobIdHandler;
}

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    partial class ApplicationDatabaseImplementation
    {
        static ApplicationDatabaseImplementation()
        {
            SqlMapper.AddTypeHandler(typeof(LauncherItemId), new LauncherItemIdHandler());
            SqlMapper.AddTypeHandler(typeof(CredentialId), new CredentialIdHandler());
            SqlMapper.AddTypeHandler(typeof(LauncherToolbarId), new LauncherToolbarIdHandler());
            SqlMapper.AddTypeHandler(typeof(FontId), new FontIdHandler());
            SqlMapper.AddTypeHandler(typeof(LauncherGroupId), new LauncherGroupIdHandler());
            SqlMapper.AddTypeHandler(typeof(NoteId), new NoteIdHandler());
            SqlMapper.AddTypeHandler(typeof(NoteFileId), new NoteFileIdHandler());
            SqlMapper.AddTypeHandler(typeof(KeyActionId), new KeyActionIdHandler());
            SqlMapper.AddTypeHandler(typeof(PluginId), new PluginIdHandler());
            SqlMapper.AddTypeHandler(typeof(NotifyLogId), new NotifyLogIdHandler());
            SqlMapper.AddTypeHandler(typeof(ScheduleJobId), new ScheduleJobIdHandler());
        }
    }
}
