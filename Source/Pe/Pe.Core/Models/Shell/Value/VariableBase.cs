using System;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Value
{
    /// <summary>
    /// 変数表現基底。
    /// </summary>
    public abstract class VariableBase: ValueBase
    {
        protected VariableBase(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
        }


        #region property

        /// <summary>
        /// 変数名。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 読み取り専用か。
        /// </summary>
        public bool IsReadOnly { get; init; }

        #endregion
    }
}
