using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Bindings;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Bindings
{
    public class NotifyPropertyBaseTest
    {
        #region define

        private sealed class TestNotifyProperty: NotifyPropertyBase
        {
            public void On(string name)
            {
                OnPropertyChanged(name);
            }

            public void Raise(string name)
            {
                RaisePropertyChanged(name);
            }
        }

        #endregion

        #region function

        [Fact]
        public void OnPropertyChangedTest()
        {
            var test = new TestNotifyProperty();
            Assert.PropertyChanged(test, "Property", () => {
                test.On("Property");
            });

            test.On("Property");
        }

        [Fact]
        public void RaisePropertyChangedTest()
        {
            var test = new TestNotifyProperty();
            Assert.PropertyChanged(test, "Property", () => {
                test.Raise("Property");
            });
        }

        #endregion
    }
}
