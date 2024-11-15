// <auto-generated>
// [T4] build 2024-11-15 12:46:08Z(UTC)
// </auto-generated>
namespace ContentTypeTextNet.Pe.Library.CliProxy.System
{
    /// <inheritdoc cref="global::System.Environment"/>
    public interface IEnvironmentProxy
    {
        #region property (20)
        /// <inheritdoc cref="global::System.Environment.TickCount"/>
        public int TickCount
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.ExitCode"/>
        public int ExitCode
        {
            get;
            set;
        }

        /// <inheritdoc cref="global::System.Environment.CommandLine"/>
        public string CommandLine
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.CurrentDirectory"/>
        public string CurrentDirectory
        {
            get;
            set;
        }

        /// <inheritdoc cref="global::System.Environment.SystemDirectory"/>
        public string SystemDirectory
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.MachineName"/>
        public string MachineName
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.ProcessorCount"/>
        public int ProcessorCount
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.SystemPageSize"/>
        public int SystemPageSize
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.NewLine"/>
        public string NewLine
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.Version"/>
        public global::System.Version Version
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.WorkingSet"/>
        public long WorkingSet
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.OSVersion"/>
        public global::System.OperatingSystem OSVersion
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.StackTrace"/>
        public string StackTrace
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.Is64BitProcess"/>
        public bool Is64BitProcess
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.Is64BitOperatingSystem"/>
        public bool Is64BitOperatingSystem
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.HasShutdownStarted"/>
        public bool HasShutdownStarted
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.UserName"/>
        public string UserName
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.UserInteractive"/>
        public bool UserInteractive
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.UserDomainName"/>
        public string UserDomainName
        {
            get;
        }

        /// <inheritdoc cref="global::System.Environment.CurrentManagedThreadId"/>
        public int CurrentManagedThreadId
        {
            get;
        }

        #endregion

        #region function (14)
        /// <inheritdoc cref="global::System.Environment.Exit(int)"/>
        public void Exit(int exitCode);
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariable(string)"/>
        public string GetEnvironmentVariable(string variable);
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariables()"/>
        public global::System.Collections.IDictionary GetEnvironmentVariables();
        /// <inheritdoc cref="global::System.Environment.SetEnvironmentVariable(string, string)"/>
        public void SetEnvironmentVariable(string variable, string value);
        /// <inheritdoc cref="global::System.Environment.GetLogicalDrives()"/>
        public global::System.String[] GetLogicalDrives();
        /// <inheritdoc cref="global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder)"/>
        public string GetFolderPath(global::System.Environment.SpecialFolder folder);
        /// <inheritdoc cref="global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder, global::System.Environment.SpecialFolderOption)"/>
        public string GetFolderPath(global::System.Environment.SpecialFolder folder, global::System.Environment.SpecialFolderOption option);
        /// <inheritdoc cref="global::System.Environment.FailFast(string)"/>
        public void FailFast(string message);
        /// <inheritdoc cref="global::System.Environment.FailFast(string, global::System.Exception)"/>
        public void FailFast(string message, global::System.Exception exception);
        /// <inheritdoc cref="global::System.Environment.ExpandEnvironmentVariables(string)"/>
        public string ExpandEnvironmentVariables(string name);
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariable(string, global::System.EnvironmentVariableTarget)"/>
        public string GetEnvironmentVariable(string variable, global::System.EnvironmentVariableTarget target);
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariables(global::System.EnvironmentVariableTarget)"/>
        public global::System.Collections.IDictionary GetEnvironmentVariables(global::System.EnvironmentVariableTarget target);
        /// <inheritdoc cref="global::System.Environment.SetEnvironmentVariable(string, string, global::System.EnvironmentVariableTarget)"/>
        public void SetEnvironmentVariable(string variable, string value, global::System.EnvironmentVariableTarget target);
        /// <inheritdoc cref="global::System.Environment.GetCommandLineArgs()"/>
        public global::System.String[] GetCommandLineArgs();
        #endregion
    }

    /// <inheritdoc cref="global::System.Environment"/>
    public class DirectEnvironmentProxy
    {
        #region property (20)
        /// <inheritdoc cref="global::System.Environment.TickCount"/>
        public int TickCount
        {
            get => global::System.Environment.TickCount;
        }

        /// <inheritdoc cref="global::System.Environment.ExitCode"/>
        public int ExitCode
        {
            get => global::System.Environment.ExitCode;
            set => global::System.Environment.ExitCode = value;
        }

        /// <inheritdoc cref="global::System.Environment.CommandLine"/>
        public string CommandLine
        {
            get => global::System.Environment.CommandLine;
        }

        /// <inheritdoc cref="global::System.Environment.CurrentDirectory"/>
        public string CurrentDirectory
        {
            get => global::System.Environment.CurrentDirectory;
            set => global::System.Environment.CurrentDirectory = value;
        }

        /// <inheritdoc cref="global::System.Environment.SystemDirectory"/>
        public string SystemDirectory
        {
            get => global::System.Environment.SystemDirectory;
        }

        /// <inheritdoc cref="global::System.Environment.MachineName"/>
        public string MachineName
        {
            get => global::System.Environment.MachineName;
        }

        /// <inheritdoc cref="global::System.Environment.ProcessorCount"/>
        public int ProcessorCount
        {
            get => global::System.Environment.ProcessorCount;
        }

