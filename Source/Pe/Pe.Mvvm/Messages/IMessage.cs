using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.Messages
{
    /// <summary>
    /// メッセージ内容。
    /// </summary>
    /// <remarks>
    /// <para>登録時の型とIDがキーとなる。</para>
    /// <para>入力値・コールバックなどを必要に応じて実装していく。</para>
    /// </remarks>
    public interface IMessage
    {
        #region property

        /// <summary>
        /// メッセージを特定するID。
        /// </summary>
        /// <remarks>
        /// <para>メッセージは最初に見つかったものが使用されるため同じメッセージに対して処理を振り分けるために使用する。</para>
        /// </remarks>
        string MessageId { get; }

        #endregion
    }
}
