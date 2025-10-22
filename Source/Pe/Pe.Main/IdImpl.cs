using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Dapper;
using ContentTypeTextNet.Pe.Main.Models.Applications.Internal;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Id;
#else
// docfx 用ダミー
[AttributeUsage(AttributeTargets.Class)]
file class GenerateGuidIdTypeHandler: Attribute
{
    public GenerateGuidIdTypeHandler(Type type)
    { }
}
#endif

// 各 IdHandler の構築も自動化したいなぁ

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Internal
{
    /// <inheritdoc  cref="LauncherItemId" />
    [GenerateGuidIdTypeHandler(typeof(LauncherItemId))]
    internal partial class LauncherItemIdHandler;

    /// <inheritdoc  cref="CredentialId" />
    [GenerateGuidIdTypeHandler(typeof(CredentialId))]
    internal partial class CredentialIdHandler;

    /// <inheritdoc  cref="LauncherToolbarId" />
    [GenerateGuidIdTypeHandler(typeof(LauncherToolbarId))]
    internal partial class LauncherToolbarIdHandler;

    /// <inheritdoc  cref="FontId" />
    [GenerateGuidIdTypeHandler(typeof(FontId))]
    internal partial class FontIdHandler;

    /// <inheritdoc  cref="LauncherGroupId" />
    [GenerateGuidIdTypeHandler(typeof(LauncherGroupId))]
    internal partial class LauncherGroupIdHandler;

    /// <inheritdoc  cref="NoteId" />
    [GenerateGuidIdTypeHandler(typeof(NoteId))]
    internal partial class NoteIdHandler;

    /// <inheritdoc  cref="NoteFileId" />
    [GenerateGuidIdTypeHandler(typeof(NoteFileId))]
    internal partial class NoteFileIdHandler;

    /// <inheritdoc  cref="KeyActionId" />
    [GenerateGuidIdTypeHandler(typeof(KeyActionId))]
    internal partial class KeyActionIdHandler;

    /// <inheritdoc  cref="PluginId" />
    [GenerateGuidIdTypeHandler(typeof(PluginId))]
    internal partial class PluginIdHandler;

    /// <inheritdoc  cref="NotifyLogId" />
    [GenerateGuidIdTypeHandler(typeof(NotifyLogId))]
    internal partial class NotifyLogIdHandler;

    /// <inheritdoc  cref="ScheduleJobId" />
    [GenerateGuidIdTypeHandler(typeof(ScheduleJobId))]
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
