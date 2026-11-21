using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test
{
    public class GeneratedIdHandlerCoverageTest
    {
        [Fact]
        public void GeneratedMethods_AreExcludedFromCoverage_Test()
        {
            var type = typeof(App).Assembly.GetType("ContentTypeTextNet.Pe.Main.Models.Applications.Internal.LauncherItemIdHandler", true)!;
            var exclusion = typeof(ExcludeFromCodeCoverageAttribute);

            Assert.False(Attribute.IsDefined(type, exclusion));
            Assert.True(Attribute.IsDefined(type.GetMethod("SetValue")!, exclusion));
            Assert.True(Attribute.IsDefined(type.GetMethod("Parse")!, exclusion));
        }
    }
}
