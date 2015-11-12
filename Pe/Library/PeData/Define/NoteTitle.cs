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
namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ノートの通常タイトル文字列。
    /// </summary>
    public enum NoteTitle
    {
        /// <summary>
        /// タイムスタンプを使用。
        /// </summary>
        Timestamp,
        /// <summary>
        /// デフォルトタイトルを使用。
        /// <para>言語設定に依存。</para>
        /// </summary>
        DefaultCaption
    }
}
