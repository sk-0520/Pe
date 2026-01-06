using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Args
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

        public static string ToUsage(this CommandLineParser parser)
        {
            var items = parser.Options
                .OrderBy(a => a.Value.Required ? 0 : 1)
                .ThenBy(a => a.Value.Kind == CommandLineOptionKind.Value ? 0 : 1)
                .ThenBy(a => a.Key)
                .Select(a => {
                    var builder = new StringBuilder();

                    var notRequired = !a.Value.Required || a.Value.Kind == CommandLineOptionKind.Switch;

                    if(notRequired) {
                        builder.Append('[');
                    }

                    builder.Append(parser.Helper.OptionPrefix);
                    builder.Append(a.Key);

                    if(a.Value.Kind == CommandLineOptionKind.Value) {
                        builder.Append(parser.Helper.Separator);
                        builder.Append('<');
                        if(string.IsNullOrWhiteSpace(a.Value.ValueName)) {
                            builder.Append("value");
                        } else {
                            builder.Append(a.Value.ValueName);
                        }
                        builder.Append('>');
                    }

                    if(notRequired) {
                        builder.Append(']');
                    }

                    if(!string.IsNullOrWhiteSpace(a.Value.Description)) {
                        builder.AppendLine();
                        builder.Append('\t');
                        builder.Append(a.Value.Description);
                    }

                    return builder.ToString();
                })
            ;
            return string.Join(System.Environment.NewLine, items);
        }

        #endregion
    }
}
