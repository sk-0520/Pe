using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Provider
{
    /// <summary>
    /// 環境変数系の抽象化基底。
    /// </summary>
    public abstract class EnvironmentProvider
    {
        #region property

        public static EnvironmentProvider Default { get; } = new DefaultEnvironmentProvider();

        /// <inheritdoc cref="Environment.NewLine"/>
        public virtual string NewLine { get; } = Environment.NewLine;

        /// <inheritdoc cref="Environment.Is64BitProcess"/>
        public virtual bool Is64BitProcess { get; } = Environment.Is64BitProcess;

        /// <inheritdoc cref="Environment.Is64BitOperatingSystem"/>
        public virtual bool Is64BitOperatingSystem { get; } = Environment.Is64BitOperatingSystem;

        /// <inheritdoc cref="Environment.OSVersion"/>
        public virtual OperatingSystem OSVersion { get; } = Environment.OSVersion;


        #endregion

        #region function

        /// <inheritdoc cref="Environment.GetCommandLineArgs()"/>
        public virtual IReadOnlyList<string> GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

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

        /// <inheritdoc cref="Environment.GetEnvironmentVariables()"/>
        public virtual Dictionary<string, string> GetEnvironmentVariables()
        {
            return Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>()
                .Where(a => a.Value is not null)
                .ToDictionary(k => (string)k.Key, v => (string)v.Value!)
            ;
        }

        /// <inheritdoc cref="Environment.ExpandEnvironmentVariables(string)"/>
        public virtual string ExpandEnvironmentVariables(string name)
        {
            return Environment.ExpandEnvironmentVariables(name);
        }

        /// <inheritdoc cref="Environment.SetEnvironmentVariable(string, string?)"/>
        public virtual void SetEnvironmentVariable(string name, string? value)
        {
            Environment.SetEnvironmentVariable(name, value);
        }

        /// <inheritdoc cref="Environment.Exit(int)"/>
        public virtual void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        /// <inheritdoc cref="Environment.FailFast(string?)"/>
        public virtual void FailFast(string? message)
        {
            Environment.FailFast(message);
        }

        /// <inheritdoc cref="Environment.FailFast(string?, Exception?)"/>
        public virtual void FailFast(string? message, Exception? exception)
        {
            Environment.FailFast(message, exception);
        }

        /// <inheritdoc cref="Environment.GetLogicalDrives()"/>
        public virtual string[] GetLogicalDrives()
        {
            return Environment.GetLogicalDrives();
        }

        /// <inheritdoc cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>
        public virtual string GetFolderPath(Environment.SpecialFolder folder)
        {
            return Environment.GetFolderPath(folder);
        }

        #endregion
    }

    file sealed class DefaultEnvironmentProvider: EnvironmentProvider
    { }
}
