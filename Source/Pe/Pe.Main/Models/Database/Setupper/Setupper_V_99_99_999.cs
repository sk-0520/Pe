using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    /// <summary>
    /// マイグレーションの最後に実行される。
    /// </summary>
    [DatabaseSetupVersion(99, 99, 999)]
    [DatabaseNoSetup("最終処理はマイグレーション一覧とは別枠(補正的な感じ)")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Setupper_V_99_99_999: SetupperBase
    {
        public Setupper_V_99_99_999(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override void ExecuteMainDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteMainDML(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteFileDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteFileDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteTemporaryDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        #endregion
    }
}
