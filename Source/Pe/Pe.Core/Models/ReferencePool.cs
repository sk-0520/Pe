using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 参照中の値を管理。
    /// </summary>
    /// <typeparam name="TValue">参照対象型。</typeparam>
    internal class ReferenceItem<TValue>
        where TValue : class
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="value">参照値。</param>
        /// <param name="timelimit">生存時間。</param>
        /// <param name="isManage"><see cref="IDisposable.Dispose"/> 管理対象か。</param>
        public ReferenceItem(TValue value, TimeSpan timelimit, bool isManage)
        {
            Value = value;
            Timelimit = timelimit;
            IsManage = isManage;
            LifeTime = Stopwatch.StartNew();
        }

        #region property

        /// <summary>
        /// 参照値。
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// 生存時間。
        /// </summary>
        Stopwatch LifeTime { get; set; }
        /// <summary>
        /// 死ぬまでの猶予。
        /// </summary>
        TimeSpan Timelimit { get; }
        /// <summary>
        /// <typeparamref name="TValue"/>が<see cref="IDisposable"/>なら<see cref="IDisposable.Dispose"/>の面倒を見てあげるか。
        /// </summary>
        public bool IsManage { get; }

        /// <summary>
        /// 生存中か。
        /// </summary>
        public bool Alive => LifeTime.Elapsed < Timelimit;

        #endregion

        #region function

        /// <summary>
        /// 再使用可能にする。
        /// </summary>
        public void Recycle()
        {
            LifeTime.Restart();
        }

        #endregion
    }


    /// <summary>
    /// 参照をふわっと管理。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReferencePool<TKey, TValue> : DisposerBase
        where TKey : notnull
        where TValue : class
    {
        public ReferencePool(TimeSpan garbageTime, TimeSpan defaultTimelimit, bool defaultManage, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            DefaultTimelimit = defaultTimelimit;
            DefaultManage = defaultManage;
            Timer = new Timer() {
                Interval = garbageTime.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }


        #region property

        ILogger Logger { get; }

        /// <summary>
        /// キャッシュの中身。
        /// </summary>
        private ConcurrentDictionary<TKey, ReferenceItem<TValue>> Store { get; } = new ConcurrentDictionary<TKey, ReferenceItem<TValue>>();

        /// <summary>
        /// キャッシュ時間。
        /// </summary>
        public TimeSpan DefaultTimelimit { get; }
        public bool DefaultManage { get; }

        Timer Timer { get; }

        #endregion

        #region function


        private TValue GetCore(TKey key, TimeSpan timelimit, bool isManage, Func<TKey, TValue> creator)
        {
            static ReferenceItem<TValue> GetOrAdd(ConcurrentDictionary<TKey, ReferenceItem<TValue>> map, TKey key, TimeSpan timelimit, bool isManage, Func<TKey, TValue> creator, ILogger logger)
            {
                return map.GetOrAdd(key, (key, arg) => {
                    logger.LogTrace("参照アイテム生成: {0}", key);
                    var value = arg.creator(key);
                    var item = new ReferenceItem<TValue>(value, arg.timelimit, arg.isManage);
                    return item;
                }, (timelimit, isManage, creator));
            }

            var result = GetOrAdd(Store, key, timelimit, isManage, creator, Logger);
            lock(result) {
                if(result.Alive) {
                    Logger.LogTrace("参照アイテム生成/再使用: {0}", key);
                    result.Recycle();
                    return result.Value;
                }
            }

            if(Store.TryRemove(key, out var livingDead)) {
                IncinerateCore(key, livingDead);
            }

            return GetOrAdd(Store, key, timelimit, isManage, creator, Logger).Value;
        }

        public TValue Get(TKey key, Func<TKey, TValue> creator)
        {
            return GetCore(key, DefaultTimelimit, DefaultManage, creator);
        }
        public TValue Get(TKey key, TimeSpan timelimit, Func<TKey, TValue> creator)
        {
            return GetCore(key, timelimit, DefaultManage, creator);
        }
        public TValue Get(TKey key, bool isManage, Func<TKey, TValue> creator)
        {
            return GetCore(key, DefaultTimelimit, isManage, creator);
        }
        public TValue Get(TKey key, TimeSpan timelimit, bool isManage, Func<TKey, TValue> creator)
        {
            return GetCore(key, timelimit, isManage, creator);
        }


        /// <summary>
        /// 破棄処理。
        /// </summary>
        /// <param name="item">呼び出し側で lock すること。</param>
        private void IncinerateCore(TKey key, ReferenceItem<TValue> item)
        {
            if(item.Alive) {
                return;
            }

            Logger.LogTrace("参照アイテム削除: {0}", key);
            if(!item.IsManage) {
                return;
            }

            if(item.Value is IDisposable disposer) {
                Logger.LogTrace("参照アイテム破棄: {0}", key);
                disposer.Dispose();
            }
        }

        public void Refresh()
        {
            Logger.LogTrace("参照アイテム削除一括削除開始");
            var pairs = Store.Where(i => !i.Value.Alive).ToList();
            foreach(var pair in pairs) {
                lock(pair.Value) {
                    IncinerateCore(pair.Key, pair.Value);
                }
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Timer.Elapsed -= Timer_Elapsed;
                if(disposing) {
                    Timer.Dispose();
                    foreach(var pair in Store) {
                        // ここでロックする事態は状態がおかしいと思う
                        IncinerateCore(pair.Key, pair.Value);
                    }

                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Stop();
            try {
                Refresh();
            } finally {
                Timer.Start();
            }
        }
    }
}