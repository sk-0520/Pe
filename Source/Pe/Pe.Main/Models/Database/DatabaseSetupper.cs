using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Setupper;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Library.Database.Implementations;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    public class DatabaseSetupper
    {
        public DatabaseSetupper(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property
        private IIdFactory IdFactory { get; }
        private IDatabaseStatementLoader StatementLoader { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        private IDatabaseCommonStatus CommonStatus { get; } = DatabaseCommonStatus.CreateCurrentAccount();

        #endregion

        #region function

        private SetupDto CreateSetupDto(Version lastVersion)
        {
            var result = new SetupDto() {
                LastVersion = lastVersion,
                ExecuteVersion = Assembly.GetExecutingAssembly().GetName().Version,
            };
            CommonStatus.WriteCommonTo(result);

            return result;
        }

        private void ExecuteCore(IDatabaseAccessor accessor, IReadOnlySetupDto dto, Action<IDatabaseContext, IReadOnlySetupDto> ddl, Action<IDatabaseContext, IReadOnlySetupDto> dml)
        {
            Logger.LogDebug("DDL");
            ddl(accessor, dto);

            Logger.LogDebug("DML");
            using(var transaction = accessor.BeginTransaction()) {
                dml(transaction, dto);
                transaction.Commit();
            }
        }

        private void Execute(IDatabaseAccessorPack accessorPack, IReadOnlySetupDto dto, SetupperBase setupper)
        {
            Logger.LogInformation("セットアップ処理: バージョン{0}, {1}", setupper.Version, setupper.GetType().Name);
            var start = DateTime.UtcNow;

            ExecuteCore(accessorPack.Main, dto, setupper.ExecuteMainDDL, setupper.ExecuteMainDML);
            ExecuteCore(accessorPack.Large, dto, setupper.ExecuteFileDDL, setupper.ExecuteFileDML);
            ExecuteCore(accessorPack.Temporary, dto, setupper.ExecuteTemporaryDDL, setupper.ExecuteTemporaryDML);

            var end = DateTime.UtcNow;
            Logger.LogInformation("対象バージョンセットアップ完了: {0}, {1}", setupper.Version, end - start);
        }

        /// <summary>
        /// データベース初期化。
        /// </summary>
        /// <param name="accessorPack">DBアクセス処理群。</param>
        public void Initialize(IDatabaseAccessorPack accessorPack)
        {
            Logger.LogInformation("初期化処理実行");

            var dto = CreateSetupDto(new Version(0, 0, 0, 0));
            var setup = new Setupper_V_00_84_000(IdFactory, StatementLoader, LoggerFactory);

            Execute(accessorPack, dto, setup);
        }

        /// <summary>
        /// データベースマイグレーション。
        /// </summary>
        /// <param name="accessorPack">DBアクセス処理群。</param>
        /// <param name="lastVersion">最終使用バージョン。</param>
        public void Migrate(IDatabaseAccessorPack accessorPack, Version lastVersion)
        {
            Logger.LogInformation("マイグレーション処理実行");

            var dto = CreateSetupDto(lastVersion);

            var setupperTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(a => a.IsClass && a.Namespace == "ContentTypeTextNet.Pe.Main.Models.Database.Setupper")
                .Where(a => a.IsSubclassOf(typeof(SetupperBase)))
                .Where(a => a.GetCustomAttribute<ObsoleteAttribute>() is null)
                .Where(a => a.GetCustomAttribute<DatabaseNoSetupAttribute>() is null)
                .Select(a => (setupperType: a, version: a.GetCustomAttribute<DatabaseSetupVersionAttribute>()))
                .Where(a => a.version is not null)
                .Select(a => (a.setupperType, version: a.version!.Version))
                .OrderBy(a => a.version)
                .ToArray()
            ;

            foreach(var (setupperType, version) in setupperTypes) {
                if(lastVersion < version) {
                    Logger.LogInformation("マイグレーション対象: {0} < {1}", lastVersion, version);
                    var setupper = (SetupperBase?)Activator.CreateInstance(setupperType, new object[] { IdFactory, StatementLoader, LoggerFactory });
                    Debug.Assert(setupper is not null);
                    Execute(accessorPack, dto, setupper);
                }
            }
        }

        /// <summary>
        /// 最終処理。
        /// </summary>
        /// <param name="accessorPack"></param>
        public void Adjust(IDatabaseAccessorPack accessorPack, Version lastVersion)
        {
            Logger.LogInformation("マイグレーション最終処理実行");

            var setupper = new Setupper_V_99_99_999(IdFactory, StatementLoader, LoggerFactory);
            Execute(accessorPack, CreateSetupDto(lastVersion), setupper);
        }

        private bool ExistsExecuteTable(IDatabaseAccessor mainAccessor)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return mainAccessor.Query<bool>(statement, null, false).FirstOrDefault();
        }

        public Version? GetLastVersion(IDatabaseAccessor mainAccessor)
        {
            if(!ExistsExecuteTable(mainAccessor)) {
                Logger.LogWarning("not found: version table");
                return null;
            }

            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return mainAccessor.Query<Version>(statement, null, false).FirstOrDefault();
        }


        public void Cleanup(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            var management = implementation.CreateManagement(context);
            management.Refresh();
        }

        public void CheckForeignKey(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var table = context.GetDataTable(statement);
            if(table.Rows.Count == 0) {
                return;
            }

            // データ不整合, さようなら！
            var errors = new StringBuilder();
            errors.AppendJoin(", ", table.Columns.Cast<DataColumn>().Select(i => i.ColumnName));
            errors.AppendLine();
            foreach(var row in table.AsEnumerable()) {
                errors.AppendJoin(", ", row.ItemArray);
                errors.AppendLine();
            }
            var error = errors.ToString();
            Logger.LogError("{0}", error);

            throw new Exception("CheckForeignKey") {
                Source = error
            };

        }

        #endregion
    }
}
