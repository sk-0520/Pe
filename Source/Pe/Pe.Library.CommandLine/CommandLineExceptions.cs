using System;
using System.Collections.Generic;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Throws;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GeneratedExceptionAttribute: System.Attribute
{
    public GeneratedExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    [GeneratedException]
    public partial class CommandLineException: System.Exception
    { }

    [GeneratedException]
    public partial class CommandLineInvalidKeyException: CommandLineException
    { }

    [GeneratedException]
    public partial class CommandLineRequiredSwitchException: CommandLineException
    { }

    public partial class CommandLineDuplicateKeyException: CommandLineException
    {
        public CommandLineDuplicateKeyException(string key)
            : base($"key duplicate: {key}")
        {
            Key = key;
        }

        #region property

        public string Key { get; }

        #endregion
    }

    [GeneratedException]
    public partial class CommandLineParseException: CommandLineException
    { }

    public partial class CommandLineRequiredException: CommandLineParseException
    {
        public CommandLineRequiredException(IReadOnlyCollection<string> keys)
            : base($"required: {string.Join(", ", keys)}")
        {
            Keys = keys;
        }

        #region property

        public IReadOnlyCollection<string> Keys { get; }

        #endregion
    }

    [GeneratedException]
    public partial class CommandLineConverterException: CommandLineException
    { }

    public partial class CommandLineTypeConvertException: CommandLineConverterException
    {
        public CommandLineTypeConvertException(Type type)
            : base($"type convert error: value='{type}'")
        {
            Type = type;
        }
        #region property

        public Type Type { get; }

        #endregion
    }

}
