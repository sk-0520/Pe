using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    public static class CommandLineHelper
    {
        #region function

        /// <summary>
        /// 文字列をコマンドライン引数の値に変換する。
        /// </summary>
        /// <param name="input">対象文字列。</param>
        /// <returns></returns>
        public static string Escape(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) {
                return "\"" + input + "\"";
            }

            var s = input.Trim();
            var result = s.Replace("\"", "\"\"");
            if(s.IndexOf(' ') == -1) {
                return result;
            } else {
                return "\"" + result + "\"";
            }
        }

        /// <summary>
        /// <see cref="IDictionary{TKey, TValue}"/>をいい感じにつなげる。
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToCommandLineArguments(IReadOnlyDictionary<string, string> map, string header = "--", char separator = '=')
        {
            return map.Select(i => header + i.Key + separator + Escape(i.Value));
        }

        #endregion
    }
}
