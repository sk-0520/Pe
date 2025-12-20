using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Provider
{
    public static class FileSystemProviderExtensions
    {
        #region function

        public static bool Exists(this FileSystemProvider provider, FileInfo file)
        {
            return provider.ExistsFile(file.FullName);
        }

        public static bool Exists(this FileSystemProvider provider, DirectoryInfo directory)
        {
            return provider.ExistsDirectory(directory.FullName);
        }

        public static void Delete(this FileSystemProvider service, FileInfo file)
        {
            service.DeleteFile(file.FullName);
        }

        public static void Delete(this FileSystemProvider service, DirectoryInfo directory, bool recursive = false)
        {
            service.DeleteDirectory(directory.FullName, recursive);
        }

        #endregion
    }
}
