using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Provider
{
    public static class FileSystemProviderExtensions
    {
        #region property

        // System.IO.DefaultBufferSize
        public static int DefaultBufferSize { get; } = 1024 * 4;

        #endregion

        #region function

        /// <summary>
        /// ファイル・ディレクトリ問わずに存在するか
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="path">対象パス。</param>
        /// <returns>存在するか。</returns>
        public static bool Exists(this FileSystemProvider provider, string path)
        {
            return provider.ExistsFile(path) || provider.ExistsDirectory(path);
        }

        public static bool Exists(this FileSystemProvider provider, FileInfo file)
        {
            var old = file.Exists;
            var now = provider.ExistsFile(file.FullName);
            if(old != now) {
                file.Refresh();
            }
            return now;
        }

        public static bool Exists(this FileSystemProvider provider, DirectoryInfo directory)
        {
            var old = directory.Exists;
            var now = provider.ExistsDirectory(directory.FullName);
            if(old != now) {
                directory.Refresh();
            }
            return now;
        }

        /// <summary>
        /// ファイル・ディレクトリ問わずに対象パスを削除。
        /// </summary>
        /// <remarks>
        /// <para>ディレクトリの場合は再帰的削除。</para>
        /// </remarks>
        /// <param name="path">対象パス。</param>
        public static void Delete(this FileSystemProvider service, string path)
        {
            if(service.ExistsFile(path)) {
                service.DeleteFile(path);
            } else if(service.ExistsDirectory(path)) {
                service.DeleteDirectory(path, true);
            } else {
                //TODO: IOUtility 再構築の過程で思ってること → ない場合に例外投げるのは微妙なんでは？ という思いとディレクトリ側との整合性がとれない件での微妙な思い
                throw new IOException($"not found: {path}");
            }
        }

        public static void Delete(this FileSystemProvider service, FileInfo file)
        {
            service.DeleteFile(file.FullName);
            file.Refresh();
        }

        public static void Delete(this FileSystemProvider service, DirectoryInfo directory, bool recursive = false)
        {
            service.DeleteDirectory(directory.FullName, recursive);
            directory.Refresh();
        }

        public static FileStream CreateFile(this FileSystemProvider service, string path)
        {
            return service.CreateFile(path, DefaultBufferSize, FileOptions.None);
        }

        public static FileStream CreateFile(this FileSystemProvider service, string path, int bufferSize)
        {
            return service.CreateFile(path, bufferSize, FileOptions.None);
        }

        #endregion
    }
}
