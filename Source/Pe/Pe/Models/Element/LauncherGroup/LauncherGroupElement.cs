using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherGroupElement : ElementBase
    {
        public LauncherGroupElement(Guid launcherGroupId, IOrderManager orderManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }

        #region property
        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }

        public Guid LauncherGroupId { get; }

        public string? Name { get; private set; }
        public LauncherGroupKind Kind { get; private set; }
        public LauncherGroupImageName ImageName { get; private set; }
        public Color ImageColor { get; private set; }
        public long Sort { get; private set; }

        List<Guid> LauncherItemIds { get; } = new List<Guid>();

        #endregion

        #region function

        public IReadOnlyList<Guid> GetLauncherItemIds() => LauncherItemIds.ToList();

        void LoadGroup()
        {
            LauncherGroupData data;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                data = dao.SelectLauncherGroup(LauncherGroupId);
            }

#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
            Name = data.Name;
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
            Kind = data.Kind;
            ImageName = data.ImageName;
            ImageColor = data.ImageColor;
            Sort = data.Sort;
        }

        IEnumerable<Guid> GetLauncherItemsForNormal()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectLauncherItemIds(LauncherGroupId);
            }
        }

        IEnumerable<Guid> GetLauncherItems()
        {
            switch(Kind) {
                case LauncherGroupKind.Normal:
                    return GetLauncherItemsForNormal();

                default:
                    throw new NotImplementedException();
            }
        }

        void LoadLauncherItems()
        {
            var items = GetLauncherItems();
            LauncherItemIds.Clear();
            LauncherItemIds.AddRange(items);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadGroup();
            LoadLauncherItems();
        }

        #endregion
    }
}
