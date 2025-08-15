using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class MathUtilityTest
    {
        #region function

        [Theory]
        [InlineData(true, 1.0f, 1.0f, 0.00001f)]
        [InlineData(true, 1.0f, 1.000001f, 0.00001f)]
        [InlineData(true, 1.000001f, 1.0f, 0.00001f)]
        [InlineData(true, 1.0f, 1.1f, 0.5f)]
        [InlineData(true, 1.1f, 1.0f, 0.5f)]
        [InlineData(false, 1.0f, 1.1f, 0.00001f)]
        [InlineData(false, 1.1f, 1.0f, 0.00001f)]
        public void AlmostEquals_Float_Test(bool expected, float a, float b, float epsilon)
        {
            var actual = MathUtility.AlmostEquals(a, b, epsilon);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(true, 1.0d, 1.0d, 0.00001d)]
        [InlineData(true, 1.0d, 1.000001d, 0.00001d)]
        [InlineData(true, 1.000001d, 1.0d, 0.00001d)]
        [InlineData(true, 1.0d, 1.1d, 0.5d)]
        [InlineData(true, 1.1d, 1.0d, 0.5d)]
        [InlineData(false, 1.0d, 1.1d, 0.00001d)]
        [InlineData(false, 1.1d, 1.0d, 0.00001d)]
        public void AlmostEquals_Double_Test(bool expected, double a, double b, double epsilon)
        {
            var actual = MathUtility.AlmostEquals(a, b, epsilon);
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
