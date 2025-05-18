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
            Assert.Throws<TestException>(() => testIO.Data.GetFile("sub.txt"));
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
            Assert.Throws<TestException>(() => testIO.Data.GetDirectory("sub"));
        }

        #endregion
    }
}
