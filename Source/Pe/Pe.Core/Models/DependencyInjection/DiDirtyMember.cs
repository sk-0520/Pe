using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// 対象オブジェクトのプロパティに値を設定。
    /// </summary>
    public sealed class DiDirtyMember
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="baseType">対象オブジェクト。</param>
        /// <param name="memberInfo"><paramref name="baseType"/>の対象プロパティ。</param>
        /// <param name="objectType">設定する値。</param>
        public DiDirtyMember(Type baseType, MemberInfo memberInfo, Type objectType)
        {
            BaseType = baseType;
            MemberInfo = memberInfo;
            ObjectType = objectType;
        }

        #region property

        /// <summary>
        /// 対象オブジェクト。
        /// </summary>
        public Type BaseType { get; }
        /// <summary>
        /// <see cref="BaseType"/>の対象プロパティ。
        /// </summary>
        public MemberInfo MemberInfo { get; }
        /// <summary>
        /// 設定する値。
        /// </summary>
        public Type ObjectType { get; }

        #endregion
    }
}
