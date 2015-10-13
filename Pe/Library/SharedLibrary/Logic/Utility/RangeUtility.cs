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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class RangeUtility
    {
        /// <summary>
        /// min &lt;= value &lt;= max
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between<T>(T value, T min, T max)
            where T : IComparable
        {
            return min.CompareTo(value) <= 0 && 0 <= max.CompareTo(value);
        }

        /// <summary>
        /// 丸め。
        /// <para>valueがmin未満かmaxより多ければminかmaxの適応する方に丸める。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T Clamp<T>(T value, T min, T max)
            where T : IComparable
        {
            if(min.CompareTo(value) > 0) {
                return min;
            } else if(max.CompareTo(value) < 0) {
                return max;
            } else {
                return value;
            }
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static int Increment(int value)
        {
            if(value == int.MaxValue) {
                return value;
            }

            return value + 1;
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static uint Increment(uint value)
        {
            if(value == uint.MaxValue) {
                return value;
            }

            return value + 1;
        }

    }
}
