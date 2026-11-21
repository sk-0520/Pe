using System;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Implementations
{
    public class DatabaseManagementTest
    {
        #region function

        [Fact]
        public void GetDatabaseItemsTest()
        {
            var test = new DatabaseManagement();
            Assert.Throws<NotImplementedException>(() => test.GetDatabaseItems());
        }

        [Fact]
        public void GetSchemaItemsTest()
        {
            var test = new DatabaseManagement();
            Assert.Throws<NotImplementedException>(() => test.GetSchemaItems(new DatabaseInformationItem(string.Empty)));
        }

        [Fact]
        public void GetResourceItemsTest()
        {
            var test = new DatabaseManagement();
            Assert.Throws<NotImplementedException>(() => test.GetResourceItems(new DatabaseSchemaItem(new DatabaseInformationItem(string.Empty), string.Empty, true), DatabaseResourceKinds.Table));
        }

        [Fact]
        public void GetColumnsTest()
        {
            var test = new DatabaseManagement();
            Assert.Throws<NotImplementedException>(() => test.GetColumns(new DatabaseResourceItem(new DatabaseSchemaItem(new DatabaseInformationItem(string.Empty), string.Empty, true), string.Empty, DatabaseResourceKinds.All, string.Empty)));
        }

        [Fact]
        public void RefreshTest()
        {
            var test = new DatabaseManagement();
            Assert.Throws<NotImplementedException>(() => test.Refresh());
        }

        #endregion
    }
}
