using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Common
{
    public static class SizeConverterExtensions
    {
        #region function

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public static string ConvertHumanReadableByte(this SizeConverter sizeConverter, long byteSize, IReadOnlyList<string> units)
        {
            return sizeConverter.ConvertHumanReadableByte(byteSize, "{0:0.00} {1}", units);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public static string ConvertHumanReadableByte(this SizeConverter sizeConverter, long byteSize, string sizeFormat)
        {
            return sizeConverter.ConvertHumanReadableByte(byteSize, sizeFormat, sizeConverter.Units);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public static string ConvertHumanReadableByte(this SizeConverter sizeConverter, long byteSize)
        {
            return sizeConverter.ConvertHumanReadableByte(byteSize, sizeConverter.Units);
        }

        #endregion
    }
}
