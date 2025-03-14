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
    public class CommandLine: ICommandLine
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
                Arguments = arguments.Skip(1).ToArray();
            } else {
                CommandName = string.Empty;
                Arguments = arguments.ToArray();
            }
        }

        #region property

        /// <summary>
        /// デミリタ。
        /// </summary>
        public string Delimiter { get; } = CommandLineHelper.Delimiter;

        private IReadOnlyDictionary<CommandLineKey, ICommandLineValue>? CachedValues { get; set; }

        /// <summary>
        /// キーアイテム一覧実体。
        /// </summary>
        private List<CommandLineKey> KeyItems { get; } = new List<CommandLineKey>();

        /// <summary>
        /// 値データ一覧実体。
        /// </summary>
        private Dictionary<CommandLineKey, CommandLineValue> ValueItems { get; } = new Dictionary<CommandLineKey, CommandLineValue>();

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
            ArgumentNullException.ThrowIfNull(key);

            if(!key.IsEnabledLongKey) {
                throw new ArgumentException($"{nameof(key.LongKey)} is empty");
            } else if(key.LongKey.Length == 1) {
                throw new ArgumentException($"{nameof(key.LongKey)}.{nameof(key.LongKey.Length)} == 1", nameof(key));
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
        public CommandLineKey Add(string longKey = "", CommandLineKeyKind kind = CommandLineKeyKind.Switch, string description = "")
        {
            var value = new CommandLineKey(longKey, kind, description);
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
                val.Add(value);
            } else {
                var commandLineValue = new CommandLineValue();
                commandLineValue.Add(value);
                ValueItems.Add(key, commandLineValue);
            }
            CachedValues = null;
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
        private void ParseCore()
        {
            for(var i = 0; i < Arguments.Count; i++) {
                var argument = Arguments[i];
                var arg = CommandLineHelper.StripDoubleQuotes(argument);
                if(string.IsNullOrWhiteSpace(arg)) {
                    continue;
                }

                var hasHeader = arg.StartsWith(Delimiter);
                if(!hasHeader) {
                    SetUnknown(arg);
                    continue;
                }

                var separatorIndex = arg.IndexOf('=');
                if(separatorIndex == -1) {
                    var key = GetCommandLineKey(arg.Substring(Delimiter.Length));
                    if(key is null) {
                        SetUnknown(arg);
                        continue;
                    }

                    if(key.kind == CommandLineKeyKind.Value) {
                        if(i < Arguments.Count - 1) {
                            SetValue(key, Arguments[i + 1]);
                            i += 1;
                            continue;
                        }

                        SetValue(key, string.Empty);
                        continue;
                    }

                    SetSwitch(key);
                } else {
                    var key = GetCommandLineKey(arg.Substring(Delimiter.Length, separatorIndex - Delimiter.Length));
                    if(key == null) {
                        SetUnknown(arg);
                        continue;
                    }

                    if(key.kind == CommandLineKeyKind.Value) {
                        var val = arg.Substring(separatorIndex + 1);
                        SetValue(key, CommandLineHelper.StripDoubleQuotes(val));
                        continue;
                    }

                    SetSwitch(key);
                }
            }
        }

        /// <summary>
        /// 解析処理実行。
        /// </summary>
        /// <remarks>これ戻り値を結果にしたいなぁ。</remarks>
        /// <returns></returns>
        [MemberNotNullWhen(true, nameof(ParseException))]
        public bool Parse()
        {
            if(IsParsed) {
                throw new InvalidOperationException();
            }

            bool result = false;
            try {
                ParseCore();
                result = true;
            } catch(Exception ex) {
                ParseException = ex;
            }

            IsParsed = true;

            return result;
        }

        #endregion

        #region ICommandLine

        public string CommandName { get; }

        public IReadOnlyList<string> Arguments { get; }

        public bool IsParsed { get; private set; }

        public IReadOnlyList<CommandLineKey> Keys => KeyItems;

        public IReadOnlyDictionary<CommandLineKey, ICommandLineValue> Values
        {
            get
            {
                if(CachedValues is null) {
                    CachedValues = ValueItems.ToDictionary(k => k.Key, v => (ICommandLineValue)v.Value);
                }

                return CachedValues;
            }
        }

        public IReadOnlySet<CommandLineKey> Switches => SwitchItems;

        public IReadOnlyList<string> Unknowns => UnknownItems;

        public Exception? ParseException { get; private set; }

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
