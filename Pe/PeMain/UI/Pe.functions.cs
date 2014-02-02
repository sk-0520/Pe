﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_functions.
	/// </summary>
	public partial class Pe
	{
		IEnumerable<Form> GetWindows()
		{
			var result = new List<Form>();
			result.AddRange(this._toolbarForms.Values);
			result.Add(this._logForm);
			
			foreach(var f in this._toolbarForms.Values.Where(f => f.OwnedForms.Length > 0)) {
				result.AddRange(f.OwnedForms);
			}
			
				
			return result;
		}
		/// <summary>
		/// TODO: 未実装
		/// </summary>
		/// <param name="action"></param>
		void PauseOthers(Action action)
		{
			var windowVisible = new Dictionary<Form, bool>();
			//var appbarDock = new Dictionary<AppbarForm, DesktopDockType>();
			foreach(var window in GetWindows()) {
				windowVisible[window] = window.Visible;
				window.Visible = false;
				/*
				var appbar = window as AppbarForm;
				if(appbar != null && appbar.IsDocking) {
					appbarDock[appbar] = appbar.DesktopDockType;
				}
				*/
			}
			
			action();
			/*
			foreach(var pair in appbarDock) {
				pair.Key.DesktopDockType = pair.Value;
			}
			*/
			foreach(var pair in windowVisible) {
				pair.Key.Visible = pair.Value;
			}
		}
		
		void SaveSetting()
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			SaveMainSetting(this._mainSetting, mainSettingFilePath);
		}
		
		void SaveMainSetting(MainSetting mainSetting, string mainSettingPath)
		{
			Debug.Assert(mainSetting != null);
			FileUtility.MakeFileParentDirectory(mainSettingPath);

			var serializer = new XmlSerializer(typeof(MainSetting));
			using(var stream = new FileStream(mainSettingPath, FileMode.Create)) {
				var sortedSet = new HashSet<LauncherItem>();
				foreach(var item in mainSetting.Launcher.Items.OrderBy(item => item.Name)) {
					sortedSet.Add(item);
				}
				var nowItems = mainSetting.Launcher.Items;
				mainSetting.Launcher.Items = sortedSet;
				serializer.Serialize(stream, mainSetting);
				mainSetting.Launcher.Items = nowItems; 
			}
		}
		
		void CloseApplication(bool save)
		{
			SaveSetting();
			Application.Exit();
		}
		
		void OpenSetting()
		{
			using(var settingForm = new SettingForm(this._language, this._mainSetting)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					var mainSetting = settingForm.MainSetting;
					this._mainSetting = mainSetting;
					InitializeLanguage(null, null);
					ApplyLanguage();
					this._logForm.SetSettingData(this._language, this._mainSetting);
					foreach(var toolbar in this._toolbarForms.Values) {
						toolbar.SetSettingData(this._language, this._mainSetting);
					}
				}
			}
		}
	}
}
