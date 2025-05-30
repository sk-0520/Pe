using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public class DatabaseDelayWriter: DisposerBase, IDatabaseDelayWriter
    {
        #region define

        private readonly struct DelayStockItem
        {
            public DelayStockItem(Action<IDatabaseTransaction> action)
            {
                Action = action;
                StockUtcTimestamp = DateTime.UtcNow;
            }

            #region property

            public Action<IDatabaseTransaction> Action { get; }
            public DateTime StockUtcTimestamp { get; }

            #endregion
        }

        #endregion

        #region variable

        private readonly Lock _sync = new Lock();

        #endregion

        public DatabaseDelayWriter(IDatabaseBarrier databaseBarrier, TimeSpan pauseRetryTime, ILoggerFactory loggerFactory)
        {
            if(databaseBarrier == null) {
                throw new ArgumentNullException(nameof(databaseBarrier));
            }
            if(loggerFactory == null) {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            DatabaseBarrier = databaseBarrier;
            PauseRetryTime = pauseRetryTime;
            Logger = loggerFactory.CreateLogger(GetType());

            DelayTimer = new Timer(DelayCallback);
        }

        #region property

        private IDatabaseBarrier DatabaseBarrier { get; }

        /// <summary>
        /// リトライ間隔。
        /// </summary>
        private TimeSpan PauseRetryTime { get; }
        private ILogger Logger { get; }

        private Timer DelayTimer { get; [Unused(UnusedKinds.Dispose)] set; }

        private List<DelayStockItem> StockItems { get; } = new List<DelayStockItem>();
        private Dictionary<object, DelayStockItem> UniqueItems { get; } = new Dictionary<object, DelayStockItem>();

        #endregion

        #region function

        private void StopTimer()
        {
            DelayTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private void StartTimer()
        {
            ThrowIfDisposed();

            DelayTimer.Change(PauseRetryTime, PauseRetryTime);
        }

        private void StockCore(Action<IDatabaseTransaction> action, object? uniqueKey)
        {
            lock(this._sync) {
                StopTimer();

                // 既に登録されている処理が存在する場合は破棄しておく
                if(uniqueKey != null) {
                    if(UniqueItems.TryGetValue(uniqueKey, out var stockedItem)) {
                        Logger.LogTrace("待機処理破棄: {0} {1}", stockedItem.StockUtcTimestamp, uniqueKey.GetHashCode());
                        StockItems.Remove(stockedItem);
                        UniqueItems.Remove(uniqueKey);
                    }
                }

                var item = new DelayStockItem(action);
                StockItems.Add(item);
                if(uniqueKey != null) {
                    UniqueItems.Add(uniqueKey, item);
                }

                StartTimer();
            }
        }

        private void DelayCallback(object? state)
        {
            if(IsPausing) {
                return;
            }
            Flush();
        }

        private void FlushCore(DelayStockItem[] stockItems)
        {
            using var transaction = DatabaseBarrier.WaitWrite();
            foreach(var stockItem in stockItems) {
                stockItem.Action(transaction);
            }
            transaction.Commit();
        }


        #endregion

        #region IDatabaseDelayWriter

        /// <inheritdoc cref="IDatabaseDelayWriter.IsPausing"/>
        public bool IsPausing { get; private set; }

        /// <inheritdoc cref="IDatabaseDelayWriter.Pause"/>
        public IDisposeObservable Pause()
        {
            ThrowIfDisposed();

            IsPausing = true;
            return new ActionDisposer(d => {
                IsPausing = false;
            });
        }

        /// <inheritdoc cref="IDatabaseDelayWriter.Stock(Action{IDatabaseTransaction}, object)"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "デバッグ時のみのあれ")]
        public void Stock(Action<IDatabaseTransaction> action, object uniqueKey)
        {
            if(uniqueKey == null) {
                throw new ArgumentNullException(nameof(uniqueKey));
            }
#if DEBUG
            if(uniqueKey is UniqueKeyPool) {
                Debug.Assert(false, $"完全な事故: {nameof(UniqueKeyPool)}.{nameof(UniqueKeyPool.Get)} を使用していない可能性あり");
            }
#endif

            ThrowIfDisposed();

            StockCore(action, uniqueKey);
        }

        /// <inheritdoc cref="IDatabaseDelayWriter.Stock(Action{IDatabaseTransaction})"/>
        public void Stock(Action<IDatabaseTransaction> action)
        {
            ThrowIfDisposed();
            StockCore(action, null);
        }

        /// <inheritdoc cref="IDatabaseDelayWriter.ClearStock"/>
        public void ClearStock()
        {
            ThrowIfDisposed();

            lock(this._sync) {
                StopTimer();

                StockItems.Clear();

                StartTimer();
            }
        }

        #endregion

        #region IFlush

        void Flush(bool disposing)
        {
            DelayStockItem[] items;
            lock(this._sync) {
                if(disposing) {
                    StopTimer();
                }
                items = StockItems.ToArray();
                StockItems.Clear();
                UniqueItems.Clear();
            }

            if(items.Length == 0) {
                return;
            }

            FlushCore(items);
        }

        /// <inheritdoc cref="IFlushable.Flush"/>
        public void Flush()
        {
            Flush(true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush(disposing);
                StockItems.Clear();
                UniqueItems.Clear();
                if(disposing) {
                    DelayTimer.Dispose();
                }
                DelayTimer = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
