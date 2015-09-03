﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Attached;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Control;

	public static class LanguageUtility
	{
		static bool SetUI_Impl(DependencyObject baseElement, LanguageManager language, IReadOnlyDictionary<string, string> map, Action<string, string> action)
		{
			var key = Language.GetWord(baseElement);
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
				}
				if(!string.IsNullOrEmpty(hint)) {
					ui.ToolTip = language[hint, map];
				}
			});
		}
		public static bool SetContent(ContentPresenter ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(ui.Content is string) {
					ui.Content = language[key, map];
				}
				if(!string.IsNullOrEmpty(hint)) {
					ui.ToolTip = language[hint, map];
				}
			});
		}
		

		public static bool SetText(TextBlock ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				ui.Text = language[key, map];
				if(!string.IsNullOrEmpty(hint)) {
					ui.ToolTip = language[hint, map];
				}
			});
		}

		public static bool SetHeader(HeaderedItemsControl ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(!ui.HasHeader || ui.Header is string) {
					ui.Header = language[key, map];
				}
				if(!string.IsNullOrEmpty(hint)) {
					ui.ToolTip = language[hint, map];
				}
			});
		}
		public static bool SetHeader(HeaderedContentControl ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			return SetUI_Impl(ui, language, map, (key, hint) => {
				if(!ui.HasHeader || ui.Header is string) {
					ui.Header = language[key, map];
				}
				if(!string.IsNullOrEmpty(hint)) {
					ui.ToolTip = language[hint, map];
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
					if(!string.IsNullOrEmpty(hint)) {
						ui.ToolTip = language[hint, map];
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
#if !true
#if DEBUG
			var s = Language.GetWord(control);
			if(s != null) {
				Debug.WriteLine(s);
			}
#endif
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

		/// <summary>
		/// 指定要素から再帰的に言語設定。
		/// </summary>
		/// <param name="root"></param>
		/// <param name="language"></param>
		/// <param name="map"></param>
		public static void RecursiveSetLanguage(DependencyObject root, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			var window = root as Window;
			if(window != null) {
				SetTitle(window, language, map);
				//window.ApplyTemplate();
			}

			var processedElements = new HashSet<DependencyObject>();
			
			foreach(var dependencyObject in UIUtility.FindChildren<DependencyObject>(root)) {
				if(SetLanguageItem(dependencyObject, language, map)) {
					processedElements.Add(dependencyObject);
				}

				var dataGrid = dependencyObject as AppDataGrid;
				if(dataGrid != null) {
					// 素直にリソース使えばよかった
					EventDisposer<EventHandler> eventDisposer = null;
					dataGrid.Rendered += EventUtility.Create<EventHandler>(
						(sender, e) => {
							RecursiveSetLanguage(dataGrid, language, map);
							eventDisposer.Dispose();
							eventDisposer = null;
						},
						releaseEvent => {
							dataGrid.Rendered -= releaseEvent;
							dataGrid = null;
							language = null;
							map = null;
						},
						out eventDisposer
					);
				}

				if(dependencyObject is Visual) {
					foreach(var visualElement in UIUtility.FindVisualChildren<Visual>(dependencyObject)) {
						if(processedElements.Add(visualElement)) {
							SetLanguageItem(visualElement, language, map);
						}
					}
				}
			}

		}

		static string GetEnumKeyName(Type type, object value)
		{
			return "enum/" + type.Name + "/" + value.ToString();
		}

		static string GetTextFromDockType(DockType value, ILanguage language)
		{
			var key = GetEnumKeyName(value.GetType(), value);
			return language[key];
		}

		static string GetTextFromIconScale(IconScale value, ILanguage language)
		{
			var key = GetEnumKeyName(value.GetType(), value);
			return language[key];
		}

		static string GetTextFromClipboardType(ClipboardType value, ILanguage language)
		{
			var key = GetEnumKeyName(value.GetType(), value);
			return language[key];
		}


		public static string GetTextFromEnum<TEnum>(TEnum value, ILanguage language)
		{
			return GetTextFromEnum(typeof(TEnum), value, language);
		}
		public static string GetTextFromEnum(Type type, object value, ILanguage language)
		{
			var map = new Dictionary<Type, Func<string>>() {
				{ typeof(DockType), () => GetTextFromDockType((DockType)value, language) },
				{ typeof(IconScale), () => GetTextFromIconScale((IconScale)value, language) },
				{ typeof(ClipboardType), () => GetTextFromClipboardType((ClipboardType)value, language) },
			};

			Func<string> getText;
			if(map.TryGetValue(type, out getText)) {
				return getText();
			} else {
				return string.Format("####{0}####", value);
			}
		}

		public static string GetTextFromSingleKey(Key value, ILanguage language)
		{
			if((Key.A <= value && value <= Key.Z) || (Key.F1 <= value && value <= Key.F24)) {
				return value.ToString();
			}
			
			var key = GetEnumKeyName(value.GetType(), value);
			return language[key];
		}

		public static string GetTextFromSingleModifierKey(ModifierKeys value, ILanguage language)
		{
			var key = GetEnumKeyName(value.GetType(), value);
			return language[key];
		}

		public static string GetKeySeparatorText(ILanguage language)
		{
			var key = "enum/Key/separator";
			return language[key];
		}

		public static string GetTextFromHotKeyModel(HotKeyModel hotkey, ILanguage language)
		{
			var buffer = new StringBuilder();

			var mk = new[] {
				ModifierKeys.Alt,
				ModifierKeys.Control,
				ModifierKeys.Shift,
				ModifierKeys.Windows,
			};
			var modText = mk
				.Where(m => hotkey.ModifierKeys.HasFlag(m))
				.Select(m => GetTextFromSingleModifierKey(m, language))
			;
			if(modText.Any()) {
				buffer.Append(string.Join(GetKeySeparatorText(language), modText));
				if(hotkey.Key != Key.None) {
					buffer.Append(GetKeySeparatorText(language));
				}
			}
			buffer.Append(GetTextFromSingleKey(hotkey.Key, language));

			return buffer.ToString();
		}

		public static string GetMenuTextFromHotKeyModel(HotKeyModel hotkey, ILanguage language)
		{
			if(hotkey.Key == Key.None) {
				return string.Empty;
			}

			return GetTextFromHotKeyModel(hotkey, language);
		}
	}
}
