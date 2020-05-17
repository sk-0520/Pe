using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test
{
    class TestDatabase
    {
        #region function

        public void Initialize(IDiRegisterContainer diContainer)
        {
            var factoryPack = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var sqlRootDirPath = Path.Combine(Path.GetDirectoryName(testAssemblyPath)!, "etc", "sql", "ContentTypeTextNet.Pe.Main");

            diContainer
                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(new DirectoryInfo(sqlRootDirPath), TimeSpan.FromMinutes(6), null, false, diContainer.Build<LoggerFactory>()))
                .RegisterDatabase(factoryPack, lazyWriterWaitTimePack, diContainer.Build<ILoggerFactory>())
            ;

            var databaseAccessorPack = diContainer.Build<IDatabaseAccessorPack>();

            var databaseSetupper = diContainer.Build<DatabaseSetupper>();
            databaseSetupper.Initialize(databaseAccessorPack);
            var initVersion = databaseSetupper.GetLastVersion(databaseAccessorPack.Main);
            foreach(var accessor in databaseAccessorPack.Items) {
                accessor.Execute("pragma foreign_keys = false");
            }
            databaseSetupper.Migrating(databaseAccessorPack, initVersion!);
            foreach(var accessor in databaseAccessorPack.Items) {
                databaseSetupper.CheckForeignKey(accessor);
                accessor.Execute("pragma foreign_keys = true");
            }
            var lastVersion = databaseSetupper.GetLastVersion(databaseAccessorPack.Main);
            // 3桁バージョンってこうなってんのかよ
            var vers = new[] {
                new { Now = BuildStatus.Version.Major, Init = lastVersion!.Major},
                new { Now = BuildStatus.Version.Minor, Init = lastVersion!.Minor},
                new { Now = BuildStatus.Version.Build, Init = lastVersion!.Build},
            };
            Assert.IsTrue(vers.All(i => i.Now == i.Init));
        }


        #endregion
    }
}
