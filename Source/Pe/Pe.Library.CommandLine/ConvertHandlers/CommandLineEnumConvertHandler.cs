using System;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
    /// <summary>
    /// 列挙型変換処理。
    /// </summary>
    public class CommandLineEnumConvertHandler: CommandLineConvertHandlerBase<Enum>
    {
        #region CommandLineConvertHandlerBase

        public override bool IsSupportType(Type targetType)
        {
            return targetType.IsEnum;
        }

        protected override Enum ConvertTarget(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                throw new CommandLineConverterException();
            }

            return (Enum)Enum.Parse(targetType, value.Trim(), true);
        }

        #endregion
    }
}
