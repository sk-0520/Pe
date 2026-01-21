namespace ContentTypeTextNet.Pe.Core.Models.Shell.Redirect
{
    /// <summary>
    /// 標準エラー出力。
    /// </summary>
    public class ErrorRedirect: RedirectBase
    {
        #region property

        /// <summary>
        /// 標準出力に追記する。
        /// </summary>
        public bool StandardOutput { get; set; }

        #endregion
    }
}
