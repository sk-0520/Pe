using System;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    public static class CommandLineParserExtensions
    {
        #region function

        /// <inheritdoc cref="CommandLineParser.Add(CommandLineOption)"/>
        public static CommandLineOption Add(this CommandLineParser parser, string key, CommandLineOptionKind kind, string description)
        {
            return parser.Add(new CommandLineOption(key, kind, description));
        }

        /// <inheritdoc cref="CommandLineParser.Add(CommandLineOption)"/>
        public static CommandLineOption Add(this CommandLineParser parser, string key, CommandLineOptionKind kind) => Add(parser, key, kind, string.Empty);

        private static string ToUsageCore(CommandLineHelper helper, CommandLineOption option)
        {
            var builder = new StringBuilder();

            var notRequired = !option.Required || option.Kind == CommandLineOptionKind.Switch;

            if(notRequired) {
                builder.Append('[');
            }

            builder.Append(helper.OptionPrefix);
            builder.Append(option.Key);

            if(option.Kind == CommandLineOptionKind.Value) {
                builder.Append(helper.Separator);
                builder.Append('<');
                if(string.IsNullOrWhiteSpace(option.ValueName)) {
                    builder.Append("value");
                } else {
                    builder.Append(option.ValueName);
                }
                builder.Append('>');
            }

            if(notRequired) {
                builder.Append(']');
            }

            if(!string.IsNullOrWhiteSpace(option.Description)) {
                builder.AppendLine();
                builder.Append(helper.DescriptionIndent);
                builder.Append(option.Description);
            }

            return builder.ToString();
        }

        public static string ToUsage(this CommandLineParser parser)
        {
            var items = parser.Options
                .OrderBy(a => a.Value.Required ? 0 : 1)
                .ThenBy(a => a.Value.Kind == CommandLineOptionKind.Value ? 0 : 1)
                .ThenBy(a => a.Key)
                .Select(a => ToUsageCore(parser.Helper, a.Value))
            ;

            return string.Join(Environment.NewLine, items);
        }

        #endregion
    }
}
