using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPluginIdentifiers
    {
        #region property

        /// <summary>
        /// プラグインの識別ID。
        /// <para>重複してるとバグる。</para>
        /// </summary>
        public Guid PluginId { get; }

        /// <summary>
        /// プラグインを人が見て判断するための名前。
        /// <para><see cref="PluginId"/>と異なり重複してもいいけどなるべく重複しない方針。</para>
        /// <para>ローカライズは考えなくていい。</para>
        /// </summary>
        public string PluginName { get; }

        #endregion
    }

    public class PluginIdentifiers: IPluginIdentifiers
    {
        public PluginIdentifiers(Guid pluginId, string pluginName)
        {
            PluginId = pluginId;
            PluginName = pluginName;
        }

        #region IPluginIdentifiers

        /// <inheritdoc cref="IPluginIdentifiers.PluginId"/>
        public Guid PluginId { get; }
        /// <inheritdoc cref="IPluginIdentifiers.PluginName"/>
        public string PluginName { get; }

        #endregion
    }

    /// <summary>
    /// プラグインバージョン。
    /// </summary>
    public interface IPluginVersions
    {
        #region property

        /// <summary>
        /// プラグインのバージョン情報。
        /// </summary>
        Version PluginVersion { get; }

        /// <summary>
        /// プラグインが動作可能な Pe の最低バージョン(以上)。
        /// <para>0.0.0 で全バージョン稼働OK。</para>
        /// </summary>
        Version MinimumSupportVersion { get; }
        /// <summary>
        /// プラグインが動作可能な Pe の最大バージョン(以下)。
        /// <para>0.0.0 で全バージョン稼働OK。</para>
        /// </summary>
        Version MaximumSupportVersion { get; }

        #endregion
    }

    /// <inheritdoc cref="IPluginVersions"/>
    public class PluginVersions: IPluginVersions
    {
        public PluginVersions(Version pluginVersion, Version minimumSupportVersion, Version maximumSupportVersion)
        {
            PluginVersion = pluginVersion;
            MinimumSupportVersion = minimumSupportVersion;
            MaximumSupportVersion = maximumSupportVersion;

        }

        #region IPluginVersion

        /// <inheritdoc cref="IPluginVersions.PluginVersion"/>
        public Version PluginVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MinimumSupportVersion"/>
        public Version MinimumSupportVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MaximumSupportVersion"/>
        public Version MaximumSupportVersion { get; }

        #endregion
    }

    public static class PluginLicense
    {
        #region property

        public static string Unknown => "unknown";

        public static string DoWhatTheF_ckYouWantToPublicLicense1 => "WTFPLv1";
        public static string DoWhatTheF_ckYouWantToPublicLicense2 => "WTFPLv2";
        public static string GnuGeneralPublicLicense1 => "GPLv1";
        public static string GnuGeneralPublicLicense2 => "GPLv2";
        public static string GnuGeneralPublicLicense3 => "GPLv3";

        #endregion
    }

    /// <summary>
    /// プラグイン作者情報。
    /// </summary>
    public interface IPluginAuthors
    {
        /// <summary>
        /// プラグイン作者。
        /// </summary>
        IAuthor PluginAuthor { get; }
        /// <summary>
        /// プラグイン情報。
        /// </summary>
        string PluginLicense { get; }
    }

    /// <inheritdoc cref="IPluginAuthors"/>
    public class PluginAuthors: IPluginAuthors
    {
        public PluginAuthors(IAuthor pluginAuthor, string pluginLicense)
        {
            PluginAuthor = pluginAuthor;
            PluginLicense = pluginLicense;
        }
        #region IPluginAuthors

        /// <inheritdoc cref="IPluginAuthors.PluginAuthor"/>
        public IAuthor PluginAuthor { get; }
        /// <inheritdoc cref="IPluginAuthors.PluginLicense"/>
        public string PluginLicense { get; }

        #endregion
    }


    /// <summary>
    /// プラグイン情報。
    /// </summary>
    public interface IPluginInformations
    {

        /// <summary>
        /// プラグインID。
        /// </summary>
        IPluginIdentifiers PluginIdentifiers { get; }
        /// <summary>
        /// バージョン情報。
        /// </summary>
        IPluginVersions PluginVersions { get; }
        /// <summary>
        /// 作者情報。
        /// </summary>
        IPluginAuthors PluginAuthors { get; }
    }

    /// <inheritdoc cref="IPluginInformations"/>
    public class PluginInformations: IPluginInformations
    {
        public PluginInformations(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IPluginAuthors pluginAuthors)
        {
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            PluginAuthors = pluginAuthors;
        }

        #region IPluginInformations

        /// <inheritdoc cref="IPluginInformations.PluginIdentifiers"/>
        public IPluginIdentifiers PluginIdentifiers { get; }

        /// <inheritdoc cref="IPluginInformations.PluginVersions"/>
        public IPluginVersions PluginVersions { get; }

        /// <inheritdoc cref="IPluginInformations.PluginAuthors"/>
        public IPluginAuthors PluginAuthors { get; }

        #endregion
    }
}
