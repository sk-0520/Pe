using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Generator.Id;
using Xunit;

namespace ContentTypeTextNet.Pe.Generator.Id.Test
{
    /// <summary>
    /// テストデータ。
    /// </summary>
    [GenerateGuidId()]
    public partial record struct TestData;

    [GenerateGuidIdTypeHandler(typeof(TestData))]
    public partial class TestDataHandler;

    public class GenerateGuidIdAttributeTest
    {
        #region function

        [Fact]
        public void Constructor_Empty_Test()
        {
            var actual = new TestData();
            Assert.Equal(TestData.Empty, actual);
        }

        [Fact]
        public void GeneratedMembers_AreIncludedInCoverage_Test()
        {
            var type = typeof(TestData);
            var exclusion = typeof(System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute);

            Assert.False(Attribute.IsDefined(type.GetConstructor(new[] { typeof(Guid) })!, exclusion));
            Assert.False(Attribute.IsDefined(type.GetProperty(nameof(TestData.Id))!, exclusion));
            Assert.False(Attribute.IsDefined(type.GetProperty(nameof(TestData.Empty))!, exclusion));
            Assert.False(Attribute.IsDefined(type.GetMethod(nameof(TestData.TryParse))!, exclusion));
            Assert.False(Attribute.IsDefined(type.GetMethod(nameof(TestData.Parse))!, exclusion));
            Assert.False(Attribute.IsDefined(type.GetMethod(nameof(TestData.NewId))!, exclusion));
            Assert.False(Attribute.IsDefined(type.GetMethod(nameof(TestData.ToString))!, exclusion));
            Assert.False(Attribute.IsDefined(typeof(GenerateGuidIdAttribute), exclusion));
            Assert.False(Attribute.IsDefined(typeof(GenerateGuidIdTypeHandlerAttribute), exclusion));
            Assert.False(Attribute.IsDefined(typeof(TestDataHandler).GetMethod(nameof(TestDataHandler.SetValue))!, exclusion));
            Assert.False(Attribute.IsDefined(typeof(TestDataHandler).GetMethod(nameof(TestDataHandler.Parse))!, exclusion));
        }

        [Fact]
        public void TypeHandler_Parse_Test()
        {
            var handler = new TestDataHandler();
            var id = TestData.NewId();

            Assert.Equal(id, handler.Parse(id.ToString()));
            Assert.Equal(TestData.Empty, handler.Parse("invalid"));
        }

        #endregion
    }
}
