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
namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    public class IconCaching<TChildKey>
    {
        public IconCaching()
        {
            Initialize();
        }

        #region property

        protected Dictionary<IconScale, Caching<TChildKey, BitmapSource>> Cache { get; private set; }

        #endregion

        #region indexer

        public Caching<TChildKey, BitmapSource> this[IconScale key]
        {
            get
            {
                return Cache[key];
            }
        }

        #endregion

        #region function

        protected void Initialize()
        {
            Cache = EnumUtility.GetMembers<IconScale>()
                .ToDictionary(
                    k => k,
                    v => new Caching<TChildKey, BitmapSource>()
                )
            ;
        }

        public void Clear()
        {
            foreach(var value in Cache.Values) {
                value.Clear();
            }

            Cache.Clear();

            Initialize();
        }

        #endregion
    }
}
