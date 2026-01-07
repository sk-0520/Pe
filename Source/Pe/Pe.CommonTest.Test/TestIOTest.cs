using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest.Test
{
    public class TestIOTest
    {
        #region function

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "<保留中>")]
        public void InitializeMethod_Initialized_Test()
        {
            // 行番号をもとに一意としているため無理やり一意とする
            // [Theory] なんかで同じテストを回す場合は workSuffix を使用する想定。
            var line = 0;
            TestIO.InitializeMethod(this, callerLineNumber: line);
            Assert.Throws<TestIOInitializedException>(() => TestIO.InitializeMethod(this, callerLineNumber: line));
        }

        [Fact]
        public void InitializeMethod_Simple_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            Assert.Throws<DirectoryNotFoundException>(() => testIO.Data.Directory.EnumerateFiles());
            Assert.Empty(testIO.Work.Directory.EnumerateFiles());
        }

        [Fact]
        public void InitializeMethod_Work_CreateDirectory_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var sub = testIO.Work.CreateDirectory("sub");
            var actual = testIO.Work.Directory.EnumerateDirectories().Single();
            Assert.Equal("sub", actual.Name);
        }

        [Fact]
        public void InitializeMethod_Work_NewFile_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var sub = testIO.Work.NewFile("sub");
            Assert.Empty(testIO.Work.Directory.EnumerateFiles());
        }

        [Fact]
        public void InitializeMethod_Work_CreateEmptyFile_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var sub = testIO.Work.CreateEmptyFile("sub");
            var actual = testIO.Work.Directory.EnumerateFiles().Single();
            Assert.Equal("sub", actual.Name);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        public void InitializeMethod_Work_Suffix_Test(string workSuffix)
        {
            var testIO = TestIO.InitializeMethod(this, workSuffix: workSuffix);
            var sub = testIO.Work.CreateEmptyFile("sub");
            var actual = testIO.Work.Directory.EnumerateFiles().Single();
            Assert.Equal("sub", actual.Name);
        }

        [Fact]
        public void InitializeMethod_Work_CreateTextFile_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var sub = testIO.Work.CreateTextFile("sub", "content");
            var actual = testIO.Work.Directory.EnumerateFiles().Single();
            Assert.Equal("sub", actual.Name);
            using var reader = actual.OpenText();
            Assert.Equal("content", reader.ReadToEnd());
        }

        [Fact]
        public void InitializeMethod_Work_CombineGhostPath_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var actual = testIO.Work.CombineGhostPath("path");
            Assert.False(File.Exists(actual));
        }

        [Fact]
        public void InitializeMethod_Work_CombineGhostPath_File_Throw_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var path = testIO.Work.CreateEmptyFile("path");    
            var exception = Assert.Throws<TestIOException>(() => testIO.Work.CombineGhostPath("path"));
            Assert.StartsWith("Path already exists:", exception.Message);
            Assert.Contains(path.FullName, exception.Message);
        }

        [Fact]
        public void InitializeMethod_Work_CombineGhostPath_Dir_Throw_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var path = testIO.Work.CreateDirectory("path");
            var exception = Assert.Throws<TestIOException>(() => testIO.Work.CombineGhostPath("path"));
            Assert.StartsWith("Path already exists:", exception.Message);
            Assert.Contains(path.Directory.FullName, exception.Message);
        }

        [Fact]
        public void InitializeMethod_Work_CreateGhostFile_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Work.CreateGhostFile("path");
            Assert.False(file.Exists);
            file.Create().Dispose();
            Assert.True(file.Exists);
        }

        [Fact]
        public void InitializeMethod_Work_CreateGhostDirectory_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var dir = testIO.Work.CreateGhostDirectory("path");
            Assert.False(dir.Exists);
            dir.Create();
            Assert.True(dir.Exists);
        }

        [Fact]
        public void InitializeMethod_Data_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            using var reader = testIO.Data.Read("sub.txt");
            var actual = reader.ReadToEnd();
            Assert.Equal("FILE", actual);
        }

        [Fact]
        public void InitializeMethod_Data_Extension_Test()
        {
            var testIO = TestIO.InitializeMethod(this, dataExtension: "ext");
            using var reader = testIO.Data.Read("sub.txt");
            var actual = reader.ReadToEnd();
            Assert.Equal("EXT", actual);
        }

        [Fact]
        public void InitializeMethod_Data_GetFile_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var file = testIO.Data.GetFile("sub.txt");
            Assert.True(file.Exists);
        }

        [Fact]
        public void InitializeMethod_Data_GetFile_Throw_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            Assert.Throws<TestIOException>(() => testIO.Data.GetFile("sub.txt"));
        }

        [Fact]
        public void InitializeMethod_Data_GetDirectory_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var dir = testIO.Data.GetDirectory("sub");
            Assert.True(dir.Exists);

            var actual = dir.EnumerateFiles().Single();
            Assert.Equal("child.txt", actual.Name);
        }

        [Fact]
        public void InitializeMethod_Data_GetDirectory_Throw_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            Assert.Throws<TestIOException>(() => testIO.Data.GetDirectory("sub"));
        }

        #endregion
    }
}
