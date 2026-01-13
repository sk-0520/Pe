namespace ContentTypeTextNet.Pe.Library.Database.StatementBuilder
{
    public class StatementBuilder
    {
        public StatementBuilder(IDatabaseImplementation implementation)
        {
            Implementation = implementation;
        }

        #region property

        public IDatabaseImplementation Implementation { get; }

        #endregion
    }
}
