namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドラインオプションの種別。
    /// </summary>
    public enum CommandLineOptionKind
    {
        /// <summary>
        /// スイッチ。
        /// </summary>
        /// <remarks>値を持たない。</remarks>
        Switch,
        /// <summary>
        /// 値を持つ。
        /// </summary>
        Value,
    }
}
