using System;

namespace ContentTypeTextNet.Pe.Library.Common
{
    public static class MathUtility
    {
        #region function

        /// <summary>
        /// 浮動小数点数(<see langword="float" />)がほぼ一緒か。
        /// </summary>
        /// <param name="a">比較対象1。</param>
        /// <param name="b">比較対象2。</param>
        /// <param name="epsilon">許容誤差。差の絶対値がこの値以下であれば等しいとみなす。</param>
        /// <returns></returns>
        public static bool AlmostEquals(float a, float b, float epsilon)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        /// <inheritdoc cref="AlmostEquals(float, float, float)"/>/>
        /// <seealso cref="float.Epsilon"/>
        public static bool AlmostEquals(float a, float b) => AlmostEquals(a, b, float.Epsilon);

        /// <summary>
        /// 浮動小数点数(<see langword="double" />)がほぼ一緒か。
        /// </summary>
        /// <param name="a"><inheritdoc cref="AlmostEquals(float, float, float)"/></param>
        /// <param name="b"><inheritdoc cref="AlmostEquals(float, float, float)"/></param>
        /// <param name="epsilon"><inheritdoc cref="AlmostEquals(float, float, float)"/></param>
        /// <returns></returns>
        public static bool AlmostEquals(double a, double b, double epsilon)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        /// <inheritdoc cref="AlmostEquals(double, double, double)"/>
        /// <seealso cref="double.Epsilon"/>
        public static bool AlmostEquals(double a, double b) => AlmostEquals(a, b, double.Epsilon);

        #endregion
    }
}
