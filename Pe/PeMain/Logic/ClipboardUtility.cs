﻿namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// クリップボード関係の共通処理。
	/// </summary>
	public class ClipboardUtility
	{
		static void Copy(Action action, ClipboardSetting clipboardSetting)
		{
			//var prevCopy = false;
			if(clipboardSetting != null) {
				//prevCopy = commonData.MainSetting.Clipboard.DisabledCopy;
				clipboardSetting.DisabledCopy = !clipboardSetting.EnabledApplicationCopy;
				//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
			}
			action();
			if(clipboardSetting != null) {
				Task.Run(() => {
					Thread.Sleep(clipboardSetting.SleepTime);
					clipboardSetting.DisabledCopy = !clipboardSetting.DisabledCopy;
					//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
				});
			}
		}

		/// <summary>
		/// 文字列をクリップボードへ転写。
		/// </summary>
		/// <param name="text">対象文字列。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyText(string text, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), clipboardSetting);
		}

		/// <summary>
		/// RTFをクリップボードへ転写。
		/// </summary>
		/// <param name="rtf">対象RTF。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyRtf(string rtf, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), clipboardSetting);
		}

		/// <summary>
		/// HTMLをクリップボードへ転写。
		/// </summary>
		/// <param name="html">対象HTML。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyHtml(string html, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(html, TextDataFormat.Html), clipboardSetting);
		}

		/// <summary>
		/// 画像をクリップボードへ転写。
		/// </summary>
		/// <param name="image">対象画像。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyImage(Image image, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetImage(image), clipboardSetting);
		}

		/// <summary>
		/// ファイルをクリップボードへ転写。
		/// </summary>
		/// <param name="file">対象ファイル。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyFile(IEnumerable<string> file, ClipboardSetting clipboardSetting)
		{
			var sc = new StringCollection();
			sc.AddRange(file.ToArray());
			Copy(() => Clipboard.SetFileDropList(sc), clipboardSetting);
		}

		/// <summary>
		/// 複合データをクリップノードへ転写。
		/// </summary>
		/// <param name="data"></param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyDataObject(IDataObject data, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetDataObject(data), clipboardSetting);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="range"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		static string ConvertStringFromRawHtml(RangeItem<int> range, byte[] data)
		{
			if(-1 < range.Start && -1 < range.End && range.Start <= range.End) {
				var raw = data.Skip(range.Start).Take(range.End - range.Start);
				return Encoding.UTF8.GetString(raw.ToArray());
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clipboardHtml"></param>
		/// <param name="convertResult"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static bool TryConvertHtmlFromClipbordHtml(string clipboardHtml, out ClipboardHtmlDataItem convertResult, ILogger logger)
		{
			var result = new ClipboardHtmlDataItem();

			//Version:0.9
			//StartHTML:00000213
			//EndHTML:00001173
			//StartFragment:00000247
			//EndFragment:00001137
			//SourceURL:file:///C:/Users/sk/Documents/Programming/SharpDevelop%20Project/Pe/Pe/PeMain/etc/lang/ja-JP.accept.html

			var map = new Dictionary<string, Action<string>>() {
				{ "version", s => result.Version = decimal.Parse(s) },
				{ "starthtml", s => result.Html.Start = int.Parse(s) },
				{ "endhtml", s => result.Html.End = int.Parse(s) },
				{ "startfragment", s => result.Fragment.Start = int.Parse(s) },
				{ "endfragment", s => result.Fragment.End = int.Parse(s) },
				{ "sourceurl", s => result.SourceURL = new Uri(s) },
			};
			var reg = new Regex(@"^\s*(?<KEY>Version|StartHTML|EndHTML|StartFragment|EndFragment|SourceURL)\s*:\s*(?<VALUE>.+?)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			for(var match = reg.Match(clipboardHtml); match.Success; match = match.NextMatch()) {
				var key = match.Groups["KEY"].Value.ToLower();
				var value = match.Groups["VALUE"].Value;
				try {
					map[key](value);
				} catch(Exception ex) {
					logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage(key, ex));
				}
			}
			//
			//clipboardHtml.
			var rawHtml = Encoding.UTF8.GetBytes(clipboardHtml);
			result.HtmlText = ConvertStringFromRawHtml(result.Html, rawHtml); ;
			result.FragmentText = ConvertStringFromRawHtml(result.Fragment, rawHtml);
			result.SelectionText = ConvertStringFromRawHtml(result.Selection, rawHtml);
			
			convertResult = result;

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clipboardHtml"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static ClipboardHtmlDataItem ConvertHtmlFromClipbordHtml(string clipboardHtml, ILogger logger)
		{
			ClipboardHtmlDataItem temp;
			TryConvertHtmlFromClipbordHtml(clipboardHtml, out temp, logger);
			return temp;
		}

		//static ClipboardItem CreateClipboardItemFromPInvoke(ClipboardType enabledTypes, IntPtr hWnd)
		//{
		//	var clipboardItem = new ClipboardItem();

		//	NativeMethods.OpenClipboard(hWnd);
		//	try {
		//		var formatCount = NativeMethods.CountClipboardFormats();

		//		var format = NativeMethods.EnumClipboardFormats(0);
		//		while(format != 0) {
		//			format = NativeMethods.EnumClipboardFormats(format);
		//		}
		//	} finally {
		//		NativeMethods.CloseClipboard();
		//	}

		//	return clipboardItem;
		//}

		static ClipboardItem CreateClipboardItemFromFramework(ClipboardType enabledTypes)
		{
			var clipboardItem = new ClipboardItem();
			var clipboardData = Clipboard.GetDataObject();

			if(enabledTypes.HasFlag(ClipboardType.Text)) {
				if(clipboardData.GetDataPresent(DataFormats.UnicodeText)) {
					clipboardItem.Text = (string)clipboardData.GetData(DataFormats.UnicodeText);
					clipboardItem.ClipboardTypes |= ClipboardType.Text;
				} else if(clipboardData.GetDataPresent(DataFormats.Text)) {
					clipboardItem.Text = (string)clipboardData.GetData(DataFormats.Text);
					clipboardItem.ClipboardTypes |= ClipboardType.Text;
				}
			}

			if(enabledTypes.HasFlag(ClipboardType.Rtf) && clipboardData.GetDataPresent(DataFormats.Rtf)) {
				clipboardItem.Rtf = (string)clipboardData.GetData(DataFormats.Rtf);
				clipboardItem.ClipboardTypes |= ClipboardType.Rtf;
			}

			if(enabledTypes.HasFlag(ClipboardType.Html) && clipboardData.GetDataPresent(DataFormats.Html)) {
				clipboardItem.Html = (string)clipboardData.GetData(DataFormats.Html);
				clipboardItem.ClipboardTypes |= ClipboardType.Html;
			}

			if(enabledTypes.HasFlag(ClipboardType.Image) && clipboardData.GetDataPresent(DataFormats.Bitmap)) {
				var image = clipboardData.GetData(DataFormats.Bitmap) as Bitmap;
				if(image != null) {
					clipboardItem.Image = (Bitmap)image.Clone();
					clipboardItem.ClipboardTypes |= ClipboardType.Image;
				}
			}

			if(enabledTypes.HasFlag(ClipboardType.File) && clipboardData.GetDataPresent(DataFormats.FileDrop)) {
				var files = clipboardData.GetData(DataFormats.FileDrop) as string[];
				clipboardItem.Files = new List<string>(files);
				clipboardItem.Text = string.Join(Environment.NewLine, files);
				clipboardItem.ClipboardTypes |= ClipboardType.Text | ClipboardType.File;
			}

			if(clipboardItem.ClipboardTypes == ClipboardType.None) {
				clipboardItem.Dispose();
				return null;
			}

			return clipboardItem;
		}

		/// <summary>
		/// 現在のクリップボードからクリップボードアイテムを生成する。
		/// </summary>
		/// <param name="enabledTypes">取り込み対象とするクリップボード種別。</param>
		/// <returns>生成されたクリップボードアイテム。生成可能な種別がなければnullを返す。</returns>
		public static ClipboardItem CreateClipboardItem(ClipboardType enabledTypes, IntPtr hWnd)
		{
			var clipboardItem = CreateClipboardItemFromFramework(enabledTypes);
			return clipboardItem;
		}

		/// <summary>
		/// クリップボードアイテム一覧から指定したクリップボードのみのデータを抽出。
		/// </summary>
		/// <param name="list"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		public static List<ClipboardItem> FilterClipboardItemList(IReadOnlyList<ClipboardItem> list, ClipboardType types)
		{
			Debug.Assert(types != ClipboardType.None);

			Action<ClipboardType, ClipboardType, ClipboardItem, ClipboardItem> copyFunc = (filterTypes, type, src, dst) => {
				if(filterTypes.HasFlag(type) && src.ClipboardTypes.HasFlag(type)) {
					dst.ClipboardTypes |= type;
					switch(type) {
						case ClipboardType.Text:
							dst.Text = src.Text;
							break;
						case ClipboardType.Rtf:
							dst.Rtf = src.Rtf;
							break;
						case ClipboardType.Html:
							dst.Html = src.Html;
							break;
						case ClipboardType.Image:
							dst.Image = src.Image;
							break;
						case ClipboardType.File:
							dst.Files = src.Files;
							break;
						default:
							throw new NotImplementedException();
					}
				}
			};
			var result = new List<ClipboardItem>(list.Count);
			foreach(var item in list.Where(i => (i.ClipboardTypes & types) != 0)) {
				var clipboardItem = new ClipboardItem() {
					Name = item.Name,
					Timestamp = item.Timestamp,
					ClipboardTypes = ClipboardType.None,
				};
				foreach(var t in item.GetClipboardTypeList()) {
					copyFunc(types, t, item, clipboardItem);
				}
				if(clipboardItem.ClipboardTypes != ClipboardType.None) {
					result.Add(clipboardItem);
				}
			}
			return result;
		}

		static bool EqualClipboardItem_Impl(ClipboardItem a, ClipboardItem b, ClipboardType type)
		{
			switch(type) {
				case ClipboardType.Text:
					Debug.WriteLine("text: {0:8}, {1:8}", a.Text.GetHashCode(), b.Text.GetHashCode());
					return a.Text.GetHashCode() == b.Text.GetHashCode();
				case ClipboardType.Rtf:
					Debug.WriteLine("rtf: {0:8}, {1:8}", a.Rtf.GetHashCode(), b.Rtf.GetHashCode());
					return a.Rtf.GetHashCode() == b.Rtf.GetHashCode();
				case ClipboardType.Html:
					Debug.WriteLine("html: {0:8}, {1:8}", a.Html.GetHashCode(), b.Html.GetHashCode());
					return a.Html.GetHashCode() == b.Html.GetHashCode();
				case ClipboardType.Image:
					Debug.WriteLine("image: {0:8}, {1:8}", a.Image.GetHashCode(), b.Image.GetHashCode());
					return a.Image.GetHashCode() == b.Image.GetHashCode();
				case ClipboardType.File:
					Debug.WriteLine("file: {0:8}, {1:8}", a.Files.GetHashCode(), b.Files.GetHashCode());
					return a.Files.GetHashCode() == b.Files.GetHashCode();
				default:
					throw new NotImplementedException();
			}
		}

		public static bool EqualClipboardItem(ClipboardItem a, ClipboardItem b)
		{
			Debug.Assert(a != null);
			Debug.Assert(b != null);

			if(a.ClipboardTypes != b.ClipboardTypes) {
				return false;
			}

			var types = new[] {
				ClipboardType.Text,
				ClipboardType.Rtf,
				ClipboardType.Html,
				ClipboardType.Image,
				ClipboardType.File,
			};
			foreach(var type in types) {
				if(a.ClipboardTypes.HasFlag(type) && b.ClipboardTypes.HasFlag(type)) {
					var result = EqualClipboardItem_Impl(a, b, type);
					if(!result) {
						return false;
					}
				}
			}

			return true;
		}
	}
}
