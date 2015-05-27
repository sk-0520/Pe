﻿namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.Model;

	/// <summary>
	/// アンマネージドオブジェクトハンドルを管理。
	/// </summary>
	public abstract class UnmanagedHandleModel: UnmanagedModelBase
	{
		public UnmanagedHandleModel(IntPtr hHandle)
			: base()
		{
			if(hHandle == IntPtr.Zero) {
				throw new ArgumentNullException("hHandle");
			}

			Handle = hHandle;
		}

		/// <summary>
		/// ハンドル。
		/// </summary>
		public IntPtr Handle { get; private set; }

		/// <summary>
		/// 解放処理。
		/// 
		/// ハンドルにより処理色々なんでオーバーライドしてごちゃごちゃする。
		/// </summary>
		protected virtual void ReleaseHandle()
		{
			NativeMethods.DeleteObject(Handle);
		}

		#region UnmanagedBase

		protected override void Dispose(bool disposing)
		{
			ReleaseHandle();
			Handle = IntPtr.Zero;

			base.Dispose(disposing);
		}

		#endregion
	}
}
