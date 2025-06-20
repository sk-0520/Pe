using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Common.Linq;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings.Collections
{
    /// <summary>
    /// <see cref="ObservableCollection{TValue}"/> の変更通知を処理する。
    /// </summary>
    /// <remarks>
    /// <para>原則このクラスは使用せず、<see cref="ModelViewModelObservableCollectionOptions{TModel, TViewModel}"/>の使用を想定している。ただし実装上一本にまとめると複雑になるために本クラスを継承元として分割している。</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:識別子は、正しいサフィックスを含んでいなければなりません", Justification = "<保留中>")]
    public abstract class ObservableCollectionManagerBase<TValue>: BindModelBase, IReadOnlyCollection<TValue>, IDisposed
    {
        private ObservableCollectionManagerBase(IReadOnlyList<TValue> collection, INotifyCollectionChanged collectionNotifyCollectionChanged)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));

            CollectionNotifyCollectionChanged = collectionNotifyCollectionChanged;
            CollectionNotifyCollectionChanged.CollectionChanged += Collection_CollectionChanged;
        }

        protected ObservableCollectionManagerBase(ReadOnlyObservableCollection<TValue> collection)
            : this(collection, collection)
        { }

        protected ObservableCollectionManagerBase(ObservableCollection<TValue> collection)
            : this(collection, collection)
        { }

        ~ObservableCollectionManagerBase()
        {
            Dispose(disposing: false);
        }


        #region property

        /// <summary>
        /// 管理対象コレクション。
        /// </summary>
        protected IReadOnlyList<TValue> Collection { get; private set; }
        /// <summary>
        /// <see cref="Collection"/> の変更通知。
        /// </summary>
        protected INotifyCollectionChanged CollectionNotifyCollectionChanged { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// アイテム追加処理実装部。
        /// </summary>
        /// <param name="newItems"></param>
        protected abstract void AddItemsImpl(IReadOnlyList<TValue> newItems);
        private void AddItems(IReadOnlyList<TValue> newItems)
        {
            ThrowIfDisposed();

            AddItemsImpl(newItems);
        }

        /// <summary>
        /// アイテム挿入処理実装部。
        /// </summary>
        /// <param name="insertIndex"></param>
        /// <param name="newItems"></param>
        protected abstract void InsertItemsImpl(int insertIndex, IReadOnlyList<TValue> newItems);
        private void InsertItems(int insertIndex, IReadOnlyList<TValue> newItems)
        {
            ThrowIfDisposed();

            InsertItemsImpl(insertIndex, newItems);
        }

        /// <summary>
        /// アイテム削除処理実装部。
        /// </summary>
        /// <param name="oldStartingIndex"></param>
        /// <param name="oldItems"></param>
        protected abstract void RemoveItemsImpl(int oldStartingIndex, IReadOnlyList<TValue> oldItems);
        private void RemoveItems(IReadOnlyList<TValue> oldItems, int oldStartingIndex)
        {
            ThrowIfDisposed();

            RemoveItemsImpl(oldStartingIndex, oldItems);
        }

        /// <summary>
        /// アイテム置き換え処理実装部。
        /// </summary>
        /// <param name="newItems"></param>
        /// <param name="oldItems"></param>
        protected abstract void ReplaceItemsImpl(int startIndex, IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems);
        private void ReplaceItems(int startIndex, IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems)
        {
            ThrowIfDisposed();

            ReplaceItemsImpl(startIndex, newItems, oldItems);
        }

        /// <summary>
        /// アイテム移動処理実装部。
        /// </summary>
        /// <param name="newStartingIndex"></param>
        /// <param name="oldStartingIndex"></param>
        protected abstract void MoveItemsImpl(int newStartingIndex, int oldStartingIndex);
        private void MoveItems(int newStartingIndex, int oldStartingIndex)
        {
            ThrowIfDisposed();

            MoveItemsImpl(newStartingIndex, oldStartingIndex);
        }

        /// <summary>
        /// コレクションリセット処理実装部。
        /// </summary>
        protected abstract void ResetItemsImpl();

        private void ResetItems()
        {
            ThrowIfDisposed();

            ResetItemsImpl();
        }

        /// <summary>
        /// 非ジェネリックス<see cref="IList"/>を<see cref="List{TValue}"/>として扱う。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IReadOnlyList<TValue> ConvertList(IList list)
        {
            return list.Cast<TValue>().ToList();
        }

        protected virtual void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action) {
                case NotifyCollectionChangedAction.Add:
                    if(e.NewItems != null) {
                        // Collection.Count はすでに増えている(イベントから動いているので本処理は事後となる)
                        // ただし終端への挿入は追加扱いとなる(3要素ある際に Insert(`3', obj) とした場合は追加)
                        if(e.NewStartingIndex + 1 == Collection.Count) {
                            AddItems(ConvertList(e.NewItems));
                        } else {
                            InsertItems(e.NewStartingIndex, ConvertList(e.NewItems));
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if(e.OldItems != null) {
                        RemoveItems(ConvertList(e.OldItems), e.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if(e.NewItems != null && e.OldItems != null) {
                        ReplaceItems(e.NewStartingIndex, ConvertList(e.NewItems), ConvertList(e.OldItems));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    MoveItems(e.NewStartingIndex, e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ResetItems();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// インデックスを取得。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(TValue value)
        {
            ThrowIfDisposed();

            return Collection.IndexOf(value);
        }


        #endregion

        #region IDisposed

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 既に破棄済みの場合は処理を中断する。
        /// </summary>
        /// <param name="callerMemberName"></param>
        /// <exception cref="ObjectDisposedException">破棄済み。</exception>
        /// <seealso cref="IDisposed"/>
        protected void ThrowIfDisposed([CallerMemberName] string callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(callerMemberName);
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// </summary>
        /// <remarks>
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </remarks>
        /// <param name="disposing">CLRの管理下か。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2953:Methods named \"Dispose\" should implement \"IDisposable.Dispose\"", Justification = "OK")]
        protected virtual void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    CollectionNotifyCollectionChanged.CollectionChanged -= Collection_CollectionChanged;
                }
                CollectionNotifyCollectionChanged = null!;
                Collection = null!;

                IsDisposed = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3971:\"GC.SuppressFinalize\" should not be called", Justification = "OK")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2953:Methods named \"Dispose\" should implement \"IDisposable.Dispose\"", Justification = "OK")]
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IReadOnlyCollection

        public int Count
        {
            get
            {
                ThrowIfDisposed();

                return Collection.Count;
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            ThrowIfDisposed();

            return Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private void Collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ThrowIfDisposed();

            CollectionChanged(e);
        }

    }
}
