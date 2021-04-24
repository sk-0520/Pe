using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao
{
    [TestClass]
    public class ApplicationDatabaseObjectBaseTest
    {
        #region define

        class Dc: IDatabaseContext
        {
            public int Execute(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public DataTable GetDataTable(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
            {
                throw new NotImplementedException();
            }

            public T QueryFirst<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public T QuerySingle<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

        }

        class Dsl: IDatabaseStatementLoader
        {
            public string LoadStatement(string key)
            {
                throw new NotImplementedException();
            }

            public string LoadStatementByCurrent(Type caller, [CallerMemberName] string callerMemberName = "")
            {
                throw new NotImplementedException();
            }
        }

        class Di: IDatabaseImplementation
        {
            public bool SupportedTransactionDDL => throw new NotSupportedException();

            public bool SupportedTransactionDML => throw new NotSupportedException();

            public bool SupportedSingleLineComment => throw new NotSupportedException();
            public bool SupportedMultiLineComment => throw new NotSupportedException();


            public string PreFormatStatement(string statement)
            {
                throw new NotSupportedException();
            }

            public string ToStatementColumnName(string columnName)
            {
                throw new NotSupportedException();
            }

            public string ToStatementParameterName(string parameterName, int index)
            {
                throw new NotSupportedException();
            }

            public string ToStatementTableName(string tableName)
            {
                throw new NotSupportedException();
            }

            public string ToSingleLineComment(string statement) => throw new NotSupportedException();
            public string ToMultiLineComment(string statement) => throw new NotSupportedException();


            public string Escape(string input)
            {
                throw new NotSupportedException();
            }

            public string EscapeLike(string pattern) => throw new NotSupportedException();
        }

        class Adao: ApplicationDatabaseObjectBase
        {
            public Adao()
                : base(new Dc(), new Dsl(), new Di(), Test.LoggerFactory)
            { }

            public int ToInt_Public(long v) => base.ToInt(v);
        }

        #endregion

        #region function

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(int.MaxValue, int.MaxValue)]
        [DataRow(int.MaxValue, (long)int.MaxValue + 1)]
        [DataRow(int.MinValue, (long)int.MinValue - 1)]
        [DataRow(int.MinValue, int.MinValue)]
        public void ToIntTest(int expected, long value)
        {
            var adao = new Adao();
            var actual = adao.ToInt_Public(value);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
