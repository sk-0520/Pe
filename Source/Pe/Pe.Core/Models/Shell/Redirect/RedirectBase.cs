using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Redirect
{
    /// <summary>
    /// リダイレクト基底。
    /// </summary>
    public abstract class RedirectBase: IExpression
    {
        #region property

        /// <summary>
        /// リダイレクト先。
        /// </summary>
        public Express? Target { get; init; }

        /// <summary>
        /// 追記するか。
        /// </summary>
        public bool Append { get; init; }

        #endregion

        #region IExpression

        public virtual string Expression
        {
            get
            {
                if(Target is null) {
                    return string.Empty;
                }

                var sb = new StringBuilder();

                var redirectMode = Append
                    ? ">>"
                    : ">"
                ;
                sb.Append(redirectMode);
                sb.Append(' ');
                sb.Append(Target.Expression);

                return sb.ToString();
            }
        }

        #endregion
    }
}
