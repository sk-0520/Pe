using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Setup
{
    /// <summary>
    /// 誰が何と言おうと新生初期バージョン。
    /// </summary>
    public class Setup_V_00_84_00_00 : SetupBase
    {
        public Setup_V_00_84_00_00(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override Version Version { get; } = new Version(0, 84, 0, 0);

        public override void ExecuteMainDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteSql(commander, StatementLoader.LoadStatementByCurrent(), dto);
        }

        public override void ExecuteMainDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteSql(commander, StatementLoader.LoadStatementByCurrent(), dto);
        }

        public override void ExecuteFileDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteSql(commander, StatementLoader.LoadStatementByCurrent(), dto);
        }

        public override void ExecuteFileDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        #endregion
    }
}
