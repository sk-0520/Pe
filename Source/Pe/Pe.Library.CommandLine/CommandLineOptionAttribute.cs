using System;

namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    /// <summary>
    /// コマンドラインオプション属性。
    /// </summary>
    /// <seealso cref="CommandLineConverter"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CommandLineOptionAttribute: Attribute
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="key"><see cref="CommandLineOption.Key"/></param>
        /// <param name="kind"><see cref="CommandLineOption.Kind"/></param>
        /// <param name="description"><see cref="CommandLineOption.Description"/></param>
        /// <param name="required"><see cref="CommandLineOption.Required"/></param>
        /// <seealso cref="CommandLineOption"/>
        public CommandLineOptionAttribute(string key, CommandLineOptionKind kind, string description = "", bool required = false)
        {
            Option = new CommandLineOption(key, kind, description) {
                Required = required,
            };
        }

        #region property

        /// <summary>
        /// コマンドラインオプション。
        /// </summary>
        public CommandLineOption Option { get; }

        #endregion
    }
}
