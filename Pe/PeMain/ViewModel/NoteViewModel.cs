﻿/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.Attribute;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.IF;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.IF.ViewExtend;
    using ContentTypeTextNet.Pe.PeMain.Logic.Property;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View;

    public class NoteViewModel: HavingViewSingleModelWrapperViewModelBase<NoteIndexItemModel, NoteWindow>, IHavingAppNonProcess, IWindowHitTestData, IWindowAreaCorrectionData, ICaptionDoubleClickData, IHavingAppSender, IColorPair, INoteMenuItem
    {
        #region static

        public Thickness CaptionPadding { get { return Constants.noteCaptionPadding; } }

        #endregion

        #region variable

        IndexBodyItemModelBase _indexBody = null;
        double _compactHeight;
        Visibility _titleEditVisibility = Visibility.Collapsed;

        bool _editingTitle = false;
        bool _editingBody = false;

        Brush _borderBrush;

        #endregion

        public NoteViewModel(NoteIndexItemModel model, NoteWindow view, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view)
        {
            AppNonProcess = appNonProcess;
            AppSender = appSender;

            BorderBrush = MakeBorderBrush();

            SetCompactArea();

            ResetChangeFlag();
        }

        #region property

        NoteBodyItemModel IndexBody { get { return this._indexBody as NoteBodyItemModel; } }

        public bool IsTemporary { get; set; }

        public Brush BorderBrush
        {
            get { return this._borderBrush; }
            set { SetVariableValue(ref this._borderBrush, value); }
        }

        public double TitleHeight { get { return Constants.noteCaptionHeight + CaptionPadding.GetHorizon(); } }

        public Visibility CaptionButtonVisibility
        {
            get
            {
                if(IsLocked) {
                    return Visibility.Collapsed;
                } else {
                    return Visibility.Visible;
                }
            }
        }

        public Visibility TitleCaptionVisibility
        {
            get { return this._titleEditVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }
        public Visibility TitleEditVisibility
        {
            get { return this._titleEditVisibility; }
            set
            {
                if(SetVariableValue(ref this._titleEditVisibility, value)) {
                    OnPropertyChanged(nameof(TitleCaptionVisibility));
                }
            }
        }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                    if(HasView) {
                        var map = new Dictionary<string, string>() {
                            { LanguageKey.noteTitle, Name },
                        };
                        LanguageUtility.SetTitle(View, AppNonProcess.Language, map);
                    }
                }
            }
        }

        public bool IsLocked
        {
            get { return Model.IsLocked; }
            set
            {
                if(SetModelValue(value)) {
                    OnPropertyChanged(nameof(CaptionButtonVisibility));
                    OnPropertyChanged(nameof(IsBodyReadOnly));
                }
            }
        }

        public bool IsCompacted
        {
            get { return Model.IsCompacted; }
            set
            {
                if(SetModelValue(value)) {
                    if(value) {
                        SetCompactArea();
                    }
                    CallOnPropertyChangeDisplayItem();
                    OnPropertyChanged(nameof(WindowHeight));
                }
            }
        }

        public bool IsBodyReadOnly
        {
            get
            {
                if(IsLocked) {
                    return !this._editingBody;
                }

                return IsLocked;
            }
        }

        public bool AutoLineFeed
        {
            get { return Model.AutoLineFeed; }
            set { SetModelValue(value); }
        }

        public string Body
        {
            get
            {
                if(IndexBody == null) {
                    this._indexBody = AppSender.SendLoadIndexBody(Library.PeData.Define.IndexKind.Note, Model.Id);
                    if(this._indexBody == null) {
                        this._indexBody = new NoteBodyItemModel();
                    }
                }
                var indexBody = IndexBody;
                return indexBody.Text ?? string.Empty;
            }
            set
            {
                if(IsTemporary) {
                    return;
                }
                if(IndexBody == null) {
                    this._indexBody = new NoteBodyItemModel();
                }
                var indexBody = IndexBody;
                if(SetPropertyValue(indexBody, value, nameof(indexBody.Text))) {
                    indexBody.History.Update();
                    AppSender.SendSaveIndexBody(IndexBody, Model.Id, Timing.Delay);
                }
            }
        }

        //public FontFamily FontFamily
        //{
        //	get { return FontUtility.MakeFontFamily(Model.Font.Family, SystemFonts.MessageFontFamily); }
        //	set 
        //	{
        //		if(value != null) {
        //			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
        //			SetPropertyValue(Model.Font, fontFamily, "Family");
        //		}
        //	}
        //}

        //public bool FontBold
        //{
        //	get { return Model.Font.Bold; }
        //	set { SetPropertyValue(Model.Font, value, "Bold"); }
        //}

        //public bool FontItalic
        //{
        //	get { return Model.Font.Italic; }
        //	set { SetPropertyValue(Model.Font, value, "Italic"); }
        //}

        //public double FontSize
        //{
        //	get { return Model.Font.Size; }
        //	set { SetPropertyValue(Model.Font, value, "Size"); }

        //}

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Font); }
            set { FontModelProperty.SetFamily(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Font); }
            set { FontModelProperty.SetBold(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Font); }
            set { FontModelProperty.SetItalic(Model.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Font); }
            set { FontModelProperty.SetSize(Model.Font, value, OnPropertyChanged); }
        }

        public Brush ForeColorBrush
        {
            get { return new SolidColorBrush(ForeColor); }
        }

        #endregion


        #endregion

        #region command

        public ICommand SaveIndexCommnad
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        EndEditTitle();
                        EndEditBody();

                        if(IsTemporary) {
                            return;
                        }

                        if(IsChanged) {
                            Model.History.Update();
                            AppSender.SendSaveIndex(IndexKind.Note, Timing.Delay);
                            ResetChangeFlag();
                        }
                        if(HasView) {
                            // フォーカス外れたときにうまいこと反映されない対策
                            Body = View.body.Text;
                            ResetChangeFlag();
                        }
                    }
                );

                return result;
            }
        }

        //public ICommand SwitchCompactCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //				IsCompacted = !IsCompacted;
        //			}
        //		);

        //		return result;
        //	}
        //}

        //public ICommand SwitchTopMostCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //				IsTopmost = !IsTopmost;
        //			}
        //		);

        //		return result;
        //	}
        //}

        public ICommand HideCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            // 表示切替はイベント内で実施。
                            View.UserClose();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Close();
                        }
                        AppSender.SendRemoveIndex(IndexKind.Note, Model.Id, Timing.Delay);
                    }
                );

                return result;
            }
        }

        public ICommand CopyBodyCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(string.IsNullOrEmpty(Body)) {
                            AppNonProcess.Logger.Information("empty body");
                            return;
                        }

                        ClipboardUtility.CopyText(Body, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand EditTitleCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        TitleEditVisibility = Visibility.Visible;
                        this._editingTitle = true;
                        if(HasView) {
                            View.title.SelectAll();
                            View.title.Focus();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand HideTitleEditCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        EndEditTitle();
                    }
                );

                return result;
            }
        }

        public ICommand EditBodyCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        this._editingBody = true;
                        OnPropertyChanged(nameof(IsBodyReadOnly));
                        if(HasView) {
                            if(View.body.SelectionLength == 0) {
                                View.body.SelectAll();
                            }
                            View.body.Focus();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand ReturnTitleCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(this._editingTitle) {
                            EndEditTitle();
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region HavingViewSingleModelWrapperViewModelBase

        protected override void InitializeView()
        {
            SetCompactArea();
            OnPropertyChanged(nameof(IsBodyReadOnly));

            View.UserClosing += View_UserClosing;
            PopupUtility.Attachment(View, View.popup);

            base.InitializeView();
        }

        protected override void UninitializeView()
        {
            View.UserClosing -= View_UserClosing;

            base.UninitializeView();
        }

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();

            var propertyNames = new[] {
                nameof(MenuIcon),
                nameof(MenuText),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion

        #region IColorPair

        public Color ForeColor
        {
            get { return ColorPairProperty.GetNoneAlphaForeColor(Model); }
            set
            {
                if(ColorPairProperty.SetNoneAlphaForekColor(Model, value, OnPropertyChanged)) {
                    CallOnPropertyChange(nameof(ForeColorBrush));
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        public Color BackColor
        {
            get { return ColorPairProperty.GetNoneAlphaBackColor(Model); }
            set
            {
                if(ColorPairProperty.SetNoneAlphaBackColor(Model, value, OnPropertyChanged)) {
                    BorderBrush = MakeBorderBrush();
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        #endregion

        #region function

        void SetCompactArea()
        {
            this._compactHeight = CaptionArea.Height + ResizeThickness.GetVertical();
        }

        void EndEditTitle()
        {
            if(this._editingTitle) {
                this._editingTitle = false;
                if(HasView) {
                    Name = View.title.Text;
                }
                TitleEditVisibility = Visibility.Collapsed;
            }
        }

        void EndEditBody()
        {
            if(this._editingBody) {
                this._editingBody = false;
                OnPropertyChanged(nameof(IsBodyReadOnly));
            }
        }

        Brush MakeBorderBrush()
        {
            var brush = new SolidColorBrush(Model.BackColor);
            FreezableUtility.SafeFreeze(brush);

            return brush;
        }

        #endregion

        #region ITopMost

        public bool IsTopmost
        {
            get { return TopMostProperty.GetTopMost(Model); }
            set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
        }

        #endregion

        #region IVisible

        public Visibility Visibility
        {
            get { return VisibleVisibilityProperty.GetVisibility(Model); }
            set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
        }

        public bool IsVisible
        {
            get { return VisibleVisibilityProperty.GetVisible(Model); }
            set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
        }

        #endregion

        #region window

        public double WindowLeft
        {
            get { return WindowAreaProperty.GetWindowLeft(Model); }
            set
            {
                if(!IsLocked) {
                    WindowAreaProperty.SetWindowLeft(Model, value, OnPropertyChanged);
                }
            }
        }

        public double WindowTop
        {
            get { return WindowAreaProperty.GetWindowTop(Model); }
            set
            {
                if(!IsLocked) {
                    WindowAreaProperty.SetWindowTop(Model, value, OnPropertyChanged);
                }
            }
        }
        public double WindowWidth
        {
            get
            {
                return WindowAreaProperty.GetWindowWidth(Model);
            }
            set
            {
                if(!IsLocked && !IsCompacted) {
                    WindowAreaProperty.SetWindowWidth(Model, value, OnPropertyChanged);
                }
            }
        }
        public double WindowHeight
        {
            get
            {
                if(IsCompacted) {
                    return this._compactHeight;
                } else {
                    return WindowAreaProperty.GetWindowHeight(Model);
                }
            }
            set
            {
                if(!IsLocked && !IsCompacted) {
                    WindowAreaProperty.SetWindowHeight(Model, value, OnPropertyChanged);
                }
            }
        }

        #endregion

        #region IHavingAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IWindowHitTestData

        /// <summary>
        /// ヒットテストを行うか
        /// </summary>
        public bool UsingBorderHitTest { get { return !(IsCompacted || IsLocked); } }

        public bool UsingCaptionHitTest
        {
            get
            {
                if(this._editingTitle) {
                    return false;
                }

                return !IsLocked;
            }
        }

        /// <summary>
        /// タイトルバーとして認識される領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect CaptionArea
        {
            get
            {
                var resizeThickness = ResizeThickness;
                var rect = new Rect(
                    resizeThickness.Left,
                    resizeThickness.Top,
                    View.caption.ActualWidth,
                    TitleHeight
                );

                return rect;
            }
        }
        /// <summary>
        /// サイズ変更に使用する境界線。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness ResizeThickness { get { return SystemParameters.WindowResizeBorderThickness; } }

        #endregion

        #region IWindowAreaCorrectionData

        /// <summary>
        /// ウィンドウサイズの倍数制御を行うか。
        /// </summary>
        public bool UsingMultipleResize { get { return false; } }
        /// <summary>
        /// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size MultipleSize { get { throw new NotImplementedException(); } }
        /// <summary>
        /// タイトルバーとかボーダーを含んだ領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness MultipleThickness { get { throw new NotImplementedException(); } }
        /// <summary>
        /// 移動制限を行うか。
        /// </summary>
        public bool UsingMoveLimitArea { get { return false; } }
        /// <summary>
        /// 移動制限に使用する論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect MoveLimitArea { get { throw new NotImplementedException(); } }
        /// <summary>
        /// 最大化・最小化を抑制するか。
        /// </summary>
        public bool UsingMaxMinSuppression { get { return true; } }

        #endregion

        #region ICaptionDoubleClickData

        public void OnCaptionDoubleClick(object sender, CancelEventArgs e)
        { }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region INoteMenuItem

        public string MenuText { get { return NoteUtility.MakeMenuText(Model); } }

        public FrameworkElement MenuIcon { get { return NoteUtility.MakeMenuIcon(Model); } }

        public ICommand NoteMenuSelectedCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Activate();
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region HavingViewSingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                AppNonProcess = null;
                AppSender = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_UserClosing(object sender, CancelEventArgs e)
        {
            IsVisible = false;
        }

    }
}
