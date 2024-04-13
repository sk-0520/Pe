using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Binding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Binding
{
    public class SimpleModelViewModelBaseTest
    {
        #region function

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
                Assert.Equal(nameof(vm.PropertyA), e.PropertyName);
                called = true;
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
                Assert.Equal(nameof(vm.PropertyB), e.PropertyName);
                called = true;
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
