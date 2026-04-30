using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// <see cref="DatabaseAccessObjectBase"/>ファクトリ。
    /// </summary>
    public class DaoFactory
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statementLoader"></param>
        /// <param name="loggerFactory"></param>
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:プロパティは配列を返すことはできません", Justification = $"{nameof(ConstructorInfo.Invoke)}が配列しか受け取らん")]
        protected object[] ConstructorParameters { get; }

        #endregion

        #region function

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

    public static class DaoFactoryExtensions
    {
        #region function

        public static TDao Create<TDao>(this DaoFactory daoFactory)
            where TDao : DatabaseAccessObjectBase
        {
            var type = typeof(TDao);
            return (TDao)daoFactory.Create(type);
        }

        #endregion
    }
}
