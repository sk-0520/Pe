using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public interface IApplicationDatabaseBarrier: IDatabaseBarrier
    { }

    public interface IMainDatabaseBarrier: IApplicationDatabaseBarrier
    { }
    public interface ILargeDatabaseBarrier: IApplicationDatabaseBarrier
    { }
    public interface ITemporaryDatabaseBarrier: IApplicationDatabaseBarrier
    { }

    internal sealed class ApplicationDatabaseBarrier: DatabaseBarrier, IMainDatabaseBarrier, ILargeDatabaseBarrier, ITemporaryDatabaseBarrier
    {
        public ApplicationDatabaseBarrier(IDatabaseAccessor accessor, ReadWriteLockHelper locker, ILoggerFactory loggerFactory)
            : base(accessor, locker, loggerFactory)
        { }
    }
}
