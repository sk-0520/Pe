using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class UriUtilityTest
    {
        #region function

        [Fact]
        public void CombinePath_Exception_Test()
        {
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(null!, ""));
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://example.com"), null!));
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://example.com"), "", null!));
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://example.com"), "", "", null!));
        }

        [Theory]
        [InlineData("http://example.com/c", "http://example.com", "c")]
        [InlineData("http://example.com:8080/c", "http://example.com:8080", "c")]
        [InlineData("http://u:p@example.com/c", "http://u:p@example.com", "c")]
        [InlineData("http://u:p@example.com:8080/c", "http://u:p@example.com:8080", "c")]
        [InlineData("http://example.com/c", "http://example.com", "c/")]
        [InlineData("http://example.com/c", "http://example.com", "/c/")]
        [InlineData("http://example.com/c", "http://example.com", "//c//")]
        [InlineData("http://example.com/c", "http://example.com/", "//c//")]
        [InlineData("http://example.com/c/d", "http://example.com/c", "//d//")]
        [InlineData("http://example.com/c/d", "http://example.com/c/", "//d//")]
        [InlineData("http://example.com/c/d?q=v", "http://example.com/c?q=v", "//d//")]
        [InlineData("http://example.com/c/d/e/f/g", "http://example.com/", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://example.com/x/y/z/c/d/e/f/g", "http://example.com/x/y/z", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://example.com/x/y/z/c/d/e/f/g", "http://example.com/x/y/z/", "//c//", "d", "/e", "f/", "/g/")]
        public void CombinePathTest(string expected, string url, string path, params string[] paths)
        {
            var uri = UriUtility.CombinePath(new Uri(url), path, paths);
            Assert.Equal(expected, uri.ToString());
        }

        [Theory]
        [InlineData("http://example.com/c/", "http://example.com", "c")]
        [InlineData("http://example.com:8080/c/", "http://example.com:8080", "c")]
        [InlineData("http://u:p@example.com/c/", "http://u:p@example.com", "c")]
        [InlineData("http://u:p@example.com:8080/c/", "http://u:p@example.com:8080", "c")]
        [InlineData("http://example.com/c/", "http://example.com", "c/")]
        [InlineData("http://example.com/c/", "http://example.com", "/c/")]
        [InlineData("http://example.com/c/", "http://example.com", "//c//")]
        [InlineData("http://example.com/c/", "http://example.com/", "//c//")]
        [InlineData("http://example.com/c/d/", "http://example.com/c", "//d//")]
        [InlineData("http://example.com/c/d/", "http://example.com/c/", "//d//")]
        [InlineData("http://example.com/c/d/?q=v", "http://example.com/c?q=v", "//d//")]
        [InlineData("http://example.com/c/d/e/f/g/", "http://example.com/", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://example.com/x/y/z/c/d/e/f/g/", "http://example.com/x/y/z", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://example.com/x/y/z/c/d/e/f/g/", "http://example.com/x/y/z/", "//c//", "d", "/e", "f/", "/g/")]
        public void CombinePath_appendLastSeparator_Test(string expected, string url, string path, params string[] paths)
        {
            var uri = UriUtility.CombinePath(new Uri(url), true, path, paths);
            Assert.Equal(expected, uri.ToString());
        }

        #endregion
    }

}
