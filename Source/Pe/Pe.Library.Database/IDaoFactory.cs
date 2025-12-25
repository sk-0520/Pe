using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public interface IDaoFactory
    {
        #region function

        DatabaseAccessObjectBase Create(Type type);

        TDao Create<TDao>()
            where TDao : DatabaseAccessObjectBase
        ;

        #endregion
    }
}
