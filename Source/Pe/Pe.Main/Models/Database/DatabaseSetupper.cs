using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Setupper;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

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
        IIdFactory IdFactory { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }
        IDatabaseCommonStatus CommonStatus { get; } = DatabaseCommonStatus.CreateCurrentAccount();

        #endregion

        #region function

        SetupDto CreateSetupDto(Version lastVersion)
        {
            var result = new SetupDto() {
                LastVersion = lastVersion,
                ExecuteVersion = Assembly.GetExecutingAssembly().GetName().Version,
            };
            CommonStatus.WriteCommonTo(result);

            return result;
        }

        void ExecuteCore(IDatabaseAccessor accessor, IReadOnlySetupDto dto, Action<IDatabaseContext, IReadOnlySetupDto> ddl, Action<IDatabaseContext, IReadOnlySetupDto> dml)
        {
            if(accessor.DatabaseFactory.CreateImplementation().SupportedTransactionDDL) {
                var result = accessor.Batch(context => {
                    Logger.LogDebug("DDL");
                    ddl(context, dto);

                    Logger.LogDebug("DML");
                    dml(context, dto);

                    return true;
                });
                if(!result.Success) {
                    // この時点で FailureValue は例外が入っている
                    throw result.FailureValue!;
                }
            } else {
                Logger.LogDebug("DDL");
                ddl(accessor, dto);

                Logger.LogDebug("DML");
                using(var tran = accessor.BeginTransaction()) {
                    dml(tran, dto);
                }
            }
        }

        void Execute(IDatabaseAccessorPack accessorPack, IReadOnlySetupDto dto, SetupperBase setupper)
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

            var setuppers = new SetupperBase[] {
                new Setupper_V_00_94_000(IdFactory, StatementLoader, LoggerFactory),
                new Setupper_V_00_98_000(IdFactory, StatementLoader, LoggerFactory),
                new Setupper_V_00_99_000(IdFactory, StatementLoader, LoggerFactory),
                new Setupper_V_00_99_010(IdFactory, StatementLoader, LoggerFactory),
                new Setupper_V_00_99_038(IdFactory, StatementLoader, LoggerFactory),
                new Setupper_V_00_99_063(IdFactory, StatementLoader, LoggerFactory),
                new Setupper_V_00_99_147(IdFactory, StatementLoader, LoggerFactory),
            };

            foreach(var setupper in setuppers) {
                if(lastVersion < setupper.Version) {
                    Logger.LogInformation("マイグレーション対象: {0} < {1}", lastVersion, setupper.Version);
                    Execute(accessorPack, dto, setupper);
                }
            }
        }

        /// <summary>
        /// 最終処理。
        /// </summary>
        /// <param name="accessorPack"></param>
        public void Tune(IDatabaseAccessorPack accessorPack, Version lastVersion)
        {
            Logger.LogInformation("マイグレーション最終処理実行");

            var setupper = new Setupper_V_99_99_999(IdFactory, StatementLoader, LoggerFactory);
            Execute(accessorPack, CreateSetupDto(lastVersion), setupper);
        }

        bool ExistsExecuteTable(IDatabaseAccessor mainAccessor)
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


        private void Vacuum(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            context.Execute(statement);
        }

        private void Reindex(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            context.Execute(statement);
        }

        private void Analyze(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            context.Execute(statement);
        }

        public void Tune(IDatabaseContext context)
        {
            Vacuum(context);
            Reindex(context);
            Analyze(context);
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
