namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    /// <summary>
    /// テキスト表現。
    /// </summary>
    public interface IExpression
    {
        #region property

        /// <summary>
        /// テキストを取得。
        /// </summary>
        string Expression { get; }

        #endregion
    }
}
