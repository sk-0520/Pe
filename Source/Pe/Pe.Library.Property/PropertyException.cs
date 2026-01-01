using System;
using System.Reflection;
using ContentTypeTextNet.Pe.Generator.Exception;

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

    [GenerateException]
    public partial class PropertyCanNotReadException: PropertyCanNotAccessException
    { }

    [GenerateException]
    public partial class PropertyCanNotWriteException: PropertyCanNotAccessException
    { }

    [GenerateException]
    public partial class PropertyNotFoundException: PropertyException
    { }
}
