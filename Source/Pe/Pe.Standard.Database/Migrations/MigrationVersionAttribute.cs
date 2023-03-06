using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Database.Migrations
{
    /// <summary>
    /// マイグレーションクラスに対して付与するバージョン属性。
    /// </summary>
    /// <typeparam name="TVersion">バージョン指定に使用する型。</typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationVersionAttribute<TVersion>: Attribute
        where TVersion : IComparable<TVersion>, IEquatable<TVersion>
    {
        public MigrationVersionAttribute(TVersion version)
        {
            Version = version;
        }

        #region property

        public TVersion Version { get; }

        #endregion
    }
}
