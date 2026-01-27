using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NSubstitute.Exceptions;
using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest.Test
{
    public class MockLogTest
    {
        #region function

        [Fact]
        public void Create_Default_None_Test()
        {
            var mockLog = MockLog.Create();
            mockLog.VerifyFactoryNever();
        }

        [Fact]
        public void Create_Default_VerifyFactory_Test()
        {
            var mockLog = MockLog.Create();

            mockLog.Factory.CreateLogger("1");
            mockLog.VerifyFactory(1);

            mockLog.Factory.CreateLogger("2");
            mockLog.VerifyFactory(2);
        }

        [Fact]
        public void Create_Default_VerifyFactoryNever_Throw_Test()
        {
            var mockLog = MockLog.Create();
            mockLog.Factory.CreateLogger("test");
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyFactoryNever());
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyFactory(2));
        }

        [Fact]
        public void Create_Default_VerifyLogNever_Test()
        {
            var mockLog = MockLog.Create();
            mockLog.VerifyLogNever();
        }

        [Fact]
        public void Create_Default_VerifyLogNever_Throw_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");
            logger.Log(LogLevel.Information, "message");
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyLogNever());
            mockLog.VerifyLogNever(LogLevel.Trace);
            mockLog.VerifyLogNever(LogLevel.Debug);
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyLogNever(LogLevel.Information));
            mockLog.VerifyLogNever(LogLevel.Warning);
            mockLog.VerifyLogNever(LogLevel.Error);
            mockLog.VerifyLogNever(LogLevel.Critical);
        }

        [Fact]
        public void Create_Default_VerifyLog_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");
            logger.Log(LogLevel.Information, "message 1");
            mockLog.VerifyLog(LogLevel.None, 1);
            logger.Log(LogLevel.Information, "message 2");
            mockLog.VerifyLog(LogLevel.None, 2);
            logger.Log(LogLevel.Warning, "message 3");
            mockLog.VerifyLog(LogLevel.None, 3);

            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyLog(LogLevel.Trace, 1));
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyLog(LogLevel.Debug, 1));
            mockLog.VerifyLog(LogLevel.Information, 2);
            mockLog.VerifyLog(LogLevel.Warning, 1);
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyLog(LogLevel.Error, 1));
            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyLog(LogLevel.Critical, 1));
        }

        [Fact]
        public void Create_Default_VerifyMessagePredicate_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");

            logger.LogInformation("info 1");
            logger.LogInformation("INFO 2");
            logger.LogInformation("Info 3");
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "info 1", 1);
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "INFO 2", 1);
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "Info 3", 1);

            logger.LogInformation("Info 3");
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "Info 3", 2);

            Assert.Throws<ReceivedCallsException>(() => mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "Info 3", 3));
        }

        [Fact]
        public void Create_Default_VerifyMessage_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");

            logger.LogTrace("trace");
            logger.LogInformation("info");
            logger.LogError("error");

            mockLog.VerifyMessage(LogLevel.None, "trace", 1);
            mockLog.VerifyMessage(LogLevel.None, "info", 1);
            mockLog.VerifyMessage(LogLevel.None, "error", 1);

            mockLog.VerifyMessage(LogLevel.None, "Trace", 0);
            mockLog.VerifyMessage(LogLevel.None, "information", 0);
            mockLog.VerifyMessage(LogLevel.None, " error", 0);

            mockLog.VerifyMessage(LogLevel.Trace, "trace", 1);
            mockLog.VerifyMessage(LogLevel.Information, "trace", 0);
            mockLog.VerifyMessage(LogLevel.Error, "trace", 0);

            mockLog.VerifyMessage(LogLevel.Trace, "info", 0);
            mockLog.VerifyMessage(LogLevel.Information, "info", 1);
            mockLog.VerifyMessage(LogLevel.Error, "info", 0);

            mockLog.VerifyMessage(LogLevel.Trace, "error", 0);
            mockLog.VerifyMessage(LogLevel.Information, "error", 0);
            mockLog.VerifyMessage(LogLevel.Error, "error", 1);
        }

        [Fact]
        public void Create_Default_VerifyMessageStartsWith_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");

            logger.LogDebug("debug");
            logger.LogWarning("warn");

            mockLog.VerifyMessageStartsWith(LogLevel.None, "de", 1);
            mockLog.VerifyMessageStartsWith(LogLevel.None, "debug", 1);
            mockLog.VerifyMessageStartsWith(LogLevel.None, "debug!", 0);

            mockLog.VerifyMessageStartsWith(LogLevel.None, "wa", 1);
            mockLog.VerifyMessageStartsWith(LogLevel.None, "warn", 1);
            mockLog.VerifyMessageStartsWith(LogLevel.None, "warn!", 0);

            mockLog.VerifyMessageStartsWith(LogLevel.Debug, "de", 1);
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "de", 0);
            mockLog.VerifyMessageStartsWith(LogLevel.Debug, "wa", 0);
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "wa", 1);
        }

        [Fact]
        public void Create_Default_VerifyMessageEndsWith_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");

            logger.LogDebug("debug");
            logger.LogWarning("warn");

            mockLog.VerifyMessageEndsWith(LogLevel.None, "ug", 1);
            mockLog.VerifyMessageEndsWith(LogLevel.None, "debug", 1);
            mockLog.VerifyMessageEndsWith(LogLevel.None, "!debug", 0);

            mockLog.VerifyMessageEndsWith(LogLevel.None, "rn", 1);
            mockLog.VerifyMessageEndsWith(LogLevel.None, "warn", 1);
            mockLog.VerifyMessageEndsWith(LogLevel.None, "!warn", 0);

            mockLog.VerifyMessageEndsWith(LogLevel.Debug, "ug", 1);
            mockLog.VerifyMessageEndsWith(LogLevel.Warning, "ug", 0);
            mockLog.VerifyMessageEndsWith(LogLevel.Debug, "rn", 0);
            mockLog.VerifyMessageEndsWith(LogLevel.Warning, "rn", 1);
        }

        [Fact]
        public void Create_Default_VerifyMessageContains_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");

            logger.LogDebug("debug");
            logger.LogWarning("warn");

            mockLog.VerifyMessageContains(LogLevel.None, "ebu", 1);
            mockLog.VerifyMessageContains(LogLevel.None, "debug", 1);
            mockLog.VerifyMessageContains(LogLevel.None, "debug debug", 0);

            mockLog.VerifyMessageContains(LogLevel.None, "ar", 1);
            mockLog.VerifyMessageContains(LogLevel.None, "warn", 1);
            mockLog.VerifyMessageContains(LogLevel.None, "warn warn", 0);

            mockLog.VerifyMessageContains(LogLevel.Debug, "ebu", 1);
            mockLog.VerifyMessageContains(LogLevel.Warning, "ebu", 0);
            mockLog.VerifyMessageContains(LogLevel.Debug, "ar", 0);
            mockLog.VerifyMessageContains(LogLevel.Warning, "ar", 1);
        }

        [Fact]
        public void Create_Default_VerifyMessageRegex_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.CreateLogger("test");

            logger.LogCritical("critical");

            mockLog.VerifyMessageRegex(LogLevel.None, new TestRegex("^itical$"), 0);
            mockLog.VerifyMessageRegex(LogLevel.None, new TestRegex("itical"), 1);

            mockLog.VerifyMessageRegex(LogLevel.Information, new TestRegex("Critical", RegexOptions.IgnoreCase), 0);
            mockLog.VerifyMessageRegex(LogLevel.Information, new TestRegex("Critical"), 0);
            mockLog.VerifyMessageRegex(LogLevel.Critical, new TestRegex("Critical", RegexOptions.IgnoreCase), 1);
            mockLog.VerifyMessageRegex(LogLevel.Critical, new TestRegex("Critical"), 0);
        }

        #endregion
    }
}
