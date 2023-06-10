using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Main.AppMode.Database.Models.Data
{
    internal class DatabaseOptions
    {
        #region property

        /// <summary>
        /// 言語。
        /// </summary>
        [CommandLine(longKey: "language", hasValue: false)]
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// 対象データベース一覧。
        /// </summary>
        [CommandLine(longKey: "database")]
        public string[] Database { get; set; } = Array.Empty<string>();

        #endregion
    }
}
