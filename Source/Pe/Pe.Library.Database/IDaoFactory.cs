using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public interface IDaoFactory
    {
        #region function

        DatabaseAccessObjectBase Create(Type type);

        #endregion
    }

    public static class IDaoFactoryExtensions
    {
        #region function

        public static TDao Create<TDao>(this IDaoFactory daoFactory)
            where TDao : DatabaseAccessObjectBase
        {
            var type = typeof(TDao);
            return (TDao)daoFactory.Create(type);
        }

        #endregion
    }
}
