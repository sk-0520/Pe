using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public interface ILauncherToolbarManager
    {
        #region property
        #endregion

        #region function
        #endregion
    }

    public class LauncherToolbarManager : ManagerBase, ILauncherToolbarManager
    {
        public LauncherToolbarManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region ILauncherToolbarManager
        #endregion
    }
}
