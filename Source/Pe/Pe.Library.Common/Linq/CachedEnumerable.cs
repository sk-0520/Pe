using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ContentTypeTextNet.Pe.Library.Common.Linq
{
    /// <summary>
    /// 列挙処理のキャッシュ基底。
    /// </summary>
    /// <typeparam name="T">列挙対象の型。</typeparam>
    public abstract class CachedEnumerableBase<T>: DisposerBase, IEnumerable<T>
    {
        #region variable

        /// <summary>
        /// 列挙対象。
        /// </summary>
        private protected IEnumerator<T> _enumerator;
        /// <summary>
        /// キャッシュ済み要素一覧。
        /// </summary>
        /// <remarks>実体。</remarks>
        private protected List<T> _cachedItems;
        /// <summary>
        /// 列挙が完了したか。
        /// </summary>
        /// <remarks>
        /// <para>実体。</para>
        /// </remarks>
        private protected volatile bool _isEnumerationCompleted;

        #endregion

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="enumerable"></param>
        protected CachedEnumerableBase(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator())
        { }

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="enumerator"></param>
        protected CachedEnumerableBase(IEnumerator<T> enumerator)
        {
            this._enumerator = enumerator;
            this._cachedItems = new List<T>();
        }

        #region property

        /// <summary>
        /// キャッシュ済み要素一覧。
        /// </summary>
        public IReadOnlyList<T> Cached => this._cachedItems;
        /// <summary>
        /// 列挙が完了したか。
        /// </summary>
        public bool IsEnumerationCompleted => this._isEnumerationCompleted;

        #endregion

        #region function

        /// <summary>
        /// 要素を取得。
        /// </summary>
        /// <param name="index">インデックス。</param>
        /// <param name="result">結果。</param>
        /// <returns>取得できたか。<see langword="true"/> の場合に <paramref name="result"/> に要素が格納される。</returns>
        /// <remarks>
        /// <para>関数名からは予測しにくいが、列挙完了後に元となる _enumerator のクリーンアップまで行う。</para>
        /// </remarks>
        protected virtual bool TryGetNextElement(int index, out T result)
        {
            // キャッシュ済みであればロック不要
            if(index < this._cachedItems.Count) {
                result = this._cachedItems[index];
                return true;
            }

            if(this._enumerator is null) {
                result = default!;
                return false;
            }

            // 順々取得してキャッシュ化
            if(this._enumerator.MoveNext()) {
                var current = this._enumerator.Current;
                this._cachedItems.Add(current);
                result = current;
                return true;
            }

            // 列挙完了!
            this._isEnumerationCompleted = true;
            this._enumerator.Dispose();
            this._enumerator = null!;

            result = default!;
            return false;
        }

        #endregion

        #region IEnumerable

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            ThrowIfDisposed();

            return GetEnumerator();
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                // this._enumerator の破棄は継承先で行う
                this._enumerator = null!;
                this._cachedItems = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class CachedEnumerable<T>: CachedEnumerableBase<T>
    {
        /// <inheritdoc cref="CachedEnumerableBase{T}.CachedEnumerableBase(IEnumerable{T})"/>
        public CachedEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        /// <inheritdoc cref="CachedEnumerableBase{T}.CachedEnumerableBase(IEnumerator{T})"/>
        public CachedEnumerable(IEnumerator<T> enumerator)
            : base(enumerator)
        { }


        #region CachedEnumerableBase

        public override IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();

            // すでに完了している場合はキャッシュから返すだけ
            if(this._isEnumerationCompleted) {
                foreach(var cachedValue in this._cachedItems) {
                    yield return cachedValue;
                }
                yield break;
            }

            // キャッシュ構築しながら順次取得
            var index = 0;
            while(TryGetNextElement(index++, out var current)) {
                yield return current;
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    this._enumerator?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class ConcurrentCachedEnumerable<T>: CachedEnumerableBase<T>
    {
        #region variable

        /// <summary>
        /// マルチスレッド用ロック。
        /// </summary>
        private readonly Lock _sync = new Lock();

        #endregion

        /// <inheritdoc cref="CachedEnumerableBase{T}.CachedEnumerableBase(IEnumerable{T})"/>
        public ConcurrentCachedEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        /// <inheritdoc cref="CachedEnumerableBase{T}.CachedEnumerableBase(IEnumerator{T})"/>
        public ConcurrentCachedEnumerable(IEnumerator<T> enumerator)
            : base(enumerator)
        { }

        #region function

        protected override bool TryGetNextElement(int index, out T result)
        {
            // キャッシュ済みであればロック不要
            if(index < this._cachedItems.Count) {
                result = this._cachedItems[index];
                return true;
            }

            lock(this._sync) {
                // ロック待ち中にキャッシュに追加されている可能性あり
                if(index < this._cachedItems.Count) {
                    result = this._cachedItems[index];
                    return true;
                }

                if(this._enumerator is null) {
                    result = default!;
                    return false;
                }

                // 順々取得してキャッシュ化
                if(this._enumerator.MoveNext()) {
                    var current = this._enumerator.Current;
                    this._cachedItems.Add(current);
                    result = current;
                    return true;
                }

                // 列挙完了!
                this._isEnumerationCompleted = true;
                this._enumerator.Dispose();
                this._enumerator = null!;

                result = default!;
                return false;
            }
        }

        #endregion

        #region IEnumerable

        public override IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();

            // すでに完了している場合はキャッシュから返すだけ
            if(this._isEnumerationCompleted) {
                foreach(var cachedValue in this._cachedItems) {
                    yield return cachedValue;
                }
                yield break;
            }

            // キャッシュ構築しながら順次取得
            var index = 0;
            while(TryGetNextElement(index++, out var current)) {
                yield return current;
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    lock(this._sync) {
                        this._enumerator?.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="CachedEnumerable{T}"/> 生成用。
    /// </summary>
    public static class CachedEnumerable
    {
        #region function

        public static CachedEnumerable<T> Create<T>(IEnumerable<T> enumerable)
        {
            return new CachedEnumerable<T>(enumerable);
        }

        public static CachedEnumerable<T> Create<T>(IEnumerator<T> enumerator)
        {
            return new CachedEnumerable<T>(enumerator);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ConcurrentCachedEnumerable{T}"/> 生成用。
    /// </summary>
    public static class ConcurrentCachedEnumerable
    {
        #region function

        public static ConcurrentCachedEnumerable<T> Create<T>(IEnumerable<T> enumerable)
        {
            return new ConcurrentCachedEnumerable<T>(enumerable);
        }

        public static ConcurrentCachedEnumerable<T> Create<T>(IEnumerator<T> enumerator)
        {
            return new ConcurrentCachedEnumerable<T>(enumerator);
        }

        #endregion
    }
}
