using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection.Test
{
    public class DiConstructorCacheTest
    {
        #region function

        [Fact]
        public void Constructor_Throw_Test()
        {
            var mockConstructorInfo = Substitute.For<ConstructorInfo>();
            mockConstructorInfo.ReflectedType.Returns((Type?)null);
            var exception = Assert.Throws<ArgumentNullException>(() => new DiConstructorCache(mockConstructorInfo, []));
            Assert.Equal("constructorInfo.ReflectedType", exception.ParamName);
        }

        #endregion
    }
}
