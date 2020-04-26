using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class LauncherFilesEntityPathDto : DtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }
        public string File { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectory { get; set; } = string.Empty;


        #endregion
    }

    internal class LauncherFilesEntityDto : CommonDtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }

        public string File { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectory { get; set; } = string.Empty;

        public bool IsEnabledCustomEnvVar { get; set; }
        public bool IsEnabledStandardIo { get; set; }
        public string StandardIoEncoding { get; set; } = string.Empty;
        public bool RunAdministrator { get; set; }

        #endregion
    }

    public class LauncherFilesEntityDao : EntityDaoBase
    {
        public LauncherFilesEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
         : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string File { get; } = "File";
            public static string Option { get; } = "Option";
            public static string WorkDirectory { get; } = "WorkDirectory";

            public static string IsEnabledCustomEnvVar { get; } = "IsEnabledCustomEnvVar";
            public static string IsEnabledStandardIo { get; } = "IsEnabledStandardIo";
            public static string StandardIoEncoding { get; } = "StandardIoEncoding";
            public static string RunAdministrator { get; } = "RunAdministrator";

            #endregion
        }

        #endregion

        #region function

        LauncherExecutePathData ConvertFromDto(LauncherFilesEntityPathDto dto)
        {
            var data = new LauncherExecutePathData() {
                Path = dto.File ?? string.Empty,
                Option = dto.Option ?? string.Empty,
                WorkDirectoryPath = dto.WorkDirectory ?? string.Empty,
            };

            return data;
        }

        LauncherFileData ConvertFromDto(LauncherFilesEntityDto dto)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var data = new LauncherFileData() {
                Path = dto.File,
                Option = dto.Option,
                WorkDirectoryPath = dto.WorkDirectory,
                IsEnabledCustomEnvironmentVariable = dto.IsEnabledCustomEnvVar,
                IsEnabledStandardInputOutput = dto.IsEnabledStandardIo,
                StandardInputOutputEncoding = encodingConverter.Parse(dto.StandardIoEncoding),
                RunAdministrator = dto.RunAdministrator,
            };

            return data;
        }

        public LauncherExecutePathData SelectPath(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QuerySingle<LauncherFilesEntityPathDto>(statement, parameter);
            var data = ConvertFromDto(dto);
            return data;
        }

        public LauncherFileData SelectFile(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QueryFirst<LauncherFilesEntityDto>(statement, parameter);
            var data = ConvertFromDto(dto);
            return data;

        }

        public bool InsertFile(Guid launcherItemId, LauncherExecutePathData data, IDatabaseCommonStatus commonStatus)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var statement = LoadStatement();
            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;
            param[Column.File] = data.Path;
            param[Column.Option] = data.Option;
            param[Column.WorkDirectory] = data.WorkDirectoryPath;
            param[Column.StandardIoEncoding] = encodingConverter.ToString(EncodingConverter.DefaultStandardInputOutputEncoding);

            return Commander.Execute(statement, param) == 1;
        }

        [Obsolete]
        public bool InsertFileWithCustom(Guid launcherItemId, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IDatabaseCommonStatus commonStatus)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var statement = LoadStatement();
            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;
            param[Column.File] = pathParameter.Path;
            param[Column.Option] = pathParameter.Option;
            param[Column.WorkDirectory] = pathParameter.WorkDirectoryPath;
            param[Column.IsEnabledCustomEnvVar] = customParameter.IsEnabledCustomEnvironmentVariable;
            param[Column.IsEnabledStandardIo] = customParameter.IsEnabledStandardInputOutput;
            param[Column.StandardIoEncoding] = encodingConverter.ToString(customParameter.StandardInputOutputEncoding);
            param[Column.RunAdministrator] = customParameter.RunAdministrator;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateCustomizeLauncherFile(Guid launcherItemId, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IDatabaseCommonStatus commonStatus)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.File] = pathParameter.Path;
            parameter[Column.Option] = pathParameter.Option;
            parameter[Column.WorkDirectory] = pathParameter.WorkDirectoryPath;
            parameter[Column.IsEnabledCustomEnvVar] = customParameter.IsEnabledCustomEnvironmentVariable;
            parameter[Column.IsEnabledStandardIo] = customParameter.IsEnabledStandardInputOutput;
            parameter[Column.StandardIoEncoding] = encodingConverter.ToString(customParameter.StandardInputOutputEncoding);
            parameter[Column.RunAdministrator] = customParameter.RunAdministrator;
            parameter[Column.LauncherItemId] = launcherItemId;

            return Commander.Execute(statement, parameter) == 1;
        }

        public bool DeleteFileByLauncherItemId(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
