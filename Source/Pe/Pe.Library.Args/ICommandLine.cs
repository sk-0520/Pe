using System;
using System.Collections.Generic;

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
        public string CommandName { get; }
        /// <summary>
        /// プログラム名を含まないコマンドライン引数。
        /// </summary>
        public IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// 解析が完了したか。
        /// </summary>
        public bool IsParsed { get; }

        /// <summary>
        /// キーアイテム一覧。
        /// </summary>
        public IReadOnlyList<CommandLineKey> Keys { get; }

        /// <summary>
        /// 値一覧。
        /// </summary>
        public IReadOnlyDictionary<CommandLineKey, ICommandLineValue> Values { get; }

        /// <summary>
        /// スイッチ一覧。
        /// </summary>
        public IReadOnlyCollection<CommandLineKey> Switches { get; }

        /// <summary>
        /// 不明アイテム一覧。
        /// </summary>
        public IReadOnlyList<string> Unknowns { get; }

        /// <summary>
        /// 解析時例外。
        /// </summary>
        public Exception? ParseException { get; }

        #endregion

        #region function

        /// <summary>
        /// 長いキーから値取得。
        /// </summary>
        /// <param name="longKey">長いキー。</param>
        /// <returns>取得した値。取得できない場合は<see langword="null" />。</returns>
        public CommandLineKey? GetKey(string longKey);

        #endregion
    }
}
