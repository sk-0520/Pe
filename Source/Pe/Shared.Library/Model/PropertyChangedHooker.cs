using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface IReadOnlyHookItem
    {
        #region property

        /// <summary>
        /// 変更通知プロパティ名。
        /// </summary>
        string NotifyPropertyName { get; }
        /// <summary>
        /// 変更通知を送るプロパティ名。
        /// </summary>
        IReadOnlyList<string> RaisePropertyNames { get; }
        /// <summary>
        /// 状態を更新するコマンド。
        /// </summary>
        IReadOnlyList<ICommand> RaiseCommands { get; }
        /// <summary>
        /// 変更通知により呼び出される処理。
        /// </summary>
        Action Callback { get; }

        #endregion
    }

    public class HookItem : IReadOnlyHookItem
    {
        public HookItem(string notifyPropertyName, IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, Action callback)
        {
            NotifyPropertyName = notifyPropertyName;

            if(raisePropertyNames != null) {
                RaisePropertyNames = raisePropertyNames.ToList();
            }

            if(raiseCommands != null) {
                RaiseCommands = raiseCommands.ToList();
            }

            Callback = callback;
        }

        #region IReadOnlyHookItem

        /// <summary>
        /// 変更通知プロパティ名。
        /// </summary>
        public string NotifyPropertyName { get; }
        /// <summary>
        /// 変更通知を送るプロパティ名。
        /// </summary>
        public List<string> RaisePropertyNames { get; }
        IReadOnlyList<string> IReadOnlyHookItem.RaisePropertyNames => RaisePropertyNames;
        /// <summary>
        /// 状態を更新するコマンド。
        /// </summary>
        public List<ICommand> RaiseCommands { get; }
        IReadOnlyList<ICommand> IReadOnlyHookItem.RaiseCommands => RaiseCommands;

        /// <summary>
        /// 変更通知により呼び出される処理。
        /// </summary>
        public Action Callback { get; }

        #endregion
    }

    class HookItemCache
    {
        public HookItemCache(IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, IEnumerable<DelegateCommandBase> raiseDelegateCommands, IEnumerable<Action> callbacks)
        {
            RaisePropertyNames = raisePropertyNames.ToList();
            RaiseCommands = raiseCommands.ToList();
            RaiseDelegateCommands = raiseDelegateCommands.ToList();
            Callbacks = callbacks.ToList();
        }

        #region property

        public IReadOnlyList<string> RaisePropertyNames { get; }
        public IReadOnlyList<ICommand> RaiseCommands { get; }
        public IReadOnlyList<DelegateCommandBase> RaiseDelegateCommands { get; }
        public IReadOnlyList<Action> Callbacks { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> を受けて何かを更新する ViewModel でよく使うあれな処理の管理役。
    /// </summary>
    public class PropertyChangedHooker : DisposerBase
    {
        public PropertyChangedHooker(IDispatcherWapper dispatcherWapper, ILogger logger)
        {
            DispatcherWapper = dispatcherWapper;
            Logger = logger;
        }
        public PropertyChangedHooker(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected IDispatcherWapper DispatcherWapper { get; }
        protected ILogger Logger { get; }

        protected IDictionary<string, List<HookItem>> Hookers { get; } = new Dictionary<string, List<HookItem>>();
        IDictionary<string, HookItemCache> Cache { get; } = new Dictionary<string, HookItemCache>();

        #endregion

        #region function

        IReadOnlyHookItem AddHookCore(HookItem hookItem)
        {
            if(string.IsNullOrWhiteSpace(hookItem.NotifyPropertyName)) {
                throw new ArgumentException($"{nameof(hookItem.NotifyPropertyName)}");
            }
            if(hookItem.RaisePropertyNames == null && hookItem.RaiseCommands == null && hookItem.Callback == null) {
                throw new ArgumentException($"null: {nameof(hookItem.RaisePropertyNames)}, {nameof(hookItem.RaiseCommands)}, {nameof(hookItem.Callback)}");
            }

            if(hookItem.RaisePropertyNames != null) {
                if(hookItem.RaisePropertyNames.Count == 0) {
                    throw new ArgumentException($"{nameof(hookItem.RaisePropertyNames)}: 0");
                }
                if(hookItem.RaisePropertyNames.Any(i => string.IsNullOrWhiteSpace(i))) {
                    throw new ArgumentException($"{nameof(hookItem.RaisePropertyNames)}: invalid name");
                }
            }

            if(hookItem.RaiseCommands != null) {
                if(hookItem.RaiseCommands.Count == 0) {
                    throw new ArgumentException($"{nameof(hookItem.RaiseCommands)}: 0");
                }
                if(hookItem.RaiseCommands.Any(i => i == null)) {
                    throw new ArgumentException($"{nameof(hookItem.RaiseCommands)}: null element");
                }
            }

            if(Hookers.TryGetValue(hookItem.NotifyPropertyName, out var items)) {
                items.Add(hookItem);
            } else {
                var newItems = new List<HookItem>() {
                    hookItem
                };
                Hookers.Add(hookItem.NotifyPropertyName, newItems);
            }
            Cache.Remove(hookItem.NotifyPropertyName);

            return hookItem;
        }

        public IReadOnlyHookItem AddHook(HookItem hookItem)
        {
            if(hookItem == null) {
                throw new ArgumentNullException(nameof(hookItem));
            }

            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyAndRaisePropertyName)
        {
            if(string.IsNullOrWhiteSpace(notifyAndRaisePropertyName)) {
                throw new ArgumentException(nameof(notifyAndRaisePropertyName));
            }

            var hookItem = new HookItem(
                notifyAndRaisePropertyName,
                new[] { notifyAndRaisePropertyName },
                null,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, string raisePropertyName)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(string.IsNullOrWhiteSpace(raisePropertyName)) {
                throw new ArgumentException(nameof(raisePropertyName));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                new[] { raisePropertyName },
                null,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, params string[] raisePropertyNames)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null || raisePropertyNames.Length == 0) {
                throw new ArgumentException(nameof(raisePropertyNames));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                null,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, ICommand raiseCommand)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raiseCommand == null) {
                throw new ArgumentNullException(nameof(raiseCommand));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                null,
                new[] { raiseCommand },
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, params ICommand[] raiseCommands)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raiseCommands == null || raiseCommands.Length == 0) {
                throw new ArgumentException(nameof(raiseCommands));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                null,
                raiseCommands,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, Action callback)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(callback == null) {
                throw new ArgumentException(nameof(callback));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                null,
                null,
                callback
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null) {
                throw new ArgumentException(nameof(raisePropertyNames));
            }
            if(raiseCommands == null) {
                throw new ArgumentException(nameof(raiseCommands));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                raiseCommands,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, Action callback)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null) {
                throw new ArgumentException(nameof(raisePropertyNames));
            }
            if(raiseCommands == null) {
                throw new ArgumentException(nameof(raiseCommands));
            }
            if(callback == null) {
                throw new ArgumentException(nameof(callback));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                raiseCommands,
                callback
            );
            return AddHookCore(hookItem);
        }

        HookItemCache MakeCache(IEnumerable<IReadOnlyHookItem> hookItems)
        {
            var commands = hookItems
                .Where(i => i.RaiseCommands != null)
                .SelectMany(i => i.RaiseCommands)
                .ToList()
            ;

            var result = new HookItemCache(
                hookItems.Where(i => i.RaisePropertyNames != null).SelectMany(i => i.RaisePropertyNames),
                commands.Where(i => !(i is DelegateCommandBase)),
                commands.OfType<DelegateCommandBase>(),
                hookItems.Where(i => i.Callback != null).Select(i => i.Callback)
            );

            return result;
        }

        bool ExecutePropertyies(IReadOnlyList<string> raisePropertyNames, Action<string> raiser)
        {
            if(raisePropertyNames.Count == 0) {
                return false;
            }

            DispatcherWapper.Invoke(() => {
                foreach(var raisePropertyName in raisePropertyNames) {
                    raiser(raisePropertyName);
                }
            });

            return true;
        }
        bool ExecuteCommands(IReadOnlyList<ICommand> raiseCommands, IReadOnlyList<DelegateCommandBase> raiseDelegateCommands)
        {
            if(raiseCommands.Count == 0 && raiseDelegateCommands.Count == 0) {
                return false;
            }

            DispatcherWapper.Invoke(() => {
                foreach(var raiseCommand in raiseDelegateCommands) {
                    raiseCommand.RaiseCanExecuteChanged();
                }
            });

            if(raiseCommands.Count != 0) {
                // 個別にやる方法はわからん
                DispatcherWapper.Invoke(() => {
                    CommandManager.InvalidateRequerySuggested();
                });
            }

            return true;
        }
        bool ExecuteCallback(IReadOnlyList<Action> callbacks)
        {
            if(callbacks.Count == 0) {
                return false;
            }

            DispatcherWapper.Begin(() => {
                foreach(var callback in callbacks) {
                    callback();
                }
            });

            return true;
        }

        bool ExecuteCache(HookItemCache hookItemCache, Action<string> raiser)
        {
            var property = ExecutePropertyies(hookItemCache.RaisePropertyNames, raiser);
            var command = ExecuteCommands(hookItemCache.RaiseCommands, hookItemCache.RaiseDelegateCommands);
            var callback = ExecuteCallback(hookItemCache.Callbacks);

            return property || command || callback;
        }

        public bool Execute(string noifyPropertyName, Action<string> raiser)
        {

            if(!Hookers.TryGetValue(noifyPropertyName, out var hookItems)) {
                return false;
            }

            if(raiser == null) {
                throw new ArgumentNullException(nameof(raiser));
            }

            if(!Cache.TryGetValue(noifyPropertyName, out var hookItemCache)) {
                hookItemCache = MakeCache(hookItems);
                Cache.Add(noifyPropertyName, hookItemCache);
            }

            return ExecuteCache(hookItemCache, raiser);
        }

        public bool Execute(PropertyChangedEventArgs e, Action<string> raiser) => Execute(e.PropertyName, raiser);

        #endregion
    }

    public static class PropertyChangedHookerExtensions
    {
        #region function

        public static void AddProperties(this PropertyChangedHooker @this, Type type)
        {
            var properties = type.GetProperties();
            foreach(var property in properties) {
                @this.AddHook(property.Name);
            }
        }
        public static void AddProperties<Type>(this PropertyChangedHooker @this) => AddProperties(@this, typeof(Type));


        #endregion
    }
}
