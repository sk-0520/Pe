﻿/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    internal static class LauncherGroupUtility
    {
        static BitmapSource GetRawGroupIconImage(LauncherGroupIconType groupIconType)
        {
            var bitmapMap = new Dictionary<LauncherGroupIconType, BitmapSource>() {
                { LauncherGroupIconType.Folder, AppResource.ToolbarToolbarGroupFolderImage },
                { LauncherGroupIconType.File, AppResource.ToolbarToolbarGroupFileImage },
                { LauncherGroupIconType.Bookmark,AppResource.ToolbarToolbarGroupBookmarkImage },
                { LauncherGroupIconType.Builder,AppResource.ToolbarToolbarGroupBuilderImage },
                { LauncherGroupIconType.Building,AppResource.ToolbarToolbarGroupBuildingImage },
                { LauncherGroupIconType.Config,AppResource.ToolbarToolbarGroupConfigImage },
                { LauncherGroupIconType.Gear,AppResource.ToolbarToolbarGroupGearImage },
                { LauncherGroupIconType.Library,AppResource.ToolbarToolbarGroupLibraryImage },
                { LauncherGroupIconType.LightBulb,AppResource.ToolbarToolbarGroupLightBulbImage },
                { LauncherGroupIconType.Lock,AppResource.ToolbarToolbarGroupLockImage },
                { LauncherGroupIconType.Server,AppResource.ToolbarToolbarGroupServerImage },
                { LauncherGroupIconType.Shortcut,AppResource.ToolbarToolbarGroupShortcutImage },
                { LauncherGroupIconType.Storage,AppResource.ToolbarToolbarGroupStorageImage },
                { LauncherGroupIconType.User,AppResource.ToolbarToolbarGroupUserImage },
            };

            var bitmap = bitmapMap[groupIconType];
            FreezableUtility.SafeFreeze(bitmap);
            return bitmap;
        }

        public static BitmapSource CreateGroupIconImage(LauncherGroupIconType groupIconType, Color color)
        {
            var rawImage = GetRawGroupIconImage(groupIconType);

            var pixels = MediaUtility.GetPixels(rawImage);
            var pixcelWidth = 4;
            for(var i = 0; i < pixels.Length; i += pixcelWidth) {
                // 0:b 1:g 2:r 3:a
                var b = pixels[i + 0];
                var g = pixels[i + 1];
                var r = pixels[i + 2];
                pixels[i + 0] = (byte)(b + (1 - b / 255.0) * color.B);
                pixels[i + 1] = (byte)(g + (1 - g / 255.0) * color.G);
                pixels[i + 2] = (byte)(r + (1 - r / 255.0) * color.R);
            }

            var result = new WriteableBitmap(rawImage);
            result.Lock();
            result.WritePixels(new Int32Rect(0, 0, rawImage.PixelWidth, rawImage.PixelHeight), pixels, rawImage.PixelWidth * pixcelWidth, 0);
            result.Unlock();

            FreezableUtility.SafeFreeze(result);

            return result;
        }
    }
}