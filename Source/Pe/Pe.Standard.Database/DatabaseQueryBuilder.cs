using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    public class DatabaseQueryBuilder
    {
        public DatabaseQueryBuilder(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            Context = context;
            Implementation = implementation;
        }

        #region property

        private IDatabaseContext Context { get; }
        private IDatabaseImplementation Implementation { get; }

        #endregion

        #region function

        public void From(string tableName)
        {

        }

        #endregion
    }
}
