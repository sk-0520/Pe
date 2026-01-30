using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Common;
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

        private class SimpleVariableValidateViewModel: ViewModelBase
        {
            public SimpleVariableValidateViewModel(PropertyMode propertyMode)
                : base(propertyMode, NullLoggerFactory.Instance)
            { }

            #region property

            [MaxLength(3)]
            public string Name
            {
                get => field;
                set => SetProperty(ref field, value);
            } = string.Empty;

            #endregion
        }

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void SimpleVariableValidateTest(PropertyMode propertyMode)
        {
            var vm = new SimpleVariableValidateViewModel(propertyMode);

            vm.Name = "abc";
            Assert.Equal("abc", vm.Name);
            Assert.False(vm.HasErrors);

            vm.Name = "abcd";
            Assert.Equal("abcd", vm.Name);
            Assert.True(vm.HasErrors);
            Assert.Single(vm.GetErrors(nameof(vm.Name)));

            vm.Name = "";
            Assert.Equal("", vm.Name);
            Assert.False(vm.HasErrors);
            Assert.Empty(vm.GetErrors(nameof(vm.Name)));
        }

        private class SimplePropertyValidateModel
        {
            #region property

            public string Name { get; set; } = string.Empty;

            #endregion
        }

        private class SimplePropertyValidateViewModel: ViewModelBase
        {
            public SimplePropertyValidateViewModel(SimplePropertyValidateModel model, PropertyMode propertyMode)
                : base(propertyMode, NullLoggerFactory.Instance)
            {
                Model = model;
            }

            #region property

            private SimplePropertyValidateModel Model { get; }

            [MaxLength(3)]
            public string Name
            {
                get => Model.Name;
                set => SetProperty(Model, value);
            }

            #endregion
        }

        [Theory]
        [InlineData(PropertyMode.Reflection)]
        [InlineData(PropertyMode.Cache)]
        public void SimplePropertyValidateTest(PropertyMode propertyMode)
        {
            var model = new SimplePropertyValidateModel();
            var vm = new SimplePropertyValidateViewModel(model, propertyMode);

            vm.Name = "abc";
            Assert.Equal("abc", vm.Name);
            Assert.False(vm.HasErrors);

            vm.Name = "abcd";
            Assert.Equal("abcd", vm.Name);
            Assert.True(vm.HasErrors);
            Assert.Single(vm.GetErrors(nameof(vm.Name)));

            vm.Name = "";
            Assert.Equal("", vm.Name);
            Assert.False(vm.HasErrors);
            Assert.Empty(vm.GetErrors(nameof(vm.Name)));
        }

        public enum ObservePropertyOperator
        {
            Add,
            Sub,
            Mul,
            Div
        }

        private sealed class ObservePropertyViewModel: ViewModelBase
        {
            #region variable

            private ObservePropertyOperator _operator = (ObservePropertyOperator)(-1);

            #endregion
            public ObservePropertyViewModel(PropertyMode propertyMode)
                : base(propertyMode, NullLoggerFactory.Instance)
            { }

            #region property

            public int A
            {
                get => field;
                set => SetProperty(ref field, value);
            }

            public int B
            {
                get => field;
                set => SetProperty(ref field, value);
            }

            public ObservePropertyOperator Operator
            {
                get => this._operator;
                set => SetProperty(ref this._operator, value);
            }

            [ObserveProperty(nameof(A))]
            [ObserveProperty(nameof(B))]
            [ObserveProperty(nameof(Operator))]
            public string Reactive
            {
                get
                {
                    string op;
                    int result;
                    switch(Operator) {
                        case ObservePropertyOperator.Add:
                            op = "+";
                            result = A + B;
                            break;

                        case ObservePropertyOperator.Sub:
                            op = "-";
                            result = A - B;
                            break;

                        case ObservePropertyOperator.Mul:
                            op = "*";
                            result = A * B;
                            break;

                        case ObservePropertyOperator.Div:
                            op = "/";
                            result = A / B;
                            break;

                        default:
                            throw new NotImplementedException();
                    }

                    return $"{A} {op} {B} = {result}";
                }
            }

            #endregion
        }

        public static TheoryData<PropertyMode, (string expected, int a, ObservePropertyOperator op, int b)> ObservePropertyData = new MatrixTheoryData<PropertyMode, (string, int, ObservePropertyOperator, int)>(
            [PropertyMode.Reflection, PropertyMode.Cache],
            [
                // デフォルト値は反応しないので避けるべし
                (
                    "6 + 4 = 10",
                    6,
                    ObservePropertyOperator.Add,
                    4
                ),
                (
                    "1 + 2 = 3",
                    1,
                    ObservePropertyOperator.Add,
                    2
                ),
                (
                    "6 - 4 = 2",
                    6,
                    ObservePropertyOperator.Sub,
                    4
                ),
                (
                    "6 * 4 = 24",
                    6,
                    ObservePropertyOperator.Mul,
                    4
                ),
                (
                    "10 / 5 = 2",
                    10,
                    ObservePropertyOperator.Div,
                    5
                ),
            ]
        );

        [Theory]
        [MemberData(nameof(ObservePropertyData))]
        public void ObservePropertyTest(PropertyMode propertyMode, (string expected, int a, ObservePropertyOperator op, int b) input)
        {
            var vm = new ObservePropertyViewModel(propertyMode);

            int step = 0;
            vm.PropertyChanged += (sender, e) => {
                switch(step) {
                    case 0:
                        Assert.Equal(nameof(vm.A), e.PropertyName);
                        step += 1;
                        break;

                    case 1:
                        Assert.Equal(nameof(vm.Reactive), e.PropertyName);
                        step += 1;
                        break;

                    case 2:
                        Assert.Equal(nameof(vm.Operator), e.PropertyName);
                        step += 1;
                        break;

                    case 3:
                        Assert.Equal(nameof(vm.Reactive), e.PropertyName);
                        step += 1;
                        break;

                    case 4:
                        Assert.Equal(nameof(vm.B), e.PropertyName);
                        step += 1;
                        break;

                    case 5:
                        Assert.Equal(nameof(vm.Reactive), e.PropertyName);
                        step += 1;
                        break;

                    default:
                        Assert.Fail();
                        break;
                }
            };

            vm.A = input.a;
            vm.Operator = input.op;
            vm.B = input.b;
            Assert.Equal(input.expected, vm.Reactive);
            Assert.Equal(6, step);
        }

        #region validate

        private class ValidateViewModelBase: ViewModelBase
        {
            protected ValidateViewModelBase(PropertyMode propertyMode, EventReference propertyChangedEventReference)
                : base(propertyMode, propertyChangedEventReference, DefaultDisposing, NullLoggerFactory.Instance)
            { }
        }

        private sealed class ValidateNest1ViewModel: ValidateViewModelBase
        {
            public ValidateNest1ViewModel(PropertyMode propertyMode, EventReference propertyChangedEventReference)
                : base(propertyMode, propertyChangedEventReference)
            { }

            #region property

            [Required]
            public string? Required
            {
                get => field;
                set => SetProperty(ref field, value);
            }

            #endregion
        }

        private sealed class ValidateNest2ViewModel: ValidateViewModelBase
        {
            #region variable

            public string? stringLength;

            #endregion

            public ValidateNest2ViewModel(PropertyMode propertyMode, EventReference propertyChangedEventReference)
                : base(propertyMode, propertyChangedEventReference)
            { }

            #region property

            [StringLength(3)]
            public string? StringLength
            {
                get => this.stringLength;
                set => SetProperty(ref this.stringLength, value);
            }

            #endregion
        }

        private sealed class ValidateNestViewModel: ValidateViewModelBase
        {
            public ValidateNestViewModel(PropertyMode propertyMode, EventReference propertyChangedEventReference)
                : base(propertyMode, propertyChangedEventReference)
            {
                Nest1 = new ValidateNest1ViewModel(propertyMode, propertyChangedEventReference);
                Nest2 = new ValidateNest2ViewModel(propertyMode, propertyChangedEventReference);
            }

            #region property

            public ValidateNest1ViewModel Nest1 { get; }
            public ValidateNest2ViewModel Nest2 { get; }

            [Range(0, 5)]
            public int Range
            {
                get => field;
                set => SetProperty(ref field, value);
            }

            #endregion
        }

        private sealed class ValidateRootViewModel: ValidateViewModelBase
        {
            public ValidateRootViewModel(PropertyMode propertyMode, EventReference propertyChangedEventReference)
                : base(propertyMode, propertyChangedEventReference)
            {
                Nest = new ValidateNestViewModel(propertyMode, propertyChangedEventReference);
            }

            #region property

            public ValidateNestViewModel Nest { get; }

            [RegularExpression(@"[A-Z]{2}")]
            public string RegularExpression
            {
                get => field;
                set => SetProperty(ref field, value);
            } = "AA";

            #endregion
        }

        #endregion

        public static TheoryData<PropertyMode, EventReference> ValidateData = new MatrixTheoryData<PropertyMode, EventReference>(
            [PropertyMode.Reflection, PropertyMode.Cache],
            [EventReference.Strong, EventReference.Weak]
        );

        [Theory]
        [MemberData(nameof(ValidateData))]
        public void ValidateTest(PropertyMode propertyMode, EventReference propertyChangedEventReference)
        {
            var test = new ValidateRootViewModel(propertyMode, propertyChangedEventReference);

            Assert.False(test.HasErrors);

            Assert.False(test.Validate());
            Assert.False(test.HasErrors);
            Assert.True(test.Nest.Nest1.HasErrors);

            // リセット
            test.Nest.Nest1.Required = "Required";
            Assert.False(test.Nest.Nest1.HasErrors);
            Assert.True(test.Validate());

            // 孫検証
            test.Nest.Nest2.StringLength = "abc";
            Assert.False(test.Nest.Nest2.HasErrors);
            test.Nest.Nest2.StringLength = "abcd";
            Assert.True(test.Nest.Nest2.HasErrors);
            Assert.False(test.HasErrors);
            Assert.False(test.Validate());
            Assert.False(test.HasErrors);

            // リセット
            test.Nest.Nest2.StringLength = "abc";
            Assert.False(test.Nest.Nest2.HasErrors);
            Assert.True(test.Validate());

            // 孫非検証の検証
            test.Nest.Nest2.stringLength = "abcd";
            Assert.False(test.Nest.Nest2.HasErrors); // プロパティ未経由なので未検証
            Assert.False(test.Validate()); // 全検証を通すので検証失敗となる
            Assert.True(test.Nest.Nest2.HasErrors); // 検証後なのでエラーなのだ
            Assert.False(test.HasErrors);

            // リセット
            test.Nest.Nest2.StringLength = "abc";
            Assert.True(test.Validate());
            Assert.False(test.Nest.Nest2.HasErrors);

            // 子検証
            test.Nest.Range = 5;
            Assert.False(test.Nest.HasErrors);
            Assert.False(test.HasErrors);
            Assert.True(test.Validate());
            test.Nest.Range = 0;
            Assert.False(test.Nest.HasErrors);
            Assert.False(test.HasErrors);
            Assert.True(test.Validate());

            test.Nest.Range = -1;
            Assert.True(test.Nest.HasErrors);
            Assert.False(test.HasErrors);
            Assert.False(test.Validate());

            test.Nest.Range = 0;
            Assert.False(test.Nest.HasErrors);
            Assert.False(test.HasErrors);
            Assert.True(test.Validate());

            test.Nest.Range = 6;
            Assert.True(test.Nest.HasErrors);
            Assert.False(test.HasErrors);
            Assert.False(test.Validate());

            // リセット
            test.Nest.Range = 0;
            Assert.False(test.Nest.HasErrors);
            Assert.False(test.HasErrors);
            Assert.True(test.Validate());

            // ルート検証
            test.RegularExpression = "00";
            Assert.True(test.HasErrors);
            Assert.False(test.Validate());

            test.RegularExpression = "ZZ";
            Assert.False(test.HasErrors);
            Assert.True(test.Validate());
        }

        #endregion
    }

}
