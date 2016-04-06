﻿/*
This file is part of Updater.

Updater is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Updater is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Updater.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.SystemApplications.Updater
{
    /// <summary>
    /// 引数を型に落とし込んだデータ保持クラス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ArgumentValue<T>
    {
        /// <summary>
        /// 引数指定されたか。
        /// </summary>
        public bool HasValue { get; set; }
        /// <summary>
        /// データ。
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 取り込み処理。
        /// </summary>
        /// <param name="s"></param>
        public void Import(string s)
        {
            Data = (T)Convert.ChangeType(s, typeof(T));
        }
    }
}
