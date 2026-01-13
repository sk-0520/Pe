using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Args
{
    public class CommandLineParser
    {
        public CommandLineParser()
            : this(new CommandLineHelper())
        { }

        public CommandLineParser(CommandLineHelper helper)
        {
            Helper = helper;
        }

        #region property

        public CommandLineHelper Helper { get; }

        /// <summary>
        /// オプション一覧実体。
        /// </summary>
        /// <remarks>本クラス内ではこちらを使用し、外部からは参照用として<see cref="Options"/>を使用する。</remarks>
        /// <seealso cref="Options"/>
        private Dictionary<string, CommandLineOption> OptionItems { get; } = new();
        /// <summary>
        /// オプション一覧。
        /// </summary>
        public IReadOnlyDictionary<string, CommandLineOption> Options => OptionItems;

        #endregion

        #region function

        /// <summary>
        /// オプション追加。
        /// </summary>
        /// <param name="option">オプション。</param>
        /// <returns></returns>
        /// <exception cref="CommandLineRequiredSwitchException"></exception>
        /// <exception cref="CommandLineDuplicateKeyException"></exception>
        protected virtual CommandLineOption AddCore(CommandLineOption option)
        {
            Helper.ThrowIfInvalidKey(option.Key);

            if(option.Kind == CommandLineOptionKind.Switch) {
                if(option.Required) {
                    // スイッチで必須はおかしい
                    throw new CommandLineRequiredSwitchException();
                }
            }

            if(OptionItems.ContainsKey(option.Key)) {
                throw new CommandLineDuplicateKeyException(option.Key);
            }

            OptionItems.Add(option.Key, option);

            return option;
        }

        /// <summary>
        /// オプション追加。
        /// </summary>
        /// <param name="option">オプション</param>
        /// <returns>追加されたオプション。</returns>
        /// <remarks>
        /// <para>追加されたオプションを破棄・変更することは考慮していない。</para>
        /// <para>それらはオプション設定処理を複雑にするため複数の条件が必要ならそれに合わせて <see cref="CommandLineParser"/> を都度生成すること。</para>
        /// </remarks>
        public CommandLineOption Add(CommandLineOption option) => AddCore(option);

        /// <summary>
        /// パース処理実体。
        /// </summary>
        /// <param name="command">コマンド・プログラム名。</param>
        /// <param name="arguments">引数一覧</param>
        /// <param name="result">パース結果。</param>
        /// <param name="exception">失敗時の例外。</param>
        /// <returns>パースに成功したか。</returns>
        protected virtual bool TryParseCore(string command, IReadOnlyList<string> arguments, [NotNullWhen(true)] out CommandLineParsedResult? result, [NotNullWhen(false)] out Exception? exception)
        {
            result = null;

            var parsing = new CommandLineParsing(command, OptionItems);
            var rawMode = false;

            for(var i = 0; i < arguments.Count; i++) {
                var argument = arguments[i];

                // 生値
                if(rawMode) {
                    parsing.AddRaw(argument);
                    continue;
                }

                // 普通の解析

                // オプション開始文字列がない場合は不明データ
                var hasHeader = argument.StartsWith(Helper.OptionPrefix);
                if(!hasHeader) {
                    parsing.AddUnknown(i, argument);
                    continue;
                }

                // オプション開始文字列のみの場合は今後 生値として扱う
                if(argument == Helper.OptionPrefix) {
                    rawMode = true;
                    continue;
                }

                var separatorIndex = argument.IndexOf(Helper.Separator);
                if(separatorIndex == -1) {
                    // セパレータなし

                    var key = argument.Substring(Helper.OptionPrefix.Length);
                    if(!OptionItems.TryGetValue(key, out var foundKey)) {
                        parsing.AddUnknown(i, argument);
                        continue;
                    }

                    if(foundKey.Kind == CommandLineOptionKind.Value) {
                        if(i < arguments.Count - 1) {
                            parsing.AddValue(key, arguments[i + 1]);
                            i += 1;
                            continue;
                        }

                        parsing.AddValue(key, string.Empty);
                        continue;
                    }

                    Debug.Assert(foundKey.Kind == CommandLineOptionKind.Switch);

                    parsing.SetSwitch(key);
                } else {
                    // セパレータあり

                    var argKey = argument.Substring(Helper.OptionPrefix.Length, separatorIndex - Helper.OptionPrefix.Length);
                    if(!OptionItems.TryGetValue(argKey, out var foundOption)) {
                        parsing.AddUnknown(i, argument);
                        continue;
                    }

                    var val = argument.Substring(separatorIndex + 1);
                    parsing.AddValue(foundOption.Key, Helper.StripEnclosing(val));
                    continue;
                }
            }

            // 必須項目チェック
            var requiredKeys = OptionItems
                .Where(a => a.Value.Required)
                .Select(a => a.Key)
                .ToArray()
            ;
            if(0 < requiredKeys.Length) {
                var needRequiredKeys = requiredKeys
                    .Except(parsing.Values.Select(a => a.Key))
                    .Order()
                    .ToArray()
                ;
                if(0 < needRequiredKeys.Length) {
                    exception = new CommandLineRequiredException(needRequiredKeys);
                    return false;
                }
            }

            result = parsing.ToResult();
            exception = null;

            return true;
        }

        /// <summary>
        /// コマンドライン引数をパース。
        /// </summary>
        /// <param name="command">コマンド・プログラム名。</param>
        /// <param name="arguments">引数一覧</param>
        /// <returns>変換結果。</returns>
        /// <remarks>変換失敗の場合は例外が投げられる。投げられる例外は関連項目も参照のこと。</remarks>
        /// <exception cref="CommandLineRequiredException">必須項目が指定されていない。</exception>
        public CommandLineParsedResult Parse(string command, IReadOnlyList<string> arguments)
        {
            if(TryParseCore(command, arguments, out var result, out var exception)) {
                return result;
            }
            throw exception;
        }

        #endregion
    }
}
