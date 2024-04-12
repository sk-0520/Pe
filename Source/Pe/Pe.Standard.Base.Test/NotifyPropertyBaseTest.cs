using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class NotifyPropertyBaseTest
    {
        #region function

        internal class TestClass: NotifyPropertyBase
        {
            #region variable

            private int _value;

            #endregion

            #region proeprty

            public int Value
            {
                get => this._value;
                set
                {
                    this._value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }

            #endregion

            #region function

            public void CallRaisePropertyChanged(int v)
            {
                this._value = v;
                RaisePropertyChanged(nameof(Value));
            }

            #endregion
        }

        [Fact]
        public void SubscribeTest()
        {
            var ts = new TestClass();

            int count = 0;

            ts.PropertyChanged += (s, e) => {
                Assert.Equal(nameof(TestClass.Value), e.PropertyName);
                Assert.Equal(count, ts.Value);
            };

            ts.Value = ++count;
            ts.Value = ++count;

            Assert.Equal(count, ts.Value);

            count = 100;
            ts.CallRaisePropertyChanged(count);
            Assert.Equal(count, ts.Value);
        }

        [Fact]
        public void UnsubscribeTest()
        {
            var ts = new TestClass();

            int count = 0;

            ts.Value = ++count;
            ts.Value = ++count;

            Assert.Equal(count, ts.Value);


            count = 100;
            ts.CallRaisePropertyChanged(count);
            Assert.Equal(count, ts.Value);
        }

        #endregion
    }
}
