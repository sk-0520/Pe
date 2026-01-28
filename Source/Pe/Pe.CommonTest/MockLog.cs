using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// ログ系のモック。
    /// </summary>
    public class MockLog
    {
        public MockLog(ILoggerFactory loggerFactory, ILogger logger)
        {
            Factory = loggerFactory;
            Logger = logger;
        }

        #region property

        public ILoggerFactory Factory { get; }
        public ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// 生成。
        /// </summary>
        /// <remarks>原則これを使用する。</remarks>
        /// <returns></returns>
        public static MockLog Create(LogLevel logLevel = LogLevel.None)
        {
            var mockLogger = Substitute.For<ILogger>();
            mockLogger.IsEnabled(default)
                .Returns(a => logLevel == LogLevel.None || a.Arg<LogLevel>() == logLevel)
            ;

            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            mockLoggerFactory
                .CreateLogger(Arg.Any<string>())
                .ReturnsForAnyArgs(mockLogger)
            ;

            return new MockLog(mockLoggerFactory, mockLogger);
        }

        /// <summary>
        /// <see cref="Factory"/> は呼び出されなかった。
        /// </summary>
        public void VerifyFactoryNever()
        {
            Factory.DidNotReceive().CreateLogger(Arg.Any<string>());
        }

        /// <summary>
        /// <see cref="Factory"/> が呼び出された。
        /// </summary>
        /// <param name="times"></param>
        public void VerifyFactory(int times)
        {
            Factory.Received(times).CreateLogger(Arg.Any<string>());
        }

        /// <summary>
        /// <see cref="Logger"/>の<see cref="ILogger.Log"/> は呼び出されなかった。
        /// </summary>
        /// <param name="logLevel">対象ログレベル。<see cref="LogLevel.None"/>は全てを指す。</param>
        public void VerifyLogNever(LogLevel logLevel = LogLevel.None)
        {
            Logger.DidNotReceive().Log(
                Arg.Is<LogLevel>(a => logLevel == LogLevel.None || a == logLevel),
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>()
            );
        }

        /// <summary>
        /// <see cref="Logger"/>の<see cref="ILogger.Log"/> は指定回数呼び出された。
        /// </summary>
        /// <param name="logLevel">対象ログレベル。<see cref="LogLevel.None"/>は全てを指す。</param>
        /// <param name="times"></param>
        public void VerifyLog(LogLevel logLevel, int times)
        {
            Logger.Received(times).Log(
                Arg.Is<LogLevel>(a => logLevel == LogLevel.None || a == logLevel),
                Arg.Any<EventId>(),
                Arg.Any<Arg.AnyType>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<Arg.AnyType, Exception?, string>>()
            );
        }

        /// <summary>
        /// <see cref="Logger"/>の<see cref="ILogger.Log"/> のメッセージを検証。
        /// </summary>
        /// <param name="logLevel">対象ログレベル。<see cref="LogLevel.None"/>は全てを指す。</param>
        /// <param name="predicate">判定処理。</param>
        /// <param name="times"></param>
        public void VerifyMessagePredicate(LogLevel logLevel, Predicate<string> predicate, int times)
        {
            Logger.Received(times).Log(
                Arg.Is<LogLevel>(a => logLevel == LogLevel.None || a == logLevel),
                Arg.Any<EventId>(),
                Arg.Is<object>(a => predicate(a.ToString()!)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>()
            );

        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">完全一致となるメッセージ。</param>
        public void VerifyMessage(LogLevel logLevel, string message, int times)
        {
            VerifyMessagePredicate(logLevel, a => message == a, times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">前方一致となるメッセージ。</param>
        public void VerifyMessageStartsWith(LogLevel logLevel, string message, int times)
        {
            VerifyMessagePredicate(logLevel, a => a.StartsWith(message), times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">後方一致となるメッセージ。</param>
        public void VerifyMessageEndsWith(LogLevel logLevel, string message, int times)
        {
            VerifyMessagePredicate(logLevel, a => a.EndsWith(message), times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">部分一致となるメッセージ。</param>
        public void VerifyMessageContains(LogLevel logLevel, string message, int times)
        {
            VerifyMessagePredicate(logLevel, a => a.Contains(message, StringComparison.Ordinal), times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="regex">メッセージに対する一致パターン。</param>
        public void VerifyMessageRegex(LogLevel logLevel, Regex regex, int times)
        {
            VerifyMessagePredicate(logLevel, a => regex.IsMatch(a), times);
        }

        #endregion
    }
}
