using System;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    public class ShellOptions
    {
        #region property

        /// <summary>
        /// エンコーディング。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.Default; // .NET Framework と心中

        /// <summary>
        /// 改行コード。
        /// </summary>
        public string NewLine { get; set; } = Environment.NewLine;

        /// <summary>
        /// インデント。
        /// </summary>
        public string Indent { get; set; } = "    ";

        #endregion

    }
}
