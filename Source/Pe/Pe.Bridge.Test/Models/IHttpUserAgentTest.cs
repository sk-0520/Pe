using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using NSubstitute;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
{
    public class IHttpUserAgentExtensionsTest
    {
        #region function

        [Fact]
        public async Task GetAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                })
            ;
            var actual = await mock.GetAsync(default!, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task PostAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Post), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                })
            ;
            var actual = await mock.PostAsync(default!, default!, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task PutAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Put), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                })
            ;
            var actual = await mock.PutAsync(default!, default!, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Delete), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                })
            ;
            var actual = await mock.DeleteAsync(default!, default!);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task GetStringAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("abc")
                })
            ;
            var actual = await mock.GetStringAsync(default!, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal("abc", actual);
        }

        [Fact]
        public async Task GetStreamAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                })
            ;
            using var stream = await mock.GetStreamAsync(default!, cancellationToken: TestContext.Current.CancellationToken);
            var actual = new byte[stream.Length];
            await stream.ReadExactlyAsync(actual, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, actual);
        }

        [Fact]
        public async Task GetByteArrayAsyncTest()
        {
            var mock = Substitute.For<IHttpUserAgent>();
            mock
                .SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                })
            ;
            var actual = await mock.GetByteArrayAsync(default!, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, actual);
        }

        #endregion
    }
}
