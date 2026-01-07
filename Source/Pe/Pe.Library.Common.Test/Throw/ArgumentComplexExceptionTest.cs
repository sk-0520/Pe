using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common.Throw;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test.Throw
{
    public class ArgumentComplexParameterNameExceptionTest
    {
        #region function

        [Fact]
        public void Constructor_Test()
        {
            var ex = new ArgumentComplexParameterNameException(1);
            Assert.Equal("Parameter name[1] is null or empty.", ex.Message);
            Assert.Equal(1, ex.Index);
        }

        #endregion
    }

    public class ArgumentComplexExceptionTest
    {
        #region function

        [Fact]
        public void Constructor_2_Test()
        {
            var ex = new ArgumentComplexException("message", "param1", "param2");
            Assert.Equal("message (Parameter 'param1, param2')", ex.Message);
            Assert.Equal("param1, param2", ex.ParamName);
            Assert.Equal("param1", ex.ParamNames[0]);
            Assert.Equal("param2", ex.ParamNames[1]);
        }

        [Theory]
        [InlineData(0, null!, "param2")]
        [InlineData(0, "", "param2")]
        [InlineData(0, " ", "param2")]
        [InlineData(1, "param1", null)]
        [InlineData(1, "param1", "")]
        [InlineData(1, "param1", " ")]
        public void Constructor_2_throw_Test(int expected, string? paramName1, string? paramName2)
        {
            var exception = Assert.Throws<ArgumentComplexParameterNameException>(()
                => new ArgumentComplexException("message", paramName1!, paramName2!)
            );
            Assert.Equal(expected, exception.Index);
            Assert.Equal($"Parameter name[{expected}] is null or empty.", exception.Message);
        }

        [Fact]
        public void Constructor_3_Test()
        {
            var ex = new ArgumentComplexException("message", "param1", "param2", "param3");
            Assert.Equal("message (Parameter 'param1, param2, param3')", ex.Message);
            Assert.Equal("param1, param2, param3", ex.ParamName);
            Assert.Equal(3, ex.ParamNames.Count);
            Assert.Equal("param1", ex.ParamNames[0]);
            Assert.Equal("param2", ex.ParamNames[1]);
            Assert.Equal("param3", ex.ParamNames[2]);
        }

        [Fact]
        public void Constructor_4_Test()
        {
            var ex = new ArgumentComplexException("message", "param1", "param2", "param3", "param4");
            Assert.Equal("message (Parameter 'param1, param2, param3, param4')", ex.Message);
            Assert.Equal("param1, param2, param3, param4", ex.ParamName);
            Assert.Equal(4, ex.ParamNames.Count);
            Assert.Equal("param1", ex.ParamNames[0]);
            Assert.Equal("param2", ex.ParamNames[1]);
            Assert.Equal("param3", ex.ParamNames[2]);
            Assert.Equal("param4", ex.ParamNames[3]);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "<保留中>")]
        [Theory]
        [InlineData(0, null!, "param2", "param3", "param4", "param5")]
        [InlineData(1, "param1", null!, "param3", "param4", "param5")]
        [InlineData(2, "param1", "param2", null!, "param4", "param5")]
        [InlineData(3, "param1", "param2", "param3", null!, "param5")]
        [InlineData(4, "param1", "param2", "param3", "param4", null!)]
        public void Constructor_5_throw_Test(int expected, string? paramName1, string? paramName2, string? paramName3, string? paramName4, string? paramName5)
        {
            var exception = Assert.Throws<ArgumentComplexParameterNameException>(()
                => new ArgumentComplexException("message", paramName1!, paramName2!, paramName3!, paramName4!, paramName5!)
            );
            Assert.Equal(expected, exception.Index);
            Assert.Equal($"Parameter name[{expected}] is null or empty.", exception.Message);
        }

        #endregion
    }
}
