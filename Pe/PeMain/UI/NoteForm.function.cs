﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Application.Data;
using ContentTypeTextNet.Pe.Application.IF;
using ContentTypeTextNet.Pe.Application.Logic;
using ContentTypeTextNet.Pe.Application.Logic.DB;
using ContentTypeTextNet.Pe.Library.Skin;

namespace ContentTypeTextNet.Pe.Application.UI
{
	partial class NoteForm
	{
		public void SetCommonData(CommonData commonData)
		{
			this._initialized = false;
			
			CommonData = commonData;
			
			ApplySetting();
			
			this._changed = false;
			this._initialized = true;
		}
		
		void ApplySetting()
		{
			/*
			this.inputTitle.Text = NoteItem.Title;
			this.inputBody.Text = NoteItem.Body;
			 */
			this.inputTitle.DataBindings.Add("Text", this._bindItem, "Title", false, DataSourceUpdateMode.OnPropertyChanged);
			this.inputBody.DataBindings.Add("Text", this._bindItem, "Body", false, DataSourceUpdateMode.OnPropertyChanged);
			
			Location = NoteItem.Location;
			Size = NoteItem.Size;
			
			TopMost = NoteItem.Topmost;
			
			// 最小サイズ
			var parentArea = CommonData.Skin.GetNoteCaptionArea(Size);
			var edge = CommonData.Skin.GetNoteWindowEdgePadding();
			var commandSize = CommonData.Skin.GetNoteCommandArea(parentArea, GetCommandList().First());
			var minSize = new Size(edge.Horizontal + commandSize.Width, edge.Vertical + commandSize.Height);
			MinimumSize = minSize;
			
			//NoteItem.Style.ForeColor;
//			if(!this.contextMenu_fore.ComboBox.Items.Cast<ColorDisplayValue>().Any(cd => cd.Value == NoteItem.Style.ForeColor)) {
//				this.contextMenu_fore.SelectedItem = this.contextMenu_fore.ComboBox.Items.Cast<ColorDisplayValue>().Single(cd => cd.Value == Color.Transparent).Value;
//			}
			//NoteItem.Style.BackColor;
			
			ApplyLanguage();
		}
		
		IEnumerable<SkinNoteCommand> GetCommandList()
		{
			return new [] {
				SkinNoteCommand.Topmost,
				SkinNoteCommand.Compact,
				SkinNoteCommand.Close,
			};
		}
		
		SkinNoteStatus GetNoteStatus()
		{
			var status = new SkinNoteStatus();
			
			status.Compact = NoteItem.Compact;
			status.Topmost = NoteItem.Topmost;
			status.Locked  = NoteItem.Locked;
			
			return status;
		}
		
		public void ToClose(bool removeData)
		{
			ExecCommand(SkinNoteCommand.Close, removeData);
		}
		
		public void ToCompact()
		{
			ExecCommand(SkinNoteCommand.Compact, false);
		}
		
		public void ToTopmost()
		{
			ExecCommand(SkinNoteCommand.Topmost, false);
		}
		
		public void ToLock()
		{
			ExecCommand(SkinNoteCommand.Lock, false);
		}
		
		void ExecCommand(SkinNoteCommand noteCommand, bool removeData)
		{
			switch(noteCommand) {
				case SkinNoteCommand.Topmost:
					{
						NoteItem.Topmost = !NoteItem.Topmost;
						TopMost = NoteItem.Topmost;
						Changed = true;
						Invalidate();
					}
					break;
					
				case SkinNoteCommand.Compact:
					{
						NoteItem.Compact = !NoteItem.Compact;
						Changed = true;
						
						ChangeCompact(NoteItem.Compact, NoteItem.Size);
						
					}
					break;
					
				case SkinNoteCommand.Close:
					{
						if(removeData) {
							// TODO: 論理削除
							Removed = true;
							RemoveItem();
						} else {
							NoteItem.Visible = false;
							Changed = true;
							SaveItem();
						}
						Close();
					}
					break;
					
				case SkinNoteCommand.Lock:
					{
						HiddenInputTitleArea();
						HiddenInputBodyArea();
						NoteItem.Locked = !NoteItem.Locked;
						Changed = true;
						Refresh();
					}
					break;
					
				default:
					Debug.Assert(false, noteCommand.ToString());
					break;
			}
		}
		
		void ChangeCompact(bool compact, Size size)
		{
			if(compact) {
				var edge = this.CommonData.Skin.GetNoteWindowEdgePadding();
				var titleArea = GetTitleArea();
				Size = new Size(titleArea.Width + edge.Horizontal, titleArea.Height + edge.Vertical);
			} else {
				Size = size;
			}
		}
		
		Rectangle GetTitleArea()
		{
			return this.CommonData.Skin.GetNoteCaptionArea(Size);
		}
		
		Rectangle GetBodyArea()
		{
			return GetBodyArea(
				this.CommonData.Skin.GetNoteWindowEdgePadding(),
				this.CommonData.Skin.GetNoteCaptionArea(Size)
			);
		}
		Rectangle GetBodyArea(Padding edge, Rectangle captionArea)
		{
			return new Rectangle(
				new Point(edge.Left, captionArea.Bottom),
				new Size(Size.Width - edge.Horizontal, Size.Height - (edge.Vertical + captionArea.Height))
			);
		}

		void ResizeInputTitleArea()
		{
			var titleArea = GetTitleArea();
			this.inputTitle.Location = titleArea.Location;
			this.inputTitle.Size = titleArea.Size;
		}
		
