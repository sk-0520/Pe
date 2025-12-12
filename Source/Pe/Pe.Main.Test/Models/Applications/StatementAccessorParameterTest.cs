using System;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Applications
{
    public class StatementAccessorParameterTest
    {
        #region function

        [Theory]
        [InlineData(typeof(ArgumentNullException), null)]
        [InlineData(typeof(ArgumentException), "")]
        [InlineData(typeof(ArgumentException), "a")]
        [InlineData(typeof(ArgumentException), "a.")]
        [InlineData(typeof(ArgumentException), ".a")]
        [InlineData(typeof(ArgumentException), ".a.")]
        public void Constructor_Throw_Test(Type exceptionType, string? fullName)
        {
            Assert.Throws(exceptionType,() => new StatementAccessorParameter(fullName!));
        }

        [Theory]
        [InlineData("", "class", "method", "class.method")]
        [InlineData("namespace", "class", "method", "namespace.class.method")]
        [InlineData("namespace.A", "class", "method", "namespace.A.class.method")]
        [InlineData("namespace.A.B", "class", "method", "namespace.A.B.class.method")]
        public void ConstructorTest(string nameSpace, string className, string methodName, string input)
        {
            var actual = new StatementAccessorParameter(input);
            Assert.Equal(nameSpace, actual.Namespace);
            Assert.Equal(className, actual.ClassName);
            Assert.Equal(methodName, actual.MethodName);
        }


        #endregion
    }
}
