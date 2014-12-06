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
using System.Linq;

using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.Library.Skin;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class NoteForm
	{
		void InitializeUI()
		{
			/*
			var colorControls = new [] {
				new { Control = contextMenu_itemForeColor, Title = "note/style/color-fore", Default = Literal.noteFore },
				new { Control = contextMenu_itemBackColor, Title = "note/style/color-back", Default = Literal.noteBack },
			};
			foreach(var control in colorControls) {
				var isFore = control.Control == contextMenu_itemForeColor;
				var colorList = new [] {
					new ColorDisplayValue(isFore ? Literal.noteForeColorBlack:  Literal.noteBackColorBlack , control.Title, "note/style/color/black"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorWhite:  Literal.noteBackColorWhite , control.Title, "note/style/color/white"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorRed:    Literal.noteBackColorRed   , control.Title, "note/style/color/red"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorGreen:  Literal.noteBackColorGreen , control.Title, "note/style/color/green"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorBlue:   Literal.noteBackColorBlue  , control.Title, "note/style/color/blue"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorYellow: Literal.noteBackColorYellow, control.Title, "note/style/color/yellow"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorOrange: Literal.noteBackColorOrange, control.Title, "note/style/color/orange"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorPurple: Literal.noteBackColorPurple, control.Title, "note/style/color/purple"),
					new ColorDisplayValue(Color.Transparent, control.Title, "note/style/color/custom"),
				};
				control.Control.ComboBox.BindingContext = BindingContext;
				control.Control.Attachment(colorList, control.Default);
			}
			*/
			
			var colorItems = new [] {
				new { Menu = contextMenu_itemForeColor, Default = Literal.noteFore, IsFore = true, },
				new { Menu = contextMenu_itemBackColor, Default = Literal.noteBack, IsFore = false, },
			};
			
			foreach(var colorItem in colorItems) {
				var colors = colorItem.IsFore ? Literal.GetNoteForeColorList(): Literal.GetNoteBackColorList();
				var pairs = GetColorMenuList(colorItem.Menu, colors);
				foreach(var pair in pairs) {
					pair.Item.Image = AppUtility.CreateNoteBoxImage(pair.Color, menuIconSize);
				}
			}
			
			ToolStripUtility.AttachmentOpeningMenuInScreen(this);
		}
		
		void Initialize()
		{
			this._commandStateMap = new Dictionary<SkinNoteCommand, SkinButtonState>() {
				{ SkinNoteCommand.Close, SkinButtonState.Normal },
				{ SkinNoteCommand.Compact, SkinButtonState.Normal },
				{ SkinNoteCommand.Topmost, SkinButtonState.Normal },
			};
			
			InitializeUI();
		}
	}
}
