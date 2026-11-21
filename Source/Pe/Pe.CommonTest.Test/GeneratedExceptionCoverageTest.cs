using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest.Test
{
    public class GeneratedExceptionCoverageTest
    {
        [Fact]
        public void GeneratedConstructors_AreExcludedFromCoverage_Test()
        {
            var exclusion = typeof(ExcludeFromCodeCoverageAttribute);

            Assert.False(Attribute.IsDefined(typeof(TestException), exclusion, false));
            Assert.False(Attribute.IsDefined(typeof(TestException).GetMethod(nameof(TestException.ThrowIfNull))!, exclusion, false));
            foreach(var constructor in typeof(TestException).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                Assert.True(Attribute.IsDefined(constructor, exclusion, false));
            }
        }
    }
}
