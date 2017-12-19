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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    [DataContract, Serializable]
    public abstract class GuidModelBase: ModelBase, ITId<Guid>, IDeepClone
    {
        #region define

        //string pattern = @"[]";

        #endregion

        public GuidModelBase()
            : base()
        {
            Id = Guid.NewGuid();
        }

        #region ITId

        [DataMember, XmlAttribute, IsDeepClone]
        public Guid Id { get; set; }

        public bool IsSafeId(Guid id)
        {
            return true;
        }

        public Guid ToSafeId(Guid id)
        {
            return id;
        }

        #endregion

        #region IDeepClone

        //public virtual void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (GuidModelBase)target;

        //    obj.Id = Id;
        //}

        public abstract IDeepClone DeepClone();

        #endregion
    }
}
