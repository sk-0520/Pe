using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class NewVersionDownloaderTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Http);

        #endregion

        #region function

        [Fact]
        public async Task ChecksumAsync_NotExists_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var mockLog = MockLog.Create();
            var applicationConfiguration = Test.GetApplicationConfiguration(testIO);
            var test = Test.DiContainer.Build<NewVersionDownloader>(applicationConfiguration, mockLog.Factory);

            var actual = await test.ChecksumAsync(
                new NewVersionItemData() {
                },
                new System.IO.FileInfo("NUL"),
                Test.DiContainer.Build<NullNotifyProgress>(),
                CancellationToken.None
            );
            Assert.False(actual);
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "検査ファイルが存在しない:", 1);
        }

        [Fact]
        public async Task ChecksumAsync_NotFileSize_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateTextFile("data.dat", "abc");

            var mockLog = MockLog.Create();
            var applicationConfiguration = Test.GetApplicationConfiguration(testIO);
            var test = Test.DiContainer.Build<NewVersionDownloader>(applicationConfiguration, mockLog.Factory);

            var actual = await test.ChecksumAsync(
                new NewVersionItemData() {
                    ArchiveSize = 0,
                },
                file,
                Test.DiContainer.Build<NullNotifyProgress>(),
                CancellationToken.None
            );
            Assert.False(actual);
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "ファイルサイズが異なる:", 1);
        }

        #endregion
    }
}
