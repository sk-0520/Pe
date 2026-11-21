using System;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.CommandLine.ConvertHandlers
{
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

        private byte ConvertUInt8(Type targetType, ReadOnlySpan<char> value)
        {
            return byte.Parse(value);
        }


        private short ConvertInt16(Type targetType, ReadOnlySpan<char> value)
        {
            return short.Parse(value);
        }

        private ushort ConvertUInt16(Type targetType, ReadOnlySpan<char> value)
        {
            return ushort.Parse(value);
        }

        private int ConvertInt32(Type targetType, ReadOnlySpan<char> value)
        {
            return int.Parse(value);
        }

        private uint ConvertUInt32(Type targetType, ReadOnlySpan<char> value)
        {
            return uint.Parse(value);
        }

        #endregion

        #region CommandLineConvertHandlerBase

        public override bool IsSupportType(Type targetType)
        {
            return
                typeof(int) == targetType
                ||
                typeof(uint) == targetType
                ||
                typeof(short) == targetType
                ||
                typeof(ushort) == targetType
                ||
                typeof(sbyte) == targetType
                ||
                typeof(byte) == targetType
            ;
        }

        [ExcludeFromCodeCoverage]
        protected override int ConvertTarget(Type targetType, string? value)
        {
            // ConvertCore で対応する
            throw new NotSupportedException();
        }

        protected override object? ConvertCore(Type targetType, string? value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return 0;
            }

            var valueSpan = value.AsSpan().Trim();

            if(targetType == typeof(int)) {
                return ConvertInt32(targetType, valueSpan);
            }
            if(targetType == typeof(uint)) {
                return ConvertUInt32(targetType, valueSpan);
            }

            if(targetType == typeof(short)) {
                return ConvertInt16(targetType, valueSpan);
            }
            if(targetType == typeof(ushort)) {
                return ConvertUInt16(targetType, valueSpan);
            }

            if(targetType == typeof(sbyte)) {
                return ConvertInt8(targetType, valueSpan);
            }
            if(targetType == typeof(byte)) {
                return ConvertUInt8(targetType, valueSpan);
            }

            throw new NotSupportedException(targetType.FullName);
        }

        #endregion
    }
}
