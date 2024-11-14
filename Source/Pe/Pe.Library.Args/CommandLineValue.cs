using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドラインの値。
    /// </summary>
    public class CommandLineValue: ICommandLineValue
    {
        #region function

        /// <summary>
        /// 値の追加。
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            Items.Add(value);
        }

        #endregion

        #region ICommandLineValue

        /// <inheritdoc cref="ICommandLineValue.Items"/>
        public List<string> Items { get; } = new List<string>();
        IReadOnlyList<string> ICommandLineValue.Items => Items;

        /// <inheritdoc cref="ICommandLineValue.First"/>
        public string First => Items.First();

        #endregion
    }
}
