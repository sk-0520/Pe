using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    internal class LauncherToolbarScreenRowDto : CommonDtoBase
    {
        #region property

        public Guid LauncherToolbarId { get; set; }

        public string ScreenName { get; set; } = string.Empty;
        [PixelKind(Px.Device)]
        public long ScreenX { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenY { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenWidth { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenHeight { get; set; }

        #endregion
    }

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
            var statement = LoadStatement();
            return Commander.Query<LauncherToolbarScreenRowDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }


        #endregion
    }
}
