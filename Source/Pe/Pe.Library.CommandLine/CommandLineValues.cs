using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    /// <summary>
    /// コマンドラインの値。
    /// </summary>
    public interface ICommandLineValues
    {
        #region property

        /// <summary>
        /// 値一覧。
        /// </summary>
        IReadOnlyList<string> Data { get; }

        /// <summary>
        /// 最初の値。
        /// </summary>
        string First { get; }

        #endregion
    }

    /// <inheritdoc cref="ICommandLineValues"/>
    /// <remarks>内部使用を目的としており外部公開されるものは <see cref="ICommandLineValues"/> を参照のこと。</remarks>
    internal class CommandLineValues: ICommandLineValues
    {
        #region function

        /// <summary>
        /// 値の追加。
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            Data.Add(value);
        }

        #endregion

        #region IArgValues

        /// <summary>
        /// 値一覧。
        /// </summary>
        public List<string> Data { get; } = new();
        IReadOnlyList<string> ICommandLineValues.Data => Data;

        /// <summary>
        /// 最初の値。
        /// </summary>
        public string First => Data[0];

        #endregion
    }
}
