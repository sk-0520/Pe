using System.ComponentModel;
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
            #region variable

            public int Number;

            #endregion
            public TestNotifyProperty(EventReference eventReference)
                : base(eventReference)
            { }

            public void On(PropertyChangedEventArgs eventArgs)
            {
                OnPropertyChanged(eventArgs);
            }

            public void On(string propertyName)
            {
                On(new PropertyChangedEventArgs(propertyName));
            }

            public void Raise(string name)
            {
                RaisePropertyChanged(name);
            }

            public void SetNumber(int num)
            {
                SetProperty(ref this.Number, num, nameof(this.Number));
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

        [Fact]
        public void SetVariableTest()
        {
            var test = new TestNotifyProperty(EventReference.Weak);
            Assert.PropertyChanged(test, nameof(test.Number), () => {
                test.SetNumber(100);
                Assert.Equal(100, test.Number);
            });

        }

        #endregion
    }
}
