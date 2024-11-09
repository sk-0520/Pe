using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// マッピング用設定。
    /// </summary>
    public static class CommandLineExtensions
    {
        #region function

        /// <summary>
        /// <see cref="CommandLine"/>のキーから値を取得。
        /// </summary>
        /// <param name="commandLine">対象の<see cref="CommandLine"/>。</param>
        /// <param name="key">キー。</param>
        /// <param name="defaultValue"><paramref name="key"/>が存在しない場合の戻り値。</param>
        /// <returns><paramref name="key"/>に対する値。存在しない場合は<paramref name="defaultValue"/>を返す。</returns>
        public static string GetValue(this CommandLine commandLine, string key, string defaultValue)
        {
            var commandLineKey = commandLine.GetKey(key);
            if(commandLineKey != null) {
                if(commandLine.Values.TryGetValue(commandLineKey, out var value)) {
                    return value.First;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// <see cref="CommandLine"/>のスイッチを取得。
        /// </summary>
        /// <param name="commandLine">対象の<see cref="CommandLine"/>。</param>
        /// <param name="key">キー。</param>
        /// <returns>スイッチ状態。</returns>
        public static bool ExistsSwitch(this CommandLine commandLine, string key)
        {
            var commandLineKey = commandLine.GetKey(key);
            if(commandLineKey != null) {
                return commandLine.Switches.Contains(commandLineKey);
            }

            return false;
        }

        #endregion
    }
}
