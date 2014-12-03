﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 標準出力取得。
	/// </summary>
	public partial class StreamForm : Form, ISetCommonData
	{
		public StreamForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			OutputStreamReceived(e.Data, true);
		}

		void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			OutputStreamReceived(e.Data, false);
		}
		
		
		void ToolStream_refresh_Click(object sender, EventArgs e)
		{
			RefreshProperty();
		}
		
		void Process_Exited(object sender, EventArgs e)
		{
			if(InvokeRequired) {
				Invoke((MethodInvoker)ExitedProcess);
			} else {
				ExitedProcess();
			}
		}
		
		void ToolStream_kill_Click(object sender, EventArgs e)
		{
			KillProcess();
		}
		
		void ViewOutput_TextChanged(object sender, EventArgs e)
		{
			var hasText = this.inputOutput.TextLength > 0;
			this.toolStream_itemSave.Enabled = hasText;
			this.toolStream_itemClear.Enabled = hasText;
		}
		
		void ToolStream_clear_Click(object sender, EventArgs e)
		{
			// #22
			this.inputOutput.Clear();
		}
		
		void ToolStream_save_Click(object sender, EventArgs e)
		{
			using(var dialog = new SaveFileDialog()) {
				dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				dialog.FileName = Literal.NowTimestampFileName + ".output.log";
				dialog.Filter = "*.output.log|*.output.log";
				if(dialog.ShowDialog() == DialogResult.OK) {
					var path = dialog.FileName;
					SaveStream(path);
				}
			}
		}
		
		void ToolStream_itemTopmost_Click(object sender, EventArgs e)
		{
			SwitchTopmost();
		}

		void ViewOutput_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Debug.WriteLine((int)e.KeyChar);
			if(e.KeyChar == 0x0a || e.KeyChar == 0x0d) {
				Process.StandardInput.WriteLine();
			} else {
				Process.StandardInput.Write(e.KeyChar);
			}
		}
		
		void StreamForm_Shown(object sender, EventArgs e)
		{
			//this.toolStream_itemTopmost.Checked = TopMost;
			this.inputOutput.Focus();
		}
		
		void StreamForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(!Process.HasExited) {
				e.Cancel = true;
				CommonData.Logger.Puts(LogType.Warning, CommonData.Language["stream/running-close"], Process.ToString());
			}
		}
	}
}
