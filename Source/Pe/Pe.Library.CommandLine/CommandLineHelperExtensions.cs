using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    public static class CommandLineHelperExtensions
    {
        #region function

        /// <summary>
        /// <see cref="IDictionary{TKey, TValue}"/>をいい感じにつなげる。
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToCommandLineArguments(this CommandLineHelper helper, IReadOnlyDictionary<string, string> map)
        {
            return map.Select(i => helper.OptionPrefix + i.Key + helper.Separator + helper.Escape(i.Value));
        }

        #endregion
    }
}
