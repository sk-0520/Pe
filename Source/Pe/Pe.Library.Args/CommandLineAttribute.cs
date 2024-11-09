using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CommandLineAttribute: Attribute
    {
        public CommandLineAttribute(string longKey = "", string description = "", bool hasValue = true)
        {
            LongKey = longKey;
            Description = description;
            HasValue = hasValue;
        }

        #region property

        /// <summary>
        /// 長いキー。
        /// </summary>
        public string LongKey { get; }

        /// <summary>
        /// 値を持つか。
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// コマンドラインの説明。
        /// </summary>
        public string Description { get; }

        #endregion
    }

}
