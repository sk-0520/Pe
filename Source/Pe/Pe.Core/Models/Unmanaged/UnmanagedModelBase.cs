using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// 非マネージドオブジェクトをマネージドオブジェクトとして扱う。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnmanagedModelBase<T> : RawModel<T>
    {
        public UnmanagedModelBase(T rawObject)
            : base(rawObject)
        { }
    }

    /// <summary>
    /// アンマネージドなOS提供ハンドルを管理。
    /// </summary>
    public abstract class UnmanagedHandleModelBase : UnmanagedModelBase<IntPtr>
    {
        public UnmanagedHandleModelBase(IntPtr hHandle)
            : base(hHandle)
        {
            if(hHandle == IntPtr.Zero) {
                throw new ArgumentNullException(nameof(hHandle));
            }
        }

        #region property

        IntPtr Handle => Raw;

        #endregion

        #region function

        /// <summary>
        /// 解放処理。
        /// <para>ハンドルにより処理色々なんでオーバーライドしてごちゃごちゃする。</para>
        /// </summary>
        protected virtual void ReleaseHandle()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UnmanagedBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                ReleaseHandle();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
