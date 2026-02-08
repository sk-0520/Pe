namespace ContentTypeTextNet.Pe.Core.Models.Shell.Value
{
    public abstract class ValueBase: IExpression
    {
        #region operator

        public static Express operator +(ValueBase a, ValueBase b)
        {
            var result = new Express();

            result.Values.Add(a);
            result.Values.Add(b);

            return result;
        }

        #endregion

        #region IExpression

        public abstract string Expression { get; }

        #endregion
    }
}
