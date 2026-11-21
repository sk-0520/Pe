using System;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
    /// <summary>
    /// <see langword="bool"/> 変換処理。
    /// </summary>
    public class CommandLineBooleanConvertHandler: CommandLineConvertHandlerBase<bool>
    {
        #region CommandLineConvertHandlerBase

        protected override bool ConvertTarget(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return false;
            }

            return bool.Parse(value.Trim());
        }

        #endregion
    }
}
