using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.Args
{
    public interface ICommandLine
    {
        #region property

        /// <summary>
        /// プログラム/コマンド名。
        /// </summary>
        /// <remarks>
        /// <para><see langword="null" />が入ることはない。</para>
        /// </remarks>
        string CommandName { get; }
        /// <summary>
        /// プログラム名を含まないコマンドライン引数。
        /// </summary>
        IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// 解析が完了したか。
        /// </summary>
        bool IsParsed { get; }

        /// <summary>
        /// キーアイテム一覧。
        /// </summary>
        IReadOnlyList<CommandLineKey> Keys { get; }

        /// <summary>
        /// 値データ一覧。
        /// </summary>
        IReadOnlyDictionary<CommandLineKey, ICommandLineValue> Values { get; }

        /// <summary>
        /// スイッチデータ一覧。
        /// </summary>
        IReadOnlyCollection<CommandLineKey> Switches { get; }

        /// <summary>
        /// 不明データ一覧。
        /// </summary>
        IReadOnlyList<string> Unknowns { get; }

        /// <summary>
        /// 解析時例外。
        /// </summary>
        Exception? ParseException { get; }

        #endregion

        #region function

        /// <summary>
        /// 長いキーからキー取得。
        /// </summary>
        /// <param name="longKey">長いキー。</param>
        /// <param name="result">取得した値。取得できない場合は<see langword="null" />。</param>
        /// <returns></returns>
        bool TryGetKey(string longKey, [NotNullWhen(true)] out CommandLineKey? result);

        /// <summary>
        /// 長いキーからキー取得。
        /// </summary>
        /// <param name="longKey">長いキー。</param>
        /// <returns>キー。</returns>
        /// <exception cref="InvalidOperationException">取得できなかった。</exception>
        CommandLineKey? GetKey(string longKey);

        #endregion
    }
}
