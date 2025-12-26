using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test
{
    public class DaoFactoryTest
    {
        #region define

        private sealed class TestDao: DatabaseAccessObjectBase
        {
            public TestDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
                : base(context, statementLoader, loggerFactory)
            { }
        }

        private sealed class InvalidConstructorDao: DatabaseAccessObjectBase
        {
            public InvalidConstructorDao(ILoggerFactory loggerFactory, IDatabaseContext context, IDatabaseStatementLoader statementLoader)
                : base(context, statementLoader, loggerFactory)
            { }
        }

        private sealed class InvalidCastDao
        {
            public InvalidCastDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            { }
        }

        #endregion

        public DaoFactoryTest()
        {
            var dcMock = new Mock<IDatabaseContext>();
            var dslMock = new Mock<IDatabaseStatementLoader>();

            DaoFactory = new DaoFactory(
                dcMock.Object,
                dslMock.Object,
                NullLoggerFactory.Instance
            );
        }

        #region property

        private DaoFactory DaoFactory { get; }

        #endregion

        #region function

        [Fact]
        public void Create_Not_Dao_Test()
        {
            var exception = Assert.Throws<DatabaseFactoryException>(() => DaoFactory.Create(typeof(object)));
            Assert.Equal("constructor", exception.Message);
        }

        [Fact]
        public void Create_InvalidConstructor_Dao_Test()
        {
            var exception = Assert.Throws<DatabaseFactoryException>(() => DaoFactory.Create(typeof(InvalidConstructorDao)));
            Assert.Equal("constructor", exception.Message);
        }

        [Fact]
        public void Create_InvalidType_Dao_Test()
        {
            var exception = Assert.Throws<DatabaseFactoryException>(() => DaoFactory.Create(typeof(InvalidCastDao)));
            Assert.Equal($"dao is not DatabaseAccessObjectBase: {typeof(InvalidCastDao)}", exception.Message);
        }

        [Fact]
        public void DaoFactoryExtensions_CreateTest()
        {
            var exception = Record.Exception(() => DaoFactory.Create<TestDao>());
            Assert.Null(exception);
        }

        #endregion
    }
}
