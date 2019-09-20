﻿/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;

namespace ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend
{
    public interface IWindowHitTestData: IWindowsViewExtendRestrictionViewModelMarker
    {
        /// <summary>
        /// ボーダーに対するヒットテストを行うか
        /// </summary>
        bool UsingBorderHitTest { get; }
        /// <summary>
        /// タイトルバーに対するヒットテストを行うか
        /// </summary>
        bool UsingCaptionHitTest { get; }

        /// <summary>
        /// タイトルバーとして認識される領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect CaptionArea { get; }
        /// <summary>
        /// サイズ変更に使用する境界線。
        /// </summary>
        [PixelKind(Px.Logical)]
        Thickness ResizeThickness { get; }
    }
}
