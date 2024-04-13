using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Binding;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Binding
{
    public class ViewModelBaseTest
    {
        #region function

        private class TestModel
        {
            private int PrivateValue { get; set; }
            public int PublicValue { get; set; }

            public int GetPrivateValue() => PrivateValue;
        }

        private class TestNestedModel
        {
            public int Value { get; set; }
        }

        private class TestViewModel: ViewModelBase
        {
            public TestViewModel()
                : base()
            { }

            private TestModel TestModel { get; } = new TestModel();
            private TestNestedModel TestNestedModel { get; } = new TestNestedModel();

            public int PrivateValue
            {
                get => TestModel.GetPrivateValue();
                set => SetProperty(TestModel, value);
            }

            public int PublicValue
            {
                get => TestModel.PublicValue;
                set => SetProperty(TestModel, value);
            }

            public int AliasValue
            {
                get => TestNestedModel.Value;
                set => SetProperty(TestNestedModel, value, nameof(TestNestedModel.Value));
            }
        }

        [Fact]
        public void SetProperty_public_Test()
        {
            var tvm = new TestViewModel();
            bool called = false;
            tvm.PropertyChanged += (s, e) => {
                Assert.Equal(nameof(TestViewModel.PublicValue), e.PropertyName);
                called = true;
            };
            Assert.False(called);
            tvm.PublicValue = 123;
            Assert.True(called);
            Assert.Equal(123, tvm.PublicValue);
        }

        [Fact]
        public void SetProperty_private_Test()
        {
            var tvm = new TestViewModel();
            bool called = false;
            tvm.PropertyChanged += (s, e) => {
                Assert.Equal(nameof(TestViewModel.PrivateValue), e.PropertyName);
                called = true;
            };
            Assert.False(called);
            tvm.PrivateValue = 123;
            Assert.True(called);
            Assert.Equal(123, tvm.PrivateValue);
        }

        [Fact]
        public void SetProperty_alias_Test()
        {
            var tvm = new TestViewModel();
            bool called = false;
            tvm.PropertyChanged += (s, e) => {
                Assert.Equal(nameof(TestViewModel.AliasValue), e.PropertyName);
                called = true;
            };
            Assert.False(called);
            tvm.AliasValue = 123;
            Assert.True(called);
            Assert.Equal(123, tvm.AliasValue);
        }

        [Fact]
        public void SetProperty_equal_Test()
        {
            var tvm = new TestViewModel();
            int callCount = 0;
            tvm.PropertyChanged += (s, e) => {
                callCount += 1;
            };
            tvm.PublicValue = 123;
            Assert.Equal(1, callCount);
            Assert.Equal(123, tvm.PublicValue);

            tvm.PublicValue = 123;
            Assert.Equal(1, callCount);
            Assert.Equal(123, tvm.PublicValue);

            tvm.PublicValue = 456;
            Assert.Equal(2, callCount);
            Assert.Equal(456, tvm.PublicValue);
        }

        //private class TestNotObserveViewModel: ViewModelBase
        //{
        //    int _prop = 0;

        //    public int Prop
        //    {
        //        get => this._prop;
        //        set => SetVariable(ref this._prop, value);
        //    }

        //    public bool PropIsEven => (this._prop % 2) == 0;
        //}



        //private class TestObserveViewModel: ViewModelBase
        //{
        //    int _prop = 0;

        //    public int Prop
        //    {
        //        get => this._prop;
        //        set => SetVariable(ref this._prop, value);
        //    }

        //    public bool PropIsEven => (this._prop % 2) == 0;
        //}

        #endregion
    }
}
