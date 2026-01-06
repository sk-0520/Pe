using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Args
{
    internal class CommandLineParsing
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="command">コマンド・プログラム名。</param>
        /// <param name="options">オプション一覧。</param>
        public CommandLineParsing(string command, IReadOnlyDictionary<string, CommandLineOption> options)
        {
            Command = command;
            Values = new(options.Count(a => a.Value.Kind == CommandLineOptionKind.Value));
            Switches = new(options.Count(a => a.Value.Kind == CommandLineOptionKind.Switch));
        }

        #region property

        /// <inheritdoc cref="CommandLineParsedResult.Command"/>
        public string Command { get; }

        /// <inheritdoc cref="CommandLineParsedResult.Values"/>
        public Dictionary<string, CommandLineValues> Values { get; }
        /// <inheritdoc cref="CommandLineParsedResult.Switches"/>
        public HashSet<string> Switches { get; }
        /// <inheritdoc cref="CommandLineParsedResult.Unknowns"/>
        public Dictionary<int, string> Unknowns { get; } = new();
        /// <inheritdoc cref="CommandLineParsedResult.Raws"/>
        public List<string> Raws { get; } = new();

        #endregion

        #region function

        /// <summary>
        /// 値の追加。
        /// </summary>
        /// <param name="key">キー。</param>
        /// <param name="value">値。</param>
        public void AddValue(string key, string value)
        {
            if(!Values.TryGetValue(key, out var argValues)) {
                argValues = new CommandLineValues();
                Values.Add(key, argValues);
            }
            argValues.Add(value);
        }

        /// <summary>
        /// スイッチ設定。
        /// </summary>
        /// <param name="key">キー。</param>
        /// <remarks>設定すれば真となる。コマンドライン引数処理として偽の指定は複雑になるため真に設定することを限定する。</remarks>
        public void SetSwitch(string key)
        {
            Switches.Add(key);
        }

        /// <summary>
        /// 不明値の追加。
        /// </summary>
        /// <param name="index">コマンドライン引数のインデックス位置。</param>
        /// <param name="data">値。</param>
        public void AddUnknown(int index, string data)
        {
            Unknowns.Add(index, data);
        }

        /// <summary>
        /// 生値の追加。
        /// </summary>
        /// <param name="data">値。</param>
        /// <remarks>
        /// <para><see cref="CommandLineHelper.OptionPrefix"/>のみ指定されたそれ以降の値を想定。</para>
        /// <para>ただし実装に依存するので詳細は割愛。</para>
        /// </remarks>
        public void AddRaw(string data)
        {
            Raws.Add(data);
        }

        /// <summary>
        /// <see cref="CommandLineParsedResult"/>を生成。
        /// </summary>
        /// <returns></returns>
        public CommandLineParsedResult ToResult()
        {
            return new CommandLineParsedResult(
                Command,
                Values.ToDictionary(a => a.Key, a => (ICommandLineValues)a.Value),
                Switches,
                Unknowns,
                Raws
            );
        }

        #endregion
    }

    /// <summary>
    /// コマンドライン引数パース結果。
    /// </summary>
    /// <param name="Command">コマンド・プログラム名。</param>
    /// <param name="Values">値一覧。</param>
    /// <param name="Switches">スイッチ一覧。</param>
    /// <param name="Unknowns">不明データ一覧。</param>
    /// <param name="Raws">生値一覧。</param>
    public record class CommandLineParsedResult(
        string Command,
        IReadOnlyDictionary<string, ICommandLineValues> Values,
        IReadOnlySet<string> Switches,
        IReadOnlyDictionary<int, string> Unknowns,
        IReadOnlyList<string> Raws
    );
}
