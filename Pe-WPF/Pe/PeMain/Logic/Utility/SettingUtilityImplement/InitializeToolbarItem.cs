﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	internal static class InitializeToolbarItem
	{
		public static void Correction(ToolbarItemModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);

			setting.HideWaitTime = Constants.toolbarHideWaitTime.GetClamp(setting.HideWaitTime);
			setting.HideAnimateTime = Constants.toolbarHideAnimateTime.GetClamp(setting.HideAnimateTime);
			setting.Font.Size = Constants.toolbarFontSize.GetClamp(setting.Font.Size);
			setting.IconScale = EnumUtility.GetNormalization(setting.IconScale, IconScale.Normal);

			if(SettingUtility.IsIllegalPlusNumber(setting.FloatToolbar.WidthButtonCount)) {
				setting.FloatToolbar.WidthButtonCount = 1;
			}
			if(SettingUtility.IsIllegalPlusNumber(setting.FloatToolbar.HeightButtonCount)) {
				setting.FloatToolbar.HeightButtonCount = 1;
			}
		}

		static void V_First(ToolbarItemModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			setting.IconScale = IconScale.Normal;
			setting.HideWaitTime = Constants.toolbarHideWaitTime.median;
			setting.HideAnimateTime = Constants.toolbarHideAnimateTime.median;
			setting.Font.Size = Constants.toolbarFontSize.median;
		}
	}
}
