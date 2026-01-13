using ContentTypeTextNet.Pe.Library.Database.StatementBuilder.Builder;

namespace ContentTypeTextNet.Pe.Library.Database.StatementBuilder
{
    public class StatementBuilder: StatementBuilderBase
    {
        public StatementBuilder(IDatabaseImplementation implementation)
            : base(implementation)
        { }

        #region function

        public virtual StatementFromAfterBuilder<TEntity> From<TEntity>(TEntity entity)
            where TEntity : EntityBase
        {
            return new StatementFromAfterBuilder<TEntity>(entity, Implementation);
        }

        #endregion
    }
}
