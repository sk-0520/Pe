using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Library.Provider;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Provider.Test
{
    public class FileSystemProviderExtensionsTest
    {
        #region function

        [Fact]
        public void Default_Exists_FileInfo_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateGhostFile("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.Exists(file));
        }

        [Fact]
        public void Default_Exists_FileInfo_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateEmptyFile("👻");

            var provider = FileSystemProvider.Default;
            Assert.True(provider.Exists(file));
        }

        [Fact]
        public void Default_Exists_Directory_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateGhostDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.False(provider.Exists(directory));
        }

        [Fact]
        public void Default_Exists_Directory_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.True(provider.Exists(directory.Directory));
        }

        [Fact]
        public void Default_Delete_FileInfo_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateGhostFile("👻");

            var provider = FileSystemProvider.Default;
            var exception = Record.Exception(() => provider.Delete(file));
            Assert.Null(exception);
        }

        [Fact]
        public void Default_Delete_FileInfo_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateEmptyFile("👻");

            var provider = FileSystemProvider.Default;
            provider.Delete(file);

            file.Refresh();
            Assert.False(file.Exists);
        }

        [Fact]
        public void Default_Delete_DirectoryInfo_NotFound_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateGhostDirectory("👻");

            var provider = FileSystemProvider.Default;
            Assert.Throws<DirectoryNotFoundException>(() => provider.Delete(directory));
        }

        [Fact]
        public void Default_Delete_DirectoryInfo_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var directory = testIO.Work.CreateDirectory("👻");

            var provider = FileSystemProvider.Default;
            provider.Delete(directory.Directory);

            directory.Directory.Refresh();
            Assert.False(directory.Directory.Exists);
        }

        #endregion
    }
}
