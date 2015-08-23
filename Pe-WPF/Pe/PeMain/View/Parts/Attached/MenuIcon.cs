﻿namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media.Effects;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public static class MenuIcon
	{
		static DropShadowEffect _effect;

		static MenuIcon()
		{
			_effect = new DropShadowEffect() {
				Color = ImageUtility.GetMenuIconColor(false, true),
				ShadowDepth = 0,
				BlurRadius = 6,
			};
			if(_effect.CanFreeze) {
				_effect.Freeze();
			}
		}

		#region IsStrongProperty

		public static readonly DependencyProperty IsStrongProperty = DependencyProperty.RegisterAttached(
			"IsStrong",
			typeof(bool),
			typeof(MenuIcon),
			new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsStrongChanged)
		);

		static void OnIsStrongChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var element = dependencyObject as UIElement;
			if(element != null) {
				SetIsStrong(element, (bool)e.NewValue);
			}
		}

		public static bool GetIsStrong(UIElement element)
		{
			return (bool)element.GetValue(IsStrongProperty);
		}
		public static void SetIsStrong(UIElement element, bool value)
		{
			element.SetValue(IsStrongProperty, value);
			//var img = imageObject as Image;
			if(element != null) {
				if(value) {
					//element.Opacity = checkedIsStrong;
					element.Effect = _effect;
				} else {
					//element.Opacity = uncheckedIsStrong;
					element.Effect = null;
				}
			}
		}

		#endregion
	}
}
