using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// 値変換処理規約。
    /// </summary>
    public interface ICommandLineConvertHandler
    {
        #region property

        /// <summary>
        /// 変換先の値。
        /// </summary>
        /// <remarks>型の昇格などは行わない。</remarks>
        Type Type { get; }

        #endregion

        #region function

        /// <summary>
        /// 変換処理。
        /// </summary>
        /// <param name="value">変換対象文字列。</param>
        /// <returns><see langword="null"/>の場合は変換できなかった扱われる。</returns>
        /// <remarks>
        /// <para>戻り値をわざわざ <see langword="null"/> にせず、例外を投げる方針で良い。</para>
        /// <para>むしろ <see langword="null"/> が特殊処理に近い。</para>
        /// </remarks>
        object? Convert(string? value);

        #endregion
    }

    /// <summary>
    /// 値変換処理 簡易的実装基底。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandLineConvertHandlerBase<T>: ICommandLineConvertHandler
    {
        #region function

        /// <summary>
        /// 値変換処理。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>変換結果。<see langword="null"/>の場合は失敗として扱う。</returns>
        [return: MaybeNull]
        public abstract T ConvertCore(string? value);

        #endregion

        #region ICommandLineConvertHandler

        public Type Type => typeof(T);

        public object? Convert(string? value)
        {
            try {
                return ConvertCore(value);
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
        #region ArgConverterHandlerBase

        public override string ConvertCore(string? value)
        {
            return value ?? string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// <see langword="int"/> 変換処理。
    /// </summary>
    public class CommandLineInt32ConvertHandler: CommandLineConvertHandlerBase<int>
    {
        #region ArgConverterHandlerBase

        public override int ConvertCore(string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return 0;
            }

            return int.Parse(value.Trim());
        }

        #endregion
    }

    /// <summary>
    /// <see langword="bool"/> 変換処理。
    /// </summary>
    public class CommandLineBooleanConvertHandler: CommandLineConvertHandlerBase<bool>
    {
        #region ArgConverterHandlerBase

        public override bool ConvertCore(string? value)
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
        #region ArgConverterHandlerBase

        public override DateTime ConvertCore(string? value)
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
        #region ArgConverterHandlerBase

        public override TimeSpan ConvertCore(string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return TimeSpan.Zero;
            }

            return TimeSpan.Parse(value.Trim(), CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
