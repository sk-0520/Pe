using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Common
{
    public readonly struct AttributeProperty<TAttribute>
        where TAttribute : Attribute
    {
        public AttributeProperty(PropertyInfo property, IReadOnlyList<TAttribute> attributes)
        {
            ArgumentNullException.ThrowIfNull(property);
            ArgumentNullException.ThrowIfNull(attributes);

            Property = property;
            Attributes = attributes;
        }

        #region property

        public PropertyInfo Property { get; }
        public IReadOnlyList<TAttribute> Attributes { get; }

        #endregion
    }

    public static class AttributeUtility
    {
        #region function

        /// <summary>
        /// プロパティから指定の属性を持つものを抽出。
        /// </summary>
        /// <typeparam name="TAttribute">抽出したい属性。</typeparam>
        /// <param name="properties">プロパティ一覧。</param>
        /// <returns>プロパティと属性一覧の対。</returns>
        public static IEnumerable<AttributeProperty<TAttribute>> GetPropertiesWithAttribute<TAttribute>(IEnumerable<PropertyInfo> properties)
            where TAttribute : Attribute
        {
            return properties
                .Select(a => (property: a, attributes: a.GetCustomAttributes<TAttribute>()))
                .Where(a => a.attributes.Any())
                .Select(a => new AttributeProperty<TAttribute>(a.property, a.attributes.ToArray()))
            ;
        }

        #endregion
    }
}
