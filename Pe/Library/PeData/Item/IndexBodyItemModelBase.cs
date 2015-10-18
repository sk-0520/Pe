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
namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.IF;

    /// <summary>
    /// インデックスデータのボディ部。
    /// </summary>
    [DataContract, Serializable]
    public abstract class IndexBodyItemModelBase: ItemModelBase, IDeepClone
    {
        public IndexBodyItemModelBase()
            : base()
        {
            History = new HistoryItemModel();
        }

        #region property

        /// <summary>
        /// インデックス種別。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public abstract IndexKind IndexKind { get; }

        /// <summary>
        /// 履歴。
        /// </summary>
        [DataMember]
        public HistoryItemModel History { get; set; }

        #endregion

        #region IDeepClone

        public virtual void DeepCloneTo(IDeepClone target)
        {
            var obj = (IndexBodyItemModelBase)target;

            History.DeepCloneTo(obj.History);
        }

        public abstract IDeepClone DeepClone();

        #endregion
    }
}
