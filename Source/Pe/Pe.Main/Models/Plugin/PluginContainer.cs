using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal abstract class PluginContainerBase
    {
        #region function

        /// <summary>
        /// コンテナの保持しているプラグイン一覧。
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IPlugin> Plugins { get; }

        #endregion
    }

    internal class PluginContainer: PluginContainerBase
    {
        public PluginContainer(AddonContainer addonContainer, ThemeContainer themeContainer, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            Addon = addonContainer;
            Theme = themeContainer;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            EnvironmentParameters = environmentParameters;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        private HashSet<IPlugin> PluginsImpl { get; } = new HashSet<IPlugin>();
        /// <summary>
        /// アドオン用コンテナ。
        /// </summary>
        public AddonContainer Addon { get; }
        /// <summary>
        /// テーマ用コンテナ。
        /// </summary>
        public ThemeContainer Theme { get; }

        #endregion

        #region function

        public FileInfo? GetPluginFile(DirectoryInfo pluginDirectory, string pluginName, IReadOnlyList<string> extensions)
        {
            foreach(var extension in extensions) {
                var pluginFileName = PathUtility.AddExtension(pluginName, extension);
                var pluginPath = Path.Combine(pluginDirectory.FullName, pluginFileName);
                bool existsPlugin;
                try {
                    existsPlugin = File.Exists(pluginPath);
                } catch(Exception ex) {
                    Logger.LogError(ex, "プラグイン実ファイル取得失敗: {0}, {1}", ex.Message, pluginPath);
                    continue;
                }

                if(existsPlugin) {
                    return new FileInfo(pluginPath);
                }
            }

            foreach(var extension in extensions) {
                var pluginPaths = Directory.EnumerateFiles(pluginDirectory.FullName, "*." + extension);
                foreach(var pluginPath in pluginPaths) {
                    bool existsPlugin;
                    try {
                        existsPlugin = File.Exists(pluginPath);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "プラグイン実ファイル取得失敗: {0}, {1}", ex.Message, pluginPath);
                        continue;
                    }

                    if(existsPlugin) {
                        return new FileInfo(pluginPath);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// プラグインの実ファイル一覧を取得。
        /// <para>Pe 付属のプラグイン(<see cref="DefaultTheme"/>とか)は含まれない。</para>
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> GetPluginFiles(DirectoryInfo baseDirectory, IReadOnlyCollection<string> ignoreBaseFileName, IReadOnlyList<string> extensions)
        {
            var pluginDirs = baseDirectory.EnumerateDirectories();
            foreach(var pluginDir in pluginDirs) {
                // ディレクトリ名に依存しない形でなんかそれっぽいのを探し出す
                // 1ディレクトリにdll/exeが１つであることを前提にしている問題
                var baseFileNames = pluginDir.EnumerateFiles()
                    .Select(i => new { BaseName = Path.GetFileNameWithoutExtension(i.Name), Extension = 1 < i.Length ? i.Extension.Substring(1) : string.Empty })
                    .Where(i => !string.IsNullOrEmpty(i.Extension))
                    .Where(i => extensions.Select(i => i.ToLowerInvariant()).Contains(i.Extension.ToLowerInvariant()))
                    .Where(i => !ignoreBaseFileName.Select(i => i.ToLowerInvariant()).Contains(i.BaseName.ToLowerInvariant()))
                    .Distinct()
                    .Select(i => i.BaseName)
                    .ToArray()
                ;
                foreach(var baseFileName in baseFileNames) {
                    var pluginFile = GetPluginFile(pluginDir, baseFileName, extensions);
                    if(pluginFile != null) {
                        yield return pluginFile;
                    }
                }
            }
        }

        /// <summary>
        /// プラグインの読み込み。
        /// <para>検証結果がダメダメな場合は解放される。</para>
        /// </summary>
        /// <param name="pluginFile"></param>
        /// <returns>読み込み結果。</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public PluginLoadStateData LoadPlugin(FileInfo pluginFile, IReadOnlyList<PluginStateData> pluginStateItems, Version applicationVersion, IPluginConstructorContext pluginConstructorContext, PauseReceiveLogDelegate pauseReceiveLog)
        {
            var pluginBaseName = Path.GetFileNameWithoutExtension(pluginFile.Name);
            var currentPlugin = pluginStateItems.FirstOrDefault(i => string.Equals(pluginBaseName, i.PluginName, StringComparison.InvariantCultureIgnoreCase));
            if(currentPlugin != null) {
                if(currentPlugin.State == PluginState.Disable) {
                    Logger.LogInformation("(名前判定)プラグイン読み込み停止中: {0}, {1}", currentPlugin.PluginName, currentPlugin.PluginId);
                    return new PluginLoadStateData(currentPlugin.PluginId, currentPlugin.PluginName, new Version(), PluginState.Disable, null, null);
                }
            }
            var libraryDirectories = new[] {
                EnvironmentParameters.ApplicationDirectory,
            };
            var loadContext = new PluginAssemblyLoadContext(pluginFile, libraryDirectories, LoggerFactory);
            Assembly pluginAssembly;
            try {
                pluginAssembly = loadContext.Load();
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインアセンブリ読み込み失敗: {0}", pluginFile.Name);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? PluginId.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            Type? pluginInterfaceImpl = null;
            try {
                // 型情報が変な場合、例外が投げられるがそいつはなんかもう解放できなくなる
                var pluginTypes = pluginAssembly.GetTypes();//.Where(i => !(i.IsAbstract || i.IsNotPublic));
                foreach(var pluginType in pluginTypes) {
                    if(pluginType.IsAbstract || pluginType.IsNotPublic) {
                        continue;
                    }
                    Logger.LogDebug("{0}", pluginType.FullName);

                    var typeInterfaces = pluginType.GetInterfaces();
                    foreach(var typeInterface in typeInterfaces) {
                        Logger.LogDebug("> {0}", typeInterface.FullName);
                    }
                    var plugins = typeInterfaces.FirstOrDefault(i => i == typeof(IPlugin));
                    if(plugins != null) {
                        pluginInterfaceImpl = pluginType;
                        break;
                    }
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインアセンブリ リフレクション失敗: {0}", pluginFile.Name);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? PluginId.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            if(pluginInterfaceImpl == null) {
                Logger.LogError("プラグインアセンブリからプラグインインターフェイス取得できず: {0}, {1}", pluginAssembly.FullName, pluginFile.FullName);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? PluginId.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            Logger.LogDebug("[{0}]", pluginInterfaceImpl.FullName);
            foreach(var constructor in pluginInterfaceImpl.GetConstructors()) {
                var paras = string.Join(", ", constructor.GetParameters().Select(i => $"{i.ParameterType.FullName} {i.Name}"));
                Logger.LogDebug("-> {0}", paras);
            }

            IPlugin plugin;
            IPluginInformations pluginInformations;
            try {
                // コンストラクタ時にメモリログが参照に残るのを抑制
                using(pauseReceiveLog()) {
                    var pluginInstance = Activator.CreateInstance(pluginInterfaceImpl, new[] { pluginConstructorContext });
                    if(pluginInstance == null) {
                        throw new PluginInvalidAssemblyException($"{nameof(Activator)}.{nameof(Activator.CreateInstance)}失敗");
                    }
                    plugin = (IPlugin)pluginInstance ?? throw new PluginInvalidAssemblyException($"{nameof(IPlugin)}へのキャスト失敗: {pluginInstance}");
                    pluginInformations = plugin.PluginInformations;
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインインターフェイスを生成できず: {0}, {1}, {2}", ex.Message, pluginAssembly.FullName, pluginFile.FullName);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? PluginId.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            var pluginId = pluginInformations.PluginIdentifiers.PluginId;
            var pluginName = new string(pluginInformations.PluginIdentifiers.PluginName.ToCharArray()); // 一応複製

            var loadedCurrentPlugin = pluginStateItems.FirstOrDefault(i => i.PluginId == pluginId);
            if(loadedCurrentPlugin != null) {
                if(loadedCurrentPlugin.State == PluginState.Disable) {
                    Logger.LogInformation("(ID判定)プラグイン読み込み停止中: {0}({1}), {2}", loadedCurrentPlugin.PluginName, pluginName, loadedCurrentPlugin.PluginId);
                    loadContext.Unload();
                    return new PluginLoadStateData(loadedCurrentPlugin.PluginId, pluginName, new Version(), PluginState.Disable, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
                }
            }

            var pluginVersion = (Version)pluginInformations.PluginVersions.PluginVersion.Clone();
            var versionConverter = new VersionConverter();

            if(!PluginUtility.IsUnlimitedVersion(pluginInformations.PluginVersions.MinimumSupportVersion)) {
                var minVersion = versionConverter.TrimUndefinedElement(pluginInformations.PluginVersions.MinimumSupportVersion);
                var ok = minVersion <= applicationVersion;
                if(!ok) {
                    Logger.LogWarning("プラグインサポート最低バージョン({0}): {1}, {2}", pluginInformations.PluginVersions.MinimumSupportVersion, pluginName, pluginId);
                    loadContext.Unload();
                    return new PluginLoadStateData(pluginId, pluginName, pluginVersion, PluginState.IllegalVersion, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
                }
            }

            if(!PluginUtility.IsUnlimitedVersion(pluginInformations.PluginVersions.MaximumSupportVersion)) {
                var maxVersion = versionConverter.TrimUndefinedElement(pluginInformations.PluginVersions.MaximumSupportVersion);
                var ok = applicationVersion <= maxVersion;
                if(!ok) {
                    Logger.LogWarning("プラグインサポート最高バージョン({0}): {1}, {2}", pluginInformations.PluginVersions.MaximumSupportVersion, pluginName, pluginId);
                    loadContext.Unload();
                    return new PluginLoadStateData(pluginId, pluginName, pluginVersion, PluginState.IllegalVersion, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
                }
            }

            // 読み込み対象！
            Logger.LogInformation("プラグイン読み込み対象: {0}, {1}", pluginName, pluginId);
            return new PluginLoadStateData(pluginId, pluginName, pluginVersion, PluginState.Enable, new WeakReference<PluginAssemblyLoadContext>(loadContext), plugin);
        }

        /// <summary>
        /// プラグインの実体をコンテナに取り込み。
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(IPlugin plugin)
        {
            PluginsImpl.Add(plugin);

            if(plugin is ITheme theme) {
                Theme.Add(theme);
            }
            if(plugin is IAddon addon) {
                Addon.Add(addon);
            }
        }

        #endregion

        #region PluginContainerBase

        /// <inheritdoc cref="PluginContainerBase.Plugins"/>
        public override IEnumerable<IPlugin> Plugins => PluginsImpl;


        #endregion
    }
}
