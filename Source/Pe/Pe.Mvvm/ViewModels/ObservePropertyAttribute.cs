using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    /// <summary>
    /// プロパティに対して指定したプロパティの変更を検知する。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ObservePropertyAttribute: Attribute
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="propertyName">監視対象プロパティ名。</param>
        public ObservePropertyAttribute(string propertyName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

            PropertyName = propertyName;
        }

        #region property

        /// <summary>
        /// 監視対象プロパティ名。
        /// </summary>
        public string PropertyName { get; }

        #endregion
    }
}
