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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
    /// <summary>
    /// ランチャグループ。
    /// </summary>
    [DataContract, Serializable]
    public class LauncherGroupSettingModel: SettingModelBase, IDeepClone
    {
        public LauncherGroupSettingModel()
            : base()
        {
            Groups = new LauncherGroupItemCollectionModel();
        }

        #region property

        [DataMember]
        public LauncherGroupItemCollectionModel Groups { get; set; }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (LauncherGroupSettingModel)target;

        //    obj.Groups.InitializeRange(Groups.Select(i => (LauncherGroupItemModel)i.DeepClone()));
        //}

        public IDeepClone DeepClone()
        {
            //var result = new LauncherGroupSettingModel();

            //DeepCloneTo(result);

            //return result;
            var result = DeepCloneUtility.Copy(this);

            result.Groups.InitializeRange(Groups.Select(i => (LauncherGroupItemModel)i.DeepClone()));

            return result;
        }

        #endregion
    }
}
