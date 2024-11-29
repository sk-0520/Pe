using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.ViewModels;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.ViewModels
{
    public class ObservePropertyAttributeTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual = new ObservePropertyAttribute("abc");
            Assert.Equal("abc", actual.PropertyName);
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException), null)]
        [InlineData(typeof(ArgumentException), "")]
        [InlineData(typeof(ArgumentException), " ")]
        public void Constructor_Throw_Test(Type expectedException, string? name)
        {
            Assert.Throws(expectedException, () => new ObservePropertyAttribute(name!));
        }

        #endregion
    }
}
