using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Library.Provider;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Provider.Test
{
    public class DefaultFileSystemProviderTest
    {
        #region function

        [Fact]
        public void ExistsFile_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateGhostFile("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.ExistsFile(file.FullName));
            Assert.False(provider.ExistsDirectory(file.FullName));
        }

        [Fact]
        public void ExistsFileTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateEmptyFile("👻");

            var provider = FileSystemProvider.Default;
            Assert.True(provider.ExistsFile(file.FullName));
            Assert.False(provider.ExistsDirectory(file.FullName));
        }


        [Fact]
        public void ExistsDirectory_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateGhostDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.ExistsFile(directory.FullName));
            Assert.False(provider.ExistsDirectory(directory.FullName));
        }

        [Fact]
        public void ExistsDirectoryTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.ExistsFile(directory.Directory.FullName));
            Assert.True(provider.ExistsDirectory(directory.Directory.FullName));
        }

        [Fact]
        public void DeleteFile_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var path = testIO.Work.CombineGhostPath("👻");

            var provider = FileSystemProvider.Default;
            var exception = Record.Exception(() => provider.DeleteFile(path));
            Assert.Null(exception);
        }

        [Fact]
        public void DeleteFileTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateEmptyFile("file");
            Assert.True(file.Exists);

            var provider = FileSystemProvider.Default;
            provider.DeleteFile(file.FullName);

            file.Refresh();
            Assert.False(file.Exists);
        }

        [Fact]
        public void DeleteDirectory_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var path = testIO.Work.CombineGhostPath("👻");

            var provider = FileSystemProvider.Default;
            Assert.Throws<DirectoryNotFoundException>(() => provider.DeleteDirectory(path));
        }

        [Fact]
        public void DeleteDirectory_DirOnly_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("a");

            directory.Directory.Refresh();
            Assert.True(directory.Directory.Exists);

            var provider = FileSystemProvider.Default;
            provider.DeleteDirectory(directory.Directory.FullName);

            directory.Directory.Refresh();
            Assert.False(directory.Directory.Exists);
        }

        [Fact]
        public void DeleteDirectory_DirWithFile_Flat_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("a");
            directory.CreateEmptyFile("b");

            directory.Directory.Refresh();
            Assert.True(directory.Directory.Exists);

            var provider = FileSystemProvider.Default;
            Assert.Throws<System.IO.IOException>(() => provider.DeleteDirectory(directory.Directory.FullName, false));
            directory.Directory.Refresh();
            Assert.True(directory.Directory.Exists);
        }

        [Fact]
        public void DeleteDirectory_DirWithFile_Nest_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("a");
            directory.CreateEmptyFile("b");

            directory.Directory.Refresh();
            Assert.True(directory.Directory.Exists);

            var provider = FileSystemProvider.Default;
            provider.DeleteDirectory(directory.Directory.FullName, true);

            directory.Directory.Refresh();
            Assert.False(directory.Directory.Exists);
        }

        #endregion
    }
}
