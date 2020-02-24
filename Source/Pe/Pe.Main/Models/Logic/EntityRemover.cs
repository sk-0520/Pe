using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public struct EntityRemoverResultItem
    {
        public EntityRemoverResultItem(string entityName, int count)
        {
            EntityName = entityName;
            Count = count;
        }

        #region

        public string EntityName { get; }
        public int Count { get; }
        #endregion
    }

    public class EntityRemoverResult
    {
        public EntityRemoverResult(Pack target)
        {
            Target = target;
        }

        #region property

        public Pack Target { get; }

        public IList<EntityRemoverResultItem> Items { get; } = new List<EntityRemoverResultItem>();

        #endregion
    }

    public abstract class EntityRemoverBase
    {
        public EntityRemoverBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region function

        public abstract bool IsTarget(Pack pack);

        protected EntityRemoverResultItem ExecuteRemove(string entityName, Func<int> func)
        {
            Debug.Assert(func != null);
            var count = func();
            return new EntityRemoverResultItem(entityName, count);
        }

        protected abstract EntityRemoverResult RemoveMain(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract EntityRemoverResult RemoveFile(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract EntityRemoverResult RemoveTemporary(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);

        public EntityRemoverResult Remove(Pack pack, IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
#if DEBUG
            if(!IsTarget(pack)) {
                throw new ArgumentException(nameof(pack));
            }
#endif

            if(commander == null) {
                throw new ArgumentNullException(nameof(commander));
            }
            if(implementation == null) {
                throw new ArgumentNullException(nameof(implementation));
            }

            switch(pack) {
                case Pack.Main:
                    return RemoveMain(commander, statementLoader, implementation);

                case Pack.File:
                    return RemoveMain(commander, statementLoader, implementation);

                case Pack.Temporary:
                    return RemoveMain(commander, statementLoader, implementation);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }

    public sealed class EntityDeleteDaoGroup
    {
        #region property

        IList<EntityDaoBase> EntityDaos { get; } = new List<EntityDaoBase>();
        IDictionary<EntityDaoBase, IList<Func<int>>> DeleteFunctions { get; } = new Dictionary<EntityDaoBase, IList<Func<int>>>();

        #endregion

        #region function

        public void Add<TEntityDao>(TEntityDao entityDao, Func<TEntityDao, int> deleter)
            where TEntityDao : EntityDaoBase
        {
            EntityDaos.Add(entityDao);
            if(!DeleteFunctions.TryGetValue(entityDao, out var list)) {
                list = new List<Func<int>>();
                DeleteFunctions.Add(entityDao, list);
            }

            list.Add(() => deleter(entityDao));
        }

        public IList<EntityRemoverResultItem> Execute()
        {
            var result = new List<EntityRemoverResultItem>(EntityDaos.Count);

            foreach(var entityDao in EntityDaos) {
                var funcs = DeleteFunctions[entityDao];
                var totalCount = 0;
                foreach(var func in funcs) {
                    var count = func();
                    totalCount += count;
                }
                result.Add(new EntityRemoverResultItem(entityDao.TableName, totalCount));
            }

            return result;
        }

        #endregion
    }

    public sealed class EntitiesRemover
    {
        public EntitiesRemover(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader statementLoader)
        {
            Barriers = new Dictionary<Pack, IApplicationDatabaseBarrier>() {
                [Pack.Main] = mainDatabaseBarrier,
                [Pack.File] = fileDatabaseBarrier,
                [Pack.Temporary] = temporaryDatabaseBarrier,
            };
            StatementLoader = statementLoader;
        }

        #region property

        IDictionary<Pack, IApplicationDatabaseBarrier> Barriers { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public IList<EntityRemoverBase> Items { get; } = new List<EntityRemoverBase>();

        #endregion

        #region function

        IDatabaseTransaction? BeginTransaction(Pack pack)
        {
            if(Items.Any(i => i.IsTarget(pack))) {
                var barrier = Barriers[pack];
                return barrier.WaitWrite();
            }

            return null;
        }

        IEnumerable<EntityRemoverResult> ExecuteCore(Pack pack, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var items = Items.Where(i => i.IsTarget(pack));
            foreach(var item in items) {
                var entityResult = item.Remove(pack, commander, StatementLoader, implementation);
                yield return entityResult;
            }
        }

        public IList<EntityRemoverResult> Execute()
        {

            var packs = new[] {
                Pack.Main,
                Pack.File,
                Pack.Temporary,
            };
#if DEBUG
            Debug.Assert(EnumUtility.GetMembers<Pack>().Count() == packs.Length);
#endif
            var transactions = new Dictionary<Pack, IDatabaseTransaction>();
            try {
                var result = new List<EntityRemoverResult>();

                foreach(var pack in packs) {
                    var transaction = BeginTransaction(pack);
                    if(transaction == null) {
                        continue;
                    }
                    transactions.Add(pack, transaction);
                    foreach(var entityResult in ExecuteCore(pack, transaction, transaction.Implementation)) {
                        result.Add(entityResult);
                    }
                }
                foreach(var transaction in transactions.Values) {
                    transaction.Commit();
                }

                return result;
            } catch(Exception) {
                foreach(var transaction in transactions.Values) {
                    transaction.Rollback();
                }
                throw;
            } finally {
                foreach(var transaction in transactions.Values) {
                    transaction.Dispose();
                }
            }
        }

        #endregion
    }
}
