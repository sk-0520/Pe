using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPluginVersion
    {
        #region property

        /// <summary>
        /// プラグインのバージョン情報。
        /// </summary>
        Version PluginVersion { get; }

        /// <summary>
        /// プラグインが動作可能な Pe の最低バージョン。
        /// <para>0.83.0.18060 より上が最低バージョンとなる。</para>
        /// </summary>
        Version MinimumSupportVersion { get; }
        /// <summary>
        /// プラグインが動作可能な Pe の最大バージョン。
        /// </summary>
        Version MaximumSupportVersion { get; }

        #endregion
    }

    public interface IPluginAuthor
    {
        IAuthor PluginAuthor { get; }
        string PluginLicense { get; }
    }

    public interface IPluginInformation
    {
        IPluginVersion PluginVersion { get; }
        IPluginAuthor PluginAuthor { get; }
    }
}
