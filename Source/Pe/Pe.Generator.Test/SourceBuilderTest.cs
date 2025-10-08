using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "<保留中>")]
public enum EmptyNamespaceEnum
{
    A,
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "<保留中>")]
public class EmptyNamespaceClass
{
}

namespace ContentTypeTextNet.Pe.Generator.Test
{
    public enum TestEnum
    {
        A
    }

    public class TestClass
    {
    }

    public class SourceBuilderTest
    {
        #region define

        public enum InnerEnum
        {
            A
        }

        public class InnerClass
        {
        }

        #endregion

        #region function

        [Theory]
        [InlineData("global::EmptyNamespaceEnum.A", EmptyNamespaceEnum.A)]
        [InlineData("global::ContentTypeTextNet.Pe.Generator.Test.TestEnum.A", TestEnum.A)]
        [InlineData("global::ContentTypeTextNet.Pe.Generator.Test.SourceBuilderTest+InnerEnum.A", InnerEnum.A)]
        public void ToCode_enum_Test(string expected, Enum input)
        {
            var sourceBuilder = new SourceBuilder();
            var actual = sourceBuilder.ToCode(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToCode_EmptyNamespaceClass_Test()
        {
            var sourceBuilder = new SourceBuilder();
            var actual = sourceBuilder.ToCode<EmptyNamespaceClass>();
            var expected = "global::EmptyNamespaceClass";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToCode_TestClass_Test()
        {
            var sourceBuilder = new SourceBuilder();
            var actual = sourceBuilder.ToCode<TestClass>();
            var expected = "global::ContentTypeTextNet.Pe.Generator.Test.TestClass";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToCode_InnerClass_Test()
        {
            var sourceBuilder = new SourceBuilder();
            var actual = sourceBuilder.ToCode<InnerClass>();
            var expected = "global::ContentTypeTextNet.Pe.Generator.Test.SourceBuilderTest+InnerClass";
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
