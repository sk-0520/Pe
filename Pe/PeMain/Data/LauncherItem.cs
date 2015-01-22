﻿namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// ランチャー設定データ。
	/// 
	/// 名前をキーとする。
	/// </summary>
	[Serializable]
	public class LauncherItem: DisposableNameItem, IDisposable, ICloneable
	{
		/// <summary>
		/// 見つからなかった時用アイコン。
		/// </summary>
		public static IReadOnlyDictionary<IconScale, Icon> notfoundIconMap;
		public static IReadOnlyDictionary<IconScale, Icon> commandIconMap;

		public static void SetSkin(ISkin skin)
		{
			var iconScaleList = new[] { IconScale.Small, IconScale.Normal, IconScale.Big };
			// NotFound 
			var tempNotfoundIconMap = new Dictionary<IconScale, Icon>(iconScaleList.Length);
			foreach(var iconScale in iconScaleList) {
				var iconSize = iconScale.ToSize();
				var icon = new Icon(skin.GetIcon(SkinIcon.NotFound), iconSize);
				var image = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(image)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, iconSize));
				}
				tempNotfoundIconMap[iconScale] = icon;
			}
			notfoundIconMap = tempNotfoundIconMap;

			// URIアイコン構築
			var tempCommandIconMap = new Dictionary<IconScale, Icon>(iconScaleList.Length);
			foreach(var iconScale in iconScaleList) {
				var iconSize = iconScale.ToSize();
				var icon = new Icon(skin.GetIcon(SkinIcon.Command), iconSize);
				var image = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(image)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, iconSize));
				}
				tempCommandIconMap[iconScale] = icon;
			}
			commandIconMap = tempCommandIconMap;
		}

		/// <summary>
		/// 現在のアイテムが保持するアイコン一覧。
		/// </summary>
		private Dictionary<IconScale, Icon> _iconMap;

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			LauncherItem item = obj as LauncherItem;
			if(item == null) {
				return false;
			}
			return IsNameEqual(item.Name);
		}

		public override int GetHashCode()
		{
			//if(this.Name == null) {
			//	return default(int);
			//}
			return Name.GetHashCode();
		}
		#endregion

		public LauncherItem()
		{
			this._iconMap = new Dictionary<IconScale, Icon>();

			HasError = false;

			IconItem = new IconItem();
			LauncherType = LauncherType.File;
			LauncherHistory = new LauncherHistory();
			EnvironmentSetting = new EnvironmentSetting();
			Tag = new List<string>();
		}

		public override void CorrectionValue()
		{
			base.CorrectionValue();

			if(IconItem == null) {
				IconItem = new IconItem();
			}

			if(LauncherType == LauncherType.URI) {
				LauncherType = LauncherType.Command;
			}
		}

		/// <summary>
		/// 登録データ種別。
		/// </summary>
		public LauncherType LauncherType { get; set; }
		/// <summary>
		/// 実行時に使用される値。
		/// </summary>
		public string Command { get; set; }
		/// <summary>
		/// 実行時の作業ディレクトリ。
		/// </summary>
		public string WorkDirPath { get; set; }
		/// <summary>
		/// 実行時に渡されるオプション。
		/// </summary>
		public string Option { get; set; }

		/// <summary>
		/// 表示アイコンパス。
		/// </summary>
		public IconItem IconItem { get; set; }

		/// <summary>
		/// 実行履歴
		/// </summary>
		public LauncherHistory LauncherHistory { get; set; }
		/// <summary>
		/// コメント
		/// </summary>
		public string Note { get; set; }
		/// <summary>
		/// タグ
		/// </summary>
		public List<string> Tag { get; set; }
		/// <summary>
		/// 標準出力(とエラー)の監視
		/// </summary>
		public bool StdOutputWatch { get; set; }
		/// <summary>
		/// 管理者として実行
		/// </summary>
		public bool Administrator { get; set; }
		/// <summary>
		/// 環境変数
		/// </summary>
		public EnvironmentSetting EnvironmentSetting { get; set; }

		/*
		public bool IsExtExec
		{
			get
			{
				if(LauncherType != LauncherType.File) {
					return false;
				}
				
				return Path.GetExtension(Command).ToLower() == ".exe";
			}
		}
		*/

		/// <summary>
		/// 存在するか
		/// </summary>
		public bool IsExists
		{
			get
			{
				if(LauncherType != LauncherType.File) {
					return false;
				}

				var expandCommand = Environment.ExpandEnvironmentVariables(Command);
				var result = (File.Exists(Command) || Directory.Exists(Command)) || (File.Exists(expandCommand) || Directory.Exists(expandCommand));
				return result;
			}
		}

		/// <summary>
		/// 現在アイテムが管理者として実行可能か
		/// </summary>
		[Obsolete("ちょっと隠居してくれ")]
		public bool CanAdministratorExecute
		{
			get
			{
				if(IsDirectory) {
					return false;
				}

				var dotExt = Path.GetExtension(Command).ToLower();
				return new[] { ".exe", ".bat", ".cmd" }.Any(ext => ext == dotExt);
			}
		}

		public bool IsExexuteFile
		{
			get
			{
				return Path.GetExtension(Command).ToLower() == ".exe";
			}
		}

		public bool IsDirectory
		{
			get
			{
				if((new[] { LauncherType.File, LauncherType.Directory }).All(lt => lt != LauncherType)) {
					return false;
				}
				var expandCommand = Environment.ExpandEnvironmentVariables(Command);
				return Directory.Exists(expandCommand);
			}
		}


		[XmlIgnoreAttribute()]
		public bool HasError { get; set; }

		protected override void Dispose(bool disposing)
		{
			foreach(var icon in _iconMap.Values) {
				icon.ToDispose();
			}

			base.Dispose(disposing);
		}

		public bool IsNameEqual(string name)
		{
			return Name == name;
		}

		public object Clone()
		{
			var result = new LauncherItem();
			result.Name = Name;
			result.LauncherType = LauncherType;
			result.Command = Command;
			result.WorkDirPath = WorkDirPath;
			result.Option = Option;
			/*
			result.IconPath = IconPath;
			result.IconIndex = IconIndex;
			 */
			result.IconItem = (IconItem)IconItem.Clone();
			result.LauncherHistory = (LauncherHistory)LauncherHistory.Clone();
			result.EnvironmentSetting = (EnvironmentSetting)EnvironmentSetting.Clone();
			result.Note = Note;
			result.Tag.AddRange(Tag);
			result.StdOutputWatch = StdOutputWatch;
			result.Administrator = Administrator;

			foreach(KeyValuePair<IconScale, Icon> pair in this._iconMap) {
				result._iconMap.Add(pair.Key, (Icon)pair.Value.Clone());
			}

			return result;
		}

		/// <summary>
		/// アイコン取得。
		/// </summary>
		/// <param name = "iconScale">アイコンサイズ</param>
		/// <param name="iconIndex">アイコンインデックス</param>
		/// <returns>アイコン</returns>
		public Icon GetIcon(IconScale iconScale, int iconIndex, ApplicationSetting applicationSetting)
		{
			var hasIcon = this._iconMap.ContainsKey(iconScale);
			if(!hasIcon) {
				string useIconPath = null;

				if(LauncherType == LauncherType.Embedded) {
					Debug.Assert(applicationSetting != null);
					var applicationItem = applicationSetting.GetApplicationItem(this);
					useIconPath = applicationItem.FilePath;
					hasIcon = true;
				} else {
					if(!string.IsNullOrWhiteSpace(IconItem.Path)) {
						var expandIconPath = Environment.ExpandEnvironmentVariables(IconItem.Path);
						//hasIcon = File.Exists(expandIconPath) || Directory.Exists(expandIconPath);
						hasIcon = FileUtility.Exists(expandIconPath);
						useIconPath = expandIconPath;
					}
					if(!hasIcon) {
						if(new[] { LauncherType.File, LauncherType.Directory }.Any(lt => lt == LauncherType)) {
							if(!string.IsNullOrWhiteSpace(Command)) {
								var expandPath = Environment.ExpandEnvironmentVariables(Command);
								//hasIcon = File.Exists(expandPath) || Directory.Exists(expandPath);
								hasIcon = FileUtility.Exists(expandPath);
								useIconPath = expandPath;
							}
						}
					}
				}
				if(hasIcon) {
					Debug.Assert(useIconPath != null);

					var icon = IconUtility.Load(useIconPath, iconScale, iconIndex);
					this._iconMap[iconScale] = icon;
				}
			}

			if(hasIcon) {
				return this._iconMap[iconScale];
			} else {
				if(LauncherType == LauncherType.URI || LauncherType == LauncherType.Command) {
					return commandIconMap[iconScale];
				} else {
					return notfoundIconMap[iconScale];
				}
			}
		}

		public Icon GetEmbeddedIcon(IconScale iconScale, ApplicationItem applicationItem)
		{
			var icon = IconUtility.Load(applicationItem.FilePath, iconScale, 0);
			return icon;
		}

		public void ClearIcon()
		{
			this._iconMap.Clear();
		}

		/// <summary>
		/// ファイルパスからランチャーアイテムの生成。
		/// </summary>
		/// <param name="expandPath"></param>
		/// <param name="useShortcut"></param>
		/// <param name = "forceLauncherType"></param>
		/// <param name = "forceType"></param>
		/// <returns></returns>
		public static LauncherItem LoadFile(string expandPath, bool useShortcut, bool forceLauncherType, LauncherType forceType)
		{
#if DEBUG
			if(forceLauncherType) {
				Debug.Assert(forceType != LauncherType.None);
			}
#endif

			var item = new LauncherItem();

			item.Name = Path.GetFileNameWithoutExtension(expandPath);
			if(item.Name.Length == 0 && expandPath.Length >= @"C:\".Length) {
				var drive = DriveInfo.GetDrives().SingleOrDefault(d => d.Name == expandPath);
				if(drive != null) {
					item.Name = drive.VolumeLabel;
				}
			}
			if(item.Name.Length == 0) {
				item.Name = expandPath;
			}
			item.Command = expandPath;
			/*
			item.IconPath = filePath;
			item.IconIndex = 0;
			 */
			item.IconItem.Path = expandPath;
			item.IconItem.Index = 0;

			if(Directory.Exists(expandPath)) {
				// ディレクトリ
				if(forceLauncherType) {
					item.LauncherType = forceType;
				} else {
					item.LauncherType = LauncherType.Directory;
				}
			} else {
				// ファイルとかもろもろ
				if(forceLauncherType) {
					item.LauncherType = forceType;
				} else {
					item.LauncherType = LauncherType.File;
				}

				var dotExt = Path.GetExtension(expandPath);
				switch(dotExt.ToLower()) {
					case ".lnk": {
							if(!useShortcut) {
								using(var shortcut = new ShortcutFile(expandPath)) {
									item.Command = shortcut.TargetPath;
									item.Option = shortcut.Arguments;
									item.WorkDirPath = shortcut.WorkingDirectory;
									item.IconItem = new IconItem(shortcut.GetIcon());
									item.Note = shortcut.Description;
								}
							}
						}
						break;

					/*
				case ".url":
					item.LauncherType = LauncherType.URI;
					break;
					 */

					case ".exe": {
							var verInfo = FileVersionInfo.GetVersionInfo(item.Command);
							if(!string.IsNullOrEmpty(verInfo.ProductName)) {
								item.Name = verInfo.ProductName;
							}
							item.Note = verInfo.Comments;
							if(!string.IsNullOrEmpty(verInfo.CompanyName)) {
								item.Tag.Add(verInfo.CompanyName);
							}

						}
						break;

					default:
						break;
				}
			}


			Debug.Assert(item.Name.Length > 0);

			return item;
		}

		public static LauncherItem LoadFile(string expandPath, bool useShortcut)
		{
			return LoadFile(expandPath, useShortcut, false, LauncherType.None);
		}

		static public string GetUniqueName(LauncherItem item, IEnumerable<LauncherItem> seq)
		{
			return TextUtility.ToUniqueDefault(item.Name, seq.Select(i => i.Name));
		}

		public bool IsValueEqual(LauncherItem target)
		{
			return
				LauncherType == target.LauncherType
				&& Administrator == target.Administrator
				//&& IconIndex == target.IconIndex
				&& StdOutputWatch == target.StdOutputWatch
				&& Command == target.Command
				&& WorkDirPath == target.WorkDirPath
				&& Option == target.Option
				//&& IconPath == target.IconPath
				&& IconItem.Equals(target.IconItem)
				&& EnvironmentSetting.EditEnvironment == target.EnvironmentSetting.EditEnvironment
				&& Note == target.Note
				&& Tag.SequenceEqual(target.Tag)
				&& EnvironmentSetting.Remove.SequenceEqual(target.EnvironmentSetting.Remove)
				&& EnvironmentSetting.Update.SequenceEqual(target.EnvironmentSetting.Update)
				;
		}

		/// <summary>
		/// リスト構造の整理。
		/// </summary>
		/// <param name="list"></param>
		/// <param name="value"></param>
		void IncrementList(List<string> list, string value)
		{
			if(!string.IsNullOrEmpty(value)) {
				var index = list.FindIndex(s => s == value);
				if(index != -1) {
					list.RemoveAt(index);
				}
				list.Insert(0, value);
			}
		}
		/// <summary>
		/// 使用回数をインクリメント。
		/// </summary>
		/// <param name="option">オプション履歴に追加する文字列</param>
		/// <param name="workDirPath">作業ディレクトリに追加する文字列</param>
		public void Increment(string option, string workDirPath)
		{
			LauncherHistory.ExecuteCount += 1;
			LauncherHistory.DateHistory.Update = DateTime.UtcNow;
			IncrementList(LauncherHistory.Options, option);
			IncrementList(LauncherHistory.WorkDirs, workDirPath);
		}
	}

}