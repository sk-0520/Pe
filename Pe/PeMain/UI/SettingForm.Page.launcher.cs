﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/21
 * 時刻: 0:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PeUtility;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_launcher.
	/// </summary>
	public partial class SettingForm
	{
		void LauncherInputClear()
		{
			this._launcherSelectedItem = null;
			
			var textList = new Control[] {
				this.inputLauncherName,
				this.inputLauncherCommand,
				this.inputLauncherOption,
				this.inputLauncherWorkDirPath,
				this.inputLauncherIconPath,
				this.inputLauncherTag,
				this.inputLauncherNote,
			};
			textList.Map(item => item.Text = string.Empty);
			this.inputLauncherIconIndex.Value = 0;
			this.selectLauncherType_file.Checked = true;
			this.selectLauncherType_uri.Checked = false;
		}
		void LauncherSelectItem(LauncherItem item)
		{
			LauncherInputClear();
			this._launcherSelectedItem = item;
			
			this.inputLauncherName.Text = item.Name;
			this.inputLauncherCommand.Text = item.Command;
			this.inputLauncherOption.Text = item.Option;
			this.inputLauncherWorkDirPath.Text = item.WorkDirPath;
			this.inputLauncherIconPath.Text = item.IconPath;
			this.inputLauncherIconIndex.Value = item.IconIndex;
			this.inputLauncherTag.Text = string.Join(", ", item.Tag.ToArray());
			this.inputLauncherNote.Text = item.Note;
		}
		void LauncherInputValueToItem(LauncherItem item)
		{
			Debug.Assert(item != null);
			var oldIcon = new {
				Path = item.IconPath,
				Index= item.IconIndex
			};
			item.Name = this.inputLauncherName.Text.Trim();
			item.Command = this.inputLauncherCommand.Text.Trim();
			item.Option = this.inputLauncherOption.Text.Trim();
			item.WorkDirPath = this.inputLauncherWorkDirPath.Text.Trim();
			item.IconPath = this.inputLauncherIconPath.Text.Trim();
			item.IconIndex = (int)this.inputLauncherIconIndex.Value;
			item.Tag = this.inputLauncherTag.Text.Split(',').Map(s => s.Trim()).ToList();
			item.Note = this.inputLauncherNote.Text.Trim();
			
			item.HasError = this.selecterLauncher.Items.Where(i => i != item).Any(i => i.Equals(item));
			if(oldIcon.Index != item.IconIndex || oldIcon.Path != item.IconPath) {
				item.ClearIcon();
			}
		}
		
		bool LauncherItemValid()
		{
			return this.selecterLauncher.Items.Any(item => item.HasError);
		}
		
		void LauncherOpenFilePath(TextBox input)
		{
			var path = input.Text.Trim();
			using(var dialog = new OpenFileDialog()) {
				if(path.Length > 0 && File.Exists(path)) {
					dialog.InitialDirectory = Path.GetDirectoryName(path);
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					input.Text = dialog.FileName;
				}
			}
		}
		
		void LauncherOpenDirPath(TextBox input)
		{
			var path = input.Text.Trim();
			using(var dialog = new FolderBrowserDialog()) {
				dialog.ShowNewFolderButton = true;
				
				if(path.Length > 0 && Directory.Exists(path)) {
					dialog.SelectedPath = path;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					input.Text = dialog.SelectedPath;
				}
			}
			
		}
		
		void LauncherOpenIcon()
		{
			var iconPath = this.inputLauncherIconPath.Text.Trim();
			var iconIndex= (int)this.inputLauncherIconIndex.Value;
			using(var dialog = new OpenIconDialog()) {
				if(iconPath.Length > 0 && File.Exists(iconPath)) {
					dialog.IconPath  = iconPath;
					dialog.IconIndex = iconIndex;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					this.inputLauncherIconPath.Text = dialog.IconPath;
					this.inputLauncherIconIndex.Value = dialog.IconIndex;
				}
			}
		}
		
		
	}
}
