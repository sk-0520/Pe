using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection.Test
{
    public class InjectAttributeTest
    {
        #region function

        [Fact]
        public void Constructor_0_Test()
        {
            var test = new DiInjectionAttribute();
            Assert.Empty(test.Name);
        }

        [Fact]
        public void Constructor_1_Test()
        {
            var test = new DiInjectionAttribute(" abc ");
            Assert.Equal(" abc ", test.Name);
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException), null)]
        [InlineData(typeof(ArgumentException), "")]
        [InlineData(typeof(ArgumentException), " ")]
        public void Constructor_throw_Test(Type expectedType, string? s)
        {
            Assert.Throws(expectedType, () => new DiInjectionAttribute(s!));
        }

        #endregion
    }
}
