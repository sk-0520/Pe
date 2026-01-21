using ContentTypeTextNet.Pe.Core.Models.Shell.Redirect;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Redirect
{
    public class CommandPromptOutputRedirect: OutputRedirect
    {
        #region OutputRedirect

        /// <summary>
        /// 空リダイレクト。
        /// </summary>
        public override OutputRedirect Null { get; } = new OutputRedirect() {
            Target = "NUL",
        };

        /// <summary>
        /// 空リダイレクト(エラー付き)。
        /// </summary>
        public override OutputRedirect NullWithError { get; } = new OutputRedirect() {
            Target = "NUL",
            Error = new ErrorRedirect() {
                StandardOutput = true,
            }
        };

        #endregion
    }
}
