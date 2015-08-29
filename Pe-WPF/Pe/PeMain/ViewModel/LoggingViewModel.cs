﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Threading;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;
	using Microsoft.Win32;

	public class LoggingViewModel : HavingViewSingleModelWrapperViewModelBase<LoggingSettingModel, LoggingWindow>, ILogAppender, IWindowStatus, IHavingNonProcess
	{
		public LoggingViewModel(LoggingSettingModel model, LoggingWindow view, CollectionModel<LogItemModel> logItems, INonProcess nonProcess)
			: base(model, view)
		{
			NonProcess = nonProcess;

			if(logItems != null) {
				LogItems = logItems;
			} else {
				LogItems = new CollectionModel<LogItemModel>();
			}
		}

		#region property

		public CollectionModel<LogItemModel> LogItems { get; set; }

		#region IWindowStatus

		public double WindowLeft
		{
			get { return WindowStatusProperty.GetWindowLeft(Model); }
			set { WindowStatusProperty.SetWindowLeft(Model, value, OnPropertyChanged); }
		}

		public double WindowTop
		{
			get { return WindowStatusProperty.GetWindowTop(Model); }
			set { WindowStatusProperty.SetWindowTop(Model, value, OnPropertyChanged); }
		}

		public double WindowWidth
		{
			get { return WindowStatusProperty.GetWindowWidth(Model); }
			set { WindowStatusProperty.SetWindowWidth(Model, value, OnPropertyChanged); }
		}

		public double WindowHeight
		{
			get { return WindowStatusProperty.GetWindowHeight(Model); }
			set { WindowStatusProperty.SetWindowHeight(Model, value, OnPropertyChanged); }
		}

		public WindowState WindowState
		{
			get { return WindowStatusProperty.GetWindowState(Model); }
			set { WindowStatusProperty.SetWindowState(Model, value, OnPropertyChanged); }
		}

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

		#region ITopMost

		public bool IsTopmost
		{
			get { return TopMostProperty.GetTopMost(Model); }
			set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
		}

		#endregion

		#endregion

		#endregion

		#region command

		public ICommand SaveCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						SaveFileInDialog(LogItems);
					}
				);

				return result;
			}
		}

		public ICommand ClearCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						LogItems.Clear();
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		bool SaveFileInDialog(IEnumerable<LogItemModel> logItems)
		{
			var filter = new DialogFilterList() {
				new DialogFilterItem(NonProcess.Language["dialog/filter/log"], Constants.dialogFilterLog),
			};
			var dialog = new SaveFileDialog() {
				AddExtension = true,
				CheckPathExists = true,
				ValidateNames = true,
				Filter = filter.FilterText,
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
				FileName = Constants.GetNowTimestampFileName(),
			};

			var dialogResult = dialog.ShowDialog();
			if (dialogResult.GetValueOrDefault()) {
				return SaveFile(dialog.FileName, logItems);
			} else {
				return false;
			}
		}

		bool SaveFile(string path, IEnumerable<LogItemModel> logItems)
		{
			using (var stream = new StreamWriter(File.Create(path))) {
				foreach (var logMessage in logItems.Select(l => LogUtility.MakeLogDetailText(l))) {
					stream.WriteLine(logMessage);
				}
			}

			return true;
		}

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing += View_UserClosing;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

		#region ILogCollector

		public void AddLog(LogItemModel item)
		{
			if(HasView) {
				View.Dispatcher.BeginInvoke(new Action(() => {
					LogItems.Add(item);
					View.listLog.SelectedItem = item;
					View.listLog.ScrollIntoView(item);
				}));
			} else {
				LogItems.Add(item);
			}
		}

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		void View_UserClosing(object sender, CancelEventArgs e)
		{
			Debug.Assert(HasView);

			e.Cancel = true;
			IsVisible = false;
		}
	}
}
