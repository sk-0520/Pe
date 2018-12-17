using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    public class LauncherGroupsRowDto: RowDtoBase
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ImageColor { get; set; }

        #endregion
    }
}
