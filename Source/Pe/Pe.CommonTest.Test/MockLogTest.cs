using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Serialization;
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

            mockLog.Factory.Object.CreateLogger("1");
            mockLog.VerifyFactory(Times.Once());

            mockLog.Factory.Object.CreateLogger("2");
            mockLog.VerifyFactory(Times.Exactly(2));
        }

        [Fact]
        public void Create_Default_VerifyFactoryNever_Throw_Test()
        {
            var mockLog = MockLog.Create();
            mockLog.Factory.Object.CreateLogger("test");
            Assert.Throws<MockException>(() => mockLog.VerifyFactoryNever());
            Assert.Throws<MockException>(() => mockLog.VerifyFactory(Times.Exactly(2)));
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
            var logger = mockLog.Factory.Object.CreateLogger("test");
            logger.Log(LogLevel.Information, "message");
            Assert.Throws<MockException>(() => mockLog.VerifyLogNever());
            mockLog.VerifyLogNever(LogLevel.Trace);
            mockLog.VerifyLogNever(LogLevel.Debug);
            Assert.Throws<MockException>(() => mockLog.VerifyLogNever(LogLevel.Information));
            mockLog.VerifyLogNever(LogLevel.Warning);
            mockLog.VerifyLogNever(LogLevel.Error);
            mockLog.VerifyLogNever(LogLevel.Critical);
        }

        [Fact]
        public void Create_Default_VerifyLog_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");
            logger.Log(LogLevel.Information, "message 1");
            mockLog.VerifyLog(LogLevel.None, Times.Once());
            logger.Log(LogLevel.Information, "message 2");
            mockLog.VerifyLog(LogLevel.None, Times.Exactly(2));
            logger.Log(LogLevel.Warning, "message 3");
            mockLog.VerifyLog(LogLevel.None, Times.Exactly(3));

            Assert.Throws<MockException>(() => mockLog.VerifyLog(LogLevel.Trace, Times.Once()));
            Assert.Throws<MockException>(() => mockLog.VerifyLog(LogLevel.Debug, Times.Once()));
            mockLog.VerifyLog(LogLevel.Information, Times.Exactly(2));
            mockLog.VerifyLog(LogLevel.Warning, Times.Once());
            Assert.Throws<MockException>(() => mockLog.VerifyLog(LogLevel.Error, Times.Once()));
            Assert.Throws<MockException>(() => mockLog.VerifyLog(LogLevel.Critical, Times.Once()));
        }

        [Fact]
        public void Create_Default_VerifyMessagePredicate_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");

            logger.LogInformation("info 1");
            logger.LogInformation("INFO 2");
            logger.LogInformation("Info 3");
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "info 1", Times.Once());
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "INFO 2", Times.Once());
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "Info 3", Times.Once());

            logger.LogInformation("Info 3");
            mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "Info 3", Times.Exactly(2));

            Assert.Throws<MockException>(() => mockLog.VerifyMessagePredicate(LogLevel.None, message => message == "Info 3", Times.Exactly(3)));
        }

        [Fact]
        public void Create_Default_VerifyMessage_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");

            logger.LogTrace("trace");
            logger.LogInformation("info");
            logger.LogError("error");

            mockLog.VerifyMessage(LogLevel.None, "trace", Times.Once());
            mockLog.VerifyMessage(LogLevel.None, "info", Times.Once());
            mockLog.VerifyMessage(LogLevel.None, "error", Times.Once());

            mockLog.VerifyMessage(LogLevel.None, "Trace", Times.Never());
            mockLog.VerifyMessage(LogLevel.None, "information", Times.Never());
            mockLog.VerifyMessage(LogLevel.None, " error", Times.Never());

            mockLog.VerifyMessage(LogLevel.Trace, "trace", Times.Once());
            mockLog.VerifyMessage(LogLevel.Information, "trace", Times.Never());
            mockLog.VerifyMessage(LogLevel.Error, "trace", Times.Never());

            mockLog.VerifyMessage(LogLevel.Trace, "info", Times.Never());
            mockLog.VerifyMessage(LogLevel.Information, "info", Times.Once());
            mockLog.VerifyMessage(LogLevel.Error, "info", Times.Never());

            mockLog.VerifyMessage(LogLevel.Trace, "error", Times.Never());
            mockLog.VerifyMessage(LogLevel.Information, "error", Times.Never());
            mockLog.VerifyMessage(LogLevel.Error, "error", Times.Once());
        }

        [Fact]
        public void Create_Default_VerifyMessageStartsWith_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");

            logger.LogDebug("debug");
            logger.LogWarning("warn");

            mockLog.VerifyMessageStartsWith(LogLevel.None, "de", Times.Once());
            mockLog.VerifyMessageStartsWith(LogLevel.None, "debug", Times.Once());
            mockLog.VerifyMessageStartsWith(LogLevel.None, "debug!", Times.Never());

            mockLog.VerifyMessageStartsWith(LogLevel.None, "wa", Times.Once());
            mockLog.VerifyMessageStartsWith(LogLevel.None, "warn", Times.Once());
            mockLog.VerifyMessageStartsWith(LogLevel.None, "warn!", Times.Never());

            mockLog.VerifyMessageStartsWith(LogLevel.Debug, "de", Times.Once());
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "de", Times.Never());
            mockLog.VerifyMessageStartsWith(LogLevel.Debug, "wa", Times.Never());
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "wa", Times.Once());
        }

        [Fact]
        public void Create_Default_VerifyMessageEndsWith_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");

            logger.LogDebug("debug");
            logger.LogWarning("warn");

            mockLog.VerifyMessageEndsWith(LogLevel.None, "ug", Times.Once());
            mockLog.VerifyMessageEndsWith(LogLevel.None, "debug", Times.Once());
            mockLog.VerifyMessageEndsWith(LogLevel.None, "!debug", Times.Never());

            mockLog.VerifyMessageEndsWith(LogLevel.None, "rn", Times.Once());
            mockLog.VerifyMessageEndsWith(LogLevel.None, "warn", Times.Once());
            mockLog.VerifyMessageEndsWith(LogLevel.None, "!warn", Times.Never());

            mockLog.VerifyMessageEndsWith(LogLevel.Debug, "ug", Times.Once());
            mockLog.VerifyMessageEndsWith(LogLevel.Warning, "ug", Times.Never());
            mockLog.VerifyMessageEndsWith(LogLevel.Debug, "rn", Times.Never());
            mockLog.VerifyMessageEndsWith(LogLevel.Warning, "rn", Times.Once());
        }

        [Fact]
        public void Create_Default_VerifyMessageContains_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");

            logger.LogDebug("debug");
            logger.LogWarning("warn");

            mockLog.VerifyMessageContains(LogLevel.None, "ebu", Times.Once());
            mockLog.VerifyMessageContains(LogLevel.None, "debug", Times.Once());
            mockLog.VerifyMessageContains(LogLevel.None, "debug debug", Times.Never());

            mockLog.VerifyMessageContains(LogLevel.None, "ar", Times.Once());
            mockLog.VerifyMessageContains(LogLevel.None, "warn", Times.Once());
            mockLog.VerifyMessageContains(LogLevel.None, "warn warn", Times.Never());

            mockLog.VerifyMessageContains(LogLevel.Debug, "ebu", Times.Once());
            mockLog.VerifyMessageContains(LogLevel.Warning, "ebu", Times.Never());
            mockLog.VerifyMessageContains(LogLevel.Debug, "ar", Times.Never());
            mockLog.VerifyMessageContains(LogLevel.Warning, "ar", Times.Once());
        }

        [Fact]
        public void Create_Default_VerifyMessageRegex_Test()
        {
            var mockLog = MockLog.Create();
            var logger = mockLog.Factory.Object.CreateLogger("test");

            logger.LogCritical("critical");

            mockLog.VerifyMessageRegex(LogLevel.None, new Regex("^itical$"), Times.Never());
            mockLog.VerifyMessageRegex(LogLevel.None, new Regex("itical"), Times.Once());

            mockLog.VerifyMessageRegex(LogLevel.Information, new Regex("Critical", RegexOptions.IgnoreCase), Times.Never());
            mockLog.VerifyMessageRegex(LogLevel.Information, new Regex("Critical"), Times.Never());
            mockLog.VerifyMessageRegex(LogLevel.Critical, new Regex("Critical", RegexOptions.IgnoreCase), Times.Once());
            mockLog.VerifyMessageRegex(LogLevel.Critical, new Regex("Critical"), Times.Never());
        }


            #endregion
        }
    }
