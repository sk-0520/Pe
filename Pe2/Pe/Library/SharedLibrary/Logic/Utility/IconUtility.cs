﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Com;
	using ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Gdi;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// アイコン取得共通処理。
	/// </summary>
	public static class IconUtility
	{
		const int sizeofGRPICONDIR_idCount = 4;
		const int offsetGRPICONDIRENTRY_nID = 12;
		const int offsetGRPICONDIRENTRY_dwBytesInRes = 8;
		static readonly int sizeofICONDIR;
		static readonly int sizeofICONDIRENTRY;
		static readonly int sizeofGRPICONDIRENTRY;

		static IconUtility()
		{
			sizeofICONDIR = Marshal.SizeOf<ICONDIR>();
			sizeofICONDIRENTRY = Marshal.SizeOf<ICONDIRENTRY>();
			sizeofGRPICONDIRENTRY = Marshal.SizeOf<GRPICONDIRENTRY>();
		}

		///// <summary>
		///// GBITAMPからBitmap作成。
		///// 
		///// http://svn.nate.deepcreek.org.au/svn/KeyboardRedirector/trunk/IconExtractor/IconExtractor.cs
		///// </summary>
		///// <param name="hBitmap"></param>
		///// <returns></returns>
		//static Bitmap BitmapFromhBitmap(BitmapHandleModel hBitmap)
		//{
		//	var plainBitmap = hBitmap.ToManagedBitmap(); //Image.FromHbitmap(hBitmap.Handle);
		//	var bmData = new BitmapData[2];
		//	var bounds = new Rectangle(0, 0, plainBitmap.Width, plainBitmap.Height);
		//	bmData[0] = plainBitmap.LockBits(bounds, ImageLockMode.ReadOnly, plainBitmap.PixelFormat);

		//	var transparentBitmap = new Bitmap(bmData[0].Width, bmData[0].Height, bmData[0].Stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmData[0].Scan0);
		//	bmData[1] = transparentBitmap.LockBits(bounds, System.Drawing.Imaging.ImageLockMode.WriteOnly, transparentBitmap.PixelFormat);
		//	try {
		//		Marshal.StructureToPtr(bmData[0].Scan0, bmData[1].Scan0, true);
		//	} finally {
		//		transparentBitmap.UnlockBits(bmData[1]);
		//		plainBitmap.UnlockBits(bmData[0]);
		//	}
		//	var isPlain = true;
		//	for (int y = 0; y < transparentBitmap.Height; y++) {
		//		for (int x = 0; x < transparentBitmap.Width; x++) {
		//			if ((x > 0) && (y > 0)) {
		//				byte alpha = transparentBitmap.GetPixel(x, y).A;
		//				if (alpha > 0) {
		//					isPlain = false;
		//					break;
		//				}
		//			}
		//		}
		//	}

		//	if (!isPlain) {
		//		return transparentBitmap;
		//	}

		//	return plainBitmap;
		//}

		/// <summary>
		/// ファイルのサムネイルを取得。
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		public static ImageSource GetThumbnailImage(string iconPath, IconScale iconScale)
		{
			try {
				IShellItem iShellItem = null;
				NativeMethods.SHCreateItemFromParsingName(iconPath, IntPtr.Zero, NativeMethods.IID_IShellItem, out iShellItem);

				var size = iconScale.ToSize();
				var siigbf = SIIGBF.SIIGBF_RESIZETOFIT;
				var hResultBitmap = IntPtr.Zero;
				using (var shellItem = new ComModel<IShellItem>(iShellItem)) {
					((IShellItemImageFactory)shellItem.Com).GetImage(PodStructUtility.Convert(size), siigbf, out hResultBitmap);
				}
				using (var hBitmap = new BitmapHandleModel(hResultBitmap)) {
					var result = hBitmap.MakeImageSource();
					return result;
				}
			} catch (COMException ex) {
				Debug.WriteLine("{0}, {1}", ex, iconPath);
				return null;
			} catch (ArgumentException ex) {
				Debug.WriteLine("{0}, {1}", ex, iconPath);
				return null;
			}
		}

		/// <summary>
		/// http://hp.vector.co.jp/authors/VA016117/rsrc2icon.html
		/// </summary>
		/// <param name="hModule"></param>
		/// <param name="name"></param>
		/// <param name="resType"></param>
		/// <returns></returns>
		static byte[] GetResourceBinaryData(IntPtr hModule, IntPtr name, ResType resType)
		{
			var hGroup = NativeMethods.FindResource(hModule, name, new IntPtr((int)resType));
			if (hGroup == IntPtr.Zero) {
				Debug.WriteLine("return FindResource");
				return null;
			}

			var hLoadGroup = NativeMethods.LoadResource(hModule, hGroup);
			if (hLoadGroup == IntPtr.Zero) {
				Debug.WriteLine("return LoadResource");
				return null;
			}

			var resData = NativeMethods.LockResource(hLoadGroup);
			if (resData == IntPtr.Zero) {
				Debug.WriteLine("return LockResource");
				return null;
			}

			var resSize = NativeMethods.SizeofResource(hModule, hGroup);
			if (resSize == 0) {
				Debug.WriteLine("return SizeofResource");
				return null;
			}

			var resBinary = new byte[resSize];
			Marshal.Copy(resData, resBinary, 0, resBinary.Length);

			return resBinary;
		}

		/// <summary>
		/// https://github.com/TsudaKageyu/IconExtractor
		/// </summary>
		/// <param name="resourcePath"></param>
		/// <returns></returns>
		static IList<byte[]> LoadIconResource(string resourcePath)
		{
			var hModule = NativeMethods.LoadLibraryEx(resourcePath, IntPtr.Zero, LOAD_LIBRARY.LOAD_LIBRARY_AS_DATAFILE);
			var binaryList = new List<byte[]>();
			EnumResNameProc proc = (hMod, type, name, lp) => {
				var binaryGroupIconData = GetResourceBinaryData(hMod, name, ResType.GROUP_ICON);
				if (binaryGroupIconData != null) {
					var iconCount = BitConverter.ToUInt16(binaryGroupIconData, sizeofGRPICONDIR_idCount);
					//Debug.WriteLine("iconCount = {0}", iconCount);

					var totalSize = sizeofICONDIR + sizeofICONDIRENTRY * iconCount;
					foreach (var i in Enumerable.Range(0, iconCount)) {
						var readOffset = sizeofICONDIR + (sizeofGRPICONDIRENTRY * i) + offsetGRPICONDIRENTRY_dwBytesInRes;
						if(binaryGroupIconData.Length < 0 && readOffset + sizeof(Int32) <binaryGroupIconData.Length) {
							break;
						}
						var length = BitConverter.ToInt32(
							binaryGroupIconData,
							readOffset
						);
						//Debug.WriteLine("[{0}] = {1} byte", i, length);
						totalSize += length;
					}
					//Debug.WriteLine("totalSize = {0}", totalSize);

					using (var stream = new BinaryWriter(new MemoryStream(totalSize))) {
						stream.Write(binaryGroupIconData, 0, sizeofICONDIR);

						var picOffset = sizeofICONDIR + sizeofICONDIRENTRY * iconCount;
						foreach (var i in Enumerable.Range(0, iconCount)) {
							stream.Seek(sizeofICONDIR + sizeofICONDIRENTRY * i, SeekOrigin.Begin);
							var offsetWrite = sizeofICONDIR + sizeofGRPICONDIRENTRY * i;
							if (binaryGroupIconData.Length <= offsetWrite + offsetGRPICONDIRENTRY_nID) {
								continue;
							}
							stream.Write(binaryGroupIconData, offsetWrite, offsetGRPICONDIRENTRY_nID);
							stream.Write(picOffset);

							stream.Seek(picOffset, SeekOrigin.Begin);

							ushort id = BitConverter.ToUInt16(binaryGroupIconData, sizeofICONDIR + sizeofGRPICONDIRENTRY * i + offsetGRPICONDIRENTRY_nID);
							var pic = GetResourceBinaryData(hModule, new IntPtr(id), ResType.ICON);
							if (pic != null) {
								stream.Write(pic, 0, pic.Length);
								picOffset += pic.Length;
							}
						}

						binaryList.Add(((MemoryStream)stream.BaseStream).ToArray());
					}
				}

				return true;
			};

			NativeMethods.EnumResourceNames(hModule, (int)ResType.GROUP_ICON, proc, IntPtr.Zero);

			return binaryList;
		}


		/// <summary>
		/// 16px, 32pxアイコン取得。
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <param name="iconIndex"></param>
		/// <param name="hasIcon"></param>
		/// <returns></returns>
		static ImageSource LoadNormalIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			Debug.Assert(new[] { IconScale.Small, IconScale.Normal }.Any(i => i == iconScale), iconScale.ToString());
			Debug.Assert(0 <= iconIndex, iconIndex.ToString());

			// 16, 32 px
			if (hasIcon) {
				var iconHandle = new IntPtr[1];
				if (iconScale == IconScale.Small) {
					NativeMethods.ExtractIconEx(iconPath, iconIndex, null, iconHandle, 1);
				} else {
					Debug.Assert(iconScale == IconScale.Normal);
					NativeMethods.ExtractIconEx(iconPath, iconIndex, iconHandle, null, 1);
				}
				if (iconHandle[0] != IntPtr.Zero) {
					using (var hIcon = new IconHandleModel(iconHandle[0])) {
						return hIcon.MakeImageSource();
					}
				}
			}

			if (iconScale == IconScale.Normal) {
				try {
					//using (var bitmap = GetThumbnailImage(iconPath, iconScale)) {
					//	if (bitmap != null) {
					//		//result = (Icon)System.Drawing.Icon.FromHandle(bitmap.GetHicon()).Clone();
					//		using (var hBitmap = new BitmapHandleModel(bitmap)) {
					//			return hIcon.MakeImageSource();
					//		}
					//	}
					//}
					var thumbnailImage = GetThumbnailImage(iconPath, iconScale);
					if (thumbnailImage != null) {
						return thumbnailImage;
					}
				} catch (Exception ex) {
					Debug.WriteLine(ex);
				}
			}

			var fileInfo = new SHFILEINFO();
			SHGFI flag = SHGFI.SHGFI_ICON;
			if (iconScale == IconScale.Small) {
				flag |= SHGFI.SHGFI_SMALLICON;
			} else {
				Debug.Assert(iconScale == IconScale.Normal);
				flag |= SHGFI.SHGFI_LARGEICON;
			}
			var fileInfoResult = NativeMethods.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flag);
			if (fileInfo.hIcon != IntPtr.Zero) {
				using (var hIcon = new IconHandleModel(fileInfo.hIcon)) {
					return hIcon.MakeImageSource();
				}
			}

			return null;
		}

		/// <summary>
		/// 48px以上のアイコン取得。
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <param name="iconIndex"></param>
		/// <param name="hasIcon"></param>
		/// <returns></returns>
		static ImageSource LoadLargeIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			//Debug.Assert(iconScale.IsIn(IconScale.Big, IconScale.Large), iconScale.ToString());
			Debug.Assert(new[] { IconScale.Big, IconScale.Large }.Any(i => i == iconScale), iconScale.ToString());
			Debug.Assert(0 <= iconIndex, iconIndex.ToString());

			if (hasIcon) {
				try {
					var iconList = LoadIconResource(iconPath);
					if (iconIndex < iconList.Count) {
						return ImageUtility.ImageSourceFromBinaryIcon(iconList[iconIndex], iconScale.ToSize());
					}
				} catch (Exception ex) {
					Debug.WriteLine(ex);
				}
			}

			//using (var bitmap = GetThumbnailImage(iconPath, iconScale)) {
			//	if (bitmap != null) {
			//		using (var hIcon = UnmanagedIcon.FromBitmap(bitmap)) {
			//			return hIcon.ToManagedIcon();
			//		}
			//	}
			//}
			var thumbnailImage = GetThumbnailImage(iconPath, iconScale);
			if (thumbnailImage != null) {
				return thumbnailImage;
			}

			var shellImageList = iconScale == IconScale.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
			var fileInfo = new SHFILEINFO() {
				iIcon = iconIndex,
			};

			var infoFlags = SHGFI.SHGFI_SYSICONINDEX;
			var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

			IImageList resultImageList = null;
			try {
				var getImageListResult = NativeMethods.SHGetImageList((int)shellImageList, ref NativeMethods.IID_IImageList, out resultImageList);

				if (getImageListResult == ComResult.S_OK) {
					Debug.Assert(resultImageList != null);
					using (var imageList = new ComModel<IImageList>(resultImageList)) {
						int n = 0;
						imageList.Com.GetImageCount(ref n);

						var hResultIcon = IntPtr.Zero;
						var hResult = imageList.Com.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hResultIcon);
						if (hResultIcon != IntPtr.Zero) {
							using (var hIcon = new IconHandleModel(hResultIcon)) {
								return hIcon.MakeImageSource();
							}
						}
					}
				}
			} catch (InvalidCastException ex) {
				Debug.WriteLine(ex);
			}

			return null;
		}

		/// <summary>
		/// アイコンを取得。
		/// </summary>
		/// <param name="iconPath">対象ファイルパス。</param>
		/// <param name="iconScale">アイコンサイズ。</param>
		/// <param name="iconIndex">アイコンインデックス。</param>
		/// <returns>取得したアイコン。呼び出し側で破棄が必要。</returns>
		public static ImageSource Load(string iconPath, IconScale iconScale, int iconIndex)
		{
			// 実行形式
			var hasIcon = PathUtility.HasIconPath(iconPath);
			var useIconIndex = Math.Abs(iconIndex);

			ImageSource result = null;
			if (iconScale == IconScale.Small || iconScale == IconScale.Normal) {
				result = LoadNormalIcon(iconPath, iconScale, useIconIndex, hasIcon);
			} else {
				result = LoadLargeIcon(iconPath, iconScale, useIconIndex, hasIcon);
			}

			return result;
		}

	}




}
