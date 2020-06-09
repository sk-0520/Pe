using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    public class LauncherItemCommandFinder: DisposerBase, ICommandFinder
    {
        public LauncherItemCommandFinder(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, INotifyManager notifyManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        internal bool FindTag { get; set; }
        internal IconBox IconBox { get; set; }

        IList<LauncherItemElement> LauncherItemElements { get; } = new List<LauncherItemElement>();
        IDictionary<Guid, LauncherItemElement> LauncherItemElementMap { get; } = new Dictionary<Guid, LauncherItemElement>();
        IDictionary<Guid, IReadOnlyCollection<string>> LauncherTags { get; } = new Dictionary<Guid, IReadOnlyCollection<string>>();


        #endregion

        #region function

        public void ClearIcon()
        {
            foreach(var element in LauncherItemElements) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
            }
        }

        private ICommandItem? GetHitItem(CommandItemKind kind, LauncherItemElement element, string targetValue, string targetLogName, string input, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            var nameMatches = hitValuesCreator.GetMatches(targetValue, inputRegex);
            if(nameMatches.Any()) {
                Logger.LogTrace("ランチャー: {0}, {1}, {2}", targetLogName, targetValue, element.LauncherItemId);
                var result = new LauncherCommandItemElement(element, DispatcherWrapper, LoggerFactory) {
                    EditableKind = kind,
                };
                result.Initialize();
                var ranges = hitValuesCreator.ConvertRanges(nameMatches);
                var hitValue = hitValuesCreator.ConvertHitValues(targetValue, ranges);
                if(kind == CommandItemKind.LauncherItemName) {
                    result.EditableHeaderValues.SetRange(hitValue);
                    result.EditableScore = hitValuesCreator.CalcScore(targetValue, result.EditableHeaderValues);
                } else {
                    result.EditableDescriptionValues.SetRange(hitValue);
                    result.EditableScore = hitValuesCreator.CalcScore(targetValue, result.EditableDescriptionValues);
                }
                return result;
            }

            return null;
        }


        void AddItem(LauncherItemElement launcherItemElement)
        {
            LauncherItemElements.Add(launcherItemElement);
            LauncherItemElementMap.Add(launcherItemElement.LauncherItemId, launcherItemElement);
            if(FindTag) {
                LoadTag(launcherItemElement.LauncherItemId);
            }
        }

        void LoadTag(Guid launcherItemId)
        {
            Debug.Assert(FindTag);

            // タグ情報再構築
            Logger.LogTrace("タグ情報再構築");
            var tags = MainDatabaseBarrier.ReadData(c => {
                var launcherTagsEntityDao = new LauncherTagsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return launcherTagsEntityDao.SelectUniqueTags(launcherItemId).ToHashSet();
            });
            LauncherTags.Remove(launcherItemId);
            if(tags.Any()) {
                LauncherTags.Add(launcherItemId, tags);
            }
        }

        #endregion

        #region ICommandFinder

        public bool IsInitialize { get; private set; }

        public void Initialize()
        {
            if(IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            NotifyManager.LauncherItemChanged += NotifyManager_LauncherItemChanged;
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;

            IsInitialize = true;
        }

        public void Refresh(IPluginContext pluginContext)
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            IReadOnlyList<Guid> ids;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                ids = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }

            var launcherItemElements = ids
                .Select(i => OrderManager.GetOrCreateLauncherItemElement(i))
                .Where(i => i.IsEnabledCommandLauncher)
                .ToList()
            ;
            LauncherItemElements.SetRange(launcherItemElements);
            LauncherItemElementMap.Clear();
            foreach(var LauncherItemElement in LauncherItemElements) {
                LauncherItemElementMap.Add(LauncherItemElement.LauncherItemId, LauncherItemElement);
            }

            if(FindTag) {
                var tagItems = new Dictionary<Guid, IReadOnlyCollection<string>>(ids.Count);
                using(var commander = MainDatabaseBarrier.WaitRead()) {
                    var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                    foreach(var id in ids) {
                        var tags = launcherTagsEntityDao.SelectUniqueTags(id).ToHashSet();
                        if(tags.Count != 0) {
                            tagItems.Add(id, tags);
                        }
                    }
                }
                LauncherTags.Clear();
                foreach(var pair in tagItems) {
                    LauncherTags.Add(pair.Key, pair.Value);
                }
            } else {
                LauncherTags.Clear();
            }
        }

        public IEnumerable<ICommandItem> EnumerateCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            if(string.IsNullOrWhiteSpace(inputValue)) {
                var items = LauncherItemElements
                    .Select(i => new LauncherCommandItemElement(i, DispatcherWrapper, LoggerFactory))
                ;
                foreach(var item in items) {
                    yield return item;
                }
                yield break;
            }

            foreach(var element in LauncherItemElements) {
                cancellationToken.ThrowIfCancellationRequested();
                var nameItem = GetHitItem(CommandItemKind.LauncherItemName, element, element.Name, "名前一致", inputValue, inputRegex, hitValuesCreator, cancellationToken);
                if(nameItem != null) {
                    yield return nameItem;
                    continue;
                }

                var codeItem = GetHitItem(CommandItemKind.LauncherItemCode, element, element.Code, "コード一致", inputValue, inputRegex, hitValuesCreator, cancellationToken);
                if(codeItem != null) {
                    yield return codeItem;
                    continue;
                }

                if(!FindTag) {
                    continue;
                }

                if(LauncherTags.TryGetValue(element.LauncherItemId, out var tags)) {
                    foreach(var tag in tags) {
                        var tagItem = GetHitItem(CommandItemKind.LauncherItemTag, element, tag, "タグ", inputValue, inputRegex, hitValuesCreator, cancellationToken);
                        if(tagItem != null) {
                            yield return tagItem;
                            continue;
                        }
                    }
                }
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NotifyManager.LauncherItemChanged -= NotifyManager_LauncherItemChanged;
                NotifyManager.LauncherItemRegistered -= NotifyManager_LauncherItemRegistered;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void NotifyManager_LauncherItemChanged(object? sender, LauncherItemChangedEventArgs e)
        {
            Debug.Assert(IsInitialize);

            var element = LauncherItemElements.FirstOrDefault(i => i.LauncherItemId == e.LauncherItemId);
            if(element != null) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
                if(!element.IsEnabledCommandLauncher) {
                    Logger.LogInformation("コマンドランチャーから既存ランチャーアイテムの除外: {0}", element.LauncherItemId);
                    LauncherItemElements.Remove(element);
                    LauncherItemElementMap.Remove(element.LauncherItemId);
                    LauncherTags.Remove(element.LauncherItemId);
                } else {
                    if(FindTag) {
                        LoadTag(e.LauncherItemId);
                    }
                }
            } else {
                // 該当アイテムの投入
                var newElement = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
                AddItem(newElement);
            }
        }

        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            Debug.Assert(IsInitialize);

            var element = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
            if(element.IsEnabledCommandLauncher) {
                Logger.LogInformation("コマンドランチャーへ新規ランチャーアイテムの追加: {0}", element.LauncherItemId);
                AddItem(element);
            }
        }
    }
}
