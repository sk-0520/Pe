using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class FreezableUtilityTest
    {
        #region define

        private class TestClass: Freezable
        {
            #region variable

            private readonly bool _canFreeze;

            #endregion

            public TestClass(bool canFreeze)
            {
                this._canFreeze = canFreeze;
            }

            #region Freezable

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
            public new bool CanFreeze => this._canFreeze;

            protected override Freezable CreateInstanceCore()
            {
                return this;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void SafeFreezeTest()
        {
            var test = new TestClass(true);
            var actual = test.SafeFreeze();
            Assert.True(actual);
        }

        [Fact]
        public void SafeFreeze_null_Test()
        {
            TestClass? test = null;
            var actual = test.SafeFreeze();
            Assert.False(actual);
        }

        [Fact]
        public void GetSafeFreezeTest()
        {
            var test = new TestClass(true);
            var actual = test.GetFreezed();
            Assert.Equal(test, actual);
        }

        [Fact]
        public void GetSafeFreeze_null_Test()
        {
            TestClass? test = null;
            var actual = test.GetFreezed();
            Assert.Null(actual);
        }

        #endregion
    }
}
