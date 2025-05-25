using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Property
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PropertyException: Exception
    {
        public PropertyException(string message) : base(message) { }
        public PropertyException(string message, Exception inner) : base(message, inner) { }

        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
        protected PropertyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
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

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<保留中>")]
    public sealed class PropertyCanNotReadException: PropertyCanNotAccessException
    {
        public PropertyCanNotReadException(Type ownerType, MemberInfo member)
            : base(ownerType, member)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<保留中>")]
    public sealed class PropertyCanNotWriteException: PropertyCanNotAccessException
    {
        public PropertyCanNotWriteException(Type ownerType, MemberInfo member)
            : base(ownerType, member)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<保留中>")]
    public sealed class PropertyNotFoundException: PropertyException
    {
        public PropertyNotFoundException(string propertyName)
            : base(propertyName)
        { }
    }

}
