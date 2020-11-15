using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// システム上処理の実行。
    /// <para>OS上での実行を行う(ファイルなら開いてEXEなら起動的な)</para>
    /// </summary>
    public class SystemExecutor
    {
        #region property
        #endregion

        #region function

        public Process? RunDLL(string command)
        {
            var rundll = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
            var startupInfo = new ProcessStartInfo(rundll, command);

            return Process.Start(startupInfo);
        }

        /// <summary>
        /// タスクトレイ通知領域履歴を開く。
        /// </summary>
        /// <param name="appNonProcess"></param>
        public void OpenNotificationAreaHistory()
        {
            RunDLL("shell32.dll,Options_RunDLL 5");
        }

        public void OpenUri(Uri uri)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = uri.ToString();
            process.Start();
        }

        public Process OpenDirectoryWithFileSelect(string filePath)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = $"/select,{CommandLine.Escape(filePath)}";
            process.Start();
            return process;
        }

        public Process ExecuteFile(string filePath)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = filePath;
            startInfo.UseShellExecute = true;

            process.Start();

            return process;
        }

        public Process ExecuteFile(string filePath, string argument)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = filePath;
            startInfo.Arguments = argument;
            startInfo.UseShellExecute = true;

            process.Start();

            return process;
        }

        public void ShowProperty(string filePath)
        {
            NativeMethods.SHObjectProperties(IntPtr.Zero, SHOP.SHOP_FILEPATH, filePath, string.Empty);
        }

        #endregion
    }

}
