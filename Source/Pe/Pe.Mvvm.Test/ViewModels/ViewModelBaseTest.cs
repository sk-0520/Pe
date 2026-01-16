using ContentTypeTextNet.Pe.Mvvm.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.ViewModels
{
    public class ViewModelBaseTest
    {
        #region function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "OK")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "OK")]
        private class TestModel
        {
            private int PrivateValue { get; set; }
            public int PublicValue { get; set; }

            public int GetPrivateValue() => PrivateValue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "OK")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "OK")]
        private class TestNestedModel
        {
            public int Value { get; set; }
        }

        private class TestViewModel: ViewModelBase
        {
            public TestViewModel()
                : base(NullLoggerFactory.Instance)
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
                if(e.PropertyName != nameof(tvm.HasErrors)) {
                    Assert.Equal(nameof(TestViewModel.PublicValue), e.PropertyName);
                    called = true;
                }
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
                if(e.PropertyName != nameof(tvm.HasErrors)) {
                    Assert.Equal(nameof(TestViewModel.PrivateValue), e.PropertyName);
                    called = true;
                }
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
                if(e.PropertyName != nameof(tvm.HasErrors)) {
                    Assert.Equal(nameof(TestViewModel.AliasValue), e.PropertyName);
                    called = true;
                }
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
                if(e.PropertyName != nameof(tvm.HasErrors)) {
                    callCount += 1;
                }
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
