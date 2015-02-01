﻿namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	public partial class ClipboardForm: CommonForm
	{
		#region Define

		const string imageText = "image_text";
		const string imageRtf = "image_rtf";
		const string imageHtml = "image_html";
		const string imageImage = "image_image";
		const string imageFile = "image_file";
		const string imageRawTemplate = "image_raw_template";
		const string imageReplaceTemplate = "image_replace_template";

		class ClipboardWebBrowser: ShowWebBrowser
		{
			public ClipboardWebBrowser()
				: base()
			{ }
		}

		#endregion ////////////////////////////////////////

		#region Variable

		FlowLayoutPanel _panelClipboradItem = new FlowLayoutPanel();
		Button _commandText = new Button();
		Button _commandRtf = new Button();
		Button _commandHtml = new Button();
		Button _commandImage = new Button();
		Button _commandFile = new Button();

		Button _commandMulti = new Button();

		Button _commandAdd = new Button();
		Button _commandUp = new Button();
		Button _commandDown = new Button();


		#endregion ////////////////////////////////////////

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		//CommonData CommonData { get; set; }
		int HoverItemIndex { get; set; }
		int SelectedItemIndex { get; set; }

		#endregion ////////////////////////////////////////

		#region Initialize

		void InitializeCommand()
		{
			var commandButtons = new[] {
				this._commandMulti,
				this._commandText,
				this._commandRtf,
				this._commandHtml,
				this._commandImage,
				this._commandFile,
				this._commandAdd,
				this._commandUp,
				this._commandDown,
			};
			var buttonSize = GetButtonSize();

			foreach(var command in commandButtons) {
				command.Size = buttonSize;
				//command.FlatStyle = FlatStyle.Flat;
				command.Margin = Padding.Empty;
				command.Click += command_Click;
			}
			this._commandMulti.Margin = new Padding(0, 0, NativeMethods.GetSystemMetrics(SM.SM_CXEDGE), 0);
			this._panelClipboradItem.Padding = Padding.Empty;
			this._panelClipboradItem.Margin = Padding.Empty;
			//this._panelClipboradItem.BackColor = Color.Transparent;
			//this._panelClipboradItem.BackColor = Color.FromArgb(50, Color.Black);
			this._panelClipboradItem.Size = Size.Empty;
			this._panelClipboradItem.AutoSize = true;
			//this._panelClipboradItem.Controls.AddRange(commandButtons);
			this.listClipboard.Controls.Add(this._panelClipboradItem);
			this._panelClipboradItem.Visible = false;
		}

		void InitializeUI()
		{
			InitializeCommand();

			this.tabPreview_pageText.ImageKey = imageText;
			this.tabPreview_pageRtf.ImageKey = imageRtf;
			this.tabPreview_pageHtml.ImageKey = imageHtml;
			this.tabPreview_pageImage.ImageKey = imageImage;
			this.tabPreview_pageFile.ImageKey = imageFile;
			this.tabPreview_pageRawTemplate.ImageKey = imageRawTemplate;
			this.tabPreview_pageReplaceTemplate.ImageKey = imageReplaceTemplate;
			

			//ChangeCommand(-1);
			//ChangeSelsectedItem(-1);
			WebBrowserUtility.AttachmentNewWindow(this.viewHtml);

			listClipboard.MouseWheel += listClipboard_MouseWheel;
		}

		void Initialize()
		{
			InitializeUI();
		}

		#endregion ////////////////////////////////////////

		#region Language

		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);

			this.toolClipboard_itemEnabled.SetLanguage(CommonData.Language);
			this.toolClipboard_itemTopmost.SetLanguage(CommonData.Language);
			this.toolClipboard_itemSave.SetLanguage(CommonData.Language);
			this.toolClipboard_itemRemove.SetLanguage(CommonData.Language);
			this.toolClipboard_itemClear.SetLanguage(CommonData.Language);
			this.toolClipboard_itemEmpty.SetLanguage(CommonData.Language);

			this.labelTemplateName.SetLanguage(CommonData.Language);
			this.selectTemplateReplace.SetLanguage(CommonData.Language);

			this.tabPreview_pageRawTemplate.SetLanguage(CommonData.Language);
			this.tabPreview_pageReplaceTemplate.SetLanguage(CommonData.Language);

			this.columnName.SetLanguage(CommonData.Language);
			this.columnPath.SetLanguage(CommonData.Language);

			this.toolClipboard_itemType_itemClipboard.Text = ClipboardListType.History.ToText(CommonData.Language);
			this.toolClipboard_itemType_itemTemplate.Text = ClipboardListType.Template.ToText(CommonData.Language);

			this.tabPreview_pageText.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.tabPreview_pageRtf.Text = ClipboardType.Rtf.ToText(CommonData.Language);
			this.tabPreview_pageHtml.Text = ClipboardType.Html.ToText(CommonData.Language);
			this.tabPreview_pageImage.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.tabPreview_pageFile.Text = ClipboardType.File.ToText(CommonData.Language);
		}

		#endregion ////////////////////////////////////////

		#region skin
		protected override void ApplySkin()
		{
			base.ApplySkin();

			this.toolClipboard_itemEnabled.Image = CommonData.Skin.GetImage(SkinImage.Clipboard);
			this.toolClipboard_itemTopmost.Image = CommonData.Skin.GetImage(SkinImage.Pin);
			this.toolClipboard_itemSave.Image = CommonData.Skin.GetImage(SkinImage.Save);
			this.toolClipboard_itemRemove.Image = CommonData.Skin.GetImage(SkinImage.Remove);
			this.toolClipboard_itemClear.Image = CommonData.Skin.GetImage(SkinImage.Clear);
			this.toolClipboard_itemEmpty.Image = CommonData.Skin.GetImage(SkinImage.Refresh);

			this.toolClipboard_itemType_itemClipboard.Image = CommonData.Skin.GetImage(SkinImage.Clipboard);
			this.toolClipboard_itemType_itemTemplate.Image = CommonData.Skin.GetImage(SkinImage.RawTemplate);

			var skinItems = new[] {
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardText), Control = this._commandText, Name = imageText },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardRichTextFormat), Control = this._commandRtf, Name = imageRtf },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardHtml), Control = this._commandHtml, Name = imageHtml },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardImage), Control = this._commandImage, Name = imageImage },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardFile), Control = this._commandFile, Name = imageFile },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardCopy), Control = this._commandMulti, Name = string.Empty },
				new { Image = CommonData.Skin.GetImage(SkinImage.RawTemplate), Control = default(Button), Name = imageRawTemplate},
				new { Image = CommonData.Skin.GetImage(SkinImage.ReplaceTemplate), Control = default(Button), Name = imageReplaceTemplate},
				new { Image = CommonData.Skin.GetImage(SkinImage.Add), Control = this._commandAdd, Name = string.Empty },
				new { Image = CommonData.Skin.GetImage(SkinImage.Up), Control = this._commandUp, Name = string.Empty },
				new { Image = CommonData.Skin.GetImage(SkinImage.Down), Control = this._commandDown, Name = string.Empty },
			};
			this.imageTab.Images.Clear();
			foreach(var skinItem in skinItems) {
				if(skinItem.Control != null) {
					skinItem.Control.Image = skinItem.Image;
				}
				if(!string.IsNullOrEmpty(skinItem.Name)) {
					this.imageTab.Images.Add(skinItem.Name, skinItem.Image);
				}
			}
		}
		#endregion ////////////////////////////////////////

		#region Function


		Size GetButtonSize()
		{
			return new Size(
				IconScale.Small.ToWidth() + NativeMethods.GetSystemMetrics(SM.SM_CXEDGE) * 4,
				IconScale.Small.ToHeight() + NativeMethods.GetSystemMetrics(SM.SM_CYEDGE) * 4
			);
		}
		
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;

		//	ApplySetting();
		//}

		void ApplySettingUI()
		{
			CommonData.MainSetting.Clipboard.TemplateItems.ListChanged += TemplateItems_ListChanged;
			CommonData.MainSetting.Clipboard.HistoryItems.ListChanged += HistoryItems_ListChanged;
			Location = CommonData.MainSetting.Clipboard.Location;
			Size = CommonData.MainSetting.Clipboard.Size;
			ChangeEnabled(CommonData.MainSetting.Clipboard.Enabled);
			ChangeTopmost(CommonData.MainSetting.Clipboard.TopMost);
			var buttonSize = GetButtonSize();
			using(var g = CreateGraphics()) {
				var fontHeight = (int)g.MeasureString("☃", Font).Height;
				var buttonHeight = buttonSize.Height;
				this.listClipboard.ItemHeight = fontHeight + buttonHeight;
			}
			this.viewText.Font = this.CommonData.MainSetting.Clipboard.TextFont.Font;
			Visible = CommonData.MainSetting.Clipboard.Visible;

			ChangeSelectListType(CommonData.MainSetting.Clipboard.ClipboardListType);
		}

		/// <summary>
		/// BUGS: Formsバインドで描画が変になる。
		/// </summary>
		protected override void ApplySetting()
		{
			base.ApplySetting();

			ApplySettingUI();

			ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
		}

		void ChangeTopmost(bool topMost)
		{
			CommonData.MainSetting.Clipboard.TopMost = topMost;
			this.toolClipboard_itemTopmost.Checked = topMost;
			TopMost = topMost;
		}

		void ChangeEnabled(bool enabled)
		{
			CommonData.MainSetting.Clipboard.Enabled = enabled;
			this.toolClipboard_itemEnabled.Checked = enabled;
		}

		void ChangeSelectTypeControl(ToolStripItem item)
		{
			this.toolClipboard_itemType.Text = item.Text;
			this.toolClipboard_itemType.Image = item.Image;

			var map = new Dictionary<ToolStripItem, ClipboardListType>() {
				{ this.toolClipboard_itemType_itemClipboard, ClipboardListType.History },
				{ this.toolClipboard_itemType_itemTemplate,  ClipboardListType.Template},
			};

			ChangeSelectType(map[item]);
		}

		/// <summary>
		/// だっさいなぁ。
		/// </summary>
		/// <param name="type"></param>
		void ChangeSelectListType(ClipboardListType type)
		{
			var map = new Dictionary<ClipboardListType, ToolStripItem>() {
				{ ClipboardListType.History, this.toolClipboard_itemType_itemClipboard },
				{ ClipboardListType.Template, this.toolClipboard_itemType_itemTemplate},
			};

			ChangeSelectTypeControl(map[type]);

			ChangeCommand(-1);
			ChangeSelsectedItem(type == ClipboardListType.Template ? 0 : -1);
		}

		void ChangeCommandType(ClipboardListType type)
		{
			Control[] commandList;
			if(type == ClipboardListType.History) {
				commandList = new[] {
					this._commandMulti,
					this._commandText,
					this._commandRtf,
					this._commandHtml,
					this._commandImage,
					this._commandFile,
				};
			} else {
				commandList = new[] {
					this._commandMulti,
					this._commandAdd,
					this._commandUp,
					this._commandDown,
				};
			}
			this._panelClipboradItem.Controls.Clear();
			this._panelClipboradItem.Controls.AddRange(commandList);
			this._panelClipboradItem.Size = Size.Empty;
		}

		void ChangeSelectType(ClipboardListType type)
		{
			CommonData.MainSetting.Clipboard.ClipboardListType = type;

				SelectedItemIndex = -1;
				HoverItemIndex = -1;
			if(type == ClipboardListType.History) {

				this.listClipboard.DataSource = this.CommonData.MainSetting.Clipboard.HistoryItems;
			} else {
				Debug.Assert(type == ClipboardListType.Template);
				if(!this.CommonData.MainSetting.Clipboard.TemplateItems.Any()) {
					// 新規アイテムの生成
					var newItem = CreateTemplate();
					this.CommonData.MainSetting.Clipboard.TemplateItems.Add(newItem);
				}
				this.listClipboard.DataSource = this.CommonData.MainSetting.Clipboard.TemplateItems;

			}
			ChangeCommandType(type);
		}


		void ChangeListItemNumber(int index, int count)
		{
			if(index == -1) {
				this.statusClipboard_itemSelectedIndex.Text = "-";
			} else {
				this.statusClipboard_itemSelectedIndex.Text = (index + 1).ToString();
			}

			this.statusClipboard_itemCount.Text = count.ToString();
			this.statusClipboard_itemLimit.Text = CommonData.MainSetting.Clipboard.Limit.ToString();
		}

		void ChangeCommand(int index)
		{
			//if((index != -1 && HoverItemIndex != index) || (index != -1 && HoverItemIndex == -1)) {
			if((index > -1 && HoverItemIndex != index)) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
					var map = new Dictionary<ClipboardType, Control>() {
						{ ClipboardType.Text, this._commandText },
						{ ClipboardType.Rtf, this._commandRtf },
						{ ClipboardType.Html, this._commandHtml },
						{ ClipboardType.Image, this._commandImage },
						{ ClipboardType.File, this._commandFile },
						//{ ClipboardType.All, this._commandMulti},
					};
					foreach(var pair in map.ToArray()) {
						pair.Value.Enabled = (clipboardItem.ClipboardTypes.HasFlag(pair.Key));
					}
				} else {
					Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
					var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					var buttons = new[] {
						this._commandMulti,
						this._commandAdd,
					};
					foreach(var button in buttons) {
						button.Enabled = true;
					}
				}
			}

			HoverItemIndex = index;
			this._panelClipboradItem.Visible = HoverItemIndex != -1;
		}

		TabPage ChangeSelsectedHistoryItem(ClipboardItem clipboardItem)
		{
			var map = new Dictionary<ClipboardType, TabPage>() {
				{ ClipboardType.Text, this.tabPreview_pageText },
				{ ClipboardType.Rtf, this.tabPreview_pageRtf },
				{ ClipboardType.Html, this.tabPreview_pageHtml},
				{ ClipboardType.Image, this.tabPreview_pageImage },
				{ ClipboardType.File, this.tabPreview_pageFile },
			};

			foreach(var type in clipboardItem.GetClipboardTypeList()) {
				this.tabPreview.TabPages.Add(map[type]);

				switch(type) {
					case ClipboardType.Text: {
							this.viewText.Text = clipboardItem.Text;
						}
						break;

					case ClipboardType.Rtf: {
							this.viewRtf.Rtf = clipboardItem.Rtf;
						}
						break;

					case ClipboardType.Html: {
							ClipboardHtmlDataItem html;
							var result = ClipboardUtility.TryConvertHtmlFromClipbordHtml(clipboardItem.Html, out html, CommonData.Logger);

							if(result) {
								this.viewHtml.DocumentText = html.ToHtml();
							} else {
								var elements = string.Format("<p style='font-weight: bold; color: #f00; background: #fff'>{0}</p><hr />", CommonData.Language["clipboard/html/error"]);
								this.viewHtml.DocumentText = elements + clipboardItem.Html;
							}
						}
						break;

					case ClipboardType.Image: {
							this.viewImage.Image = clipboardItem.Image;
						}
						break;

					case ClipboardType.File: {
							var imageList = new ImageList();
							imageList.ColorDepth = ColorDepth.Depth32Bit;
							imageList.ImageSize = IconScale.Small.ToSize();
							var listItemList = new List<ListViewItem>(clipboardItem.Files.Count());
							var showExtentions = SystemEnvironment.IsExtensionShow();
							Func<string, string> getName;
							if(showExtentions) getName = Path.GetFileName; else getName = Path.GetFileNameWithoutExtension;
							foreach(var path in clipboardItem.Files) {
								var key = path.GetHashCode().ToString();
								var name = getName(path);

								Icon icon;
								if(FileUtility.Exists(path)) {
									icon = IconUtility.Load(path, IconScale.Small, 0);
								} else {
									icon = LauncherItem.notfoundIconMap[IconScale.Small];
								}

								imageList.Images.Add(key, icon);

								var listItem = new ListViewItem();
								listItem.Text = name;
								listItem.ImageKey = key;

								listItem.SubItems.Add(path);

								listItemList.Add(listItem);
							}
							this.viewFile.Items.Clear();
							this.viewFile.SmallImageList.ToDispose();
							this.viewFile.SmallImageList = imageList;
							this.viewFile.Items.AddRange(listItemList.ToArray());

							this.viewFile.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
						}
						break;

					default:
						throw new NotImplementedException();
				}
			}

			return map[clipboardItem.GetSingleClipboardType()];
		}

		TabPage ChangeSelsectedTemplateItem(TemplateItem templateItem)
		{
			this.tabPreview.TabPages.AddRange(new[] {
				this.tabPreview_pageRawTemplate,
				this.tabPreview_pageReplaceTemplate
			});

			// あれやこれやがだるいのでバインドる。
			this.inputTemplateName.DataBindings.Clear();
			this.inputTemplateName.DataBindings.Add("Text", templateItem, "Name", false, DataSourceUpdateMode.OnPropertyChanged);

			this.inputTemplateSource.DataBindings.Clear();
			this.inputTemplateSource.DataBindings.Add("Text", templateItem, "Source", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectTemplateReplace.DataBindings.Clear();
			this.selectTemplateReplace.DataBindings.Add("Checked", templateItem, "ReplaceMode", false, DataSourceUpdateMode.OnPropertyChanged);

			return this.tabPreview_pageRawTemplate;
		}

		void ChangeSelsectedItem(int index)
		{
			//SelectedItemIndex = index;
			this.tabPreview.Enabled = index != -1;

			if(index == -1) {
				return;
			}

			this.tabPreview.SuspendLayout();
			this.tabPreview.TabPages.Clear();

			TabPage defaultTabPage;
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
				defaultTabPage = ChangeSelsectedHistoryItem(clipboardItem);
			} else {
				var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
				defaultTabPage = ChangeSelsectedTemplateItem(templateItem);
			}
			this.tabPreview.SelectedTab = defaultTabPage;
			this.tabPreview.ResumeLayout();
		}

		void CopyItem(ClipboardItem clipboardItem, ClipboardType clipboardType)
		{
			var map = new Dictionary<ClipboardType, Action<CommonData>>() {
				{ ClipboardType.Text, (setting) => {
					ClipboardUtility.CopyText(clipboardItem.Text, setting);
				} },
				{ ClipboardType.Rtf, (setting) => {
					ClipboardUtility.CopyRtf(clipboardItem.Rtf, setting);
				} },
				{ ClipboardType.Html, (setting) => {
					ClipboardUtility.CopyHtml(clipboardItem.Html, setting);
				} },
				{ ClipboardType.Image, (setting) => {
					ClipboardUtility.CopyImage(clipboardItem.Image, setting);
				} },
				{ ClipboardType.File, (setting) => {
					ClipboardUtility.CopyFile(clipboardItem.Files.Where(f => FileUtility.Exists(f)), setting);
				} },
				{ ClipboardType.All, (setting) => {
					var data = new DataObject();
					var typeFuncs = new Dictionary<ClipboardType, Action>() {
						{ ClipboardType.Text, () => data.SetText(clipboardItem.Text, TextDataFormat.UnicodeText) },
						{ ClipboardType.Rtf, () => data.SetText(clipboardItem.Rtf, TextDataFormat.Rtf) },
						{ ClipboardType.Html, () => data.SetText(clipboardItem.Html, TextDataFormat.Html) },
						{ ClipboardType.Image, () => data.SetImage(clipboardItem.Image) },
						{ ClipboardType.File, () => {
							var sc = new StringCollection();
							sc.AddRange(clipboardItem.Files.ToArray());
							data.SetFileDropList(sc); 
						}},
					};
					foreach(var type in clipboardItem.GetClipboardTypeList()) {
						typeFuncs[type]();
					}
					ClipboardUtility.CopyDataObject(data, setting);
				} },
			};
			map[clipboardType](CommonData);
		}

		void CopySingleItem(int index)
		{
			Debug.Assert(index != -1);
			
			var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
			CopyItem(clipboardItem, clipboardItem.GetSingleClipboardType());
		}

		bool SaveItem(string path, ClipboardItem clipboardItem, ClipboardType type)
		{
			Debug.Assert(type != ClipboardType.File);

			var map = new Dictionary<ClipboardType, Action>() {
				{ ClipboardType.Text, () => File.WriteAllText(path, clipboardItem.Text) },
				{ ClipboardType.Rtf, () => File.WriteAllText(path, clipboardItem.Rtf) },
				{ ClipboardType.Html, () => File.WriteAllText(path, ClipboardUtility.ConvertHtmlFromClipbordHtml(clipboardItem.Html, CommonData.Logger).ToHtml()) },
				{ ClipboardType.Image, () => clipboardItem.Image.Save(path, ImageFormat.Png) },
			};

			try {
				map[type]();
				return true;
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, clipboardItem.Name, ex);
				return false;
			}
		}

		void OpenSaveDialog(ClipboardItem clipboardItem)
		{
			var filter = new DialogFilter();
			var map = new[] {
				new { ClipboardType = ClipboardType.Text, Wildcard = new [] {"*.txt"} },
				new { ClipboardType = ClipboardType.Rtf, Wildcard = new [] {"*.rtf"} },
				new { ClipboardType = ClipboardType.Html, Wildcard = new [] {"*.html"} },
				new { ClipboardType = ClipboardType.Image, Wildcard = new [] {"*.png"} },
			}.Select(v => new {
				ClipboardType = v.ClipboardType,
				DisplayText = v.ClipboardType.ToText(CommonData.Language),
				Wildcard = v.Wildcard,
			}).ToDictionary(k => k.ClipboardType, v => new DialogFilterValueItem<ClipboardType>(v.ClipboardType, v.DisplayText, v.Wildcard));

			var defType = clipboardItem.GetSingleClipboardType();
			var defIndex = 0;
			var tempIndex = 0;
			foreach(var type in clipboardItem.GetClipboardTypeList().Where(t => t != ClipboardType.File)) {
				filter.Items.Add(map[type]);
				tempIndex += 1;
				if(defType == type) {
					defIndex = tempIndex;
				}
			}

			using(var dialog = new SaveFileDialog()) {
				dialog.Attachment(filter);
				dialog.FileName = clipboardItem.Timestamp.ToString(Literal.timestampFileName);
				dialog.FilterIndex = defIndex;
				if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					var item = (DialogFilterValueItem<ClipboardType>)filter.Items[dialog.FilterIndex - 1];
					var path = dialog.FileName;
					var type = item.Value;
					SaveItem(path, clipboardItem, type);
				}
			}
		}

		public void ClearEvent()
		{
			CommonData.MainSetting.Clipboard.TemplateItems.ListChanged -= TemplateItems_ListChanged;
			CommonData.MainSetting.Clipboard.HistoryItems.ListChanged -= HistoryItems_ListChanged;
		}

		void ListChanged<T>(ClipboardListType targetType, IList<T> itemList, Action action)
		{
			if(CommonData.MainSetting.Clipboard.ClipboardListType != targetType) {
				return;
			}

			this.listClipboard.SuspendLayout();

			var isActive = Form.ActiveForm == this;
			var selectedIndex = this.listClipboard.SelectedIndex;
			this.listClipboard.DataSource = null;

			if(action != null) {
				action();
			}

			//if(itemList.Any()) {
				this.listClipboard.DataSource = itemList;
			//}

			if(isActive) {
				if(selectedIndex + 1 < this.listClipboard.Items.Count) {
					this.listClipboard.SelectedIndex = selectedIndex + 1;
				}
			} else if(itemList.Any()) {
				this.listClipboard.SelectedIndex = 0;
			}
			this._panelClipboradItem.Visible = false;
			ChangeCommand(-1);
			this.listClipboard.ResumeLayout();

		}

		TemplateItem CreateTemplate()
		{
			Debug.Assert(CommonData != null);

			return new TemplateItem() {
				Name = TextUtility.ToUniqueDefault(CommonData.Language["new/template-item"], CommonData.MainSetting.Clipboard.TemplateItems.Select(t => t.Name)),
			};
		}

		/// <summary>
		/// テンプレートを追加。
		/// </summary>
		/// <param name="templateItem">追加するテンプレート位置のアイテム</param>
		void AddTemplate(TemplateItem templateItem)
		{
			var createdItem = CreateTemplate();
			var items = CommonData.MainSetting.Clipboard.TemplateItems;
			if(templateItem != null) {
				var index = items.IndexOf(templateItem);
				items.Insert(index, createdItem);
			}
		}

		void UpTemplate(TemplateItem templateItem)
		{
		}

		void DownTemplate(TemplateItem templateItem)
		{
		}

		void CopyTemplate(TemplateItem templateItem)
		{
		}

		#endregion ////////////////////////////////////////

		#region Draw

		void DrawClipboardItem(Graphics g, int itemIndex, Rectangle bounds, Color foreColor)
		{
			var item = CommonData.MainSetting.Clipboard.HistoryItems[itemIndex];
			var map = new Dictionary<ClipboardType, string>() {
					{ ClipboardType.Text, imageText},
					{ ClipboardType.Rtf, imageRtf},
					{ ClipboardType.Html, imageHtml},
					{ ClipboardType.Image, imageImage},
					{ ClipboardType.File, imageFile},
				};
			var image = this.imageTab.Images[map[item.GetSingleClipboardType()]];

			var drawArea = new Rectangle(bounds.X + this.listClipboard.Margin.Left, bounds.Bottom - image.Height - +this.listClipboard.Margin.Bottom - 1, image.Width, image.Height);

			g.DrawImage(image, drawArea);

			using(var sf = new StringFormat())
			using(var brush = new SolidBrush(foreColor)) {
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				g.DrawString(item.Name, Font, brush, bounds, sf);

				sf.Alignment = StringAlignment.Far;
				sf.LineAlignment = StringAlignment.Far;
				g.DrawString(item.Timestamp.ToString(), SystemFonts.SmallCaptionFont, brush, bounds, sf);
			}
		}

		void DrawTemplateItem(Graphics g, int itemIndex, Rectangle bounds, Color foreColor)
		{
			var item = CommonData.MainSetting.Clipboard.TemplateItems[itemIndex];
			using(var sf = new StringFormat())
			using(var brush = new SolidBrush(foreColor)) {
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				g.DrawString(item.Name, Font, brush, bounds, sf);
			}
		}

		void DrawItem(DrawItemEventArgs e)
		{
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				DrawClipboardItem(e.Graphics, e.Index, e.Bounds, e.ForeColor);
			} else {
				Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
				DrawTemplateItem(e.Graphics, e.Index, e.Bounds, e.ForeColor);
			}
		}

		#endregion ////////////////////////////////////////

		private void toolClipboard_itemType_itemClipboard_Click(object sender, EventArgs e)
		{
			ChangeSelectTypeControl((ToolStripItem)sender);
		}

		void TemplateItems_ListChanged(object sender, EventArgs e)
		{
			ListChanged(ClipboardListType.Template, CommonData.MainSetting.Clipboard.TemplateItems, null);
		}

		void HistoryItems_ListChanged(object sender, EventArgs e)
		{
			ListChanged(ClipboardListType.History, CommonData.MainSetting.Clipboard.HistoryItems, () => {
				if(CommonData.MainSetting.Clipboard.HistoryItems.Count == 0) {
					this.viewText.ResetText();
					this.viewRtf.ResetText();
					this.viewHtml.DocumentText = null;
					this.viewImage.Image = null;
					this.viewFile.Items.Clear();
				}
			});
		}

		private void listClipboard_DrawItem(object sender, DrawItemEventArgs e)
		{
			if(e.Index != -1) {

				e.DrawBackground();

				DrawItem(e);

				using(var pen = new Pen(Color.FromArgb(128, e.ForeColor))) {
					var bottom = e.Bounds.Bottom - 1;
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
					e.Graphics.DrawLine(pen, e.Bounds.X, bottom, e.Bounds.Right - 1, bottom);
				}
				e.DrawFocusRectangle();
			}
		}

		private void listClipboard_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Debug.WriteLine(this.listClipboard.SelectedIndex.ToString());
			//Debug.WriteLine(ActiveControl);
			//var isActive = ActiveControl == this.listClipboard;
			var index = this.listClipboard.SelectedIndex;
			if(index != SelectedItemIndex) {
				SelectedItemIndex = index;
				ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
				ChangeSelsectedItem(this.listClipboard.SelectedIndex);
				if(Form.ActiveForm == this) {
					ActiveControl = this.listClipboard;
				}
				//	this.listClipboard.Select();
			}
		}

		private void ClipboardForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
				CommonData.MainSetting.Clipboard.Visible = false;
			}
		}

		private void ClipboardForm_LocationChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Clipboard.Location = Location;
		}

		private void ClipboardForm_SizeChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Clipboard.Size = Size;
		}

		private void toolClipboard_itemTopmost_Click(object sender, EventArgs e)
		{
			var check = !toolClipboard_itemTopmost.Checked;
			ChangeTopmost(check);
		}

		private void listClipboard_MouseMove(object sender, MouseEventArgs e)
		{
			var index = this.listClipboard.IndexFromPoint(e.Location);// -this.listClipboard.TopIndex;
			var showIndex = index - this.listClipboard.TopIndex;
			var top = this.listClipboard.ItemHeight * (showIndex + 1) - GetButtonSize().Height - 1;
			
			if(top != this._panelClipboradItem.Top) {
				this._panelClipboradItem.Top = top;
			}
			ChangeCommand(index);
		}

		private void tabPreview_Selecting(object sender, TabControlCancelEventArgs e)
		{
			//Debug.Assert(SelectedItemIndex != -1);
			var index = this.listClipboard.SelectedIndex;
			Debug.Assert(index != -1);

			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
				var typeList = clipboardItem.GetClipboardTypeList();
				var list = new[] {
					new { TabPage = this.tabPreview_pageText, ClipboardType = ClipboardType.Text },
					new { TabPage = this.tabPreview_pageRtf, ClipboardType = ClipboardType.Rtf },
					new { TabPage = this.tabPreview_pageHtml, ClipboardType = ClipboardType.Html },
					new { TabPage = this.tabPreview_pageImage, ClipboardType = ClipboardType.Image },
					new { TabPage = this.tabPreview_pageFile, ClipboardType = ClipboardType.File },
				};
				foreach(var item in list) {
					if(e.TabPage == item.TabPage && typeList.Any(t => t.HasFlag(item.ClipboardType))) {
						return;
					}
				}

				e.Cancel = true;
			} else {
				Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
				if(e.TabPage == this.tabPreview_pageReplaceTemplate) {
					var clipboardItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					// TODO: RTF
					this.viewReplaceTemplate.Text = clipboardItem.Source;
				}
			}
		}

		void command_Click(object sender, EventArgs e)
		{
			if(0 > HoverItemIndex) {
				return;
			}
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				try {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[HoverItemIndex];
					var map = new Dictionary<object, ClipboardType>() {
						{ this._commandText, ClipboardType.Text },
						{ this._commandRtf, ClipboardType.Rtf },
						{ this._commandHtml, ClipboardType.Html },
						{ this._commandImage, ClipboardType.Image },
						{ this._commandFile, ClipboardType.File },
						{ this._commandMulti, ClipboardType.All },
					};
					CopyItem(clipboardItem, map[sender]);
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
				}
			} else {
				var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[HoverItemIndex];
				var map = new Dictionary<object, Action<TemplateItem>>() {
					{ this._commandMulti, CopyTemplate },
					{ this._commandAdd,   AddTemplate },
					{ this._commandUp,    UpTemplate },
					{ this._commandDown,  DownTemplate },
				};
				map[sender](templateItem);
			}
		}

		private void listClipboard_DoubleClick(object sender, EventArgs e)
		{
			var index = this.listClipboard.SelectedIndex;
			if(index != -1) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					try {
						var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
						CopyItem(clipboardItem, ClipboardType.All);
					} catch(Exception ex) {
						CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				} else {
					Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
					var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					CopyTemplate(templateItem);
				}
			}
		}

		private void toolClipboard_itemSave_Click(object sender, EventArgs e)
		{
			var index = this.listClipboard.SelectedIndex;
			if(index != -1) {
				var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
				OpenSaveDialog(clipboardItem);
			}
		}

		private void toolClipboard_itemClear_ButtonClick(object sender, EventArgs e)
		{
			var index = this.listClipboard.SelectedIndex;
			if(index != -1) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
					CommonData.MainSetting.Clipboard.HistoryItems.RemoveAt(index);
					clipboardItem.ToDispose();
				} else {
					Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
					// 最後の一つを削除するとあまりよろしくない
					if(CommonData.MainSetting.Clipboard.TemplateItems.Count != 1) {
						var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
						CommonData.MainSetting.Clipboard.HistoryItems.RemoveAt(index);
					}
				}
			}
		}

		private void toolClipboard_itemClear_Click(object sender, EventArgs e)
		{
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				foreach(var item in CommonData.MainSetting.Clipboard.HistoryItems.ToArray()) {
					item.ToDispose();
				}
				CommonData.MainSetting.Clipboard.HistoryItems.Clear();
			} else {
				Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
				var lastItem = CommonData.MainSetting.Clipboard.TemplateItems.LastOrDefault();
				if(lastItem != null) {
					CommonData.MainSetting.Clipboard.TemplateItems.Clear();
					CommonData.MainSetting.Clipboard.TemplateItems.Add(lastItem);
				}
			}
		}

		private void toolClipboard_itemEmpty_Click(object sender, EventArgs e)
		{
			Clipboard.Clear();
		}

		private void viewHtml_ShowMessage(object sender, ShowMessageEventArgs e)
		{
			e.Result = 0;
			e.Handled = false;
		}

		private void listClipboard_MouseLeave(object sender, EventArgs e)
		{
			if(this._panelClipboradItem.Visible) {
				var point = this.listClipboard.PointToClient(Cursor.Position);
				this._panelClipboradItem.Visible = this.listClipboard.DisplayRectangle.Contains(point);
			}
		}

		private void toolClipboard_itemEnabled_Click(object sender, EventArgs e)
		{
			var check = !toolClipboard_itemEnabled.Checked;
			ChangeEnabled(check);
		}

		void listClipboard_MouseWheel(object sender, MouseEventArgs e)
		{
			var noDraw = new IntPtr(0);
			var onDraw = new IntPtr(1);

			NativeMethods.SendMessage(Handle, WM.WM_SETREDRAW, noDraw, IntPtr.Zero);
			
			this.listClipboard.Invalidate();

			NativeMethods.SendMessage(Handle, WM.WM_SETREDRAW, onDraw, IntPtr.Zero);

			this._panelClipboradItem.Refresh();
		}

		private void ClipboardForm_VisibleChanged(object sender, EventArgs e)
		{
			if(Visible) {
				UIUtility.ShowFrontActive(this);
			}
		}
	}
}
