﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	public partial class App: IDisposable, IRootSender
	{
		public App(CommandLine commandLine, FileLogger fileLogger)
		{
			Initialized = true;
			
			var logger = new StartupLogger(fileLogger);

			ExistsSettingFilePath = Initialize(commandLine, logger);
			
			#if !DISABLED_UPDATE_CHECK
			CheckUpdateProcessAsync(false);
			#endif
		}
		
		public void Dispose()
		{
			this._commonData.ToDispose();
			this._messageWindow.ToDispose();
			this._logForm.ToDispose();
			this._noteWindowList.ForEach(w => w.ToDispose());
			this._toolbarForms.Values.ToList().ForEach(w => w.ToDispose());
			this._notifyIcon.ToDispose();
			
			#if DEBUG
			if(File.Exists(Literal.StartupShortcutPath)) {
				File.Delete(Literal.StartupShortcutPath);
			}
			#endif
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			/*
			var update = new Update(@"Z:temp", false);
			var info = update.Check();
			if(info.IsUpdate) {
				var s = string.Format("{0} {1}", info.Version, info.IsRcVersion ? "RC": "RELEASE");
				if(MessageBox.Show(s, "UPDATE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes) {
					update.Execute();
				}
			}
			 */
			//MessageBox.Show("PON!");
			ShowHomeDialog();
			//ResetUI();
		}
		
		void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionSwitch", e);
			if(e.Reason == SessionSwitchReason.ConsoleConnect) {
				ResetUI();
			} else if(e.Reason == SessionSwitchReason.ConsoleDisconnect) {
				SaveSetting();
			}
		}

		void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if(e.Category.IsIn(UserPreferenceCategory.VisualStyle, UserPreferenceCategory.Color)) {
				this._logForm.Puts(LogType.Information, "UserPreferenceChanged", e);
				ResetUI();
			}
		}

		void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionEnding", e);
			SaveSetting();
		}
		
		void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			if(e.Mode == PowerModes.Resume) {
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/power/resume"], e);
				CheckUpdateProcessAsync(false);
			}
		}
		
		void NoteMenu_DropDownOpening(object sender, EventArgs e)
		{
			OpeningNoteMenu();
		}

	}
	
}

