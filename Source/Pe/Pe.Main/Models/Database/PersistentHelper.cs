using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    internal static class PersistentHelper
    {
        #region define

        public class PersistentContextsPack: ApplicationDatabaseContextsPack
        {
            public PersistentContextsPack(IDatabaseTransaction mainTransaction, IDatabaseTransaction fileTransaction, IDatabaseTransaction temporaryTransaction, IDatabaseCommonStatus commonStatus)
                : base(
                    new DatabaseContexts(mainTransaction, mainTransaction.Implementation),
                    new DatabaseContexts(fileTransaction, mainTransaction.Implementation),
                    new DatabaseContexts(temporaryTransaction, mainTransaction.Implementation),
                    commonStatus
                )
            {
                MainTransaction = mainTransaction;
                FileTransaction = fileTransaction;
                TemporaryTransaction = temporaryTransaction;
            }

            #region property

            IDatabaseTransaction MainTransaction { get; }
            IDatabaseTransaction FileTransaction { get; }
            IDatabaseTransaction TemporaryTransaction { get; }

            #endregion

            #region function

            public void Commit()
            {
                MainTransaction.Commit();
                FileTransaction.Commit();
                TemporaryTransaction.Commit();
            }

            public void Rollback()
            {
                MainTransaction.Rollback();
                FileTransaction.Rollback();
                TemporaryTransaction.Rollback();
            }

            #endregion

            #region ApplicationDatabaseContextsPack

            protected override void Dispose(bool disposing)
            {
                if(!IsDisposed) {
                    if(disposing) {
                        MainTransaction.Dispose();
                        FileTransaction.Dispose();
                        TemporaryTransaction.Dispose();
                    }
                }

                base.Dispose(disposing);
            }

            #endregion
        }

        #endregion

        #region function

        static PersistentContextsPack WaitPack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus, bool isReadOnly)
        {
            static IDatabaseTransaction Do(IDatabaseBarrier databaseBarrier, bool isReadOnly)
            {
                if(isReadOnly) {
                    return databaseBarrier.WaitRead();
                }
                return databaseBarrier.WaitWrite();
            }

            var main = Do(mainDatabaseBarrier, isReadOnly);
            var file = Do(largeDatabaseBarrier, isReadOnly);
            var temp = Do(temporaryDatabaseBarrier, isReadOnly);

            var result = new PersistentContextsPack(main, file, temp, databaseCommonStatus);

            return result;
        }

        public static PersistentContextsPack WaitWritePack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, false);
        }

        public static PersistentContextsPack WaitReadPack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, true);
        }

        /// <summary>
        /// データベース内データを複製。
        /// <para>DB実装依存。Sqliteべったり。</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void Copy(IDatabaseAccessor source, IDatabaseAccessor destination)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(destination == null) {
                throw new ArgumentNullException(nameof(destination));
            }
            if(source == destination) {
                throw new ArgumentException($"{nameof(source)} == {nameof(destination)}");
            }

            var src = (ApplicationDatabaseAccessor)source;
            var dst = (ApplicationDatabaseAccessor)destination;

            var dbNames = new[] { "main", "temp" };
            foreach(var dbName in dbNames) {
                src.CopyTo(dbName, dst, dbName);
            }
        }

        #endregion
    }
}
