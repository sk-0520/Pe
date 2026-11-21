using System;
using System.Globalization;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
    /// <summary>
    /// <see cref="TimeSpan"/> 変換処理。
    /// </summary>
    public class CommandLineTimeSpanConvertHandler: CommandLineConvertHandlerBase<TimeSpan>
    {
        #region CommandLineConvertHandlerBase

        protected override TimeSpan ConvertTarget(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return TimeSpan.Zero;
            }

            return TimeSpan.Parse(value.Trim(), CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