        /// <inheritdoc cref="global::System.Environment.SystemPageSize"/>
        public int SystemPageSize
        {
            get => global::System.Environment.SystemPageSize;
        }

        /// <inheritdoc cref="global::System.Environment.NewLine"/>
        public string NewLine
        {
            get => global::System.Environment.NewLine;
        }

        /// <inheritdoc cref="global::System.Environment.Version"/>
        public global::System.Version Version
        {
            get => global::System.Environment.Version;
        }

        /// <inheritdoc cref="global::System.Environment.WorkingSet"/>
        public long WorkingSet
        {
            get => global::System.Environment.WorkingSet;
        }

        /// <inheritdoc cref="global::System.Environment.OSVersion"/>
        public global::System.OperatingSystem OSVersion
        {
            get => global::System.Environment.OSVersion;
        }

        /// <inheritdoc cref="global::System.Environment.StackTrace"/>
        public string StackTrace
        {
            get => global::System.Environment.StackTrace;
        }

        /// <inheritdoc cref="global::System.Environment.Is64BitProcess"/>
        public bool Is64BitProcess
        {
            get => global::System.Environment.Is64BitProcess;
        }

        /// <inheritdoc cref="global::System.Environment.Is64BitOperatingSystem"/>
        public bool Is64BitOperatingSystem
        {
            get => global::System.Environment.Is64BitOperatingSystem;
        }

        /// <inheritdoc cref="global::System.Environment.HasShutdownStarted"/>
        public bool HasShutdownStarted
        {
            get => global::System.Environment.HasShutdownStarted;
        }

        /// <inheritdoc cref="global::System.Environment.UserName"/>
        public string UserName
        {
            get => global::System.Environment.UserName;
        }

        /// <inheritdoc cref="global::System.Environment.UserInteractive"/>
        public bool UserInteractive
        {
            get => global::System.Environment.UserInteractive;
        }

        /// <inheritdoc cref="global::System.Environment.UserDomainName"/>
        public string UserDomainName
        {
            get => global::System.Environment.UserDomainName;
        }

        /// <inheritdoc cref="global::System.Environment.CurrentManagedThreadId"/>
        public int CurrentManagedThreadId
        {
            get => global::System.Environment.CurrentManagedThreadId;
        }

        #endregion

        #region function (14)
        /// <inheritdoc cref="global::System.Environment.Exit(int)"/>
        public void Exit(int exitCode) =>
            global::System.Environment.Exit(exitCode)
        ;
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariable(string)"/>
        public string GetEnvironmentVariable(string variable) =>
            global::System.Environment.GetEnvironmentVariable(variable)
        ;
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariables()"/>
        public global::System.Collections.IDictionary GetEnvironmentVariables() =>
            global::System.Environment.GetEnvironmentVariables()
        ;
        /// <inheritdoc cref="global::System.Environment.SetEnvironmentVariable(string, string)"/>
        public void SetEnvironmentVariable(string variable, string value) =>
            global::System.Environment.SetEnvironmentVariable(variable, value)
        ;
        /// <inheritdoc cref="global::System.Environment.GetLogicalDrives()"/>
        public global::System.String[] GetLogicalDrives() =>
            global::System.Environment.GetLogicalDrives()
        ;
        /// <inheritdoc cref="global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder)"/>
        public string GetFolderPath(global::System.Environment.SpecialFolder folder) =>
            global::System.Environment.GetFolderPath(folder)
        ;
        /// <inheritdoc cref="global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder, global::System.Environment.SpecialFolderOption)"/>
        public string GetFolderPath(global::System.Environment.SpecialFolder folder, global::System.Environment.SpecialFolderOption option) =>
            global::System.Environment.GetFolderPath(folder, option)
        ;
        /// <inheritdoc cref="global::System.Environment.FailFast(string)"/>
        public void FailFast(string message) =>
            global::System.Environment.FailFast(message)
        ;
        /// <inheritdoc cref="global::System.Environment.FailFast(string, global::System.Exception)"/>
        public void FailFast(string message, global::System.Exception exception) =>
            global::System.Environment.FailFast(message, exception)
        ;
        /// <inheritdoc cref="global::System.Environment.ExpandEnvironmentVariables(string)"/>
        public string ExpandEnvironmentVariables(string name) =>
            global::System.Environment.ExpandEnvironmentVariables(name)
        ;
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariable(string, global::System.EnvironmentVariableTarget)"/>
        public string GetEnvironmentVariable(string variable, global::System.EnvironmentVariableTarget target) =>
            global::System.Environment.GetEnvironmentVariable(variable, target)
        ;
        /// <inheritdoc cref="global::System.Environment.GetEnvironmentVariables(global::System.EnvironmentVariableTarget)"/>
        public global::System.Collections.IDictionary GetEnvironmentVariables(global::System.EnvironmentVariableTarget target) =>
            global::System.Environment.GetEnvironmentVariables(target)
        ;
        /// <inheritdoc cref="global::System.Environment.SetEnvironmentVariable(string, string, global::System.EnvironmentVariableTarget)"/>
        public void SetEnvironmentVariable(string variable, string value, global::System.EnvironmentVariableTarget target) =>
            global::System.Environment.SetEnvironmentVariable(variable, value, target)
        ;
        /// <inheritdoc cref="global::System.Environment.GetCommandLineArgs()"/>
        public global::System.String[] GetCommandLineArgs() =>
            global::System.Environment.GetCommandLineArgs()
        ;
        #endregion
    }
}


