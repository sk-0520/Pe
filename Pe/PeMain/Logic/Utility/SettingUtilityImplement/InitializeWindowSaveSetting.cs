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
namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

    internal static class InitializeWindowSaveSetting
    {
        public static void Correction(WindowSaveSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(WindowSaveSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.SaveCount = Constants.windowSaveCount.GetClamp(setting.SaveCount);
            setting.SaveIntervalTime = Constants.windowSaveIntervalTime.GetClamp(setting.SaveIntervalTime);
        }

        static void V_First(WindowSaveSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            setting.IsEnabled = true;
            setting.SaveCount = Constants.windowSaveCount.median;
            setting.SaveIntervalTime = Constants.windowSaveIntervalTime.median;
        }
    }
}
