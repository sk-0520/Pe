using System;
using System.Collections.Generic;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Throws;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GenerateExceptionAttribute: System.Attribute
{
    public GenerateExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    [GenerateException]
    public partial class CommandLineException: System.Exception
    { }

    [GenerateException]
    public partial class CommandLineInvalidKeyException: CommandLineException
    { }

    [GenerateException]
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

    [GenerateException]
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

    [GenerateException]
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
