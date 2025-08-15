using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Common
{
    public static class MathUtility
    {
        #region function

        /// <summary>
        /// 浮動小数点数(<see langword="float" />)がほぼ一緒か。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool AlmostEquals(float a, float b, float epsilon)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        /// <inheritdoc cref="AlmostEquals(float, float, float)"/>/>
        public static bool AlmostEquals(float a, float b) => AlmostEquals(a, b, float.Epsilon);

        /// <summary>
        /// 浮動小数点数(<see langword="double" />)がほぼ一緒か。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool AlmostEquals(double a, double b, double epsilon)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        /// <inheritdoc cref="AlmostEquals(double, double, double)"/>/>
        public static bool AlmostEquals(double a, double b) => AlmostEquals(a, b, double.Epsilon);

        #endregion
    }
}
