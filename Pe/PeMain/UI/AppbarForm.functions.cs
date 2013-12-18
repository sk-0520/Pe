﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of BaseToolbarForm_function.
	/// </summary>
	public partial class BaseToolbarForm
	{
		protected override CreateParams CreateParams {
			get {
				const int WS_EX_TOOLWINDOW = 0x80;

				var p = base.CreateParams;
				// AppBar として表示するには WS_EX_TOOLWINDOW スタイルが必要
				p.ExStyle = p.ExStyle | WS_EX_TOOLWINDOW;

				return p;
			}
		}
		
		private bool ResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			var appData = new APPBARDATA();
			appData.hWnd = Handle;
			
			this.callbackMessage = Windows.API.RegisterWindowMessage(MessageString);
			var registResult = Windows.API.SHAppBarMessage(ABM.ABM_NEW, ref appData);
			IsDocking = registResult.ToInt32() != 0;
			
			return IsDocking;
		}
		
		private bool UnResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			var appData = new APPBARDATA();
			appData.hWnd = Handle;

			var unregistResult = Windows.API.SHAppBarMessage(ABM.ABM_REMOVE, ref appData);
			
			IsDocking = false;
			this.callbackMessage = 0;
			
			return unregistResult.ToInt32() != 0;
		}
		
		private RECT CalcBarArea()
		{
			Debug.Assert(DockType != DockType.None);
			
			var desktopArea = DockScreen.Bounds;
			var barArea = new RECT();
			
			// 設定値からバー領域取得
			if(DockType == DockType.Left || DockType == DockType.Right) {
				barArea.Top = desktopArea.Top;
				barArea.Bottom = desktopArea.Bottom;
				if(DockType == DockType.Left) {
					barArea.Left = desktopArea.Left;
					barArea.Right = desktopArea.Left + BarSize.Width;
				} else {
					barArea.Left = desktopArea.Right - BarSize.Width;
					barArea.Right = desktopArea.Right;
				}
			} else {
				Debug.Assert(DockType == DockType.Top || DockType == DockType.Bottom);
				barArea.Left = desktopArea.Left;
				barArea.Right = desktopArea.Right;
				if(DockType == DockType.Top) {
					barArea.Top = desktopArea.Top;
					barArea.Bottom = desktopArea.Top + BarSize.Height;
				} else {
					barArea.Top = desktopArea.Bottom - BarSize.Height;
					barArea.Bottom = desktopArea.Bottom;
				}
			}
			
			return barArea;
		}
		
		/// <summary>
		/// ドッキングの実行
		/// 
		/// すでにドッキングされている場合はドッキングを再度実行する
		/// </summary>
		public void Docking()
		{
			if(DesignMode) {
				return;
			}
			if(DockType == DockType.None) {
				return;
			}
			
			// 得録済みであればいったん解除
			if(IsDocking) {
				UnResistAppBar();
			}
			// 登録
			Debug.Assert(!IsDocking);
			ResistAppBar();
			
			var appBar = new APPBARDATA();
			appBar.hWnd = Handle;
			appBar.uEdge = DockType.ToABE();
			appBar.rc = CalcBarArea();
			// 現在の希望するサイズから実際のサイズ要求する
			Windows.API.SHAppBarMessage(ABM.ABM_QUERYPOS, ref appBar);
			switch(DockType) {
				case DockType.Left:
					appBar.rc.Right = appBar.rc.Left + BarSize.Width;
					break;
					
				case DockType.Right:
					appBar.rc.Left = appBar.rc.Right - BarSize.Width;
					break;
					
				case DockType.Top:
					appBar.rc.Bottom = appBar.rc.Top + BarSize.Height;
					break;
					
				case DockType.Bottom:
					appBar.rc.Top = appBar.rc.Bottom - BarSize.Height;
					break;
					
				default:
					Debug.Assert(false, DockType.ToString());
					break;
			}
			
			// TopMost のときに領域を確保する
			if (TopMost) {
				var appbarResult = Windows.API.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
				Debug.WriteLine(appbarResult);
			}
			
			this.Bounds = new Rectangle(
				appBar.rc.Left,
				appBar.rc.Top,
				appBar.rc.Right - appBar.rc.Left,
				appBar.rc.Bottom - appBar.rc.Top
			);
		}
	}
}
