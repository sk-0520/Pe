﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 20:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SC.Windows;

namespace PeUtility
{
	public enum IconSize
	{
		/// <summary>
		/// 16px
		/// </summary>
		Small = 16,
		/// <summary>
		/// 32px
		/// </summary>
		Normal = 32,
		/// <summary>
		/// 48px
		/// </summary>
		Big = 48,
		/// <summary>
		/// 256px
		/// </summary>
		Large = 256,
	}
	
	public static class IconLoader
	{
		public static int ToHeight(this IconSize iconSize)
		{
			return (int)iconSize;
		}
		public static Icon Load(string iconPath, IconSize iconSize, int iconIndex)
		{
			Icon result = null;
			if (iconSize == IconSize.Small || iconSize == IconSize.Normal) {
				// 16, 32 px
				var fileInfo = new SHFILEINFO();
				fileInfo.iIcon = iconIndex;

				var iconFlag = iconSize == IconSize.Small ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON;

				var hImgSmall = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_ICON | iconFlag);
				if (hImgSmall != IntPtr.Zero) {
					result = (Icon)System.Drawing.Icon.FromHandle(fileInfo.hIcon).Clone();
					API.DestroyIcon(fileInfo.hIcon);
				}
			} else {
				var shellImageList = iconSize == IconSize.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
				var fileInfo = new SHFILEINFO();
				var hImgSmall = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_SYSICONINDEX);

				IImageList imageList = null;
				var getImageListResult = API.SHGetImageList((int)shellImageList, ref API.IID_IImageList, ref imageList);

				if (getImageListResult == ComResult.S_OK) {
					IntPtr hIcon = IntPtr.Zero;
					var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
					result = (Icon)System.Drawing.Icon.FromHandle(hIcon).Clone();
					API.DestroyIcon(hIcon);
				}

			}

			return result;
		}
	}
}
