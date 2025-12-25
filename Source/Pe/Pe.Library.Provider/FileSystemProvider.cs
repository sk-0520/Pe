using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Provider
{
    /// <summary>
    /// ファイルシステム系の抽象化基底。
    /// </summary>
    public abstract class FileSystemProvider
    {
        #region property

        /// <summary>
        /// デフォルトの <see cref="FileSystemProvider"/> を取得。
        /// </summary>
        /// <remarks>普通のファイル操作処理を想定して問題ない。</remarks>
        public static FileSystemProvider Default { get; } = new DefaultFileSystemService();

        #endregion

        #region function

        /// <inheritdoc cref="File.Exists(string)"/>
        public virtual bool ExistsFile(string path)
        {
            return File.Exists(path);
        }

        /// <inheritdoc cref="Directory.Exists(string)"/>
        public virtual bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <inheritdoc cref="File.Delete(string)"/>
        public virtual void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <inheritdoc cref="Directory.Delete(string, bool)"/>
        public virtual void DeleteDirectory(string path, bool recursive = false)
        {
            Directory.Delete(path, recursive);
        }

        /// <inheritdoc cref="File.Move(string, string, bool)"/>
        public virtual void MoveFile(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Move(sourceFileName, destFileName, overwrite);
        }

        /// <inheritdoc cref="Directory.Move(string, string)"/>
        public virtual void MoveDirectory(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <inheritdoc cref="File.Copy(string, string, bool)"/>
        public virtual void CopyFile(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <inheritdoc cref="File.Create(string, int, FileOptions)"/>
        public virtual FileStream CreateFile(string path, int bufferSize, FileOptions options)
        {
            return File.Create(path, bufferSize, options);
        }

        public virtual DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        [UnsupportedOSPlatform("windows")]
        public virtual DirectoryInfo CreateDirectory(string path, UnixFileMode unixCreateMode)
        {
            return Directory.CreateDirectory(path, unixCreateMode);
        }

        #endregion
    }

    file sealed class DefaultFileSystemService: FileSystemProvider
    { }
}
