using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Domain
{
    public interface IReadOnlyLauncherItemsIconRowDto : IReadOnlyRowDtoBase
    {
        #region property

        string Kind { get; }
        string FilePath { get; }
        long FileIndex { get; }
        string IconPath { get; }
        long IconIndex { get; }

        #endregion
    }

    public class LauncherItemIconRowDto : RowDtoBase, IReadOnlyLauncherItemsIconRowDto
    {
        #region IReadOnlyLauncherItemsIconRowDto

        public string Kind { get; set; }
        public string FilePath { get; set; }
        public long FileIndex { get; set; }
        public string IconPath { get; set; }
        public long IconIndex { get; set; }

        #endregion
    }
}
