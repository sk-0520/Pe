using System;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
    /// <summary>
    /// 値変換処理規約。
    /// </summary>
    public interface ICommandLineConvertHandler
    {
        #region function

        /// <summary>
        /// 変換可能か判定。
        /// </summary>
        /// <param name="targetType">変換先の型。</param>
        /// <returns>変換可能か。</returns>
        bool IsSupportType(Type targetType);

        /// <summary>
        /// 変換処理。
        /// </summary>
        /// <param name="targetType">変換先の型。</param>
        /// <param name="value">変換対象文字列。</param>
        /// <returns><see langword="null"/>の場合は変換できなかった扱われる。</returns>
        /// <remarks>
        /// <para>戻り値をわざわざ <see langword="null"/> にせず、例外を投げる方針で良い。</para>
        /// <para>むしろ <see langword="null"/> が特殊処理に近い。</para>
        /// </remarks>
        object? Convert(Type targetType, string? value);

        #endregion
    }
}
