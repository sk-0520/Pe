﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 20:24
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	partial class NativeMethods
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/user32.registerhotkey
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="id"></param>
		/// <param name="fsModifiers"></param>
		/// <param name="vk"></param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, MOD fsModifiers, uint vk);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterHotKey (IntPtr hwnd, int id);
		
	}
}