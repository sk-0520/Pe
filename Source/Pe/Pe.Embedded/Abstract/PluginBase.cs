using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Attributes;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    public abstract class PluginBase: IPlugin
    {
        protected PluginBase(IPluginConstructorContext pluginConstructorContext)
        {
            var type = GetType();
            Logger = pluginConstructorContext.LoggerFactory.CreateLogger(type);
            var interfaces = type.GetInterfaces();
            HasAddon = interfaces.Any(i => i == typeof(IAddon));
            HasTheme = interfaces.Any(i => i == typeof(ITheme));
            HasPreferences = interfaces.Any(i => i == typeof(PreferencesBase));
        }

        #region property

        protected ILogger Logger { get; }
        IPluginInformations? Informations { get; set; }

        protected bool HasAddon { get; }
        protected bool HasTheme { get; }

        protected bool IsLoadedAddon { get; private set; }
        protected bool IsLoadedTheme { get; private set; }
        protected bool HasPreferences { get; private set; }

        internal virtual AddonBase Addon { get => throw new NotImplementedException(); }
        internal virtual ThemeBase Theme { get => throw new NotImplementedException(); }
        private IPreferences? Preferences { get; set; }

        #endregion

        #region function

        protected virtual DependencyObject GetIconImpl(in IconScale iconScale) => null!;

        /// <summary>
        /// プラグインアセンブリの /Plugin.icon を取得する。
        /// </summary>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        protected DependencyObject GetPluginIcon(IImageLoader imageLoader, in IconScale iconScale)
        {
            var asm = Assembly.GetExecutingAssembly();
            var asmName = asm.GetName().Name;
            var uri = new Uri("pack://application:,,,/" + asmName + ";component/Plugin.ico");
            try {
                var decoder = BitmapDecoder.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                var bitmap = imageLoader.GetImageFromFrames(decoder.Frames, iconScale);
                //var bitmap = new BitmapImage(uri);
                var image = new System.Windows.Controls.Image() {
                    Source = bitmap,
                };
                return image;
            } catch(IOException ex) {
                Logger.LogError(ex, ex.Message);
            }
            return null!;
        }

        protected abstract void InitializeImpl(IPluginInitializeContext pluginInitializeContext);
        protected abstract void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext);


        protected virtual IPluginInformations CreateInformations()
        {
            static string CreateRandomText(string format, int count)
            {
                var rand = new Random();
                var randomValues = new byte[16];
                rand.NextBytes(randomValues);
                return string.Format(format, BitConverter.ToString(randomValues));
            }

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var assemblyType = assembly.GetType();

            var pluginIdentifiersAttr = assembly.GetCustomAttribute<PluginIdentifiersAttribute>();
            if(pluginIdentifiersAttr == null) {
                pluginIdentifiersAttr = new PluginIdentifiersAttribute(CreateRandomText("DUMMY-PLUGIN-{0}", 16), Guid.NewGuid().ToString());
                Logger.LogWarning("{0} の取得に失敗したためダミー値にて処理: {1}, {2}", nameof(PluginIdentifiersAttribute), pluginIdentifiersAttr.PluginName, pluginIdentifiersAttr.PluginId);
            }

            var supportVersionsAttr = assembly.GetCustomAttribute<PluginSupportVersionsAttribute>();
            if(supportVersionsAttr == null) {
                Logger.LogWarning("{0} の取得に失敗したため最低バージョンで補正", nameof(PluginSupportVersionsAttribute));
                supportVersionsAttr = new PluginSupportVersionsAttribute();
            }

            var pluginAuthorsAttr = assembly.GetCustomAttribute<PluginAuthorsAttribute>();
            if(pluginAuthorsAttr == null) {
                pluginAuthorsAttr = new PluginAuthorsAttribute(CreateRandomText("NAME-{0}", 4), PluginLicense.Unknown);
                Logger.LogWarning("{0} の取得に失敗したためダミー値にて処理: {1}, {2}", nameof(PluginIdentifiersAttribute), pluginAuthorsAttr.Name, pluginAuthorsAttr.License);
            }

            var pluginCategoryAttr = assembly.GetCustomAttribute<PluginCategoryAttribute>();
            if(pluginCategoryAttr == null) {
                pluginCategoryAttr = new PluginCategoryAttribute(PluginCategories.Others);
                Logger.LogWarning("{0} の取得に失敗したためダミー値にて処理: {1}", nameof(PluginCategoryAttribute), pluginCategoryAttr.Primary);
            }


            var pluginIdentifiers = new PluginIdentifiers(pluginIdentifiersAttr.PluginId, pluginIdentifiersAttr.PluginName);
            var pluginVersions = new PluginVersions(assemblyName.Version!, supportVersionsAttr.MinimumVersion, supportVersionsAttr.MaximumVersion);
            var pluginAuthors = new PluginAuthors(new Author(pluginAuthorsAttr.Name), pluginAuthorsAttr.License);
            var pluginCategory = new PluginCategory(pluginCategoryAttr.Primary, pluginCategoryAttr.Secondaries);

            return new PluginInformations(pluginIdentifiers, pluginVersions, pluginAuthors, pluginCategory);
        }

        [Conditional("DEBUG")]
        private void LoggingNotSupportAddon()
        {
            if(!HasAddon) {
                Logger.LogWarning("このプラグインはアドオンがサポートされていない");
            }
        }

        [Conditional("DEBUG")]
        private void LoggingNotSupportTheme()
        {
            if(!HasTheme) {
                Logger.LogWarning("このプラグインはテーマがサポートされていない");
            }
        }

        [Conditional("DEBUG")]
        private void LoggingNotSupportPreferences()
        {
            if(!HasPreferences) {
                Logger.LogWarning("このプラグインは設定がサポートされていない");
            }
        }


        private TTheme BuildSupporttedAddon<TArgument, TTheme>(AddonKind addonKind, string methodName, TArgument argument, Func<TArgument, TTheme> build)
        {
            if(!Addon.IsSupported(addonKind)) {
                Logger.LogWarning("{0} はサポートされていない", addonKind);
                throw new NotSupportedException();
            }

            try {
                return build(argument);
            } catch(NotImplementedException) {
                Logger.LogError("{0} の実装が必要({1})", methodName, addonKind);
                throw;
            }
        }

        private TTheme BuildSupporttedTheme<TArgument, TTheme>(ThemeKind themeKind, string methodName, TArgument argument, Func<TArgument, TTheme> build)
        {
            if(!Theme.IsSupported(themeKind)) {
                Logger.LogWarning("{0} はサポートされていない", themeKind);
                throw new NotSupportedException();
            }

            try {
                return build(argument);
            } catch(NotImplementedException) {
                Logger.LogError("{0} の実装が必要({1})", nameof(BuildGeneralTheme), themeKind);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IPreferences"/> を実装する場合に実装する必要あり。
        /// </summary>
        /// <returns></returns>
        protected virtual IPreferences CreatePreferences() => throw new NotImplementedException();

        #endregion

        #region IPlugin

        /// <inheritdoc cref="IPlugin.PluginInformations"/>
        public IPluginInformations PluginInformations => Informations ??= CreateInformations();

        /// <inheritdoc cref="IPlugin.IsInitialized"/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc cref="IPlugin.GetIcon(IImageLoader, IconScale)"/>
        public DependencyObject GetIcon(IImageLoader imageLoader, in IconScale iconScale)
        {
            try {
                var result = GetIconImpl(iconScale);
                if(result != null) {
                    return result;
                }
            } catch(NotImplementedException ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return GetPluginIcon(imageLoader, iconScale);
        }

        /// <inheritdoc cref="IPlugin"/>
        public void Initialize(IPluginInitializeContext pluginInitializeContext)
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            InitializeImpl(pluginInitializeContext);
            IsInitialized = true;
        }

        /// <inheritdoc cref="IPlugin.Uninitialize(IPluginUninitializeContext)"/>
        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext)
        {
            UninitializeImpl(pluginUninitializeContext);
            // 例外で死んだ場合は再初期化を避けるため補正しない
            IsInitialized = true;
        }

        /// <inheritdoc cref="IPlugin.Load(PluginKind, IPluginLoadContext)"/>
        public void Load(PluginKind pluginKind, IPluginLoadContext pluginLoadContext)
        {
            switch(pluginKind) {
                case PluginKind.Addon:
                    if(HasAddon) {
                        if(IsLoadedAddon) {
                            throw new InvalidOperationException(nameof(IsLoadedAddon));
                        }
                        Addon.Load(pluginLoadContext);
                        IsLoadedAddon = true;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                case PluginKind.Theme:
                    if(HasTheme) {
                        if(IsLoadedTheme) {
                            throw new InvalidOperationException(nameof(IsLoadedTheme));
                        }
                        Theme.Load(pluginLoadContext);
                        IsLoadedTheme = true;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        /// <inheritdoc cref="IPlugin.Unload(PluginKind, IPluginUnloadContext)"/>
        public void Unload(PluginKind pluginKind, IPluginUnloadContext pluginUnloadContext)
        {
            switch(pluginKind) {
                case PluginKind.Addon:
                    if(HasAddon) {
                        if(!IsLoadedAddon) {
                            throw new InvalidOperationException(nameof(IsLoadedAddon));
                        }
                        Addon.Unload(pluginUnloadContext);
                        IsLoadedAddon = false;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                case PluginKind.Theme:
                    if(HasTheme) {
                        if(!IsLoadedTheme) {
                            throw new InvalidOperationException(nameof(IsLoadedTheme));
                        }
                        Theme.Unload(pluginUnloadContext);
                        IsLoadedTheme = false;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        /// <inheritdoc cref="IPlugin.IsLoaded(PluginKind)"/>
        public bool IsLoaded(PluginKind pluginKind)
        {
            switch(pluginKind) {
                case PluginKind.Addon:
                    return IsLoadedAddon;

                case PluginKind.Theme:
                    return IsLoadedTheme;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IAddon

        /// <inheritdoc cref="IAddon.IsSupported(AddonKind)"/>
        public bool IsSupported(AddonKind addonKind)
        {
            LoggingNotSupportAddon();

            return Addon.IsSupported(addonKind);
        }

        /// <inheritdoc cref="IAddon.CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter)"/>
        public ILauncherItemExtension CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter parameter)
        {
            return BuildSupporttedAddon(AddonKind.LauncherItem, nameof(CreateLauncherItemExtension), parameter, p => Addon.CreateLauncherItemExtension(p));
        }

        /// <inheritdoc cref="IAddon.BuildCommandFinder(IAddonParameter)"/>
        public ICommandFinder BuildCommandFinder(IAddonParameter parameter)
        {
            return BuildSupporttedAddon(AddonKind.CommandFinder, nameof(BuildCommandFinder), parameter, p => Addon.BuildCommandFinder(p));
        }

        /// <inheritdoc cref="IAddon.BuildWidget(IAddonParameter)"/>
        public IWidget BuildWidget(IAddonParameter parameter)
        {
            return BuildSupporttedAddon(AddonKind.Widget, nameof(BuildWidget), parameter, p => Addon.BuildWidget(p));
        }

        /// <inheritdoc cref="IAddon.BuildBackground(IAddonParameter)"/>
        public IBackground BuildBackground(IAddonParameter parameter)
        {
            return BuildSupporttedAddon(AddonKind.Background, nameof(BuildBackground), parameter, p => Addon.BuildBackground(p));
        }


        #endregion

        #region ITheme

        /// <inheritdoc cref="ITheme.IsSupported(ThemeKind)"/>
        public bool IsSupported(ThemeKind themeKind)
        {
            LoggingNotSupportTheme();

            return Theme.IsSupported(themeKind);
        }

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.General, nameof(BuildGeneralTheme), parameter, p => Theme.BuildGeneralTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.LauncherToolbar, nameof(BuildLauncherToolbarTheme), parameter, p => Theme.BuildLauncherToolbarTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public INoteTheme BuildNoteTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Note, nameof(BuildNoteTheme), parameter, p => Theme.BuildNoteTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Command, nameof(BuildCommandTheme), parameter, p => Theme.BuildCommandTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Notify, nameof(BuildNotifyLogTheme), parameter, p => Theme.BuildNotifyLogTheme(p));
        }

        #endregion

        #region IPreferences

        /// <inheritdoc cref="IPreferences.BeginPreferences(IPreferencesLoadContext, IPreferencesParameter)"/>
        public virtual UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext, IPreferencesParameter preferencesParameter)
        {
            LoggingNotSupportPreferences();
            if(Preferences != null) {
                throw new InvalidOperationException();
            }

            Preferences = CreatePreferences();
            return Preferences.BeginPreferences(preferencesLoadContext, preferencesParameter);
        }

        /// <inheritdoc cref="IPreferences.CheckPreferences(IPreferencesCheckContext)"/>
        public virtual void CheckPreferences(IPreferencesCheckContext preferencesCheckContext)
        {
            LoggingNotSupportPreferences();
            if(Preferences == null) {
                throw new InvalidOperationException();
            }

            Preferences.CheckPreferences(preferencesCheckContext);
        }

        /// <inheritdoc cref="IPreferences.SavePreferences(IPreferencesSaveContext)"/>
        public virtual void SavePreferences(IPreferencesSaveContext preferencesSaveContext)
        {
            LoggingNotSupportPreferences();
            if(Preferences == null) {
                throw new InvalidOperationException();
            }

            Preferences.SavePreferences(preferencesSaveContext);
        }

        /// <inheritdoc cref="IPreferences.EndPreferences(IPreferencesEndContext)"/>
        public virtual void EndPreferences(IPreferencesEndContext preferencesEndContext)
        {
            LoggingNotSupportPreferences();
            if(Preferences == null) {
                throw new InvalidOperationException();
            }

            Preferences.EndPreferences(preferencesEndContext);
            Preferences = null;
        }

        #endregion

    }
}
