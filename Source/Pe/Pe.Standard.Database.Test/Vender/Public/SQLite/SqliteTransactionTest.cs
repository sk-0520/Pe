using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Standard.Database.Test.TestImpl.Vender.Public.SQLite;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Database.Test.Vender.Public.SQLite
{
    [TestClass]
    public class SqliteTransactionTest
    {
        #region function

        private IDatabaseAccessor InitializeDatabase()
        {
            var factory = new TestSqliteFactory();
            var databaseAccessor = new SqliteAccessor(factory, NullLoggerFactory.Instance);

            var sqls = new[] {
@"
create table TestTable1 (
    ColKey integer,
    ColVal text
)
",
@"
insert into
    TestTable1(ColKey, ColVal)
values
    (1, 'A'),
    (2, 'B'),
    (3, 'C'),
    (4, 'D')
"
                };

            using var c = databaseAccessor.BeginTransaction();
            foreach(var sql in sqls) {
                c.Execute(sql);
            }
            c.Commit();

            return databaseAccessor;
        }

        private void InsertDatabase(IDatabaseWriter writer, int key, string value)
        {
            var parameter = new {
                Key = key,
                Value = value,
            };

            writer.InsertSingle(@"
                insert into
                    TestTable1(ColKey, ColVal)
                values
                    (@Key, @Value)
            ", parameter);
        }

        [TestMethod]
        public void UsingNoCommitTest()
        {
            var databaseAccessor = InitializeDatabase();

            var acutual1 = databaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = @Key", new { Key = 0 });
            Assert.IsNull(acutual1);

            using(var t = databaseAccessor.BeginTransaction()) {
                InsertDatabase(t, 0, "!");

                var acutual2 = t.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = @Key", new { Key = 0 });
                Assert.AreEqual(acutual2, "!");
            }

            var acutual3 = databaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = @Key", new { Key = 0 });
            Assert.IsNull(acutual3);
        }

        [TestMethod]
        public void UsingCommitTest()
        {
            var databaseAccessor = InitializeDatabase();

            var acutual1 = databaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = @Key", new { Key = 0 });
            Assert.IsNull(acutual1);

            using(var t = databaseAccessor.BeginTransaction()) {
                InsertDatabase(t, 0, "!");

                var acutual2 = t.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = @Key", new { Key = 0 });
                Assert.AreEqual(acutual2, "!");

                t.Commit();
            }

            var acutual3 = databaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = @Key", new { Key = 0 });
            Assert.AreEqual(acutual3, "!");
        }

        [TestMethod]
        public void NestedTransactionTest()
        {
            var databaseAccessor = InitializeDatabase();

            using(var t = databaseAccessor.BeginTransaction()) {
                Assert.ThrowsException<NotSupportedException>(() => databaseAccessor.BeginTransaction());
                Assert.ThrowsException<NotSupportedException>(() => databaseAccessor.BeginReadOnlyTransaction());
            }

            using(var t = databaseAccessor.BeginReadOnlyTransaction()) {
                Assert.ThrowsException<NotSupportedException>(() => databaseAccessor.BeginTransaction());
                Assert.ThrowsException<NotSupportedException>(() => databaseAccessor.BeginReadOnlyTransaction());
            }

        }

        #endregion
    }
}
