namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ウィジェットアドオンと Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IWidgetAddonCreateContext
    {
        #region property

        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// ウィジェットのViewが閉じた際の Pe との架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IWidgetAddonClosedContext
    {
        #region property

        IPluginStorage Storage { get; }

        #endregion
    }

}
