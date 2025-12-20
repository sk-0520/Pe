using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Common.Service
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

        public virtual bool ExistsFile(string path)
        {
            return File.Exists(path);
        }

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

        #endregion
    }

    file sealed class DefaultFileSystemService: FileSystemProvider
    { }
}
