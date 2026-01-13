using System;

namespace ContentTypeTextNet.Pe.Library.Database.StatementBuilder
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityPrimaryKeyAttribute: Attribute
    { }

    [AttributeUsage(AttributeTargets.Property)]
    public class EntityColumnNameAttribute: Attribute
    {
        public EntityColumnNameAttribute(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name;
        }

        #region property

        public string Name { get; }

        #endregion
    }
}
