using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ContentTypeTextNet.Pe.Library.CommandLine
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
        public abstract TTarget ConvertTarget(Type targetType, string? value);

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
        public virtual object? ConvertCore(Type targetType, string? value)
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

    /// <summary>
    /// <see langword="string"/> 変換処理。
    /// </summary>
    public class CommandLineStringConvertHandler: CommandLineConvertHandlerBase<string>
    {
        #region CommandLineConvertHandlerBase

        public override bool IsSupportType(Type targetType)
        {
            return typeof(string) == targetType;
        }

        public override string ConvertTarget(Type targetType, string? value)
        {
            return value ?? string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// <see langword="int"/> 変換処理。
    /// </summary>
    public class CommandLineIntegerConvertHandler: CommandLineConvertHandlerBase<int>
    {
        #region function

        private sbyte ConvertInt8(Type targetType, ReadOnlySpan<char> value)
        {
            return sbyte.Parse(value);
        }

        private short ConvertInt16(Type targetType, ReadOnlySpan<char> value)
        {
            return short.Parse(value);
        }

        private int ConvertInt32(Type targetType, ReadOnlySpan<char> value)
        {
            return int.Parse(value);
        }


        #endregion

        #region CommandLineConvertHandlerBase

        public override bool IsSupportType(Type targetType)
        {
            return
                typeof(int) == targetType
                ||
                typeof(short) == targetType
                ||
                typeof(sbyte) == targetType
            ;
        }

        public override int ConvertTarget(Type targetType, string? value)
        {
            // ConvertCore で対応する
            throw new NotImplementedException();
        }

        public override object? ConvertCore(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return 0;
            }

            var valueSpan = value.AsSpan().Trim();

            if(targetType == typeof(int)) {
                return ConvertInt32(targetType, valueSpan);
            }
            if(targetType == typeof(short)) {
                return ConvertInt16(targetType, valueSpan);
            }
            if(targetType == typeof(sbyte)) {
                return ConvertInt8(targetType, valueSpan);
            }

            throw new NotSupportedException(targetType.FullName);
        }

        #endregion
    }

    /// <summary>
    /// <see langword="bool"/> 変換処理。
    /// </summary>
    public class CommandLineBooleanConvertHandler: CommandLineConvertHandlerBase<bool>
    {
        #region CommandLineConvertHandlerBase

        public override bool ConvertTarget(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return false;
            }

            return bool.Parse(value.Trim());
        }

        #endregion
    }

    /// <summary>
    /// <see cref="DateTime"/> 変換処理。
    /// </summary>
    public class CommandLineDateTimeConvertHandler: CommandLineConvertHandlerBase<DateTime>
    {
        #region CommandLineConvertHandlerBase

        public override DateTime ConvertTarget(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return DateTime.MinValue;
            }

            var result = DateTime.Parse(value.Trim(), CultureInfo.InvariantCulture);
            if(result.Kind == DateTimeKind.Unspecified) {
                result = DateTime.SpecifyKind(result, DateTimeKind.Utc);
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    /// <see cref="TimeSpan"/> 変換処理。
    /// </summary>
    public class CommandLineTimeSpanConvertHandler: CommandLineConvertHandlerBase<TimeSpan>
    {
        #region CommandLineConvertHandlerBase

        public override TimeSpan ConvertTarget(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return TimeSpan.Zero;
            }

            return TimeSpan.Parse(value.Trim(), CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
