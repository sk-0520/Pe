using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドラインの値。
    /// </summary>
    public interface ICommandLineValue
    {
        #region property

        /// <summary>
        /// 値一覧。
        /// </summary>
        IReadOnlyList<string> Items { get; }
        /// <summary>
        /// 最初の値。
        /// </summary>
        string First { get; }

        #endregion
    }
}
