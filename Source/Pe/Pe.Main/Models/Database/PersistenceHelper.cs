using System;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Database.Sqlite;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    internal static class PersistenceHelper
    {
        #region define

        public class PersistenceContextPack: ApplicationDatabaseContextPack
        {
            public PersistenceContextPack(IDatabaseTransaction main, IDatabaseTransaction large, IDatabaseTransaction temporary, IDatabaseCommonStatus commonStatus)
                : base(main, large, temporary, commonStatus)
            {
                MainTransaction = main;
                LargeTransaction = large;
                TemporaryTransaction = temporary;
            }

            #region property

            private IDatabaseTransaction MainTransaction { get; }
            private IDatabaseTransaction LargeTransaction { get; }
            private IDatabaseTransaction TemporaryTransaction { get; }

            #endregion

            #region function

            public void Commit()
            {
                MainTransaction.Commit();
                LargeTransaction.Commit();
                TemporaryTransaction.Commit();
            }

            public void Rollback()
            {
                MainTransaction.Rollback();
                LargeTransaction.Rollback();
                TemporaryTransaction.Rollback();
            }

            #endregion

            #region ApplicationDatabaseContextPack

            protected override void Dispose(bool disposing)
            {
                if(!IsDisposed) {
                    if(disposing) {
                        MainTransaction.Dispose();
                        LargeTransaction.Dispose();
                        TemporaryTransaction.Dispose();
                    }
                }

                base.Dispose(disposing);
            }

            #endregion
        }

        #endregion

        #region function

        private static PersistenceContextPack WaitPack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus, bool isReadOnly)
        {
            static IDatabaseTransaction Do(IDatabaseBarrier databaseBarrier, bool isReadOnly)
            {
                if(isReadOnly) {
                    return databaseBarrier.WaitRead();
                }
                return databaseBarrier.WaitWrite();
            }

            var main = Do(mainDatabaseBarrier, isReadOnly);
            var large = Do(largeDatabaseBarrier, isReadOnly);
            var temp = Do(temporaryDatabaseBarrier, isReadOnly);

            var result = new PersistenceContextPack(main, large, temp, databaseCommonStatus);

            return result;
        }

        public static PersistenceContextPack WaitWritePack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, false);
        }

        public static PersistenceContextPack WaitReadPack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, true);
        }

        /// <summary>
        /// データベース内データを複製。
        /// </summary>
        /// <remarks>
        /// <para>DB実装依存。Sqliteべったり。</para>
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void Copy(IDatabaseAccessor source, IDatabaseAccessor destination)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            if(source == destination) {
                throw new ArgumentException($"{nameof(source)} == {nameof(destination)}");
            }

            //--------------------------------
            // SQLite しか知らん
            var src = (SqliteAccessor)source;
            var dst = (SqliteAccessor)destination;

            var dbNames = new[] { "main", "temp" };
            foreach(var dbName in dbNames) {
                src.CopyTo(dbName, dst, dbName);
            }
        }

        #endregion
    }
}
