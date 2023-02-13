using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.AppMode.Database.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.AppMode.Database.Models.Element
{
    internal class DatabaseElement: ElementBase
    {
        public DatabaseElement(DatabaseOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region ElementBase

        protected override void InitializeImpl()
        {
            //nop
        }

        #endregion
    }
}
