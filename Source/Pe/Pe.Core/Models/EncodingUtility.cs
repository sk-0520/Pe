using System.Text;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// エンコーディング。。。
    /// </summary>
    public static class EncodingUtility
    {
        #region property

        /// <summary>
        /// BOMありUTF-8名。
        /// </summary>
        public static string UTF8BomName => "utf-8bom";
        /// <summary>
        /// BOMなしUTF-8名。
        /// </summary>
        public static string UTF8nName => "utf-8n";

        /// <summary>
        /// BOMありUTF-8エンコーディング。
        /// </summary>
        public static Encoding UTF8Bom => new UTF8Encoding(true);
        /// <summary>
        /// BOMなしUTF-8エンコーディング。
        /// </summary>
        public static Encoding UTF8N => new UTF8Encoding(false);

        #endregion

        #region function

        public static Encoding Parse(string encodingName)
        {
            if(encodingName == UTF8BomName) {
                return UTF8Bom;
            }
            if(encodingName == UTF8nName) {
                return UTF8N;
            }

            return Encoding.GetEncoding(encodingName);
        }

        public static string ToString(Encoding encoding)
        {
            if(encoding is UTF8Encoding utf8) {
                if(utf8.GetPreamble().Length != 0) {
                    return UTF8BomName;
                }
            }

            return encoding.WebName;
        }

        /// <summary>
        /// OS 標準のエンコーディングを取得。
        /// </summary>
        /// <param name="fallbackEncoding">取得できなかった場合のエンコーディング。</param>
        public static Encoding GetDefaultEncoding(Encoding fallbackEncoding)
        {
            try {
                var ansiCodePage = (int)NativeMethods.GetACP();
                return Encoding.GetEncoding(ansiCodePage);
            } catch {
                return fallbackEncoding;
            }
        }

        /// <inheritdoc cref="GetDefaultEncoding(Encoding)"/>
        /// <remarks>どうにもならん場合は <see cref="Encoding.Default"/> を使用する。</remarks>
        public static Encoding GetDefaultEncoding()
        {
            return GetDefaultEncoding(Encoding.Default);
        }


        #endregion
    }
}
