using System;
using System.Globalization;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
    /// <summary>
    /// <see cref="DateTime"/> 変換処理。
    /// </summary>
    public class CommandLineDateTimeConvertHandler: CommandLineConvertHandlerBase<DateTime>
    {
        #region CommandLineConvertHandlerBase

        protected override DateTime ConvertTarget(Type targetType, string? value)
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
}
