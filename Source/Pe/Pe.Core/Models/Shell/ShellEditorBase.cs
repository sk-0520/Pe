using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;

namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    /// <summary>
    /// シェル編集基底。
    /// </summary>
    /// <typeparam name="TShellImplementation">実装。</typeparam>
    /// <typeparam name="TShellOptions">オプション。</typeparam>
    public abstract class ShellEditorBase<TShellImplementation, TShellOptions>
        where TShellImplementation : ShellImplementationBase<TShellOptions>
        where TShellOptions : ShellOptions, new()
    {
        #region variable

        /// <inheritdoc cref="Commands"/>
        /// <remarks>実体。</remarks>
        protected List<CommandBase> _commands = new List<CommandBase>();

        #endregion

        #region property

        /// <summary>
        /// 実装。
        /// </summary>
        public abstract TShellImplementation Implementation { get; }

        /// <summary>
        /// オプション。
        /// </summary>
        public TShellOptions Options => Implementation.Options;

        /// <summary>
        /// コマンド一覧。
        /// </summary>
        public IList<CommandBase> Commands => this._commands;

        #endregion

        #region function

        /// <summary>
        /// 空行を生成。
        /// </summary>
        /// <returns>空行。</returns>
        public virtual EmptyLine CreateEmptyLine()
        {
            var result = new EmptyLine();
            return result;
        }

        /// <summary>
        /// 空行を追加。
        /// </summary>
        /// <returns>生成された空行。</returns>
        public EmptyLine AddEmptyLine()
        {
            var result = CreateEmptyLine();

            Commands.Add(result);

            return result;
        }

        /// <summary>
        /// 空行を追加。
        /// </summary>
        /// <param name="length">空行数。</param>
        /// <returns>生成された空行一覧。</returns>
        public EmptyLine[] AddEmptyLines(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

            var result = Enumerable
                .Repeat(0, length)
                .Select(x => CreateEmptyLine())
                .ToArray()
            ;

            this._commands.AddRange(result);

            return result;
        }

        /// <summary>
        /// シェル共通コマンドを生成。
        /// </summary>
        /// <typeparam name="TCommand">コマンド。</typeparam>
        /// <param name="creator">生成処理。</param>
        /// <returns>生成されたコマンド。</returns>
        public TCommand CreateShellCommand<TCommand>(Func<TShellImplementation, TCommand> creator)
            where TCommand : CommandBase
        {
            var command = creator(Implementation);
            return command;
        }

        /// <summary>
        /// シェル共通コマンドを追加。
        /// </summary>
        /// <typeparam name="TCommand">コマンド。</typeparam>
        /// <param name="creator">生成処理。</param>
        /// <returns>生成されたコマンド。</returns>
        public TCommand AddShellCommand<TCommand>(Func<TShellImplementation, TCommand> creator)
            where TCommand : CommandBase
        {
            var command = CreateShellCommand(creator);
            AppendCommand(command);
            return command;
        }

        /// <summary>
        /// シェル専用コマンドを生成。
        /// </summary>
        /// <typeparam name="TCommand">コマンド。</typeparam>
        /// <param name="creator">生成処理。</param>
        /// <returns>生成されたコマンド。</returns>
        public TCommand CreateCommand<TCommand>(Func<TShellImplementation, TCommand> creator)
            where TCommand : CommandPromptCommandBase
        {
            var command = creator(Implementation);
            return command;
        }

        /// <summary>
        /// シェル専用コマンドを追加。
        /// </summary>
        /// <typeparam name="TCommand">コマンド。</typeparam>
        /// <param name="creator">生成処理。</param>
        /// <returns>生成されたコマンド。</returns>
        public TCommand AddCommand<TCommand>(Func<TShellImplementation, TCommand> creator)
            where TCommand : CommandPromptCommandBase
        {
            var command = CreateShellCommand(creator);
            AppendCommand(command);
            return command;
        }

        /// <summary>
        /// コマンドを追加。
        /// </summary>
        /// <param name="command">コマンド。</param>
        public void AppendCommand(CommandBase command)
        {
            this._commands.Add(command);
        }

        /// <summary>
        /// 出力！
        /// </summary>
        /// <param name="stream">書き込み先。</param>
        /// <param name="cancellationToken">きゃんせるー。</param>
        /// <returns>ひどーき。</returns>
        public async Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            using(var writer = new StreamWriter(stream, Options.Encoding, 1024, true) {
                NewLine = Options.NewLine,
            }) {
                var indentContext = new IndentContext(Implementation.Indent, 0);
                foreach(var action in this._commands) {
                    cancellationToken.ThrowIfCancellationRequested();

                    var statement = action.ToStatement(indentContext);
                    await writer.WriteLineAsync(statement);
                }
            }
        }

        #endregion

        #region object

        public override string ToString()
        {
            var sb = new StringBuilder();

            var indentContext = new IndentContext(Implementation.Indent, 0);
            foreach(var action in this._commands) {
                var statement = action.ToStatement(indentContext);
                sb.Append(statement);
                sb.Append(Options.NewLine);
            }

            return sb.ToString();
        }

        #endregion
    }
}
