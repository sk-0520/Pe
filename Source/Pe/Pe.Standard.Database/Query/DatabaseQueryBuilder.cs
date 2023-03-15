using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Database.Query
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

        public void From(Type type)
        {
            var attr = type.GetCustomAttribute<DatabaseEntityTableAttribute>();
            if(attr is null) {
                throw new ArgumentException(type.ToString(), nameof(type));
            }

            From(attr.TableName);
        }


        public void From<TEntity>() => From(typeof(TEntity));

        #endregion
    }
}
