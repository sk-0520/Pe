using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class AttributePropertyTest
    {
        #region function

        class Constructor_throw_Test_Class
        {
            public string Property { get; } = "Property";
        }

        [Fact]
        public void Constructor_throw_Test()
        {
            var exception1 = Assert.Throws<ArgumentNullException>(() => new AttributeProperty<Attribute>(null!, Array.Empty<Attribute>()));
            Assert.Equal("property", exception1.ParamName);

            var test2 = new Constructor_throw_Test_Class();
            var test2property = test2.GetType().GetProperty("Property");
            Assert.NotNull(test2property);
            var exception2 = Assert.Throws<ArgumentNullException>(() => new AttributeProperty<Attribute>(test2property, null!));
            Assert.Equal("attributes", exception2.ParamName);
        }

        #endregion
    }

    public class AttributeUtilityTest
    {
        #region function



        #endregion
    }
}
