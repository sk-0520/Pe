using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Common.Provider
{
    /// <summary>
    /// 環境変数系の抽象化基底。
    /// </summary>
    public abstract class EnvironmentProvider
    {
        #region property

        public static EnvironmentProvider Default { get; } = new DefaultEnvironmentProvider();

        /// <inheritdoc cref="Environment.NewLine"/>
        public string NewLine => Environment.NewLine;

        #endregion

        #region function

        /// <summary>
        /// 環境変数の値を取得。
        /// </summary>
        /// <param name="variable">変数名。</param>
        /// <param name="result">値。</param>
        /// <returns>取得できたか。</returns>
        public virtual bool TryGetVariableValue(string variable, [NotNullWhen(true)] out string? result)
        {
            var value = Environment.GetEnvironmentVariable(variable);
            if(value is null) {
                result = null;
                return false;
            }

            result = value;
            return true;
        }

        #endregion
    }

    file sealed class DefaultEnvironmentProvider: EnvironmentProvider
    { }
}
