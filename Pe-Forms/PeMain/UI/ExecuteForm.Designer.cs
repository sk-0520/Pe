﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/14
 * 時刻: 23:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class ExecuteForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.viewCommand = new System.Windows.Forms.TextBox();
			this.inputOption = new System.Windows.Forms.ComboBox();
			this.inputWorkDirPath = new System.Windows.Forms.ComboBox();
			this.tabExecute = new System.Windows.Forms.TabControl();
			this.tabExecute_pageBasic = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.selectAdministrator = new System.Windows.Forms.CheckBox();
			this.labelOption = new System.Windows.Forms.Label();
			this.selectStdStream = new System.Windows.Forms.CheckBox();
			this.labelWorkDirPath = new System.Windows.Forms.Label();
			this.commandOption_file = new System.Windows.Forms.Button();
			this.commandWorkDirPath = new System.Windows.Forms.Button();
			this.commandOption_dir = new System.Windows.Forms.Button();
			this.tabExecute_pageEnv = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.selectEnvironment = new System.Windows.Forms.CheckBox();
			this.groupUpdate = new System.Windows.Forms.GroupBox();
			this.envUpdate = new ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvUpdateControl();
			this.groupRemove = new System.Windows.Forms.GroupBox();
			this.envRemove = new ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvRemoveControl();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.tabExecute.SuspendLayout();
			this.tabExecute_pageBasic.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tabExecute_pageEnv.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupUpdate.SuspendLayout();
			this.groupRemove.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// viewCommand
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.viewCommand, 4);
			this.viewCommand.Dock = System.Windows.Forms.DockStyle.Top;
			this.viewCommand.Location = new System.Drawing.Point(3, 3);
			this.viewCommand.Name = "viewCommand";
			this.viewCommand.ReadOnly = true;
			this.viewCommand.Size = new System.Drawing.Size(648, 23);
			this.viewCommand.TabIndex = 0;
			this.viewCommand.TabStop = false;
			// 
			// inputOption
			// 
			this.inputOption.Dock = System.Windows.Forms.DockStyle.Top;
			this.inputOption.FormattingEnabled = true;
			this.inputOption.Location = new System.Drawing.Point(154, 32);
			this.inputOption.Name = "inputOption";
			this.inputOption.Size = new System.Drawing.Size(429, 23);
			this.inputOption.TabIndex = 0;
			// 
			// inputWorkDirPath
			// 
			this.inputWorkDirPath.AllowDrop = true;
			this.inputWorkDirPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.inputWorkDirPath.FormattingEnabled = true;
			this.inputWorkDirPath.Location = new System.Drawing.Point(154, 63);
			this.inputWorkDirPath.Name = "inputWorkDirPath";
			this.inputWorkDirPath.Size = new System.Drawing.Size(429, 23);
			this.inputWorkDirPath.TabIndex = 3;
			this.inputWorkDirPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputWorkDirPath_DragDrop);
			this.inputWorkDirPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputWorkDirPath_DragEnter);
			// 
			// tabExecute
			// 
			this.tabExecute.Alignment = System.Windows.Forms.TabAlignment.Left;
			this.tabExecute.Controls.Add(this.tabExecute_pageBasic);
			this.tabExecute.Controls.Add(this.tabExecute_pageEnv);
			this.tabExecute.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabExecute.Location = new System.Drawing.Point(0, 0);
			this.tabExecute.Multiline = true;
			this.tabExecute.Name = "tabExecute";
			this.tabExecute.SelectedIndex = 0;
			this.tabExecute.Size = new System.Drawing.Size(704, 183);
			this.tabExecute.TabIndex = 0;
			// 
			// tabExecute_pageBasic
			// 
			this.tabExecute_pageBasic.AllowDrop = true;
			this.tabExecute_pageBasic.Controls.Add(this.tableLayoutPanel2);
			this.tabExecute_pageBasic.Location = new System.Drawing.Point(46, 4);
			this.tabExecute_pageBasic.Name = "tabExecute_pageBasic";
			this.tabExecute_pageBasic.Size = new System.Drawing.Size(654, 175);
			this.tabExecute_pageBasic.TabIndex = 0;
			this.tabExecute_pageBasic.Text = ":execute/page/basic";
			this.tabExecute_pageBasic.UseVisualStyleBackColor = true;
			this.tabExecute_pageBasic.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabExecute_pageBasic_DragDrop);
			this.tabExecute_pageBasic.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabExecute_pageBasic_DragEnter);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
			this.tableLayoutPanel2.Controls.Add(this.viewCommand, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.selectAdministrator, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.labelOption, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.selectStdStream, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.labelWorkDirPath, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.inputOption, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.inputWorkDirPath, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.commandOption_file, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.commandWorkDirPath, 2, 2);
			this.tableLayoutPanel2.Controls.Add(this.commandOption_dir, 3, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 5;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(654, 175);
			this.tableLayoutPanel2.TabIndex = 10;
			// 
			// selectAdministrator
			// 
			this.selectAdministrator.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.selectAdministrator, 2);
			this.selectAdministrator.Location = new System.Drawing.Point(3, 119);
			this.selectAdministrator.Name = "selectAdministrator";
			this.selectAdministrator.Size = new System.Drawing.Size(163, 19);
			this.selectAdministrator.TabIndex = 6;
			this.selectAdministrator.Text = ":common/check/admin";
			this.selectAdministrator.UseVisualStyleBackColor = true;
			// 
			// labelOption
			// 
			this.labelOption.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelOption.AutoSize = true;
			this.labelOption.Location = new System.Drawing.Point(3, 37);
			this.labelOption.Name = "labelOption";
			this.labelOption.Size = new System.Drawing.Size(133, 15);
			this.labelOption.TabIndex = 3;
			this.labelOption.Text = ":execute/label/option";
			// 
			// selectStdStream
			// 
			this.selectStdStream.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.selectStdStream, 2);
			this.selectStdStream.Location = new System.Drawing.Point(3, 94);
			this.selectStdStream.Name = "selectStdStream";
			this.selectStdStream.Size = new System.Drawing.Size(187, 19);
			this.selectStdStream.TabIndex = 5;
			this.selectStdStream.Text = ":execute/check/std-stream";
			this.selectStdStream.UseVisualStyleBackColor = true;
			// 
			// labelWorkDirPath
			// 
			this.labelWorkDirPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelWorkDirPath.AutoSize = true;
			this.labelWorkDirPath.Location = new System.Drawing.Point(3, 68);
			this.labelWorkDirPath.Name = "labelWorkDirPath";
			this.labelWorkDirPath.Size = new System.Drawing.Size(145, 15);
			this.labelWorkDirPath.TabIndex = 4;
			this.labelWorkDirPath.Text = ":execute/label/work-dir";
			// 
			// commandOption_file
			// 
			this.commandOption_file.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandOption_file.Location = new System.Drawing.Point(589, 32);
			this.commandOption_file.Name = "commandOption_file";
			this.commandOption_file.Size = new System.Drawing.Size(28, 25);
			this.commandOption_file.TabIndex = 1;
			this.commandOption_file.UseVisualStyleBackColor = true;
			this.commandOption_file.Click += new System.EventHandler(this.CommandOption_file_Click);
			// 
			// commandWorkDirPath
			// 
			this.commandWorkDirPath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandWorkDirPath.Location = new System.Drawing.Point(589, 63);
			this.commandWorkDirPath.Name = "commandWorkDirPath";
			this.commandWorkDirPath.Size = new System.Drawing.Size(28, 25);
			this.commandWorkDirPath.TabIndex = 4;
			this.commandWorkDirPath.UseVisualStyleBackColor = true;
			this.commandWorkDirPath.Click += new System.EventHandler(this.CommandWorkDirPath_Click);
			// 
			// commandOption_dir
			// 
			this.commandOption_dir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandOption_dir.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandOption_dir.Location = new System.Drawing.Point(623, 32);
			this.commandOption_dir.Name = "commandOption_dir";
			this.commandOption_dir.Size = new System.Drawing.Size(28, 25);
			this.commandOption_dir.TabIndex = 2;
			this.commandOption_dir.UseVisualStyleBackColor = true;
			this.commandOption_dir.Click += new System.EventHandler(this.CommandOption_dir_Click);
			// 
			// tabExecute_pageEnv
			// 
			this.tabExecute_pageEnv.Controls.Add(this.tableLayoutPanel1);
			this.tabExecute_pageEnv.Location = new System.Drawing.Point(46, 4);
			this.tabExecute_pageEnv.Name = "tabExecute_pageEnv";
			this.tabExecute_pageEnv.Size = new System.Drawing.Size(654, 175);
			this.tabExecute_pageEnv.TabIndex = 1;
			this.tabExecute_pageEnv.Text = ":common/page/env";
			this.tabExecute_pageEnv.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.63142F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.36858F));
			this.tableLayoutPanel1.Controls.Add(this.selectEnvironment, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupUpdate, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupRemove, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(654, 175);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// selectEnvironment
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.selectEnvironment, 2);
			this.selectEnvironment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectEnvironment.Location = new System.Drawing.Point(3, 3);
			this.selectEnvironment.Name = "selectEnvironment";
			this.selectEnvironment.Size = new System.Drawing.Size(648, 24);
			this.selectEnvironment.TabIndex = 0;
			this.selectEnvironment.Text = ":execute/check/edit-env";
			this.selectEnvironment.UseVisualStyleBackColor = true;
			this.selectEnvironment.CheckedChanged += new System.EventHandler(this.SelectUserDefault_CheckedChanged);
			// 
			// groupUpdate
			// 
			this.groupUpdate.Controls.Add(this.envUpdate);
			this.groupUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupUpdate.Location = new System.Drawing.Point(3, 33);
			this.groupUpdate.Name = "groupUpdate";
			this.groupUpdate.Size = new System.Drawing.Size(397, 139);
			this.groupUpdate.TabIndex = 1;
			this.groupUpdate.TabStop = false;
			this.groupUpdate.Text = ":common/label/edit";
			// 
			// envUpdate
			// 
			this.envUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envUpdate.Location = new System.Drawing.Point(3, 19);
			this.envUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.envUpdate.Name = "envUpdate";
			this.envUpdate.Size = new System.Drawing.Size(391, 117);
			this.envUpdate.TabIndex = 0;
			// 
			// groupRemove
			// 
			this.groupRemove.Controls.Add(this.envRemove);
			this.groupRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupRemove.Location = new System.Drawing.Point(406, 33);
			this.groupRemove.Name = "groupRemove";
			this.groupRemove.Size = new System.Drawing.Size(245, 139);
			this.groupRemove.TabIndex = 2;
			this.groupRemove.TabStop = false;
			this.groupRemove.Text = ":common/label/remove";
			// 
			// envRemove
			// 
			this.envRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envRemove.Location = new System.Drawing.Point(3, 19);
			this.envRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.envRemove.Name = "envRemove";
			this.envRemove.Size = new System.Drawing.Size(239, 117);
			this.envRemove.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tabExecute);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(704, 224);
			this.splitContainer1.SplitterDistance = 183;
			this.splitContainer1.TabIndex = 4;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(704, 37);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// commandCancel
			// 
			this.commandCancel.AutoSize = true;
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(623, 3);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(78, 29);
			this.commandCancel.TabIndex = 1;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			this.commandCancel.Click += new System.EventHandler(this.commandCancel_Click);
			// 
			// commandSubmit
			// 
			this.commandSubmit.AutoSize = true;
			this.commandSubmit.Location = new System.Drawing.Point(542, 3);
			this.commandSubmit.Name = "commandSubmit";
			this.commandSubmit.Size = new System.Drawing.Size(75, 29);
			this.commandSubmit.TabIndex = 0;
			this.commandSubmit.Text = "{OK}";
			this.commandSubmit.UseVisualStyleBackColor = true;
			this.commandSubmit.Click += new System.EventHandler(this.CommandSubmit_Click);
			// 
			// ExecuteForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(704, 224);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExecuteForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/execute";
			this.tabExecute.ResumeLayout(false);
			this.tabExecute_pageBasic.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tabExecute_pageEnv.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupUpdate.ResumeLayout(false);
			this.groupRemove.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox selectAdministrator;
		private ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvRemoveControl envRemove;
		private ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvUpdateControl envUpdate;
		private System.Windows.Forms.GroupBox groupRemove;
		private System.Windows.Forms.GroupBox groupUpdate;
		private System.Windows.Forms.CheckBox selectEnvironment;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button commandSubmit;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabPage tabExecute_pageEnv;
		private System.Windows.Forms.Label labelOption;
		private System.Windows.Forms.Label labelWorkDirPath;
		private System.Windows.Forms.Button commandOption_file;
		private System.Windows.Forms.Button commandOption_dir;
		private System.Windows.Forms.Button commandWorkDirPath;
		private System.Windows.Forms.CheckBox selectStdStream;
		private System.Windows.Forms.TabPage tabExecute_pageBasic;
		private System.Windows.Forms.TabControl tabExecute;
		private System.Windows.Forms.ComboBox inputWorkDirPath;
		private System.Windows.Forms.ComboBox inputOption;
		private System.Windows.Forms.TextBox viewCommand;
	}
}
