using System;
using ContentTypeTextNet.Pe.Generator.Exception;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグイン処理にて明示的に発生させる基底の例外。
    /// </summary>
    [GenerateException]
    public abstract partial class PluginException: Exception
    { }

    /// <summary>
    /// プラグインの読み込み失敗時に投げられる。
    /// </summary>
    [GenerateException]
    public partial class PluginInvalidAssemblyException: PluginException
    { }

    /// <summary>
    /// プラグインが存在しない場合に投げられる。
    /// </summary>
    [GenerateException]
    public partial class PluginNotFoundException: PluginException
    { }

    /// <summary>
    /// プラグインアーカイブの種別不明時に投げられる。
    /// </summary>
    [GenerateException]
    public partial class PluginInvalidArchiveKindException: PluginException
    { }

    /// <summary>
    /// プラグインアーカイブの展開ディレクトリが重複時に投げられる。
    /// </summary>
    [GenerateException]
    public partial class PluginDuplicateExtractDirectoryException: PluginException
    { }

    /// <summary>
    /// プラグインぶっ壊れ系。
    /// </summary>
    [GenerateException]
    public partial class PluginBrokenException: PluginException
    { }

    /// <summary>
    /// プラグインインストール失敗処理。
    /// </summary>
    [GenerateException]
    public partial class PluginInstallException: PluginException
    { }

    /// <summary>
    /// プラグインアーカイブ名不正。
    /// </summary>
    [GenerateException]
    public partial class PluginArchiveNameException: PluginException
    { }

    /// <summary>
    /// プラグイン使用不可例外。
    /// </summary>
    [GenerateException]
    public partial class PluginUnavailableContextException: PluginException
    { }
}
