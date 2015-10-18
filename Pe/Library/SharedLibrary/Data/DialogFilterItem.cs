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
namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;

    public class DialogFilterItem: IDisplayText
    {
        public DialogFilterItem(string displayText, IEnumerable<string> wildcard)
        {
            DisplayText = displayText;
            Wildcard = new List<string>(wildcard);
        }

        public DialogFilterItem(string displayText, params string[] wildcard)
            : this(displayText, (IEnumerable<string>)wildcard)
        { }

        #region property

        public IReadOnlyList<string> Wildcard { get; private set; }

        #endregion

        #region IDisplayText

        public string DisplayText { get; private set; }

        #endregion

        #region object

        public override string ToString()
        {
            return string.Format("{0}|{1}", DisplayText, string.Join(";", Wildcard));
        }

        #endregion
    }
}
