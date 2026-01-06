using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Args
{
    public static class CommandLineParsedResultExtensions
    {
        #region function

        /// <summary>
        /// キーから値を取得。
        /// </summary>
        /// <param name="parsedResult">対象の<see cref="CommandLineParsedResult"/>。</param>
        /// <param name="key">キー。</param>
        /// <param name="fallback"><paramref name="key"/>が存在しない場合の戻り値。</param>
        /// <returns><paramref name="key"/>に対する値。存在しない場合は<paramref name="fallback"/>を返す。</returns>
        public static string GetValue(this CommandLineParsedResult parsedResult, string key, string fallback)
        {
            if(parsedResult.Values.TryGetValue(key, out var commandLineKey)) {
                return commandLineKey.First;
            }

            return fallback;
        }

        /// <summary>
        /// キーから値の配列を取得。
        /// </summary>
        /// <param name="parsedResult">対象の<see cref="CommandLineParsedResult"/>。</param>
        /// <param name="key">キー。</param>
        /// <returns><paramref name="key"/>に対する値。存在しない場合は空コレクション。</returns>
        public static IReadOnlyList<string> GetValues(this CommandLineParsedResult parsedResult, string key)
        {
            if(parsedResult.Values.TryGetValue(key, out var commandLineKey)) {
                return commandLineKey.Data;
            }

            return Array.Empty<string>();
        }

        /// <summary>
        /// <see cref="CommandLineParsedResult"/>のスイッチを取得。
        /// </summary>
        /// <param name="parsedResult">対象の<see cref="CommandLineParsedResult"/>。</param>
        /// <param name="key">キー。</param>
        /// <returns>スイッチ状態。</returns>
        public static bool ExistsSwitch(this CommandLineParsedResult parsedResult, string key)
        {
            return parsedResult.Switches.Contains(key);
        }

        #endregion
    }
}
