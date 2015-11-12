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
namespace ContentTypeTextNet.Pe.PeMain.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Pe.Library.PeData.Item;

    public class WindowSaveData: ItemModelBase
    {
        public WindowSaveData()
        {
            TimerItems = new FixedSizeCollectionModel<WindowItemCollectionModel>();
            SystemItems = new FixedSizeCollectionModel<WindowItemCollectionModel>();
        }

        #region property

        public WindowItemCollectionModel TemporaryItem { get; set; }

        /// <summary>
        /// タイマー保存。
        /// </summary>
        public FixedSizeCollectionModel<WindowItemCollectionModel> TimerItems { get; private set; }
        /// <summary>
        /// 環境変更時の保存。
        /// </summary>
        public FixedSizeCollectionModel<WindowItemCollectionModel> SystemItems { get; private set; }

        #endregion
    }
}