		void ResizeInputBodyArea()
		{
			var bodyArea = GetBodyArea();
			this.inputBody.Location = bodyArea.Location;
			this.inputBody.Size = bodyArea.Size;
		}
		
		void ShowInputTitleArea(int recursive)
		{
			this._prevTitle = NoteItem.Title;
			//this.inputTitle.Text = NoteItem.Title;
			this.inputTitle.Font = CommonData.MainSetting.Note.CaptionFontSetting.Font;
			
			if(!this.inputTitle.Visible) {
				ResizeInputTitleArea();
				this.inputTitle.Visible = true;
				this.inputTitle.Focus();
			}
			if(!this.inputTitle.Visible && recursive > 0) {
				ShowInputTitleArea(recursive - 1);
			}
		}
		
		void ShowInputBodyArea(int recursive)
		{
			this._prevBody = NoteItem.Body;
			//this.inputBody.Text = NoteItem.Body;
			this.inputBody.Font = NoteItem.Style.FontSetting.Font;
			
			if(!this.inputBody.Visible) {
				ResizeInputBodyArea();
				this.inputBody.Visible = true;
				this.inputBody.Focus();
			}
			if(!this.inputBody.Visible && recursive > 0) {
				ShowInputBodyArea(recursive - 1);
			}
		}
		
		void HiddenInputTitleArea()
		{
			if(!this.inputTitle.Visible) {
				return;
			}
			/*
			var value = this.inputTitle.Text.Trim();
			if(value.Length == 0 && NoteItem.Body.Length > 0) {
				value = TextUtility.SplitLines(NoteItem.Body).First().Trim();
			}
			var change = NoteItem.Title != value;
			if(change) {
				NoteItem.Title = value;
				this._changed |= true;
			}
			 */
			this._changed = true;
			this.inputTitle.Visible = false;
		}
		
		void HiddenInputBodyArea()
		{
			if(!this.inputBody.Visible) {
				return;
			}
			/*
			var value = this.inputBody.Text.Trim();
			var change = NoteItem.Body != value;
			if(change) {
				NoteItem.Body = value;
				this._changed |= true;
			}
			if(value.Length > 0 && NoteItem.Title.Trim().Length == 0) {
				NoteItem.Title = TextUtility.SplitLines(value).First().Trim();
			}
			 */
			
			this._changed = true;
			this.inputBody.Visible = false;
		}
		
		void ShowContextMenu(Point point)
		{
			this.contextMenu.Show(this, point);
		}
		
		public void SaveItem()
		{
			if(this._changed) {
				lock(CommonData.Database) {
					var noteDB = new NoteDB(CommonData.Database);
					using(var tran = noteDB.BeginTransaction()) {
						try {
							noteDB.Resist(new [] { NoteItem });
							tran.Commit();
						} catch(Exception ex) {
							tran.Rollback();
							CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
						}
					}
				}
				
				this._changed = false;
				#if DEBUG
				var map = new Dictionary<string,string>() {
					{ AppLanguageName.noteTitle, NoteItem.Title },
				};
				CommonData.Logger.Puts(LogType.Information, CommonData.Language["note/save", map], NoteItem);
				#endif
			}
		}
		
		void RemoveItem()
		{
			lock(CommonData.Database) {
				var noteDB = new NoteDB(CommonData.Database);
				using(var tran = noteDB.BeginTransaction()) {
					try {
						noteDB.ToDisabled(new [] { NoteItem });
						tran.Commit();
					} catch(Exception ex) {
						tran.Rollback();
						CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
		}
		
		/*
		Color GetSelectedColor(ToolStripComboBox control)
		{
			var index = control.ComboBox.SelectedIndex;
			Debug.Assert(index >= 0, control.ComboBox.SelectedIndex.ToString());
			var item = control.ComboBox.Items[index] as ColorDisplayValue;
			return item.Value;
		}
		
		bool IsCustomColor(Color color)
		{
			return color == Color.Transparent;
		}
		
		Color SetAcceptColor(Color color, Color revertColor)
		{
			var resultColor = color;
			var accept = false;
			if(IsCustomColor(color)) {
				using(var dialog = new ColorDialog()) {
					if(dialog.ShowDialog() == DialogResult.OK) {
						resultColor = dialog.Color;
						accept = true;
					} else {
						resultColor = revertColor;
					}
				}
			} else {
				accept = true;
			}
			if(accept) {
				Changed = true;
			}
			return resultColor;
		}
		 */
		
		IList<ColorMenuItem> GetColorMenuList(ToolStripMenuItem parentItem, IList<Color> colorList)
		{
			return colorList
				.Zip(
					parentItem.DropDownItems
					.Cast<ToolStripItem>()
					.Where(i => i is ToolStripMenuItem)
					.Take(colorList.Count),
					(color, item) => new ColorMenuItem(item, color)
				)
				.ToList()
				;
		}
		
		Color SelectedPlainColor(ToolStripItem selectItem,  IList<ColorMenuItem> colorItemList)
		{
			return colorItemList.Single(c => c.Item == selectItem).Color;
		}
		
		Color SelectedCustomColor(Color nowColor)
		{
			var resultColor = nowColor;
			using(var dialog = new ColorDialog()) {
				dialog.CustomColors[0] = nowColor.ToArgb();
				if(dialog.ShowDialog() == DialogResult.OK) {
					resultColor = dialog.Color;
				}
			}
			
			return resultColor;
		}
	}
}