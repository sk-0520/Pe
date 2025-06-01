using System;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Property.Test
{
    public class PropertyExpressionFactoryTest
    {
        #region define

        private class Get
        {
            private int Property { get; } = 1;
        }

        private class GetCanNotRead
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S4487:Unread \"private\" fields should be removed", Justification = "<保留中>")]
            public int fieldValue;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2376:Write-only properties should not be used", Justification = "<保留中>")]
            public int Property
            {
                set
                {
                    this.fieldValue = value;
                }
            }
        }

        private class GetSet
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "OK")]
            private int Property { get; set; }
        }

        private interface ITypeData
        {
            int Property { get; set; }
        }

        private class TypeData: ITypeData
        {
            public int Property { get; set; }
            private string Custom { get; set; } = string.Empty;
        }

        public class InitField
        {
            public readonly int field;

            public InitField(int field)
            {
                this.field = field;
            }
        }

        #endregion

        #region function

        [Fact]
        public void CreateGetterTest()
        {
            var gi = new Get();
            var go = Property.PropertyExpressionFactory.CreateOwner(gi);
            var propertyGetter = Property.PropertyExpressionFactory.CreateGetter<Get, int>(go, "Property");
            var gi1 = propertyGetter(gi);
            Assert.Equal(1, gi1);
        }

        [Fact]
        public void CreateGetter_Throw_Test()
        {
            var gcmri = new GetCanNotRead();
            var gcmro = Property.PropertyExpressionFactory.CreateOwner(gcmri);
            var exception = Assert.Throws<PropertyCanNotReadException>(() => Property.PropertyExpressionFactory.CreateGetter<GetCanNotRead, int>(gcmro, "Property"));
            Assert.Equal("Property", exception.Member.Name);
        }

        [Fact]
        public void CreateSetterOnlyTest()
        {
            var gsi = new Get();
            var gso = Property.PropertyExpressionFactory.CreateOwner(gsi);
            var exception = Record.Exception(() => Property.PropertyExpressionFactory.CreateGetter<Get, int>(gso, "Property"));
            Assert.Null(exception);
            Assert.Throws<PropertyCanNotWriteException>(() => Property.PropertyExpressionFactory.CreateSetter<Get, int>(gso, "Property"));
        }

        [Fact]
        public void CreateSetterTest()
        {
            var gsi = new GetSet();
            var gso = Property.PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = Property.PropertyExpressionFactory.CreateGetter<GetSet, int>(gso, "Property");
            var propertySetter = Property.PropertyExpressionFactory.CreateSetter<GetSet, int>(gso, "Property");
            propertySetter(gsi, 10);
            var gi1 = propertyGetter(gsi);
            Assert.Equal(10, gi1);
        }

        [Fact]
        public void CreateSetter_InitField_Throw_Test()
        {
            var ifi = new InitField(10);
            var ifo = Property.PropertyExpressionFactory.CreateOwner(ifi);
            var exception = Assert.Throws<PropertyCanNotWriteException>(() => Property.PropertyExpressionFactory.CreateSetter<InitField, int>(ifo, "field"));
            Assert.Equal("field", exception.Member.Name);
        }

        [Fact]
        public void CreateObjectGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = Property.PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = Property.PropertyExpressionFactory.CreateGetter<GetSet, object>(gso, "Property");
            var propertySetter = Property.PropertyExpressionFactory.CreateSetter<GetSet, object>(gso, "Property");
            propertySetter(gsi, 10);
            var gi1 = propertyGetter(gsi);
            Assert.Equal(10, gi1);
        }

        [Fact]
        public void CreateDelegateGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = Property.PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = Property.PropertyExpressionFactory.CreateGetter(gso, "Property");
            var propertySetter = Property.PropertyExpressionFactory.CreateSetter(gso, "Property");
            propertySetter.DynamicInvoke(gsi, 10);
            var gi1 = propertyGetter.DynamicInvoke(gsi);
            Assert.Equal(10, gi1);
        }

        [Fact]
        public void CreateType_interface_Test()
        {
            var td = new TypeData();

            var tdo = Property.PropertyExpressionFactory.CreateOwner<ITypeData>();
            var propertyGetter = Property.PropertyExpressionFactory.CreateGetter(tdo, "Property");
            var propertySetter = Property.PropertyExpressionFactory.CreateSetter(tdo, "Property");

            propertySetter.DynamicInvoke(td, 10);
            var gi1 = propertyGetter.DynamicInvoke(td);
            Assert.Equal(10, gi1);
            Assert.Equal(10, td.Property);
        }

        [Fact]
        public void CreateType_interface_no_property_Test()
        {
            var td = new TypeData();

            var tdo = Property.PropertyExpressionFactory.CreateOwner<ITypeData>();
            // 型から作成しているので存在しないプロパティは許容されない
            Assert.Throws<ArgumentException>(() => Property.PropertyExpressionFactory.CreateGetter(tdo, "Custom"));
            Assert.Throws<ArgumentException>(() => Property.PropertyExpressionFactory.CreateSetter(tdo, "Custom"));
        }


        #endregion
    }
}
