namespace ContentTypeTextNet.Pe.Library.Database.StatementBuilder.Builder
{
    public class StatementBuilderBase
    {
        protected StatementBuilderBase(IDatabaseImplementation implementation)
        {
            Implementation = implementation;
        }

        #region property

        public IDatabaseImplementation Implementation { get; }

        #endregion
    }
}
