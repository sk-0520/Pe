using System;
using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest.Test
{
    public class PrivateObjectTest
    {
        #region define

        private sealed class Class
        {
            #region variable

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:読み取り専用修飾子を追加します", Justification = "<保留中>")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
#pragma warning disable 0169
            private int Field;
#pragma warning restore 0169

            #endregion

            #region property

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private int Property { get; set; }

            #endregion

            #region function

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private void Method()
            {
                //nop
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private int Method(int a)
            {
                return a + a;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private IReadOnlyList<int> ReadOnlyListMethod(IReadOnlyList<int> values)
            {
                return values;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private void ThrowingMethod()
            {
                throw new InvalidOperationException(nameof(ThrowingMethod));
            }
            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void FieldTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Throws<PrivateObjectFieldException>(() => test.SetField("<Field>", null));
            Assert.Throws<PrivateObjectFieldException>(() => test.GetField("<Field>"));

            Assert.Equal(0, test.GetField("Field"));
            test.SetField("Field", 123);
            Assert.Equal(123, test.GetField("Field"));
        }

        [Fact]
        public void PropertyTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Throws<PrivateObjectPropertyException>(() => test.SetProperty("<Property>", null));
            Assert.Throws<PrivateObjectPropertyException>(() => test.GetProperty("<Property>"));

            Assert.Equal(0, test.GetProperty("Property"));
            test.SetProperty("Property", 123);
            Assert.Equal(123, test.GetProperty("Property"));
        }

        [Fact]
        public void MethodTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Null(test.Invoke("Method"));
            Assert.Equal(2, test.Invoke("Method", 1));
            Assert.Equal(2, test.Invoke("Method", 1));

            Assert.Throws<PrivateObjectMethodException>(() => test.Invoke("<Method>"));
            Assert.Throws<PrivateObjectMethodParametersException>(() => test.Invoke("Method", (short)1));
            Assert.Throws<PrivateObjectMethodParametersException>(() => test.Invoke("Method", (long)1));
            Assert.Throws<PrivateObjectMethodParametersException>(() => test.Invoke("Method", (float)1));
            Assert.Throws<PrivateObjectMethodParametersException>(() => test.Invoke("Method", (double)1));
            Assert.Throws<PrivateObjectMethodParametersException>(() => test.Invoke("Method", (decimal)1));
            Assert.Throws<PrivateObjectMethodParametersException>(() => test.Invoke("Method", "A"));
        }
        [Fact]
        public void Invoke_ReadOnlyListArgument_Test()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);
            var values = new List<int> { 1, 2, 3 };

            Assert.Same(values, test.Invoke("ReadOnlyListMethod", values));
        }

        [Fact]
        public void Invoke_Exception_Test()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            var exception = Assert.Throws<InvalidOperationException>(
                () => test.Invoke("ThrowingMethod"));

            Assert.Equal("ThrowingMethod", exception.Message);
        }

        #endregion
    }
}
