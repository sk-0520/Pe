using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public abstract class ConfigurationBase
    {
        public ConfigurationBase(IConfigurationSection section)
        { }

        #region property

        protected static string GetString(IConfigurationSection section, string key) => section.GetValue<string>(key);
        protected static int GetInteger(IConfigurationSection section, string key) => section.GetValue<int>(key);

        #endregion
    }

    public class GeneralConfiguration : ConfigurationBase
    {
        public GeneralConfiguration(IConfigurationSection section) : base(section)
        {
            ProjectName = section.GetValue<string>("project-name");
            MutexName = section.GetValue<string>("mutex-name");
            LoggingConfigFileName = section.GetValue<string>("log-conf-file-name");
        }

        #region IGeneralConfiguration

        public string ProjectName { get; }
        public string MutexName { get; }
        public string LoggingConfigFileName { get; }

        #endregion
    }

    public class BackupConfiguration : ConfigurationBase
    {
        public BackupConfiguration(IConfigurationSection section)
            : base(section)
        {
            SettingCount = section.GetValue<int>("setting-count");
            ArchiveCount = section.GetValue<int>("archive-count");
        }

        #region property

        public int SettingCount { get; }
        public int ArchiveCount { get; }

        #endregion
    }

    public class FileConfiguration : ConfigurationBase
    {
        public FileConfiguration(IConfigurationSection section)
            : base(section)
        {
            DirectoryRemoveWaitCount = section.GetValue<int>("dir-remove-wait-count");
            DirectoryRemoveWaitTime = section.GetValue<TimeSpan>("dir-remove-wait-time");
        }

        #region property

        public int DirectoryRemoveWaitCount { get; }
        public TimeSpan DirectoryRemoveWaitTime{ get; }

        #endregion
    }

    public class Configuration
    {
        public Configuration(IConfigurationRoot configurationRoot)
        {
            General = new GeneralConfiguration(configurationRoot.GetSection("general"));
            Backup = new BackupConfiguration(configurationRoot.GetSection("backup"));
            File = new FileConfiguration(configurationRoot.GetSection("file"));
        }

        #region property

        public GeneralConfiguration General { get; }
        public BackupConfiguration Backup { get; }
        public FileConfiguration File { get; }
        #endregion
    }
}
