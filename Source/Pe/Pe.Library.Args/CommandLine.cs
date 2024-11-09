using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <para>短いキーをいっぱいくっつけてどうとかはできない。</para>
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
        /// プログラム/コマンド名。
        /// </summary>
        /// <remarks>
        /// <para><see langword="null" />が入ることはない。</para>
        /// </remarks>
        public string CommandName { get; }
        /// <summary>
        /// プログラム名を含まないコマンドライン引数。
        /// </summary>
        public IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// 解析が完了したか。
        /// </summary>
        public bool IsParsed { get; private set; }

        /// <summary>
        /// キーアイテム一覧。
        /// </summary>
        private List<CommandLineKey> KeyItems { get; } = new List<CommandLineKey>();
        /// <summary>
        /// キーアイテム一覧。
        /// </summary>
        public IReadOnlyList<CommandLineKey> Keys => KeyItems;

        /// <summary>
        /// 値一覧実体。
        /// </summary>
        private Dictionary<CommandLineKey, ICommandLineValue> ValueItems { get; } = new Dictionary<CommandLineKey, ICommandLineValue>();
        /// <summary>
        /// 値一覧。
        /// </summary>
        public IReadOnlyDictionary<CommandLineKey, ICommandLineValue> Values => ValueItems;

        /// <summary>
        /// スイッチ一覧実体。
        /// </summary>
        private HashSet<CommandLineKey> SwitchItems { get; } = new HashSet<CommandLineKey>();
        /// <summary>
        /// スイッチ一覧。
        /// </summary>
        public IReadOnlyCollection<CommandLineKey> Switches => SwitchItems;

        /// <summary>
        /// 不明アイテム一覧実体。
        /// </summary>
        private List<string> UnknownItems { get; } = new List<string>();
        /// <summary>
        /// 不明アイテム一覧。
        /// </summary>
        public IReadOnlyList<string> Unknowns => UnknownItems;

        /// <summary>
        /// 解析時例外。
        /// </summary>
        public Exception? ParseException { get; private set; }

        #endregion

        #region function

        private CommandLineKey AddCore(CommandLineKey key)
        {
            KeyItems.Add(key);
            return key;
        }

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
                throw new ArgumentException($"{nameof(key.IsEnabledLongKey)} and {nameof(key.LongKey)}.{key.LongKey.Length} == 1", nameof(key));
            }

            if(KeyItems.Where(k => k.IsEnabledLongKey).Any(k => k.LongKey == key.LongKey)) {
                throw new ArgumentException($"exists {nameof(key.LongKey)}: {key.LongKey}");
            }

            return AddCore(key);
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

        string StripDoubleQuotes(string s)
        {
            if(s.Length > "\"\"".Length && s[0] == '"' && s[^1] == '"') {
                return s.Substring(1, s.Length - 1 - 1);
            }

            return s;
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
                    new { Key = "/",  IsLong = true },
                };

                for(var i = 0; i < Arguments.Count; i++) {
                    var argument = Arguments[i];
                    var arg = StripDoubleQuotes(argument);
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
                                SetValue(key, StripDoubleQuotes(val));
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

        /// <summary>
        /// 長いキーから値取得。
        /// </summary>
        /// <param name="longKey">長いキー。</param>
        /// <returns>取得した値。取得できない場合は<see langword="null" />。</returns>
        public CommandLineKey? GetKey(string longKey)
        {
            return KeyItems
                .Concat(SwitchItems)
                .Where(k => k.IsEnabledLongKey)
                .FirstOrDefault(k => k.LongKey == longKey)
            ;
        }

        /// <summary>
        /// 文字列をコマンド実行可能な書式に変換する。
        /// </summary>
        /// <param name="input">対象文字列。</param>
        /// <returns></returns>
        public static string Escape(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) {
                return string.Empty;
            }

            var s = input.Trim();
            var result = s.Replace("\"", "\"\"");
            if(s.IndexOf(' ') == -1) {
                return result;
            } else {
                return "\"" + result + "\"";
            }
        }

        /// <summary>
        /// <see cref="IDictionary{TKey, TValue}"/>をいい感じにつなげる。
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToCommandLineArguments(IReadOnlyDictionary<string, string> map, string header = "--", char separator = '=')
        {
            return map.Select(i => header + i.Key + separator + Escape(i.Value));
        }

        #endregion
    }
}
