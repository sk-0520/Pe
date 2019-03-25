using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public class LauncherFactory
    {
        public LauncherFactory(IIdFactory idFactory, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        IIdFactory IdFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <param name="loadShortcut">ショートカットの内容を使用するか。</param>
        /// <returns></returns>
        public LauncherItemSimpleNewData FromFile(FileInfo file, bool loadShortcut)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(file.FullName);

            var isProgram = PathUtility.IsProgram(expandedPath);
            var isShortCut = PathUtility.IsShortcut(expandedPath);

            var result = new LauncherItemSimpleNewData() {
                LauncherItemId = IdFactory.CreateLauncherItemId(),
                Kind = LauncherItemKind.File,
                Name = FileUtility.GetName(expandedPath),
            };
            if(loadShortcut && PathUtility.IsShortcut(expandedPath)) {
                using(var shortcut = new ShortcutFile(expandedPath)) {
                    result.Code = Path.GetFileNameWithoutExtension(shortcut.TargetPath);

                    result.Command.Command = shortcut.TargetPath;
                    result.Command.Option = shortcut.Arguments;
                    result.Command.WorkDirectoryPath =  shortcut.WorkingDirectory;

                    result.Icon.Path = shortcut.IconPath;
                    result.Icon.Index = shortcut.IconIndex;
                    try {
                        result.Note = shortcut.Description;
                    } catch(COMException ex) {
                        Logger.Warning($"{expandedPath}");
                        Logger.Warning(ex);
                    }

                }
            } else {
                result.Code = Path.GetFileNameWithoutExtension(file.Name);
                result.Command.Command = file.FullName;
            }

            return result;
        }

        public IEnumerable<string> GetTags(FileInfo file)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(file.FullName);
            if(PathUtility.IsProgram(expandedPath) && File.Exists(expandedPath)) {
                var versionInfo = FileVersionInfo.GetVersionInfo(expandedPath);
                if(!string.IsNullOrEmpty(versionInfo.CompanyName)) {
                    yield return versionInfo.CompanyName;
                }
            }
        }

        public LauncherGroupData CreateGroupData(string name)
        {
            return new LauncherGroupData() {
                LauncherGroupId = IdFactory.CreateLauncherGroupId(),
                Name = name,
                Kind = LauncherGroupKind.Normal,
                ImageName = LauncherGroupImageName.Directory,
                ImageColor = Colors.Yellow,
            };
        }

        #endregion
    }
}
