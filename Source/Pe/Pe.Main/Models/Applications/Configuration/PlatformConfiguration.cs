using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class PlatformConfiguration: ConfigurationBase
    {
        #region define

        public class PlatformFullscreenConfiguration: ConfigurationBase
        {
            #region define

            public class PlatformFullscreenClassAndTextConfiguration: ConfigurationBase
            {
                public PlatformFullscreenClassAndTextConfiguration(IConfigurationSection section)
                    : base(section)
                {
                    WindowClassName = section.GetValue<string>("class");
                    WindowText = section.GetValue<string>("text");
                }

                #region property

                public string WindowClassName { get; }
                public string WindowText { get; }

                #endregion
            }

            #endregion

            public PlatformFullscreenConfiguration(IConfigurationSection section)
                : base(section)
            {
                IgnoreWindowClasses = section.GetSection("ignore_window_class").Get<string[]>();

                var classTextSection = section.GetSection("ignore_window_class_text");
                IgnoreClassAndTexts = classTextSection.GetChildren().Select(i => new PlatformFullscreenClassAndTextConfiguration(i)).ToArray();

                TopmostOnly = section.GetValue<bool>("topmost_only");
                ExcludeNoActive = section.GetValue<bool>("exclude_noactive");
                ExcludeToolWindow = section.GetValue<bool>("exclude_toolwindow");
            }

            #region proeprty

            public IReadOnlyList<string> IgnoreWindowClasses { get; }
            public IReadOnlyList<PlatformFullscreenClassAndTextConfiguration> IgnoreClassAndTexts { get; }

            public bool TopmostOnly { get; }
            public bool ExcludeNoActive { get; }
            public bool ExcludeToolWindow { get; }

            #endregion
        }

        #endregion

        public PlatformConfiguration(IConfigurationSection section)
            : base(section)
        {
            ThemeAccentColorMinimumAlpha = section.GetValue<byte>("theme_accent_color_minimum_alpha");
            ThemeAccentColorDefaultAlpha = section.GetValue<byte>("theme_accent_color_default_alpha");

            ExplorerSupporterRefreshTime = section.GetValue<TimeSpan>("explorer_supporter_refresh_time");
            ExplorerSupporterCacheSize = section.GetValue<int>("explorer_supporter_cache_size");

            ScreenElementsResetWaitTime = section.GetValue<TimeSpan>("screen_elements_reset_wait_time");

            Fullscreen = new PlatformFullscreenConfiguration(section.GetSection("fullscreen"));
        }

        #region property

        /// <summary>
        /// アクセントカラーの透明度を無効と判断する最低A値。
        /// <para>この値未満であれば無効。</para>
        /// </summary>
        public byte ThemeAccentColorMinimumAlpha { get; }
        /// <summary>
        /// アクセントカラーの透明度が<see cref="ThemeAccentColorMinimumAlpha"/>で無効判定なら使用するA値。
        /// </summary>
        public byte ThemeAccentColorDefaultAlpha { get; }
        public TimeSpan ExplorerSupporterRefreshTime { get; }
        public int ExplorerSupporterCacheSize { get; }

        public TimeSpan ScreenElementsResetWaitTime { get; }

        public PlatformFullscreenConfiguration Fullscreen { get; }

        #endregion
    }
}
