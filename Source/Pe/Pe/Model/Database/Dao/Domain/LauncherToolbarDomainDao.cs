using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Domain;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Domain
{
    public class LauncherToolbarDomainDao : DomainDaoBase
    {
        public LauncherToolbarDomainDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        LauncherToolbarsScreenData ConvertFromDto(LauncherToolbarScreenRowDto dto)
        {
            var data = new LauncherToolbarsScreenData() {
                LauncherToolbarId = dto.LauncherToolbarId,
                ScreenName = dto.ScreenName,
                X = dto.ScreenX,
                Y = dto.ScreenY,
                Height = dto.ScreenHeight,
                Width = dto.ScreenWidth,
            };

            return data;
        }

        public IEnumerable<LauncherToolbarsScreenData> SelectAllScreenToolbars()
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<LauncherToolbarScreenRowDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }


        #endregion
    }
}
