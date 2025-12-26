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

            ConstructorParameters = [
                Context,
                StatementLoader,
                LoggerFactory
            ];
        }

        #region property

        /*
         * 型と値をキャッシュするとちょっとだけ改善されるんだ。意味はたぶんない。
            | Method       | Mean     | Error   | StdDev  | Min      | Max      | Rank | Gen0   | Allocated |
            |------------- |---------:|--------:|--------:|---------:|---------:|-----:|-------:|----------:|
            | Test1_Create | 259.7 ns | 1.70 ns | 1.50 ns | 257.5 ns | 263.0 ns |    2 | 0.0606 |     792 B | 未キャッシュ
            | Test2_Create | 232.6 ns | 1.64 ns | 1.53 ns | 230.4 ns | 234.4 ns |    1 | 0.0384 |     504 B | 今の実装
        */
        private static Type[] ConstructorTypes { get; } = [
            typeof(IDatabaseContext),
            typeof(IDatabaseStatementLoader),
            typeof(ILoggerFactory),
        ];

        protected IDatabaseContext Context { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILoggerFactory LoggerFactory { get; }
        protected object[] ConstructorParameters { get; }

        #endregion

        #region IDaoFactory

        public virtual DatabaseAccessObjectBase Create(Type type)
        {
            var constructor = type.GetConstructor(ConstructorTypes);
            if(constructor is null) {
                throw new DatabaseFactoryException("constructor");
            }

            var dao = constructor.Invoke(ConstructorParameters);
            if(dao is DatabaseAccessObjectBase result) {
                return result;
            }

            throw new DatabaseFactoryException($"dao is not {nameof(DatabaseAccessObjectBase)}: {dao.GetType()}");
        }

        #endregion
    }
}
