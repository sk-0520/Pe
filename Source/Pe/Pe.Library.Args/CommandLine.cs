using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item><c>--key value</c></item>
    ///     <item><c>--key=value</c></item>
    ///     <item><c>--switch</c></item>
    /// </list>
    /// </remarks>
    public class CommandLine
    {
        /// <summary>
        /// アプリ状態から生成。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Environment.GetCommandLineArgs()"/>からコマンドライン分解。</para>
        /// <para><see cref="CommandName"/>を含む。</para>
        /// </remarks>
        public CommandLine()
            : this(Environment.GetCommandLineArgs(), true)
        { }

        /// <summary>
        /// 指定引数から生成。
        /// </summary>
        /// <param name="arguments">コマンドライン引数。</param>
        /// <param name="withCommand"><paramref name="arguments"/>の先頭はプログラム/コマンドか。<para>Main関数だと含まれず、<see cref="Environment.GetCommandLineArgs()"/>だと含まれてる的な。</para></param>
        public CommandLine(IEnumerable<string> arguments, bool withCommand = false)
        {
            if(withCommand) {
                CommandName = arguments.FirstOrDefault() ?? string.Empty;
                Arguments = arguments.Skip(1).ToList();
            } else {
                CommandName = string.Empty;
                Arguments = arguments.ToList();
            }
        }

        #region property

        /// <summary>
        /// キーアイテム一覧実体。
        /// </summary>
        private List<CommandLineKey> KeyItems { get; } = new List<CommandLineKey>();

        /// <summary>
        /// 値データ一覧実体。
        /// </summary>
        private Dictionary<CommandLineKey, ICommandLineValue> ValueItems { get; } = new Dictionary<CommandLineKey, ICommandLineValue>();

        /// <summary>
        /// スイッチデータ一覧実体。
        /// </summary>
        private HashSet<CommandLineKey> SwitchItems { get; } = new HashSet<CommandLineKey>();

        /// <summary>
        /// 不明データ一覧実体。
        /// </summary>
        private List<string> UnknownItems { get; } = new List<string>();

        #endregion

        #region function

        /// <summary>
        /// コマンドラインキーの追加。
        /// </summary>
        /// <param name="key">キー情報。</param>
        /// <returns>追加したキー。</returns>
        public CommandLineKey Add(CommandLineKey key)
        {
            if(key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            if(!key.IsEnabledLongKey) {
                throw new ArgumentException($"{nameof(key.LongKey)} is empty");
            }

            if(key.IsEnabledLongKey && key.LongKey.Length == 1) {
                throw new ArgumentException($"{nameof(key.IsEnabledLongKey)} and {nameof(key.LongKey)}.{nameof(key.LongKey.Length)} == 1", nameof(key));
            }

            if(KeyItems.Where(k => k.IsEnabledLongKey).Any(k => k.LongKey == key.LongKey)) {
                throw new ArgumentException($"exists {nameof(key.LongKey)}: {key.LongKey}");
            }

            KeyItems.Add(key);
            return key;
        }

        /// <summary>
        /// コマンドラインキーの追加。
        /// </summary>
        /// <param name="longKey">長いキー。</param>
        /// <param name="hasValue">値を持つか。</param>
        /// <param name="description">説明。</param>
        /// <returns>追加したキー。</returns>
        public CommandLineKey Add(string longKey = "", bool hasValue = false, string description = "")
        {
            var value = new CommandLineKey(longKey, hasValue, description);
            return Add(value);
        }

        private CommandLineKey? GetCommandLineKey(string key)
        {
            Debug.Assert(key.Length != 0);

            return KeyItems.Find(k => k.IsEnabledLongKey && k.LongKey == key);
        }

        private void SetValue(CommandLineKey key, string value)
        {
            if(ValueItems.TryGetValue(key, out var val)) {
                ((CommandLineValue)val).Add(value);
            } else {
                var commandLineValue = new CommandLineValue();
                commandLineValue.Add(value);
                ValueItems.Add(key, commandLineValue);
            }
        }

        private void SetSwitch(CommandLineKey value)
        {
            SwitchItems.Add(value);
        }

        private void SetUnknown(string value)
        {
            UnknownItems.Add(value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S127:\"for\" loop stop conditions should be invariant", Justification = "<保留中>")]
        private bool ParseCore()
        {
            try {
                var map = new[] {
                    new { Key = "--", IsLong = true },
                    //new { Key = "-",  IsLong = false },
                    //new { Key = "/",  IsLong = true },
                };

                for(var i = 0; i < Arguments.Count; i++) {
                    var argument = Arguments[i];
                    var arg = CommandLineHelper.StripDoubleQuotes(argument);
                    if(string.IsNullOrWhiteSpace(arg)) {
                        continue;
                    }

                    var pair = Array.Find(map, i => arg.StartsWith(i.Key));
                    if(pair != null) {
                        var separatorIndex = arg.IndexOf('=');
                        if(separatorIndex == -1) {
                            var key = GetCommandLineKey(arg.Substring(pair.Key.Length));
                            if(key == null) {
                                SetUnknown(arg);
                                continue;
                            }
                            if(key.HasValue) {
                                if(i < Arguments.Count - 1) {
                                    SetValue(key, Arguments[i + 1]);
                                    i += 1;
                                    continue;
                                } else {
                                    SetValue(key, string.Empty);
                                    continue;
                                }
                            } else {
                                SetSwitch(key);
                                continue;
                            }
                        } else {
                            var key = GetCommandLineKey(arg.Substring(pair.Key.Length, separatorIndex - pair.Key.Length));
                            if(key == null) {
                                SetUnknown(arg);
                                continue;
                            }
                            if(key.HasValue) {
                                var val = arg.Substring(separatorIndex + 1);
                                SetValue(key, CommandLineHelper.StripDoubleQuotes(val));
                                continue;
                            } else {
                                SetSwitch(key);
                                continue;
                            }
                        }
                    } else {
                        SetUnknown(arg);
                    }
                }
                return true;
            } catch(Exception ex) {
                ParseException = ex;
                return false;
            }
        }

        /// <summary>
        /// 解析処理実行。
        /// </summary>
        /// <returns></returns>
        public bool Parse()
        {
            if(IsParsed) {
                throw new InvalidOperationException();
            }

            var result = ParseCore();
            IsParsed = true;

            return result;
        }

        #endregion

        #region ICommandLine

        /// <inheritdoc cref="ICommandLine.CommandName"/>
        public string CommandName { get; }
        /// <inheritdoc cref="ICommandLine.Arguments"/>
        public IReadOnlyList<string> Arguments { get; }

        /// <inheritdoc cref="ICommandLine.IsParsed"/>
        public bool IsParsed { get; private set; }

        /// <inheritdoc cref="ICommandLine.Keys"/>
        public IReadOnlyList<CommandLineKey> Keys => KeyItems;

        /// <inheritdoc cref="ICommandLine.Values"/>
        public IReadOnlyDictionary<CommandLineKey, ICommandLineValue> Values => ValueItems;

        /// <inheritdoc cref="ICommandLine.Switches"/>
        public IReadOnlySet<CommandLineKey> Switches => SwitchItems;

        /// <inheritdoc cref="ICommandLine.Unknowns"/>
        public IReadOnlyList<string> Unknowns => UnknownItems;

        /// <inheritdoc cref="ICommandLine.ParseException"/>
        public Exception? ParseException { get; private set; }

        /// <inheritdoc cref="ICommandLine.TryGetKey(string, out CommandLineKey?)"/>
        public bool TryGetKey(string longKey, [NotNullWhen(true)] out CommandLineKey? result)
        {
            var key = KeyItems
                .Concat(SwitchItems)
                .Where(k => k.IsEnabledLongKey)
                .FirstOrDefault(k => k.LongKey == longKey)
            ;
            if(key is null) {
                result = null;
                return false;
            }

            result = key;
            return true;
        }

        /// <inheritdoc cref="ICommandLine.GetKey(string)"/>
        public CommandLineKey? GetKey(string longKey)
        {
            if(TryGetKey(longKey, out var result)) {
                return result;
            }

            throw new InvalidOperationException();
        }

        #endregion
    }
}
