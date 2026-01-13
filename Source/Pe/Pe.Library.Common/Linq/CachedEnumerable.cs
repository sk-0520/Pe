using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ContentTypeTextNet.Pe.Library.Common.Linq
{
    public sealed class CachedEnumerable<T>: DisposerBase, IEnumerable<T>
    {
        #region variable

        /// <summary>
        /// 列挙対象。
        /// </summary>
        private IEnumerator<T> _enumerator;
        /// <summary>
        /// キャッシュ済み要素一覧。
        /// </summary>
        /// <remarks>実体。</remarks>
        private List<T> _cachedItems;
        /// <summary>
        /// マルチスレッド用ロック。
        /// </summary>
        private readonly Lock _locker = new Lock();
        /// <summary>
        /// 列挙が完了したか。
        /// </summary>
        /// <remarks>
        /// <para>実体。</para>
        /// </remarks>
        private volatile bool _isEnumerationCompleted = false;

        #endregion

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="enumerable"></param>
        public CachedEnumerable(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator())
        { }

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="enumerator"></param>
        public CachedEnumerable(IEnumerator<T> enumerator)
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
        private bool TryGetNextElement(int index, out T result)
        {
            // キャッシュ済みであればロック不要
            if(index < this._cachedItems.Count) {
                result = this._cachedItems[index];
                return true;
            }

            lock(this._locker) {
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

        public IEnumerator<T> GetEnumerator()
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
                if(disposing) {
                    lock(this._locker) {
                        this._enumerator?.Dispose();
                    }
                }
                this._enumerator = null!;
                this._cachedItems = null!;
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
}
