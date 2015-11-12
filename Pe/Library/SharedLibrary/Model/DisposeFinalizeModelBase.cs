﻿/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using ContentTypeTextNet.Library.SharedLibrary.IF;

    [Serializable]
    public abstract class DisposeFinalizeModelBase: ModelBase, IIsDisposed
    {
        protected DisposeFinalizeModelBase()
        {
            IsDisposed = false;
        }

        ~DisposeFinalizeModelBase()
        {
            Dispose(false);
        }

        #region IIsDisposed

        [field: NonSerialized]
        public event EventHandler Disposing;

        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; protected set; }

        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
