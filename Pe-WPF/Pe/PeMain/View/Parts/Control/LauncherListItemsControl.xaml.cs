﻿namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
	using ContentTypeTextNet.Library.SharedLibrary.View.Converter;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

	/// <summary>
	/// LauncherItemsListControl.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherListItemsControl : CommonDataUserControl//, INotifyPropertyChanged
	{
		//#region variable

		//bool _canListEdit;

		//#endregion

		public LauncherListItemsControl()
		{
			InitializeComponent();

			CanListEdit = false;

			this.listItems.SelectionChanged += ListItems_SelectionChanged;
		}

		//#region INotifyPropertyChanged

		///// <summary>
		///// プロパティが変更された際に発生。
		///// </summary>
		//public event PropertyChangedEventHandler PropertyChanged = delegate { };

		///// <summary>
		///// PropertyChanged呼び出し。
		///// </summary>
		///// <param name="propertyName"></param>
		//protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		//{
		//	this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		//}

		//#endregion

		#region SelectedLauncherItemProperty

		public static readonly DependencyProperty SelectedLauncherItemProperty = DependencyProperty.Register(
			"SelectedLauncherItem",
			typeof(LauncherListItemViewModel),
			typeof(LauncherListItemsControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedLauncherItem))
		);

		private static void OnSelectedLauncherItem(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as LauncherListItemsControl;
			if (control != null) {
				var viewModel = e.NewValue as LauncherListItemViewModel;
				if (viewModel != null) {
					control.SelectedLauncherItem = viewModel;
				}
			}
		}

		public LauncherListItemViewModel SelectedLauncherItem
		{
			get { return GetValue(SelectedLauncherItemProperty) as LauncherListItemViewModel; }
			set 
			{
				SetValue(SelectedLauncherItemProperty, value);
				this.listItems.SelectedItem = value;
				if (value != null) {
					SelectedLauncherModel = value.Model;
				}
			}
		}

		#endregion

		//#region SelectedLauncherViewModelProperty

		//public static readonly DependencyProperty SelectedLauncherViewModelProperty = DependencyProperty.Register(
		//	"SelectedLauncherViewModel",
		//	typeof(LauncherItemViewModelBase),
		//	typeof(LauncherListItemsControl),
		//	new FrameworkPropertyMetadata(null)
		//);
		//public LauncherItemViewModelBase SelectedLauncherViewModel
		//{
		//	get { return GetValue(SelectedLauncherViewModelProperty) as LauncherItemViewModelBase; }
		//	set { SetValue(SelectedLauncherViewModelProperty, value); }
		//}

		void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedLauncherItem = this.listItems.SelectedItem as LauncherListItemViewModel;
		}

		//#endregion

		#region SelectedLauncherModelProperty

		public static readonly DependencyProperty SelectedLauncherModelProperty = DependencyProperty.Register(
			"SelectedLauncherModel",
			typeof(LauncherItemModel),
			typeof(LauncherListItemsControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedLauncherModel))
		);

		private static void OnSelectedLauncherModel(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as LauncherListItemsControl;
			if (control != null) {
				var model = e.NewValue as LauncherItemModel;
				if (model != null) {
					control.SelectedLauncherModel = model;
				}
			}
		}

		public LauncherItemModel SelectedLauncherModel
		{
			get { return GetValue(SelectedLauncherModelProperty) as LauncherItemModel; }
			set
			{
				SetValue(SelectedLauncherModelProperty, value);
				if (SelectedLauncherItem != null && SelectedLauncherItem.Model == value) {
					return;
				}
				SelectedLauncherItem = this.listItems.ItemsSource
					.Cast<LauncherListItemViewModel>()
					.FirstOrDefault(vm => vm.Model == value)
				;
				//if (value != null) {
				//	SelectedLauncherViewModel = new LauncherItemSimpleViewModel(SelectedLauncherItem, CommonData.NonProcess, CommonData.AppSender);
				//}
			}
		}

		#endregion


		#region CanListEditProperty

		public static readonly DependencyProperty CanListEditProperty = DependencyProperty.Register(
			"CanListEdit",
			typeof(bool),
			typeof(LauncherListItemsControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnChangedCanListEdit))
		);

		public bool CanListEdit
		{
			get { return (bool)GetValue(CanListEditProperty); }
			set 
			{ 
				SetValue(CanListEditProperty, value);
				ChangedCanListEdit(this, (bool)value);
			}
		}

		private static void OnChangedCanListEdit(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as LauncherListItemsControl;
			if(control != null) {
				ChangedCanListEdit(control, (bool)e.NewValue);
			}
		}

		static void ChangedCanListEdit(LauncherListItemsControl control, bool value)
		{
			var converter = new BooleanVisibilityConverter();
			var visibility = (Visibility)converter.Convert(value, typeof(bool), null, null);
			var elements = new UIElement[] { control.toolAppend, control.toolRemove, control.toolSeparator };
			foreach(var element in elements) {
				element.Visibility = visibility;
			}
		}

		#endregion

		#region AppendCommandProperty

		public static readonly DependencyProperty AppendCommandProperty = DependencyProperty.Register(
			"AppendCommand",
			typeof(ICommand),
			typeof(LauncherListItemsControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnAppendCommandChanged))
		);

		private static void OnAppendCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as LauncherListItemsControl;
			if(control != null) {
				control.AppendCommand = e.NewValue as ICommand;
			}
		}

		public ICommand AppendCommand
		{
			get { return GetValue(AppendCommandProperty) as ICommand; }
			set { SetValue(AppendCommandProperty, value); }
		}

		#endregion

		#region RemoveCommandProperty

		public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
			"RemoveCommand",
			typeof(ICommand),
			typeof(LauncherListItemsControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRemoveCommandChanged))
		);

		private static void OnRemoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as LauncherListItemsControl;
			if(control != null) {
				control.RemoveCommand = e.NewValue as ICommand;
			}
		}

		public ICommand RemoveCommand
		{
			get { return GetValue(RemoveCommandProperty) as ICommand; }
			set { SetValue(RemoveCommandProperty, value); }
		}

		#endregion
	}
}
