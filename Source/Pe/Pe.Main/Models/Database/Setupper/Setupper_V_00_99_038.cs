using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    [DatabaseSetupVersion(0, 99, 38)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Setupper_V_00_99_038: SetupperBase
    {
        public Setupper_V_00_99_038(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override void ExecuteMainDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteMainDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteFileDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteFileDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        #endregion
    }
}
