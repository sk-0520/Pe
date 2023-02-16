using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupsSettingEditorElement: SettingEditorElementBase
    {
        public LauncherGroupsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            GroupItems = allLauncherGroups;
        }

        #region property

        public ObservableCollection<LauncherGroupSettingEditorElement> GroupItems { get; }
        public ObservableCollection<WrapModel<LauncherItemId>> LauncherItems { get; } = new ObservableCollection<WrapModel<LauncherItemId>>();


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
                var launcherGroupsEntityDao = new LauncherGroupsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherGroupId(targetItem.LauncherGroupId);
                launcherGroupsEntityDao.DeleteGroup(targetItem.LauncherGroupId);

                context.Commit();
            }

            targetItem.Dispose();
        }

        public LauncherGroupId AddNewGroup(LauncherGroupKind kind)
        {
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var newGroupName = launcherFactory.CreateUniqueGroupName(GroupItems.Select(i => i.Name).ToList());
            var groupData = launcherFactory.CreateGroupData(newGroupName, kind);

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var launcherGroupsDao = new LauncherGroupsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                launcherGroupsDao.InsertNewGroup(groupData, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            var group = new LauncherGroupSettingEditorElement(groupData.LauncherGroupId, MainDatabaseBarrier, DatabaseStatementLoader, IdFactory, LoggerFactory);
            group.Initialize();
            GroupItems.Add(group);

            return group.LauncherGroupId;
        }

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            ThrowIfDisposed();

            IReadOnlyList<LauncherItemId> launcherItemIds;
            //IReadOnlyList<Guid> groupIds;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();

                //var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                //groupIds = launcherGroupsEntityDao.SelectAllLauncherGroupIds().ToList();
            }

            LauncherItems.SetRange(launcherItemIds.Select(i => WrapModel.Create(i, LoggerFactory)));

            //GroupItems.Clear();
            //foreach(var groupId in groupIds) {
            //    var element = new LauncherGroupSettingEditorElement(groupId, MainDatabaseBarrier, StatementLoader, IdFactory, LoggerFactory);
            //    element.Initialize();
            //    GroupItems.Add(element);
            //}
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
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
                    foreach(var item in LauncherItems) {
                        item.Dispose();
                    }
                    LauncherItems.Clear();
                }
            }
            base.Dispose(disposing);
        }

        protected override void ReceiveLauncherItemRemoved(LauncherItemId launcherItemId)
        {
            base.ReceiveLauncherItemRemoved(launcherItemId);

            foreach(var grouItem in GroupItems) {
                var targetItems = grouItem.LauncherItems.Where(i => i.Data == launcherItemId).ToList();
                foreach(var targetItem in targetItems) {
                    grouItem.LauncherItems.Remove(targetItem);
                }
            }
        }

        #endregion
    }
}
