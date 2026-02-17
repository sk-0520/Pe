using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    /// <summary>
    /// 特定の削除処理を一括して行う。
    /// </summary>
    /// <remarks>
    /// <para>ランチャーアイテム削除時とかもうしんどいのよ。</para>
    /// </remarks>
    public abstract class EntityEraserBase
    {
        protected EntityEraserBase(IDatabaseContext mainContext, IDatabaseContext fileContext, IDatabaseContext temporaryContext, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            MainContext = mainContext;
            LargeContext = fileContext;
            TemporaryContext = temporaryContext;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        protected EntityEraserBase(IDatabaseContextPack contextPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : this(contextPack.Main, contextPack.Large, contextPack.Temporary, statementLoader, loggerFactory)
        { }

        #region property

        private IDatabaseContext MainContext { get; }
        private IDatabaseContext LargeContext { get; }
        private IDatabaseContext TemporaryContext { get; }
        private IDatabaseStatementLoader StatementLoader { get; }

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        #endregion

        #region function

        protected abstract void ExecuteMainCore(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteLargeCore(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteTemporaryCore(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);


        public void Execute()
        {
            ExecuteMainCore(MainContext, StatementLoader, MainContext.Implementation);
            ExecuteLargeCore(LargeContext, StatementLoader, LargeContext.Implementation);
            ExecuteTemporaryCore(TemporaryContext, StatementLoader, TemporaryContext.Implementation);
        }

        #endregion
    }
}
