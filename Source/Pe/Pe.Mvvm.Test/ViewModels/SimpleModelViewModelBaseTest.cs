using ContentTypeTextNet.Pe.Mvvm.Bindings;
using ContentTypeTextNet.Pe.Mvvm.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.ViewModels
{
    public class SimpleModelViewModelBaseTest
    {
        #region function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "OK")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "OK")]
        private class TestModel: BindModelBase
        {
            public int PropertyA { get; set; }
            public int PropertyANext { get; set; }
        }

        private class TestSingleModelViewModel: SingleModelViewModelBase<TestModel>
        {
            public TestSingleModelViewModel(TestModel model, ILoggerFactory loggerFactory)
                : base(model, loggerFactory)
            { }

            public int PropertyA
            {
                get => Model.PropertyA;
                set => SetModel(value);
            }

            public int PropertyB
            {
                get => Model.PropertyANext;
                set => SetModel(value, nameof(Model.PropertyANext));
            }
        }

        [Fact]
        public void SetModel_a_Test()
        {
            var model = new TestModel();
            var vm = new TestSingleModelViewModel(model, NullLoggerFactory.Instance);
            bool called = false;
            vm.PropertyChanged += (s, e) => {
                if(e.PropertyName != nameof(vm.HasErrors)) {
                    Assert.Equal(nameof(vm.PropertyA), e.PropertyName);
                    called = true;
                }
            };
            Assert.False(called);
            vm.PropertyA = 123;
            Assert.True(called);
            Assert.Equal(123, vm.PropertyA);
            Assert.Equal(model.PropertyA, vm.PropertyA);
        }

        [Fact]
        public void SetModel_b_Test()
        {
            var model = new TestModel();
            var vm = new TestSingleModelViewModel(model, NullLoggerFactory.Instance!);
            bool called = false;
            vm.PropertyChanged += (s, e) => {
                if(e.PropertyName != nameof(vm.HasErrors)) {
                    Assert.Equal(nameof(vm.PropertyB), e.PropertyName);
                    called = true;
                }
            };
            Assert.False(called);
            vm.PropertyB = 123;
            Assert.True(called);
            Assert.Equal(123, vm.PropertyB);
            Assert.Equal(model.PropertyANext, vm.PropertyB);
        }

        #endregion
    }
}
