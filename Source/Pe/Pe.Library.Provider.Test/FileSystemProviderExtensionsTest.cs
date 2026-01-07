using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Library.Provider;
using Xunit;
using static System.Net.WebRequestMethods;

namespace ContentTypeTextNet.Pe.Library.Provider.Test
{
    public class FileSystemProviderExtensions_Default_Test
    {
        #region function

        [Fact]
        public void Exists_Path_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var provider = FileSystemProvider.Default;

            // ディレクトリのみあり
            var name1 = "1";
            var path1 = testIO.Work.CreateGhostFile(name1);
            testIO.Work.CreateDirectory(name1);
            Assert.True(provider.Exists(path1.FullName));

            // ファイルのみあり
            var name2 = "2";
            var path2 = testIO.Work.CreateEmptyFile(name2);
            Assert.True(provider.Exists(path2.FullName));

            // ファイルもディレクトリもなし
            var name3 = "3";
            var path3 = testIO.Work.CreateGhostFile(name3);
            testIO.Work.CreateGhostDirectory(name3);
            Assert.False(provider.Exists(path3.FullName));

            // ファイルとディレクトリ両方あり -> そんな状況はない
        }

        [Fact]
        public void Exists_FileInfo_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateGhostFile("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.Exists(file));
        }

        [Fact]
        public void Exists_FileInfo_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateEmptyFile("👻");

            var provider = FileSystemProvider.Default;
            Assert.True(provider.Exists(file));
        }

        [Fact]
        public void Exists_Directory_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateGhostDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.Exists(directory));
        }

        [Fact]
        public void Exists_Directory_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.True(provider.Exists(directory.Directory));
        }

        [Fact]
        public void Delete_Path_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var provider = FileSystemProvider.Default;

            // ファイル
            var file1 = testIO.Work.CreateEmptyFile("1");
            provider.Delete(file1.FullName);
            file1.Refresh();
            Assert.False(file1.Exists);

            // ディレクトリ
            var dir2 = testIO.Work.CreateDirectory("2");
            provider.Delete(dir2.Directory.FullName);
            dir2.Directory.Refresh();
            Assert.False(dir2.Directory.Exists);

            // ディレクトリ（子あり）
            var dir3 = testIO.Work.CreateDirectory("3");
            dir3.CreateEmptyFile("3-child");
            dir3.CreateDirectory("3-sub").CreateEmptyFile("3-sub-child");
            provider.Delete(dir3.Directory.FullName);
            dir3.Directory.Refresh();
            Assert.False(dir3.Directory.Exists);
        }

        [Fact]
        public void Delete_Path_Throw_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var provider = FileSystemProvider.Default;

            var dir = testIO.Work.CreateGhostDirectory("👻");

            Assert.Throws<IOException>(() => provider.Delete(dir.FullName)); 
        }

        [Fact]
        public void Delete_FileInfo_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateGhostFile("👻");

            var provider = FileSystemProvider.Default;
            var exception = Record.Exception(() => provider.Delete(file));
            Assert.Null(exception);
        }

        [Fact]
        public void Delete_FileInfo_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateEmptyFile("👻");

            var provider = FileSystemProvider.Default;
            provider.Delete(file);

            file.Refresh();
            Assert.False(file.Exists);
        }

        [Fact]
        public void Delete_DirectoryInfo_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateGhostDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.Throws<DirectoryNotFoundException>(() => provider.Delete(directory));
        }

        [Fact]
        public void Delete_DirectoryInfo_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("👻");

            var provider = FileSystemProvider.Default;
            provider.Delete(directory.Directory);

            directory.Directory.Refresh();
            Assert.False(directory.Directory.Exists);
        }

        [Fact]
        public void MoveFile_Path_Normal_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateEmptyFile("src");
            var dst = testIO.Work.CreateGhostFile("dst");

            var provider = FileSystemProvider.Default;

            Assert.True(src.Exists);
            Assert.False(dst.Exists);

            provider.MoveFile(src.FullName, dst.FullName);

            src.Refresh();
            dst.Refresh();

            Assert.False(src.Exists);
            Assert.True(dst.Exists);
        }

        [Fact]
        public void MoveFile_Path_Not_Overwrite_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateTextFile("src", "SRC");
            var dst = testIO.Work.CreateTextFile("dst", "DST");

            var provider = FileSystemProvider.Default;

            Assert.Throws<IOException>(() => provider.MoveFile(src.FullName, dst.FullName, false));
        }

        [Fact]
        public void MoveFile_Path_Overwrite_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateTextFile("src", "SRC");
            var dst = testIO.Work.CreateTextFile("dst", "DST");

            var provider = FileSystemProvider.Default;

            provider.MoveFile(src.FullName, dst.FullName, true);

            src.Refresh();
            dst.Refresh();

            Assert.False(src.Exists);
            Assert.True(dst.Exists);

            using var text = dst.OpenText();
            var actual = text.ReadToEnd();
            Assert.Equal("SRC", actual);
        }

        [Fact]
        public void MoveDirectory_Path_Normal_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateDirectory("src");
            var dst = testIO.Work.CreateGhostDirectory("dst");

            src.CreateEmptyFile("file");
            var sub = src.CreateDirectory("sub");
            sub.CreateEmptyFile("sub-file");

            var provider = FileSystemProvider.Default;

            Assert.True(src.Directory.Exists);
            Assert.False(dst.Exists);

            provider.MoveDirectory(src.Directory.FullName, dst.FullName);

            src.Directory.Refresh();
            dst.Refresh();

            Assert.False(src.Directory.Exists);
            Assert.True(dst.Exists);
            var actualNodes = dst.EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                .Select(a => Path.GetRelativePath(dst.FullName, a.FullName))
                .ToArray()
            ;
            Assert.Contains("file", actualNodes);
            Assert.Contains("sub", actualNodes);
            Assert.Contains(Path.Join("sub", "sub-file"), actualNodes);
        }

        [Fact]
        public void CopyFile_Path_Normal_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateEmptyFile("src");
            var dst = testIO.Work.CreateGhostFile("dst");

            var provider = FileSystemProvider.Default;

            Assert.True(src.Exists);
            Assert.False(dst.Exists);

            provider.CopyFile(src.FullName, dst.FullName);

            src.Refresh();
            dst.Refresh();

            Assert.True(src.Exists);
            Assert.True(dst.Exists);
        }

        [Fact]
        public void CopyFile_Path_Not_Overwrite_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateTextFile("src", "SRC");
            var dst = testIO.Work.CreateTextFile("dst", "DST");

            var provider = FileSystemProvider.Default;

            Assert.Throws<IOException>(() => provider.CopyFile(src.FullName, dst.FullName, false));
        }

        [Fact]
        public void CopyFile_Path_Overwrite_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var src = testIO.Work.CreateTextFile("src", "SRC");
            var dst = testIO.Work.CreateTextFile("dst", "DST");

            var provider = FileSystemProvider.Default;

            provider.CopyFile(src.FullName, dst.FullName, true);

            src.Refresh();
            dst.Refresh();

            Assert.True(src.Exists);
            Assert.True(dst.Exists);

            using var text = dst.OpenText();
            var actual = text.ReadToEnd();
            Assert.Equal("SRC", actual);
        }

        #endregion
    }
}
