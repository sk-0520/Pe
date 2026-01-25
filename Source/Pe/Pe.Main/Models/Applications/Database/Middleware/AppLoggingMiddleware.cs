using System;
using System.Data;
using System.Data.Common;
using System.Text;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware
{
    internal class AppLoggingMiddleware: IExecuteNonQueryMiddleware, IExecuteScalarMiddleware, IExecuteDataReaderMiddleware
    {
        public AppLoggingMiddleware(TimeProvider timeProvider, ILoggerFactory loggerFactory)
        {
            TimeProvider = timeProvider;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private TimeProvider TimeProvider { get; }
        private ILogger Logger { get; }

        #endregion

        #region function

        private static string ToLogSql(string sql)
        {
            var sb = new StringBuilder();
            var lines = TextUtility.ReadLines(sql.AsSpan());
            var i = 0;
            foreach(var line in lines) {
                if(0 < i++) {
                    sb.AppendLine();
                }
                if(line.IsEmpty) {
                    continue;
                }

                sb.Append('\t', 2);
                sb.Append(line);
            }

            return sb.ToString();
        }

        private static string ToLogParameters(DbParameterCollection dbParameter)
        {
            if(dbParameter is null) {
                return "\t\t(null)";
            }
            if(dbParameter.Count == 0) {
                return "\t\t<EMPTY>";
            }

            var sb = new StringBuilder();

            var i = 0;
            foreach(DbParameter p in dbParameter) {
                if(0 < i++) {
                    sb.AppendLine();
                }
                sb.Append('\t', 2);
                sb.Append(p.ParameterName);
                sb.Append('(');
                sb.Append(p.DbType);
                sb.Append(") = ");
            }

            return sb.ToString();
        }

        private T Invoke<T>(Func<DbCommand, T> func, DbCommand command, string interfaceName)
        {
            var timestamp = TimeProvider.GetTimestamp();

            var result = func(command);

            var elapsedTime = TimeProvider.GetElapsedTime(timestamp);
            if(Logger.IsEnabled(LogLevel.Trace)) {
                var format = $$"""
                    [{InterfaceName}] time: {ElapsedTime}
                    {{'\t'}}[SQL]
                    {CommandText}
                    {{'\t'}}[PARAMS]
                    {Parameters}
                    """
                ;
                Logger.LogTrace(
                    format,
                    interfaceName,
                    elapsedTime,
                    ToLogSql(command.CommandText),
                    ToLogParameters(command.Parameters)
                );
            }

            return result;
        }

        #endregion

        #region IExecuteNonQueryMiddleware

        public int Next(IExecuteNonQueryHandler handler, DbCommand command, int input)
        {
            return Invoke((c) => handler.Invoke(c, input), command, nameof(IExecuteNonQueryMiddleware));
        }

        #endregion

        #region IExecuteScalarMiddleware

        public object? Next(IExecuteScalarHandler handler, DbCommand command, object? input)
        {
            return Invoke((c) => handler.Invoke(c, input), command, nameof(IExecuteScalarMiddleware));
        }

        #endregion

        #region IExecuteDataReaderMiddleware

        public DbDataReader Next(IExecuteDataReaderHandler handler, DbCommand command, CommandBehavior behavior, DbDataReader reader)
        {
            return Invoke((c) => handler.Invoke(c, behavior, reader), command, nameof(IExecuteDataReaderMiddleware));
        }

        #endregion
    }
}
