using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        #region define

        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
        private class TestAttribute(string name): Attribute
        {
            #region property

            public string Name { get; } = name;

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void GetPropertiesWithAttribute_0_Test()
        {
            var actual = AttributeUtility.GetPropertiesWithAttribute<TestAttribute>(Array.Empty<PropertyInfo>());
            Assert.Empty(actual);
        }

        private class A
        {
            [Test("public:1")]
            public int Public { get; set; }
            [Test("protected:1")]
            [Test("protected:2")]
            protected int Protected { get; set; }
            [Test("private:1")]
            [Test("private:2")]
            [Test("private:3")]
            private int Private { get; set; }
        }

        [Fact]
        public void GetPropertiesWithAttribute_A_Test()
        {
            var properies = typeof(A).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            var actual = AttributeUtility.GetPropertiesWithAttribute<TestAttribute>(properies).ToArray();
            Assert.Equal(3, actual.Length);

            //public
            var actualPublic = actual.First(a => a.Property.Name == nameof(A.Public));
            Assert.Single(actualPublic.Attributes);
            Assert.Contains(actualPublic.Attributes, a => a.Name == "public:1");

            //protected
            var actualProtected = actual.First(a => a.Property.Name == "Protected");
            Assert.Equal(2, actualProtected.Attributes.Count);
            Assert.Contains(actualProtected.Attributes, a => a.Name == "protected:1");
            Assert.Contains(actualProtected.Attributes, a => a.Name == "protected:2");

            //private
            var actualPrivate = actual.First(a => a.Property.Name == "Private");
            Assert.Equal(3, actualPrivate.Attributes.Count);
            Assert.Contains(actualPrivate.Attributes, a => a.Name == "private:1");
            Assert.Contains(actualPrivate.Attributes, a => a.Name == "private:2");
            Assert.Contains(actualPrivate.Attributes, a => a.Name == "private:3");
        }

        #endregion
    }
}
