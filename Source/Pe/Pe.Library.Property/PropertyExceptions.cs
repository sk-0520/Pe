using System;
using System.Reflection;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Throws;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GeneratedExceptionAttribute: System.Attribute
{
    public GeneratedExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.Library.Property
{
    public class PropertyException: Exception
    {
        public PropertyException(string message) : base(message) { }
        public PropertyException(string message, Exception inner) : base(message, inner) { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PropertyCanNotAccessException: PropertyException
    {
        public PropertyCanNotAccessException(Type ownerType, MemberInfo member)
            : base($"{ownerType.FullName}.{member.Name}")
        {
            Member = member;
        }

        #region property

        public MemberInfo Member { get; }

        #endregion
    }

    [GeneratedException]
    public partial class PropertyCanNotReadException: PropertyCanNotAccessException
    { }

    [GeneratedException]
    public partial class PropertyCanNotWriteException: PropertyCanNotAccessException
    { }

    [GeneratedException]
    public partial class PropertyNotFoundException: PropertyException
    { }
}
