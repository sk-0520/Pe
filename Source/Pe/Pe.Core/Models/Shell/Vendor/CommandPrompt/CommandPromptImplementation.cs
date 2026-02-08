using System;
using System.Linq;

namespace ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt
{
    public class CommandPromptImplementation: ShellImplementationBase<CommandPromptOptions>
    {
        #region ShellImplementationBase

        public override string EscapeValue(string value)
        {
            var needQuotation = value == string.Empty;
            var needHatEscape = false;

            needQuotation |= value.Contains(' ');
            needQuotation |= value.Contains('%');

            // % はむり
            needHatEscape |= value.Contains('^');
            needHatEscape |= value.Contains('<');
            needHatEscape |= value.Contains('>');

            if(needHatEscape) {
                value = value
                    .Replace("^", "^^")
                    .Replace("<", "^<")
                    .Replace(">", "^>")
                ;
            }

            if(needQuotation) {
                return '"' + value + '"';
            }

            return value;
        }

        public override string ToSafeVariableName(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(nameof(name));
            }

            var buffer = name.Trim().ToArray();
            for(var i = 0; i < buffer.Length; i++) {
                var c = buffer[i];
                var isSafe = ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || ('0' <= c && c <= '9') || c == '_';
                if(!isSafe) {
                    buffer[i] = '_';
                }
            }

            return new string(buffer);
        }

        #endregion
    }
}
