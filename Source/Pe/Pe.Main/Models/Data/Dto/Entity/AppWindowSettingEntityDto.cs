using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class AppWindowSettingEntityDto : CommonDtoBase
    {
        #region property

        public bool IsEnabled { get; set; }
        public long Count { get; set; }
        public TimeSpan Interval { get; set; }

        #endregion
    }
}
