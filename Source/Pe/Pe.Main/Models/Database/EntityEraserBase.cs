using System;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using ContentTypeTextNet.Pe.Main.Models.Applications;

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

        protected abstract void ExecuteMainImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);


        public void Execute()
        {
            ExecuteMainImpl(MainContext, StatementLoader, MainContext.Implementation);
            ExecuteLargeImpl(LargeContext, StatementLoader, LargeContext.Implementation);
            ExecuteTemporaryImpl(TemporaryContext, StatementLoader, TemporaryContext.Implementation);
        }

        #endregion
    }
}
