using System.Collections.Generic;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Adjust;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    public class DatabaseAdjuster
    {
        public DatabaseAdjuster(IIdFactory idFactory, IDatabaseAccessorPack accessorPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            AccessorPack = accessorPack;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="IIdFactory"/>
        private IIdFactory IdFactory { get; }
        /// <inheritdoc cref="IDatabaseAccessorPack"/>
        private IDatabaseAccessorPack AccessorPack { get; }
        /// <inheritdoc cref="IDatabaseStatementLoader"/>
        private IDatabaseStatementLoader StatementLoader { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        private void AdjustCore(IDatabaseAccessor accessor, IEnumerable<AdjustBase> tuners)
        {
            using(var transaction = accessor.BeginTransaction()) {
                foreach(var tuner in tuners) {
                    tuner.Adjust(transaction);
                }
                transaction.Commit();
            }
        }

        private void AdjustMain()
        {
            var tuners = new AdjustBase[] {
                new AdjustLauncherGroups(IdFactory, StatementLoader, LoggerFactory),
            };
            AdjustCore(AccessorPack.Main, tuners);
        }

        private void AdjustFile()
        {
            var tuners = System.Array.Empty<AdjustBase>();
            AdjustCore(AccessorPack.Large, tuners);
        }

        public void Adjust()
        {
            AdjustMain();
            AdjustFile();
        }

        #endregion
    }
}
