namespace ContentTypeTextNet.Pe.Library.CommandLine
{
    /// <summary>
    /// コマンドラインオプション。
    /// </summary>
    /// <param name="Key">キー。<see cref="CommandLineHelper.ThrowIfInvalidKey(string)"/>を通過していること。</param>
    /// <param name="Kind">オプション種別。</param>
    /// <param name="Description">説明。</param>
    public record class CommandLineOption(
        string Key,
        CommandLineOptionKind Kind,
        string Description
    )
    {
        #region property

        /// <summary>
        /// 必須項目か。
        /// </summary>
        public bool Required { get; init; }
        public string ValueName { get; init; } = string.Empty;

        #endregion
    }
}
