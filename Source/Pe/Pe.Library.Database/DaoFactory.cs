using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public class DaoFactory
    {
        // コンストラクターでプロパティに値を設定
        public DaoFactory(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            Context = context;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
        }

        #region property

        private IDatabaseContext Context { get; }
        private IDatabaseStatementLoader StatementLoader { get; }
        private ILoggerFactory LoggerFactory { get; }

        #endregion

        #region function

        public DatabaseAccessObjectBase Create(Type type)
        {
            var constructor = type.GetConstructor([
                typeof(IDatabaseContext),
                typeof(IDatabaseStatementLoader),
                typeof(ILoggerFactory)
            ]);
            if(constructor is null) {
                throw new InvalidOperationException();
            }

            var dao = constructor.Invoke([Context, StatementLoader, LoggerFactory]) as DatabaseAccessObjectBase;
            if(dao is null) {
                throw new InvalidCastException();
            }

            return dao;
        }

        public TDao Create<TDao>()
            where TDao : DatabaseAccessObjectBase
        {
            var type = typeof(TDao);
            return (TDao)Create(type);
        }

        #endregion
    }

}
