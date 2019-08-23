using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
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
            public static string RunAdministrator { get; } = "RunAdministrator";

            #endregion
        }

        #endregion

        #region function

        LauncherExecutePathData ConvertFromDto(LauncherFilesEntityPathDto dto)
        {
            var data = new LauncherExecutePathData() {
                Path = dto.File,
                Option = dto.Option,
                WorkDirectoryPath = dto.WorkDirectory,
            };

            return data;
        }

        LauncherFileData ConvertFromDto(LauncherFilesEntityDto dto)
        {
            var data = new LauncherFileData() {
                Path = dto.File,
                Option = dto.Option,
                WorkDirectoryPath = dto.WorkDirectory,
                IsEnabledCustomEnvironmentVariable = dto.IsEnabledCustomEnvVar,
                IsEnabledStandardInputOutput = dto.IsEnabledStandardIo,
                RunAdministrator = dto.RunAdministrator,
            };

            return data;
        }

        public LauncherExecutePathData SelectPath(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.File);
            builder.AddSelect(Column.Option);
            builder.AddSelect(Column.WorkDirectory);

            builder.AddValue(Column.LauncherItemId, launcherItemId);

            var dto = SelectSingle<LauncherFilesEntityPathDto>(builder);
            var data = ConvertFromDto(dto);
            return data;
        }

        public LauncherFileData SelectFile(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.File);
            builder.AddSelect(Column.Option);
            builder.AddSelect(Column.WorkDirectory);
            builder.AddSelect(Column.IsEnabledCustomEnvVar);
            builder.AddSelect(Column.IsEnabledStandardIo);
            builder.AddSelect(Column.RunAdministrator);

            builder.AddValue(Column.LauncherItemId, launcherItemId);

            var dto = SelectFirst<LauncherFilesEntityDto>(builder);
            var data = ConvertFromDto(dto);
            return data;
        }

        public bool InsertSimple(Guid launcherItemId, LauncherExecutePathData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;
            param[Column.File] = data.Path;
            param[Column.Option] = data.Option;
            param[Column.WorkDirectory] = data.WorkDirectoryPath;

            return Commander.Execute(statement, param) == 1;
        }

        #endregion
    }
}
