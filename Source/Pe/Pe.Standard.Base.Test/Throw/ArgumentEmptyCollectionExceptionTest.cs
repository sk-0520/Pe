using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base.Throw;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test.Throw
{
    public class ArgumentEmptyCollectionExceptionTest
    {
        #region function

        [Fact]
        public void Constructor_string_Test()
        {
            var actual = new ArgumentEmptyCollectionException("param");
            Assert.Equal("param", actual.ParamName);
            Assert.Equal("empty sequence (Parameter 'param')", actual.Message);
        }

        [Fact]
        public void Constructor_string_string_Test()
        {
            var actual = new ArgumentEmptyCollectionException("param", "message");
            Assert.Equal("param", actual.ParamName);
            Assert.Equal("message (Parameter 'param')", actual.Message);
        }

        [Fact]
        public void ThrowIfEmpty_null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentEmptyCollectionException.ThrowIfEmpty((IEnumerable<int>)null!, "param"));
        }

        public static IEnumerable<object[]> ThrowIfEmpty_zero_Data()
        {
            yield return new object[] { Array.Empty<int>() };
            yield return new object[] { new Collection<int>() };
            yield return new object[] { new List<int>() };
        }

        [Theory]
        [MemberData(nameof(ThrowIfEmpty_zero_Data))]
        public void ThrowIfEmpty_zero_Test(IEnumerable<int> seq)
        {
            Assert.Throws<ArgumentEmptyCollectionException>(() => ArgumentEmptyCollectionException.ThrowIfEmpty(seq, "param"));
        }

        #endregion
    }
}
