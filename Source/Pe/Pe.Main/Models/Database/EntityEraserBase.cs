using System;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Library.Database;
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
        protected EntityEraserBase(IDatabaseContextPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : this(contextsPack.Main, contextsPack.Large, contextsPack.Temporary, statementLoader, loggerFactory)
        { }

        protected EntityEraserBase(IDatabaseContext mainContext, IDatabaseContext fileContext, IDatabaseContext temporaryContext, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            MainContext = mainContext ?? throw new ArgumentNullException(nameof(mainContext));
            LargeContext = fileContext ?? throw new ArgumentNullException(nameof(fileContext));
            TemporaryContext = temporaryContext ?? throw new ArgumentNullException(nameof(temporaryContext));
            StatementLoader = statementLoader ?? throw new ArgumentNullException(nameof(statementLoader));
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

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
