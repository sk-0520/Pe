using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherFilesEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherFilesEntityPathDto: DtoBase
        {
            #region property

            Guid LauncherItemId { get; set; }
            public string File { get; set; } = string.Empty;
            public string Option { get; set; } = string.Empty;
            public string WorkDirectory { get; set; } = string.Empty;


            #endregion
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "<保留中>")]
        private sealed class LauncherFilesEntityDto: CommonDtoBase
        {
            #region property

            Guid LauncherItemId { get; set; }

            public string File { get; set; } = string.Empty;
            public string Option { get; set; } = string.Empty;
            public string WorkDirectory { get; set; } = string.Empty;
            public string ShowMode { get; set; } = string.Empty;
            public bool IsEnabledCustomEnvVar { get; set; }
            public bool IsEnabledStandardIo { get; set; }
            public string StandardIoEncoding { get; set; } = string.Empty;
            public bool RunAdministrator { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string File { get; } = "File";
            public static string Option { get; } = "Option";
            public static string WorkDirectory { get; } = "WorkDirectory";
            public static string ShowMode { get; } = "ShowMode";

            public static string IsEnabledCustomEnvVar { get; } = "IsEnabledCustomEnvVar";
            public static string IsEnabledStandardIo { get; } = "IsEnabledStandardIo";
            public static string StandardIoEncoding { get; } = "StandardIoEncoding";
            public static string RunAdministrator { get; } = "RunAdministrator";

            #endregion
        }

        #endregion

        public LauncherFilesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
         : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherExecutePathData ConvertFromDto(LauncherFilesEntityPathDto dto)
        {
            var data = new LauncherExecutePathData() {
                Path = dto.File ?? string.Empty,
                Option = dto.Option ?? string.Empty,
                WorkDirectoryPath = dto.WorkDirectory ?? string.Empty,
            };

            return data;
        }

        private LauncherFileData ConvertFromDto(LauncherFilesEntityDto dto)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);
            var showModeEnumTransfer = new EnumTransfer<ShowMode>();

            var data = new LauncherFileData() {
                Path = dto.File,
                Option = dto.Option,
                WorkDirectoryPath = dto.WorkDirectory,
                ShowMode = showModeEnumTransfer.ToEnum(dto.ShowMode),
                IsEnabledCustomEnvironmentVariable = dto.IsEnabledCustomEnvVar,
                IsEnabledStandardInputOutput = dto.IsEnabledStandardIo,
                StandardInputOutputEncoding = encodingConverter.Parse(dto.StandardIoEncoding),
                RunAdministrator = dto.RunAdministrator,
            };

            return data;
        }

        public LauncherExecutePathData SelectPath(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QuerySingle<LauncherFilesEntityPathDto>(statement, parameter);
            var data = ConvertFromDto(dto);
            return data;
        }

        public LauncherFileData SelectFile(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QueryFirst<LauncherFilesEntityDto>(statement, parameter);
            var data = ConvertFromDto(dto);
            return data;

        }

        private IEnumerable<LauncherItemId> SelectSearchFileFromParameters(Dictionary<string, object?> parameters)
        {
            var statement = LoadStatement();
            var blocks = new Dictionary<string, string>() {
                ["OPTION"] = parameters.ContainsKey(Column.Option) ? "ON": string.Empty,
            };
            var processedStatement = ProcessStatement(statement, blocks);

            return Context.SelectOrdered<LauncherItemId>(processedStatement, parameters);
        }

        /// <summary>
        /// ファイルパスから該当の <see cref="LauncherItemId"/> 一覧を取得する。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>大文字小文字は区別しない</item>
        ///     <item>'/' は `\` として扱われる</item>
        ///     <item>末尾 `\` は無視される</item>
        /// </list>
        /// <para>SQL側であれこれしてるので本アプリにおいては比較的重めの処理かも。</para>
        /// </remarks>
        /// <param name="path">ファイルパス。</param>
        /// <returns>該当するランチャーアイテムID一覧。</returns>
        public IEnumerable<LauncherItemId> SelectSearchFileFromPath(string path)
        {
            var param = new Dictionary<string, object?>() {
                [Column.File] = path,
            };

            return SelectSearchFileFromParameters(param);
        }

        /// <summary>
        /// ファイルパスとオプションから該当の <see cref="LauncherItemId"/> 一覧を取得する。
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="SelectSearchFileFromPath(string)"/>
        /// <para>オプションは通常文字列として比較される。</para>
        /// </remarks>
        /// <param name="path"><inheritdoc cref="SelectSearchFileFromPath(string)"/></param>
        /// <param name="option">オプション。</param>
        /// <returns><inheritdoc cref="SelectSearchFileFromPath(string)"/></returns>
        public IEnumerable<LauncherItemId> SelectSearchFileFromPathAndOption(string path, string option)
        {
            var param = new Dictionary<string, object?>() {
                [Column.File] = path,
                [Column.Option] = option,
            };

            return SelectSearchFileFromParameters(param);
        }

        public void InsertFile(LauncherItemId launcherItemId, LauncherExecutePathData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;
            param[Column.File] = data.Path;
            param[Column.Option] = data.Option;
            param[Column.WorkDirectory] = data.WorkDirectoryPath;
            param[Column.StandardIoEncoding] = EncodingUtility.ToString(EncodingConverter.DefaultStandardInputOutputEncoding);

            Context.InsertSingle(statement, param);
        }

        public void UpdateCustomizeLauncherFile(LauncherItemId launcherItemId, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IDatabaseCommonStatus commonStatus)
        {
            var showModeEnumTransfer = new EnumTransfer<ShowMode>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.File] = pathParameter.Path;
            parameter[Column.Option] = pathParameter.Option;
            parameter[Column.WorkDirectory] = pathParameter.WorkDirectoryPath;
            parameter[Column.ShowMode] = showModeEnumTransfer.ToString(customParameter.ShowMode);
            parameter[Column.IsEnabledCustomEnvVar] = customParameter.IsEnabledCustomEnvironmentVariable;
            parameter[Column.IsEnabledStandardIo] = customParameter.IsEnabledStandardInputOutput;
            parameter[Column.StandardIoEncoding] = EncodingUtility.ToString(customParameter.StandardInputOutputEncoding);
            parameter[Column.RunAdministrator] = customParameter.RunAdministrator;
            parameter[Column.LauncherItemId] = launcherItemId;

            Context.UpdateByKey(statement, parameter);
        }

        public void DeleteFileByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
