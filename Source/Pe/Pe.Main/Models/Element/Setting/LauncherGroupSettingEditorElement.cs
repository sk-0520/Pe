using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupSettingEditorElement: ElementBase, ILauncherGroupId
    {
        public LauncherGroupSettingEditorElement(Guid launcherGroupId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = statementLoader;
            IdFactory = idFactory;
        }


        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IIdFactory IdFactory { get; }

        public string Name { get; set; } = string.Empty;
        public LauncherGroupKind Kind { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }
        public long Sequence { get; set; }

        public ObservableCollection<WrapModel<Guid>> LauncherItems { get; } = new ObservableCollection<WrapModel<Guid>>();


        #endregion

        #region function

        public void InsertLauncherItemId(int index, Guid launcherItemId)
        {
            LauncherItems.Insert(index, WrapModel.Create(launcherItemId, LoggerFactory));
        }

        public void MoveLauncherItemId(int startIndex, int insertIndex)
        {
            var item = LauncherItems[startIndex];
            LauncherItems.RemoveAt(startIndex);
            LauncherItems.Insert(insertIndex, item);
        }

        public void RemoveLauncherItemAt(int index)
        {
            LauncherItems.RemoveAt(index);
        }

        public void Save(IDatabaseContextsPack pack)
        {
            ThrowIfDisposed();

            var launcherGroupData = new LauncherGroupData() {
                LauncherGroupId = LauncherGroupId,
                Kind = Kind,
                Name = Name,
                ImageName = ImageName,
                ImageColor = ImageColor,
                Sequence = Sequence
            };
            // ?????????????????????????????????????????????????????????????????????
            var launcherItemsEntityDao = new LauncherItemsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            var launcherItemIds = LauncherItems
                .Select(i => i.Data)
                // ??????????????????SQL????????????????????????????????????
                .Where(i => launcherItemsEntityDao.SelectExistsLauncherItem(i))
                .ToList()
            ;

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var launcherGroupsEntityDao = new LauncherGroupsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            launcherGroupsEntityDao.UpdateGroup(launcherGroupData, DatabaseCommonStatus.CreateCurrentAccount());

            var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            launcherGroupItemsDao.DeleteGroupItemsByLauncherGroupId(LauncherGroupId);

            var currentMaxSequence = launcherGroupItemsDao.SelectMaxSequence(LauncherGroupId);
            launcherGroupItemsDao.InsertNewItems(LauncherGroupId, launcherItemIds, currentMaxSequence + launcherFactory.GroupItemsStep, launcherFactory.GroupItemsStep, DatabaseCommonStatus.CreateCurrentAccount());
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LauncherGroupData data;
            IEnumerable<Guid> launcherItemIds;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherGroupsEntityDao = new LauncherGroupsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                data = launcherGroupsEntityDao.SelectLauncherGroup(LauncherGroupId);

                var launcherItemsLoader = new LauncherItemsLoader(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsLoader.LoadLauncherItemIds(LauncherGroupId, data.Kind);
            }

            Name = data.Name;
            Kind = data.Kind;
            ImageName = data.ImageName;
            ImageColor = data.ImageColor;
            Sequence = data.Sequence;
            LauncherItems.SetRange(launcherItemIds.Select(i => WrapModel.Create(i, LoggerFactory)));
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var item in LauncherItems) {
                        item.Dispose();
                    }
                    LauncherItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId { get; }

        #endregion
    }
}
