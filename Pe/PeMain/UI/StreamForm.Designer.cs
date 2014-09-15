﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class StreamForm
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
			this.toolStream = new System.Windows.Forms.ToolStrip();
			this.toolStream_save = new System.Windows.Forms.ToolStripButton();
			this.toolStream_clear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStream_refresh = new System.Windows.Forms.ToolStripButton();
			this.toolStream_kill = new System.Windows.Forms.ToolStripButton();
			this.tabStream = new System.Windows.Forms.TabControl();
			this.tabStream_pageStream = new System.Windows.Forms.TabPage();
			this.viewOutput = new System.Windows.Forms.TextBox();
			this.tabStream_pageProcess = new System.Windows.Forms.TabPage();
			this.propertyProcess = new System.Windows.Forms.PropertyGrid();
			this.tabStream_pageProperty = new System.Windows.Forms.TabPage();
			this.propertyProperty = new System.Windows.Forms.PropertyGrid();
			this.toolStream.SuspendLayout();
			this.tabStream.SuspendLayout();
			this.tabStream_pageStream.SuspendLayout();
			this.tabStream_pageProcess.SuspendLayout();
			this.tabStream_pageProperty.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStream
			// 
			this.toolStream.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStream.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStream_save,
									this.toolStream_clear,
									this.toolStripSeparator1,
									this.toolStream_refresh,
									this.toolStream_kill});
			this.toolStream.Location = new System.Drawing.Point(0, 0);
			this.toolStream.Name = "toolStream";
			this.toolStream.Size = new System.Drawing.Size(471, 25);
			this.toolStream.TabIndex = 1;
			this.toolStream.Text = "toolStrip1";
			// 
			// toolStream_save
			// 
			this.toolStream_save.Enabled = false;
			this.toolStream_save.Image = global::PeMain.Properties.Images.Save;
			this.toolStream_save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_save.Name = "toolStream_save";
			this.toolStream_save.Size = new System.Drawing.Size(172, 22);
			this.toolStream_save.Text = ":stream/command/save";
			this.toolStream_save.ToolTipText = ":stream/tips/save";
			// 
			// toolStream_clear
			// 
			this.toolStream_clear.Enabled = false;
			this.toolStream_clear.Image = global::PeMain.Properties.Images.Clear;
			this.toolStream_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_clear.Name = "toolStream_clear";
			this.toolStream_clear.Size = new System.Drawing.Size(173, 22);
			this.toolStream_clear.Text = ":stream/command/clear";
			this.toolStream_clear.ToolTipText = ":stream/tips/clear";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStream_refresh
			// 
			this.toolStream_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_refresh.Image = global::PeMain.Properties.Images.Refresh;
			this.toolStream_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_refresh.Name = "toolStream_refresh";
			this.toolStream_refresh.Size = new System.Drawing.Size(23, 22);
			this.toolStream_refresh.ToolTipText = ":stream/tips/refesh";
			this.toolStream_refresh.Click += new System.EventHandler(this.ToolStream_refresh_Click);
			// 
			// toolStream_kill
			// 
			this.toolStream_kill.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStream_kill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_kill.Image = global::PeMain.Properties.Images.Kill;
			this.toolStream_kill.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_kill.Name = "toolStream_kill";
			this.toolStream_kill.Size = new System.Drawing.Size(23, 22);
			this.toolStream_kill.ToolTipText = ":stream/tips/kill";
			this.toolStream_kill.Click += new System.EventHandler(this.ToolStream_kill_Click);
			// 
			// tabStream
			// 
			this.tabStream.Controls.Add(this.tabStream_pageStream);
			this.tabStream.Controls.Add(this.tabStream_pageProcess);
			this.tabStream.Controls.Add(this.tabStream_pageProperty);
			this.tabStream.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabStream.Location = new System.Drawing.Point(0, 25);
			this.tabStream.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabStream.Name = "tabStream";
			this.tabStream.SelectedIndex = 0;
			this.tabStream.Size = new System.Drawing.Size(471, 255);
			this.tabStream.TabIndex = 2;
			// 
			// tabStream_pageStream
			// 
			this.tabStream_pageStream.Controls.Add(this.viewOutput);
			this.tabStream_pageStream.Location = new System.Drawing.Point(4, 24);
			this.tabStream_pageStream.Name = "tabStream_pageStream";
			this.tabStream_pageStream.Size = new System.Drawing.Size(463, 227);
			this.tabStream_pageStream.TabIndex = 0;
			this.tabStream_pageStream.Text = ":stream/tab/stream";
			this.tabStream_pageStream.UseVisualStyleBackColor = true;
			// 
			// viewOutput
			// 
			this.viewOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.viewOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewOutput.Location = new System.Drawing.Point(0, 0);
			this.viewOutput.Multiline = true;
			this.viewOutput.Name = "viewOutput";
			this.viewOutput.ReadOnly = true;
			this.viewOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.viewOutput.Size = new System.Drawing.Size(463, 227);
			this.viewOutput.TabIndex = 0;
			this.viewOutput.WordWrap = false;
			this.viewOutput.TextChanged += new System.EventHandler(this.ViewOutput_TextChanged);
			// 
			// tabStream_pageProcess
			// 
			this.tabStream_pageProcess.Controls.Add(this.propertyProcess);
			this.tabStream_pageProcess.Location = new System.Drawing.Point(4, 24);
			this.tabStream_pageProcess.Name = "tabStream_pageProcess";
			this.tabStream_pageProcess.Size = new System.Drawing.Size(463, 227);
			this.tabStream_pageProcess.TabIndex = 1;
			this.tabStream_pageProcess.Text = ":stream/tab/process";
			this.tabStream_pageProcess.UseVisualStyleBackColor = true;
			// 
			// propertyProcess
			// 
			this.propertyProcess.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProcess.Location = new System.Drawing.Point(0, 0);
			this.propertyProcess.Name = "propertyProcess";
			this.propertyProcess.Size = new System.Drawing.Size(463, 227);
			this.propertyProcess.TabIndex = 0;
			// 
			// tabStream_pageProperty
			// 
			this.tabStream_pageProperty.Controls.Add(this.propertyProperty);
			this.tabStream_pageProperty.Location = new System.Drawing.Point(4, 24);
			this.tabStream_pageProperty.Name = "tabStream_pageProperty";
			this.tabStream_pageProperty.Size = new System.Drawing.Size(463, 227);
			this.tabStream_pageProperty.TabIndex = 2;
			this.tabStream_pageProperty.Text = ":stream/tab/property";
			this.tabStream_pageProperty.UseVisualStyleBackColor = true;
			// 
			// propertyProperty
			// 
			this.propertyProperty.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProperty.Location = new System.Drawing.Point(0, 0);
			this.propertyProperty.Name = "propertyProperty";
			this.propertyProperty.Size = new System.Drawing.Size(463, 227);
			this.propertyProperty.TabIndex = 1;
			// 
			// StreamForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(471, 280);
			this.Controls.Add(this.tabStream);
			this.Controls.Add(this.toolStream);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "StreamForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/stream";
			this.toolStream.ResumeLayout(false);
			this.toolStream.PerformLayout();
			this.tabStream.ResumeLayout(false);
			this.tabStream_pageStream.ResumeLayout(false);
			this.tabStream_pageStream.PerformLayout();
			this.tabStream_pageProcess.ResumeLayout(false);
			this.tabStream_pageProperty.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripButton toolStream_kill;
		private System.Windows.Forms.ToolStripButton toolStream_refresh;
		private System.Windows.Forms.PropertyGrid propertyProperty;
		private System.Windows.Forms.TabPage tabStream_pageProperty;
		private System.Windows.Forms.PropertyGrid propertyProcess;
		private System.Windows.Forms.TextBox viewOutput;
		private System.Windows.Forms.ToolStripButton toolStream_clear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.TabPage tabStream_pageProcess;
		private System.Windows.Forms.TabPage tabStream_pageStream;
		private System.Windows.Forms.TabControl tabStream;
		private System.Windows.Forms.ToolStripButton toolStream_save;
		private System.Windows.Forms.ToolStrip toolStream;
	}
}
