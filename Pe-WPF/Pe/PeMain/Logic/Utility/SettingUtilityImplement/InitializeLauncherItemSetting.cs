﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	internal static class InitializeLauncherItemSetting
	{
		public static void Correction(LauncherItemSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
			V_Last(setting, previousVersion, nonProcess);
		}

		static void V_Last(LauncherItemSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{ }

		static void V_First(LauncherItemSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}
		}
	}
}
