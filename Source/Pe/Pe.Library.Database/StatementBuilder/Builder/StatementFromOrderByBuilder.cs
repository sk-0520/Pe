using System;
using System.Linq.Expressions;

namespace ContentTypeTextNet.Pe.Library.Database.StatementBuilder.Builder
{
    public class StatementFromOrderByBuilder<TEntity>: StatementBuilderBase
            where TEntity : EntityBase
    {
        public StatementFromOrderByBuilder(Expression<Func<TEntity, object>> expression, TEntity entity, IDatabaseImplementation implementation)
            : base(implementation)
        {
            Entity = entity;
        }

        #region property

        public TEntity Entity { get; }

        #endregion

        #region function

        #endregion
    }
}
