using System;
using System.Linq.Expressions;

namespace ContentTypeTextNet.Pe.Library.Database.StatementBuilder.Builder
{
    public class StatementFromAfterBuilder<TEntity>: StatementBuilderBase
            where TEntity : EntityBase
    {
        public StatementFromAfterBuilder(TEntity entity, IDatabaseImplementation implementation)
            : base(implementation)
        {
            Entity = entity;
        }

        #region property

        public TEntity Entity { get; }

        #endregion

        #region function

        public StatementFromOrderByBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> expression)
        {
            return new StatementFromOrderByBuilder<TEntity>(expression, Entity, Implementation);
        }

        #endregion
    }
}
