﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public partial class ClipboardForm: Form, ISetCommonData
	{
		#region Define

		const string imageText = "image_text";
		const string imageRtf = "image_rtf";
		const string imageImage = "image_image";
		const string imageFile = "image_file";

		#endregion

		#region Variable
		#endregion

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		CommonData CommonData { get; set; }

		#endregion

		#region Initialize
		
		void InitializeUI()
		{
			this.tabPreview_pageText.ImageKey = imageText;
			this.tabPreview_pageRichTextFormat.ImageKey = imageRtf;
			this.tabPreview_pageImage.ImageKey = imageImage;
			this.tabPreview_pageFile.ImageKey = imageFile;
		}

		void Initialize()
		{
			this.imageTab.Images.Add(imageText, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText);
			this.imageTab.Images.Add(imageRtf, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat);
			this.imageTab.Images.Add(imageImage, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage);
			this.imageTab.Images.Add(imageFile, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile);

			InitializeUI();
		}

		#endregion

		#region Language

		private void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);

			this.toolClipboard_itemTopmost.SetLanguage(CommonData.Language);
			this.toolClipboard_itemSave.SetLanguage(CommonData.Language);
			this.toolClipboard_itemClear.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemClipboard.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemTemplate.SetLanguage(CommonData.Language);

			this.tabPreview_pageText.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.tabPreview_pageRichTextFormat.Text = ClipboardType.RichTextFormat.ToText(CommonData.Language);
			this.tabPreview_pageImage.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.tabPreview_pageFile.Text = ClipboardType.File.ToText(CommonData.Language);
		}
		
		#endregion

		#region Function

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		void ApplySetting()
		{
			ApplyLanguage();

			ChangeSelectType(this.toolClipboard_itemType_itemClipboard);
		}

		void ChangeSelectType(ToolStripItem item)
		{
			this.toolClipboard_itemType.Text = item.Text;
			this.toolClipboard_itemType.Image = item.Image;
		}

		#endregion

		private void toolClipboard_itemType_itemClipboard_Click(object sender, EventArgs e)
		{
			ChangeSelectType((ToolStripItem)sender);
		}
	}
}
