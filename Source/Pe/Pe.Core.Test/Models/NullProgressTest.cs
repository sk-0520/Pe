using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;
using Xunit;

// こいつの評価基準はモックをありゃこりゃするやつなんですよ

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class NullDoubleProgressTest
    {
        #region function

        [Fact]
        public void ReportTest()
        {
            var mockLog = MockLog.Create();

            mockLog.VerifyFactoryNever();
            mockLog.VerifyLogNever();

            var test = new NullDoubleProgress(mockLog.Factory);
            mockLog.VerifyFactory(1);
            mockLog.VerifyLogNever();

            test.Report(0.5);
            mockLog.VerifyMessage(LogLevel.Trace, "0.5", 0);
            mockLog.VerifyMessage(LogLevel.Debug, "0.5", 1);
            mockLog.VerifyMessage(LogLevel.Information, "0.5", 0);
            mockLog.VerifyMessage(LogLevel.Warning, "0.5", 0);
            mockLog.VerifyMessage(LogLevel.Error, "0.5", 0);
            mockLog.VerifyMessage(LogLevel.Critical, "0.5", 0);

            test.Report(0.75);
            mockLog.VerifyMessage(LogLevel.Trace, "0.75", 0);
            mockLog.VerifyMessage(LogLevel.Debug, "0.75", 1);
            mockLog.VerifyMessage(LogLevel.Information, "0.75", 0);
            mockLog.VerifyMessage(LogLevel.Warning, "0.75", 0);
            mockLog.VerifyMessage(LogLevel.Error, "0.75", 0);
            mockLog.VerifyMessage(LogLevel.Critical, "0.75", 0);

            test.Report(0.75);
            mockLog.VerifyMessage(LogLevel.Debug, "0.75", 2);
            mockLog.VerifyMessageStartsWith(LogLevel.Debug, "0.7", 2);
            mockLog.VerifyMessageEndsWith(LogLevel.Debug, ".75", 2);
            mockLog.VerifyMessageContains(LogLevel.Debug, "7", 2);
            mockLog.VerifyMessageRegex(LogLevel.Debug, new TestRegex("\\d\\.\\d{2}"), 2);
        }

        #endregion
    }

    public class NullStringProgressTest
    {
        #region function

        [Fact]
        public void ReportTest()
        {
            var mockLog = MockLog.Create();

            mockLog.VerifyFactoryNever();
            mockLog.VerifyLogNever();

            var test = new NullStringProgress(mockLog.Factory);
            mockLog.VerifyFactory(1);
            mockLog.VerifyLogNever(0);

            test.Report("abc");
            mockLog.VerifyMessage(LogLevel.None, "abc", 1);
        }

        #endregion
    }

}
