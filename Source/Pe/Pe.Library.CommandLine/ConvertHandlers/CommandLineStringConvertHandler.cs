using System;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
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

        protected override string ConvertTarget(Type targetType, string? value)
        {
            return value ?? string.Empty;
        }

        #endregion
    }

}
