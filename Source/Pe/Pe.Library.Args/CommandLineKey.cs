using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドラインのキー。
    /// </summary>
    /// <param name="LongKey">長いキー。</param>
    /// <param name="HasValue">値を持つか。</param>
    /// <param name="Description">説明。</param>
    public record class CommandLineKey(
        string LongKey,
        bool HasValue,
        string Description
    )
    {
        #region property

        /// <summary>
        /// 有効な<see cref="LongKey"/>か。
        /// </summary>
        public bool IsEnabledLongKey => !string.IsNullOrEmpty(LongKey);

        #endregion
    }
}
