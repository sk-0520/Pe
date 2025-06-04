using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Mvvm.Bindings;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Bindings
{
    public class NotifyPropertyBaseTest
    {
        #region define

        private sealed class TestNotifyProperty: NotifyPropertyBase
        {
            public TestNotifyProperty(EventReference eventReference)
                : base(eventReference)
            { }

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
        public void OnPropertyChanged_Strong_Test()
        {
            var test = new TestNotifyProperty(EventReference.Strong);
            Assert.PropertyChanged(test, "Property", () => {
                test.On("Property");
            });

            test.On("Property");
        }

        [Fact]
        public void RaisePropertyChanged_Strong_Test()
        {
            var test = new TestNotifyProperty(EventReference.Strong);
            Assert.PropertyChanged(test, "Property", () => {
                test.Raise("Property");
            });
        }

        [Fact]
        public void OnPropertyChanged_Weak_Test()
        {
            var test = new TestNotifyProperty(EventReference.Weak);
            Assert.PropertyChanged(test, "Property", () => {
                test.On("Property");
            });

            test.On("Property");
        }

        [Fact]
        public void RaisePropertyChanged_Weak_Test()
        {
            var test = new TestNotifyProperty(EventReference.Weak);
            Assert.PropertyChanged(test, "Property", () => {
                test.Raise("Property");
            });
        }

        #endregion
    }
}
