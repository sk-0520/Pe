using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// 読み込み専用トランザクション。
    /// </summary>
    public sealed class ReadOnlyDatabaseTransaction: DatabaseTransaction
    {
        public ReadOnlyDatabaseTransaction(IDbConnection connection, IDbTransaction transaction, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(connection, transaction, implementation, loggerFactory)
        {
            //NOP
        }

        #region DatabaseTransaction

        public override void Commit() => throw new NotSupportedException();

#if DEBUG
        public override void Rollback()
        {
            Logger.LogTrace("読み込み専用トランザクション");
        }
#endif

        public override int Execute(string statement, object? parameter) => throw new NotSupportedException();

        public override Task<int> ExecuteAsync(string statement, object? parameter, CancellationToken cancellationToken) => throw new NotSupportedException();

        #endregion
    }
}
