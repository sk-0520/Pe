using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// 生のCOMを管理。
    /// </summary>
    public class SafeCom<T>: DisposerBase
        where T : class
    {
        #region variable

        [AllowNull]
        private T _instance;

        #endregion

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="comInstance">ラップする COM オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="comInstance"/> が <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="comInstance"/> が COM オブジェクトでない。</exception>
        public SafeCom(T comInstance)
        {
            ArgumentNullException.ThrowIfNull(comInstance);

            if(!Marshal.IsComObject(comInstance)) {
                throw new ArgumentException($"{nameof(Marshal)}.{nameof(Marshal.IsComObject)}", nameof(comInstance));
            }

            this._instance = comInstance;
        }

        #region property

        /// <summary>
        /// ラップしている COM オブジェクトのインスタンスを取得する。
        /// </summary>
        /// <exception cref="ObjectDisposedException">インスタンスが破棄済み。</exception>
        public T Instance
        {
            get
            {
                ThrowIfDisposed();

                if(this._instance is null) {
                    throw new InvalidOperationException();
                }

                return this._instance;
            }
        }

        #endregion

        #region function

        /// <summary>
        /// 内部の COM オブジェクトを指定した型にキャストして新しい <see cref="SafeCom{TCastType}"/> を生成する。
        /// </summary>
        /// <typeparam name="TCastType">キャスト先の型。</typeparam>
        /// <returns>キャストに成功した場合は新しいラッパーを返す。</returns>
        /// <exception cref="InvalidCastException">キャストに失敗。</exception>
        public SafeCom<TCastType> Cast<TCastType>()
            where TCastType : class
        {
            var castValue = Instance as TCastType;
            if(castValue is null) {
                throw new InvalidCastException($"{typeof(T).Name} -> {typeof(TCastType).Name}");
            }

            return new SafeCom<TCastType>(castValue);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(this._instance != null) {
                    Marshal.ReleaseComObject(this._instance);
                    this._instance = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="SafeCom{T}"/> の生成処理の簡略化。
    /// </summary>
    public static class Com
    {
        /// <summary>
        /// 指定した COM オブジェクトをラップする <see cref="SafeCom{T}"/> インスタンスを生成する。
        /// </summary>
        /// <typeparam name="T">ラップする COM オブジェクトの型。</typeparam>
        /// <param name="comObject">ラップする COM オブジェクト。</param>
        /// <returns>新しい <see cref="SafeCom{T}"/> インスタンス。</returns>
        public static SafeCom<T> Create<T>(T comObject)
            where T : class
        {
            return new SafeCom<T>(comObject);
        }
    }
}
