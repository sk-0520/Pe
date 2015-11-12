﻿/**
This file is part of PInvoke.

PInvoke is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PInvoke is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with PInvoke.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Library.PInvoke.Windows
{
    using System;

    public enum ComResult: int
    {
        S_OK = unchecked((int)0x00000000),
        S_FALSE = unchecked((int)0x00000001),
        E_NOINTERFACE = unchecked((int)0x80004002),
        INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011),
    }
}
