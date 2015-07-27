﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Linq;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Attached;
	using System.Windows.Controls.Primitives;

	public static class LanguageUtility
	{
		static bool SetUI_Impl(DependencyObject baseElement, LanguageManager language, IReadOnlyDictionary<string, string> map, Action<string, string> action)
		{
			var key = Language.GetKey(baseElement);
			var hint = Language.GetHint(baseElement);
			if(!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(hint)) {
				action(key, hint);
				return true;
			} else {
				return false;
			}
		}

		public static bool SetTitle(Window ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => ui.Title = language[key, map]);
		}

		public static bool SetContent(ContentControl ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(!ui.HasContent || ui.Content is string) {
					ui.Content = language[key, map];
					if(string.IsNullOrEmpty(hint)) {
						ui.ToolTip = ui.Content;
					} else {
						ui.ToolTip = language[hint, map];
					}
				}
			});
		}
		public static bool SetContent(ContentPresenter ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(ui.Content is string) {
					ui.Content = language[key, map];
					if(string.IsNullOrEmpty(hint)) {
						ui.ToolTip = ui.Content;
					} else {
						ui.ToolTip = language[hint, map];
					}
				}
			});
		}
		

		public static bool SetText(TextBlock ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				ui.Text = language[key, map];
			});
		}

		public static bool SetHeader(HeaderedItemsControl ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(!ui.HasHeader || ui.Header is string) {
					ui.Header = language[key, map];
				}
			});
		}
		public static bool SetHeader(HeaderedContentControl ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(!ui.HasHeader || ui.Header is string) {
					ui.Header = language[key, map];
				}
			});
		}

		public static bool SetColumn(GridViewColumnHeader ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			if(ui.Column != null) {
				return SetUI_Impl(ui.Column, language, map, (key, hint) => {
					if(!ui.HasContent || ui.Content is string) {
						ui.Content = language[key, map];
					}
				});
			}

			return false;
		}

		/// <summary>
		/// TODO: 後で実装はうまいことする。
		/// </summary>
		/// <param name="control"></param>
		/// <param name="language"></param>
		/// <param name="map"></param>
		static bool SetLanguageItem(DependencyObject control, LanguageManager language, IReadOnlyDictionary<string, string> map)
		{
#if DEBUG
			var s = Language.GetKey(control);
			if(s != null) {
				Debug.WriteLine(s);
			}
#endif
			var gridViewColumnHeader = control as GridViewColumnHeader;
			if(gridViewColumnHeader != null) {
				return SetColumn(gridViewColumnHeader, language, map);
			} else {
				var headeredItemsControl = control as HeaderedItemsControl;
				if(headeredItemsControl != null) {
					return SetHeader(headeredItemsControl, language, map);
				} else {
					var headeredContentControl = control as HeaderedContentControl;
					if(headeredContentControl != null) {
						return SetHeader(headeredContentControl, language, map);
					} else {
						var textBlock = control as TextBlock;
						if(textBlock != null) {
							return SetText(textBlock, language, map);
						} else {
							var contentPresenter = control as ContentPresenter;
							if(contentPresenter != null) {
								return SetContent(contentPresenter, language, map);
							} else {
								var contentControl = control as ContentControl;
								if(contentControl != null) {
									return SetContent(contentControl, language, map);
								}
							}
						}
					}
				}
			}

			return false;
		}

		public static void SetLanguage(DependencyObject root, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			var window = root as Window;
			if(window != null) {
				SetTitle(window, language, map);
			}

			var processedElements = new HashSet<DependencyObject>();

			foreach(var dependencyObject in UIUtility.FindLogicalChildren<DependencyObject>(root).Concat(UIUtility.FindVisualChildren<DependencyObject>(root))) {
				//var type = dependencyObject.GetType();
				//Debug.WriteLine("L: " + type.ToString());
				//Action<DependencyObject> action;
				//if (map.TryGetValue(type, out action)) {
				//	action(dependencyObject);
				//}
				if(SetLanguageItem(dependencyObject, language, map)) {
					processedElements.Add(dependencyObject);
				}

				//var uiElement = dependencyObject as UIElement;
				//if(uiElement != null && uiElement.Visibility != Visibility.Visible) {
				//	uiElement.IsVisibleChanged += EventUtility.Auto<DependencyPropertyChangedEventHandler>((sender, e) => {
				//		SetLanguage((DependencyObject)sender, language, map);
				//	}, releaseEvent => uiElement.IsVisibleChanged -= releaseEvent);
				//}
				var dataGrid = dependencyObject as DataGrid;
				if(dataGrid != null) {
					dataGrid.Loaded += EventUtility.Auto<RoutedEventHandler>((sender, e) => {
						SetLanguage((DependencyObject)sender, language, map);
						//var uiElement = sender as UIElement;
						//if(uiElement.Visibility != Visibility.Visible) {
						//	uiElement.IsVisibleChanged += EventUtility.Auto<DependencyPropertyChangedEventHandler>((sender2, e2) => {
						//		if(uiElement.Visibility == Visibility.Visible) {
						//			SetLanguage((DependencyObject)sender2, language, map);
						//		}
						//	}, releaseEvent => uiElement.IsVisibleChanged -= releaseEvent);
						//}
					}, releaseEvent => dataGrid.Loaded -= releaseEvent);

					//dataGrid.Initialized += EventUtility.Auto<EventHandler>((sender, e) => {
					//	SetLanguage((DependencyObject)sender, language, map);

					//}, releaseEvent => dataGrid.Initialized -= releaseEvent);
				}

				if(dependencyObject is Visual) {
					foreach(var visualElement in UIUtility.FindVisualChildren<Visual>(dependencyObject)) {
						//var visualType = visualElement.GetType();
						//Debug.WriteLine("V: " + visualType.ToString());
						//if (map.TryGetValue(visualType, out action)) {
						//	action(visualElement);
						//}
						//if(visualElement is DataGridColumnHeader) {
						//	Debug.WriteLine("#");
						//}
						if(processedElements.Add(visualElement)) {
							SetLanguageItem(visualElement, language, map);
						}
					}
				}
			}

		}

	}
}
