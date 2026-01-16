using System.Windows.Input;
using ContentTypeTextNet.Pe.Mvvm.Commands;
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
            public TestViewModel(PropertyMode propertyMode)
                : base(propertyMode, NullLoggerFactory.Instance)
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

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void SetProperty_public_Test(PropertyMode propertyMode)
        {
            var tvm = new TestViewModel(propertyMode);
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

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void SetProperty_private_Test(PropertyMode propertyMode)
        {
            var tvm = new TestViewModel(propertyMode);
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

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void SetProperty_alias_Test(PropertyMode propertyMode)
        {
            var tvm = new TestViewModel(propertyMode);
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

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void SetProperty_equal_Test(PropertyMode propertyMode)
        {
            var tvm = new TestViewModel(propertyMode);
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

        private sealed class PropertyChanged_Property_ViewModel: ViewModelBase
        {
            public PropertyChanged_Property_ViewModel(PropertyMode propertyMode)
                : base(propertyMode, NullLoggerFactory.Instance)
            { }

            public int A
            {
                get;
                set => SetProperty(ref field, value);
            }

            public int B
            {
                get;
                set => SetProperty(ref field, value);
            }

            [ObserveProperty(nameof(A))]
            [ObserveProperty(nameof(B))]
            public int C => A + B;

        }

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void ViewModelBase_PropertyChanged_Property_Test(PropertyMode propertyMode)
        {
            var aCalledCount = 0;
            var bCalledCount = 0;
            var cCalledCount = 0;

            var vm = new PropertyChanged_Property_ViewModel(propertyMode);
            vm.PropertyChanged += (s, e) => {
                if(e.PropertyName == nameof(vm.A)) {
                    aCalledCount += 1;
                }

                if(e.PropertyName == nameof(vm.B)) {
                    bCalledCount += 1;
                }

                if(e.PropertyName == nameof(vm.C)) {
                    cCalledCount += 1;
                }
            };

            Assert.Equal(0, aCalledCount);
            Assert.Equal(0, bCalledCount);
            Assert.Equal(0, cCalledCount);
            Assert.Equal(0, vm.A);
            Assert.Equal(0, vm.B);
            Assert.Equal(0, vm.C);

            vm.A = 10;
            Assert.Equal(1, aCalledCount);
            Assert.Equal(0, bCalledCount);
            Assert.Equal(1, cCalledCount);
            Assert.Equal(10, vm.A);
            Assert.Equal(0, vm.B);
            Assert.Equal(10, vm.C);

            vm.B = 20;
            Assert.Equal(1, aCalledCount);
            Assert.Equal(1, bCalledCount);
            Assert.Equal(2, cCalledCount);
            Assert.Equal(10, vm.A);
            Assert.Equal(20, vm.B);
            Assert.Equal(30, vm.C);
        }

        private sealed class PropertyChanged_Command_ViewModel: ViewModelBase
        {
            public PropertyChanged_Command_ViewModel(PropertyMode propertyMode)
                : base(propertyMode, NullLoggerFactory.Instance)
            { }

            public bool A
            {
                get;
                set => SetProperty(ref field, value);
            }

            public bool B
            {
                get;
                set => SetProperty(ref field, value);
            }

            [ObserveProperty(nameof(A))]
            [ObserveProperty(nameof(B))]
            public ICommand C
            {
                get => field ??= new DelegateCommand(
                    _ => { },
                    _ => A && B
                );
            }

        }

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void ViewModelBase_PropertyChanged_Command_Test(PropertyMode propertyMode)
        {
            var aCalledCount = 0;
            var bCalledCount = 0;
            var cCalledCount = 0;

            var vm = new PropertyChanged_Command_ViewModel(propertyMode);
            vm.PropertyChanged += (s, e) => {
                if(e.PropertyName == nameof(vm.A)) {
                    aCalledCount += 1;
                }

                if(e.PropertyName == nameof(vm.B)) {
                    bCalledCount += 1;
                }

                if(e.PropertyName == nameof(vm.C)) {
                    cCalledCount += 1;
                }
            };

            Assert.Equal(0, aCalledCount);
            Assert.Equal(0, bCalledCount);
            Assert.Equal(0, cCalledCount);
            Assert.False(vm.A);
            Assert.False(vm.B);
            Assert.False(vm.C.CanExecute(default));

            vm.A = true;
            Assert.Equal(1, aCalledCount);
            Assert.Equal(0, bCalledCount);
            Assert.Equal(0, cCalledCount);
            Assert.True(vm.A);
            Assert.False(vm.B);
            Assert.False(vm.C.CanExecute(default));

            vm.B = true;
            Assert.Equal(1, aCalledCount);
            Assert.Equal(1, bCalledCount);
            Assert.Equal(0, cCalledCount);
            Assert.True(vm.A);
            Assert.True(vm.B);
            Assert.True(vm.C.CanExecute(default));
        }


        #endregion
    }

}
