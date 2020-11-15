using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class ScreensEntityDao : EntityDaoBase
    {
        #region define

        private class ScreensRowDto: CommonDtoBase
        {
            #region property

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

        #endregion

        public ScreensEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public bool SelectExistsScreen(string? screenName)
        {
            var statement = LoadStatement();
            var param = new {
                ScreenName = screenName ?? string.Empty,
            };
            return Context.QuerySingle<bool>(statement, param);
        }

        public bool InsertScreen(IScreen screen, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new ScreensRowDto() {
                ScreenName = screen.DeviceName,
                ScreenX = (long)screen.DeviceBounds.X,
                ScreenY = (long)screen.DeviceBounds.Y,
                ScreenWidth = (long)screen.DeviceBounds.Width,
                ScreenHeight = (long)screen.DeviceBounds.Height,
            };
            commonStatus.WriteCommon(dto);

            return Context.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
