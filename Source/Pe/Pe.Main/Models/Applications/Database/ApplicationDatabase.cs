using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Database.Handler;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using ContentTypeTextNet.Pe.Library.Database.Sqlite;
using ContentTypeTextNet.Pe.Main.Models.Applications.Database.Middleware;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal sealed class AppDaoFactory: DaoFactory
    {
        public AppDaoFactory(IDatabaseContext context, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(context, statementLoader, loggerFactory)
        { }
    }

    /// <summary>
    /// アプリケーション用<see cref="IDatabaseFactory"/>実装。
    /// </summary>
    /// <remarks>
    /// <para>Peは<see cref="SqliteFactoryBase"/>を継承する。</para>
    /// </remarks>
    internal class ApplicationDatabaseFactory: ConnectionStringSqliteFactory
    {
        /// <summary>
        /// インメモリDBとして構築。
        /// </summary>
        /// <param name="foreignKeys">外部制約を有効にするか。</param>
        /// <param name="isReadOnly">読み込み専用にするか。</param>
        public ApplicationDatabaseFactory(bool foreignKeys, bool isReadOnly)
        {
            var builder = CreateConnectionBuilder();
            builder.DataSource = ":memory:";
            builder.ForeignKeys = foreignKeys;
            if(isReadOnly) {
                builder.ReadOnly = isReadOnly;
            }

            ConnectionString = builder.ToString();
        }

        /// <summary>
        /// ファイルDBとして構築。
        /// </summary>
        /// <param name="file">DBファイル。</param>
        /// <param name="foreignKeys">外部制約を有効にするか。</param>
        /// <param name="isReadOnly">読み込み専用にするか。</param>
        public ApplicationDatabaseFactory(FileInfo file, bool foreignKeys, bool isReadOnly)
        {
            var builder = CreateConnectionBuilder();
            builder.DataSource = ToSafeFilePath(file);
            builder.ForeignKeys = foreignKeys;
            if(isReadOnly) {
                builder.ReadOnly = isReadOnly;
            }

            ConnectionString = builder.ToString();
        }

        #region property

        public TimeProvider TimeProvider { get; init; } = TimeProvider.System;

        #endregion

        #region IDatabaseFactory

        /// <inheritdoc cref="IDatabaseFactory.CreateConnection"/>
        public override IDbConnection CreateConnection() => new SQLiteConnection(ConnectionString);

        /// <inheritdoc cref="IDatabaseFactory.CreateImplementation"/>
        public override IDatabaseImplementation CreateImplementation() => new ApplicationDatabaseImplementation();

        #endregion
    }

    /// <summary>
    /// アプリケーション用<see cref="IDatabaseImplementation"/>実装。
    /// </summary>
    internal partial class ApplicationDatabaseImplementation: SqliteImplementation
    { }

    public interface IApplicationDatabaseAccessor: IDatabaseAccessor
    { }

    public interface IMainDatabaseAccessor: IApplicationDatabaseAccessor
    { }
    public interface ILargeDatabaseAccessor: IApplicationDatabaseAccessor
    { }
    public interface ITemporaryDatabaseAccessor: IApplicationDatabaseAccessor
    { }

    /// <summary>
    /// アプリケーション用<see cref="IDatabaseAccessor"/>実装。
    /// </summary>
    internal class ApplicationDatabaseAccessor: SqliteAccessor, IMainDatabaseAccessor, ILargeDatabaseAccessor, ITemporaryDatabaseAccessor
    {
        public ApplicationDatabaseAccessor(ApplicationDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            : base(databaseFactory, loggerFactory)
        {
            var loggingMiddleware = new AppLoggingMiddleware(databaseFactory.TimeProvider, LoggerFactory);

            MiddlewareCollection = new MiddlewareCollection() {
                Statements = [
                    new AppStatementMiddleware(Implementation, LoggerFactory),
                ],
                ExecuteNonQueries = [
                    loggingMiddleware,
                    new AppExecuteNonQueryMiddleware(Implementation, LoggerFactory),
                ],
                ExecuteScalars = [
                    loggingMiddleware,
                    new AppExecuteScalarMiddleware(Implementation, LoggerFactory),
                ],
                ExecuteDataReaders = [
                    loggingMiddleware,
                    new AppExecuteDataReaderMiddleware(Implementation, LoggerFactory),
                ],
            };
        }
    }

    internal readonly struct StatementAccessorParameter
    {
        public StatementAccessorParameter(string fullName)
        {
            if(fullName == null) {
                throw new ArgumentNullException(fullName);
            }

            var values = fullName.Split('.', StringSplitOptions.None);

            if(values.Length < 2) {
                throw new ArgumentException(null, fullName);
            }

            if(values.Any(i => string.IsNullOrWhiteSpace(i))) {
                throw new ArgumentException(null, fullName);
            }

            if(values.Length == 2) {
                Namespace = string.Empty;
                ClassName = values[0];
                MethodName = values[1];
            } else {
                Namespace = string.Join('.', values[0..^2]);
                ClassName = values[^2];
                MethodName = values[^1];
            }
        }

        #region property

        public string Namespace { get; }
        public string ClassName { get; }
        public string MethodName { get; }

        #endregion
    }

    public class ApplicationDatabaseStatementLoader: DatabaseStatementLoaderBase, IDisposable
    {
        #region define

        private const string IgnoreNamespace = "ContentTypeTextNet.Pe.Main";
        private const string SelectStatement = @"
select
    Statements.Statement
from
    Statements
where
    Statements.Namespace = @Namespace
    and
    Statements.ClassName = @ClassName
    and
    Statements.MethodName = @MethodName
limit
    1
                ";

        #endregion

        public ApplicationDatabaseStatementLoader(DirectoryInfo baseDirectory, TimeSpan timelimit, DatabaseAccessor? statementAccessor, bool givePriorityToFile, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            BaseDirectory = baseDirectory;
            StatementCache = new ReferencePool<string, string>(TimeSpan.FromMinutes(10), timelimit, false, loggerFactory);
            GivePriorityToFile = givePriorityToFile;
            StatementAccessor = statementAccessor;

            if(StatementAccessor == null) {
                Logger.LogInformation("SQL文読み込み方法 -> ファイル: {0}", BaseDirectory.FullName);
            } else {
                if(GivePriorityToFile) {
                    Logger.LogInformation("SQL文読み込み方法 -> ファイル優先の sqlite: {0} -> {1}", BaseDirectory.FullName, StatementAccessor.BaseDbConnection.ConnectionString);
                } else {
                    Logger.LogInformation("SQL文読み込み方法 -> sqlite: {0}", StatementAccessor.BaseDbConnection.ConnectionString);
                }
            }
        }

        #region property

        private DirectoryInfo BaseDirectory { get; }
        private ReferencePool<string, string> StatementCache { get; }

        /// <summary>
        /// SQLファイルを優先して読み込む。
        /// </summary>
        private bool GivePriorityToFile { get; }
        private DatabaseAccessor? StatementAccessor { get; }

        public int SqlFileBufferSize { get; set; } = 4096;
        public Encoding SqlFileEncoding { get; set; } = Encoding.UTF8;

        #endregion

        #region function

        private string ConvertFileName(string key)
        {
            var keyPath = key.Substring(IgnoreNamespace.Length + 1).Replace('.', Path.DirectorySeparatorChar) + ".sql";
            var filePath = Path.Combine(BaseDirectory.FullName, keyPath);

            return filePath;
        }

        private string CreateCacheFromFile(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, SqlFileBufferSize);
            using var reader = new StreamReader(stream, SqlFileEncoding);
            return reader.ReadToEnd();
        }

        private string CreateCacheFromAccessor(string key)
        {
            Debug.Assert(StatementAccessor != null);

            var statementAccessorParameter = new StatementAccessorParameter(key);
            return StatementAccessor.QueryFirst<string>(SelectStatement, statementAccessorParameter);
        }

        private string CreateCache(string key)
        {
#if DEBUG
            using var x = ActionDisposerHelper.Create((d, sw) => Logger.LogTrace("SQL読み込み時間: {0}, {1}", sw.Elapsed, key), Stopwatch.StartNew());
#endif
            if(StatementAccessor == null) {
                return CreateCacheFromFile(ConvertFileName(key));
            }

            if(GivePriorityToFile) {
                var filePath = ConvertFileName(key);
                if(File.Exists(filePath)) {
                    Logger.LogDebug("{0} に該当するファイルが存在するため優先実行: {1}", key, filePath);
                    return CreateCacheFromFile(filePath);
                }
            }

            return CreateCacheFromAccessor(key);
        }

        private string LoadStatementCore(string key)
        {
            Debug.Assert(0 < key.Length);

            return StatementCache.Get(key, CreateCache);
        }

        #endregion

        #region DatabaseStatementLoaderBase

        public override string LoadStatement(string key)
        {
            if(string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException(nameof(key));
            }

            return LoadStatementCore(key);
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue) {
                if(disposing) {
                    StatementAccessor?.Dispose();
                    StatementCache.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
