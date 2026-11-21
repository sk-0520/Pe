using System;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
    /// <summary>
    /// 値変換処理 簡易的実装基底。
    /// </summary>
    /// <typeparam name="TTarget">変換先の型。</typeparam>
    public abstract class CommandLineConvertHandlerBase<TTarget>: ICommandLineConvertHandler
    {
        #region function

        /// <summary>
        /// 値変換処理。
        /// </summary>
        /// <param name="targetType">変換先の型。</param>
        /// <param name="value">変換対象文字列。</param>
        /// <returns>変換結果。<see langword="null"/>の場合は失敗として扱う。</returns>
        [return: MaybeNull]
        protected abstract TTarget ConvertTarget(Type targetType, string? value);

        /// <summary>
        /// 値変換処理。
        /// </summary>
        /// <param name="targetType">変換先の型。</param>
        /// <param name="value">変換対象文字列。</param>
        /// <returns>変換結果。<see langword="null"/>の場合は失敗として扱う。</returns>
        /// <remarks>
        /// <para>基本的には <see cref="ConvertTarget(Type, string?)"/> を呼び出すだけ。</para>
        /// <para><typeparamref name="TTarget"/> に変換しない場合等はこのメソッドを <see langword="override"/> して実装する。</para>
        /// </remarks>
        protected virtual object? ConvertCore(Type targetType, string? value)
        {
            return ConvertTarget(targetType, value);
        }

        #endregion

        #region ICommandLineConvertHandler

        public virtual bool IsSupportType(Type targetType)
        {
            return typeof(TTarget) == targetType;
        }

        public object? Convert(Type targetType, string? value)
        {
            try {
                return ConvertCore(targetType, value);
            } catch(Exception ex) when(ex is not CommandLineConverterException) {
                throw new CommandLineConverterException(ex.Message, ex);
            }
        }

        #endregion
    }

}
