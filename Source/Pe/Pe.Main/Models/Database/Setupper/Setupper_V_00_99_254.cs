using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    [DatabaseSetupVersion(0, 99, 254)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Setupper_V_00_99_254: SetupperBase
    {
        public Setupper_V_00_99_254(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override void ExecuteMainDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteMainDML(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

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
