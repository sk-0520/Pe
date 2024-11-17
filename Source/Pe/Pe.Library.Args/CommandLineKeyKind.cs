using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドラインキーの種別。
    /// </summary>
    public enum CommandLineKeyKind
    {
        /// <summary>
        /// 値を持つ。
        /// </summary>
        Value,
        /// <summary>
        /// スイッチ。
        /// </summary>
        /// <remarks>値を持たない。</remarks>
        Switch,
    }
}
