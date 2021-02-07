using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Tuner
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Tuner_LauncherGroups: TunerBase
    {
        public Tuner_LauncherGroups(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        {
            IdFactory = idFactory;
        }

        #region property

        IIdFactory IdFactory { get; }

        #endregion

        #region TunerBase

        bool ExistsRows(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return context.QuerySingle<bool>(statement, GetCommonDto());
        }

        int InsertEmptyGroup(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var param = GetCommonDto();
            param["LauncherGroupId"] = IdFactory.CreateLauncherGroupId();
            param["Name"] = Properties.Resources.String_NewEmptyGroupName;
            return context.Execute(statement, param);
        }

        protected override void TuneImpl(IDatabaseContext context)
        {
            if(!ExistsRows(context)) {
                InsertEmptyGroup(context);
            }
        }

        #endregion
    }
}
