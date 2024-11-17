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
    public static class ICommandLineExtensions
    {
        #region function

        /// <summary>
        /// <see cref="ICommandLine"/>のキーから値を取得。
        /// </summary>
        /// <param name="commandLine">対象の<see cref="ICommandLine"/>。</param>
        /// <param name="key">キー。</param>
        /// <param name="defaultValue"><paramref name="key"/>が存在しない場合の戻り値。</param>
        /// <returns><paramref name="key"/>に対する値。存在しない場合は<paramref name="defaultValue"/>を返す。</returns>
        public static string GetValue(this ICommandLine commandLine, string key, string defaultValue)
        {
            if(commandLine.TryGetKey(key, out var commandLineKey)) {
                if(commandLine.Values.TryGetValue(commandLineKey, out var value)) {
                    return value.First;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// <see cref="ICommandLine"/>のキーから値の配列を取得。
        /// </summary>
        /// <param name="commandLine">対象の<see cref="ICommandLine"/>。</param>
        /// <param name="key">キー。</param>
        /// <returns><paramref name="key"/>に対する値。存在しない場合は空コレクション。</returns>
        public static IReadOnlyList<string> GetValues(this ICommandLine commandLine, string key)
        {
            if(commandLine.TryGetKey(key, out var commandLineKey)) {
                if(commandLine.Values.TryGetValue(commandLineKey, out var value)) {
                    return value.Items;
                }
            }

            return Array.Empty<string>();
        }

        /// <summary>
        /// <see cref="ICommandLine"/>のスイッチを取得。
        /// </summary>
        /// <param name="commandLine">対象の<see cref="ICommandLine"/>。</param>
        /// <param name="key">キー。</param>
        /// <returns>スイッチ状態。</returns>
        public static bool ExistsSwitch(this ICommandLine commandLine, string key)
        {
            if(commandLine.TryGetKey(key, out var commandLineKey)) {
                return commandLine.Switches.Contains(commandLineKey);
            }

            return false;
        }

        #endregion
    }
}
