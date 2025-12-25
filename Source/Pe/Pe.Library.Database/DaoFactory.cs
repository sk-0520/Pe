using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public class DaoFactory: IDaoFactory
    {
        public DaoFactory(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            Context = context;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
        }

        #region property

        protected IDatabaseContext Context { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILoggerFactory LoggerFactory { get; }

        #endregion

        #region function

        protected virtual ConstructorInfo GetDaoConstructorInfo(Type type)
        {
            //NOTE: 順序固定, DI コンテナ的なことするかどうかは使用状況次第で考慮する
            var constructor = type.GetConstructor([
                typeof(IDatabaseContext),
                typeof(IDatabaseStatementLoader),
                typeof(ILoggerFactory),
            ]);
            if(constructor is null) {
                throw new InvalidProgramException();
            }

            return constructor;
        }

        protected virtual DatabaseAccessObjectBase CreateDao(ConstructorInfo constructor)
        {
            var dao = constructor.Invoke([
                Context,
                StatementLoader,
                LoggerFactory
            ]);

            if(dao is DatabaseAccessObjectBase result) {
                return result;
            }

            throw new InvalidCastException();
        }

        #endregion

        #region IDaoFactory

        public DatabaseAccessObjectBase Create(Type type)
        {
            var constructor = GetDaoConstructorInfo(type);
            var dao = CreateDao(constructor);

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
