using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupsSettingEditorElement: SettingEditorElementBase
    {
        public LauncherGroupsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, policy, contextDispatcher, loggerFactory)
        {
            GroupItems = allLauncherGroups;
        }

        #region property

        public ObservableCollection<LauncherGroupSettingEditorElement> GroupItems { get; }
        public ObservableCollection<LauncherItemId> LauncherItems { get; } = new ObservableCollection<LauncherItemId>();


        #endregion

        #region function

        public void MoveGroupItem(int startIndex, int insertIndex)
        {
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var item = GroupItems[startIndex];
            GroupItems.RemoveAt(startIndex);
            GroupItems.Insert(insertIndex, item);

            //foreach(var group in GroupItems.Counting()) {
            //    group.Value.Sequence = group.Number * launcherFactory.GroupItemStep;
            //}
        }

        public void RemoveGroup(LauncherGroupId launcherGroupId)
        {
            var targetItem = GroupItems.First(i => i.LauncherGroupId == launcherGroupId);
            GroupItems.Remove(targetItem);

            // DB から物理削除
            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var appDaoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                var launcherGroupsEntityDao = appDaoFactory.Create<LauncherGroupsEntityDao>();
                var launcherGroupItemsEntityDao = appDaoFactory.Create<LauncherGroupItemsEntityDao>();

                launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherGroupId(targetItem.LauncherGroupId);
                launcherGroupsEntityDao.DeleteGroup(targetItem.LauncherGroupId);

                context.Commit();
            }

            targetItem.Dispose();
        }

        public async Task<LauncherGroupId> AddNewGroupAsync(LauncherGroupKind kind, CancellationToken cancellationToken)
        {
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var newGroupName = launcherFactory.CreateUniqueGroupName(GroupItems.Select(i => i.Name).ToList());
            var groupData = launcherFactory.CreateGroupData(newGroupName, kind);

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var appDaoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                var launcherGroupsDao = appDaoFactory.Create<LauncherGroupsEntityDao>();
                launcherGroupsDao.InsertNewGroup(groupData, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            var group = new LauncherGroupSettingEditorElement(groupData.LauncherGroupId, MainDatabaseBarrier, DatabaseStatementLoader, IdFactory, LoggerFactory);
            await group.InitializeAsync(cancellationToken);
            GroupItems.Add(group);

            return group.LauncherGroupId;
        }

        #endregion

        #region SettingEditorElementBase

        protected override Task LoadCoreAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            IReadOnlyList<LauncherItemId> launcherItemIds;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appDaoFactory = new AppDaoFactory(context, DatabaseStatementLoader, LoggerFactory);
                var launcherItemsEntityDao = appDaoFactory.Create<LauncherItemsEntityDao>();
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }
            LauncherItems.SetRange(launcherItemIds);

            return Task.CompletedTask;
        }

        protected override void SaveImpl(IDatabaseContextPack contextsPack)
        {
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            foreach(var group in GroupItems.Counting()) {
                group.Value.Sequence = group.Number * launcherFactory.GroupItemStep;
            }

            foreach(var group in GroupItems) {
                group.Save(contextsPack);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    LauncherItems.Clear();
                }
            }
            base.Dispose(disposing);
        }

        protected override void ReceiveLauncherItemRemoved(LauncherItemId launcherItemId)
        {
            base.ReceiveLauncherItemRemoved(launcherItemId);

            foreach(var groupItem in GroupItems) {
                var targetItems = groupItem.LauncherItems.Where(i => i == launcherItemId).ToArray();
                foreach(var targetItem in targetItems) {
                    groupItem.LauncherItems.Remove(targetItem);
                }
            }
        }

        #endregion
    }
}
