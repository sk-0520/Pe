﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	/// <summary>
	/// <para>モデルはなし。</para>
	/// </summary>
	public class CommandItemViewModel: ViewModelBase, IHavingAppNonProcess, IHavingAppSender
	{
		#region variable

		BitmapSource _image, _smallImage;

		#endregion

		CommandItemViewModel(CommandKind commandKind, IconScale iconScale, IAppNonProcess appNonProcess, IAppSender appSender)
		{
			CommandKind = commandKind;
			IconScale = iconScale;
			AppNonProcess = appNonProcess;
			AppSender = appSender;
		}

		public CommandItemViewModel(IconScale iconScale, LauncherItemModel launcherItem, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.LauncherItemName, iconScale, appNonProcess, appSender)
		{
			LauncherItemModel = launcherItem;
		}

		public CommandItemViewModel(IconScale iconScale, LauncherItemModel launcherItem, string tag, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.LauncherItemTag, iconScale, appNonProcess, appSender)
		{
			LauncherItemModel = launcherItem;
			Tag = tag;
		}

		public CommandItemViewModel(IconScale iconScale, string filePath, bool isDirectory, bool isHideFile, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.File, iconScale, appNonProcess, appSender)
		{
			IsDirectory = isDirectory;
			if (IsDirectory) {
				if (filePath.Last() == Path.DirectorySeparatorChar) {
					FilePath = filePath;
				} else {
					FilePath = filePath + Path.DirectorySeparatorChar;
				}
			} else {
				FilePath = filePath;
			}
			IsHideFile = isHideFile;
		}

		public CommandItemViewModel(IconScale iconScale, string filePath, string driveName, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.Drive, iconScale, appNonProcess, appSender)
		{
			FilePath = filePath;
			IsDirectory = true;
			DriveName = driveName;
		}

		#region property

		public CommandKind CommandKind { get; private set; }
		public IconScale IconScale { get; private set; }
		public LauncherItemModel LauncherItemModel { get; private set; }
		public string Tag { get; private set; }
		public string FilePath { get; private set; }
		public string DriveName { get; private set; }
		public bool IsDirectory { get; private set; }
		public bool IsHideFile { get; private set; }

		public BitmapSource Image
		{
			get
			{
				if(this._image == null) {
					this._image = GetImage(IconScale);
				}

				return this._image;
			}
		}

		public BitmapSource SmallImage
		{
			get
			{
				if (this._smallImage == null) {
					this._smallImage = GetImage(IconScale.Small);
				}

				return this._smallImage;
			}
		}

		#endregion

		#region function

		BitmapSource GetImage(IconScale iconScale)
		{
			if (CommandKind == CommandKind.File || CommandKind == CommandKind.Drive) {
				var iconPath = new IconPathModel() {
					Path = FilePath,
					Index = 0,
				};
				return AppUtility.LoadIconDefault(iconPath, iconScale, AppNonProcess.Logger);
			} else {
				Debug.Assert(CommandKind == CommandKind.LauncherItemName || CommandKind == CommandKind.LauncherItemTag);
				var viewModel = new LauncherItemSimpleViewModel(LauncherItemModel, AppNonProcess, AppSender);
				return viewModel.GetIcon(iconScale);
			}
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion

		#region ViewModelBase

		public override string DisplayText
		{
			get
			{
				switch(CommandKind) {
					case CommandKind.LauncherItemName:
						return DisplayTextUtility.GetDisplayName(LauncherItemModel);

					case CommandKind.LauncherItemTag: 
						{
							var map = new Dictionary<string, string>() {
								{ LanguageKey.commandItemName, DisplayTextUtility.GetDisplayName(LauncherItemModel) },
								{ LanguageKey.commandItemTag, Tag },
							};
							//var result = string.Format("{0} ({1})", DriveName, FilePath);
							var result = AppNonProcess.Language["command/tag", map];
							return result;
						}

					case CommandKind.File:
						if (IsDirectory) {
							return FilePath.Substring(0, FilePath.Length - 1);
						} else {
							return FilePath;
						}

					case CommandKind.Drive:
						if(string.IsNullOrWhiteSpace(DriveName)) {
							return FilePath;
						} else {
							var map = new Dictionary<string, string>() {
								{ LanguageKey.commandDrivePath, FilePath },
								{ LanguageKey.commandDriveVolume, DriveName },
							};
							//var result = string.Format("{0} ({1})", DriveName, FilePath);
							var result = AppNonProcess.Language["command/drive", map];
							return result;
						}

					default:
						throw new NotImplementedException();
				}
			}
		}

		#endregion
	}
}
