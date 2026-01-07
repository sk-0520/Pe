using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Library.Provider;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class FileRotatorTest
    {
        #region function

        [Fact]
        public void ExecuteRegex_0_Test()
        {
            var dir = new DirectoryInfo("nul");
            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(dir, new TestRegex("."), 10, ex => true);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void ExecuteRegexTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(testIO.Work.Directory, new TestRegex("^target"), 3, ex => true);
            Assert.Equal(2, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(3, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_4.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_5.dmy");
        }

        [Fact]
        public void ExecuteRegex_Asc_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(testIO.Work.Directory, new TestRegex("^target"), 3, Common.Linq.Order.Ascending, ex => true);
            Assert.Equal(2, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(3, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_1.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_2.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
        }

        [Fact]
        public void ExecuteWildcardTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteWildcard(testIO.Work.Directory, "*.dmy", 3, ex => true);
            Assert.Equal(2, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(3, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_4.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_5.dmy");
        }

        [Fact]
        public void ExecuteWildcard_dm_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteWildcard(testIO.Work.Directory, "*.dm", 3, ex => true);
            Assert.Equal(0, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(5, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_1.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_2.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_4.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_5.dmy");
        }

        [Fact]
        public void ExecuteWildcard_Asc_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteWildcard(testIO.Work.Directory, "*.dmy", 3, Common.Linq.Order.Ascending, ex => true);
            Assert.Equal(2, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(3, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_1.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_2.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
        }

        [Fact]
        public void ExecuteRegex_Single_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            testIO.Work.CreateEmptyFile("target_1.ymd");
            testIO.Work.CreateEmptyFile("target_2.ymd");
            testIO.Work.CreateEmptyFile("target_3.ymd");
            testIO.Work.CreateEmptyFile("target_4.ymd");
            testIO.Work.CreateEmptyFile("target_5.ymd");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteExtensions(testIO.Work.Directory, ["dmy"], 3, ex => true);
            Assert.Equal(2, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(8, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_4.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_5.dmy");
        }

        [Fact]
        public void ExecuteRegex_Double_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            testIO.Work.CreateEmptyFile("target_1.ymd");
            testIO.Work.CreateEmptyFile("target_2.ymd");
            testIO.Work.CreateEmptyFile("target_3.ymd");
            testIO.Work.CreateEmptyFile("target_4.ymd");
            testIO.Work.CreateEmptyFile("target_5.ymd");


            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteExtensions(testIO.Work.Directory, ["dmy", "ymd"], 6, ex => true);
            Assert.Equal(4, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(6, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_4.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_5.dmy");

            Assert.Contains(actualFiles, a => a.Name == "target_3.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_4.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_5.ymd");
        }


        [Fact]
        public void ExecuteRegex_Single_Asc_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            testIO.Work.CreateEmptyFile("target_1.ymd");
            testIO.Work.CreateEmptyFile("target_2.ymd");
            testIO.Work.CreateEmptyFile("target_3.ymd");
            testIO.Work.CreateEmptyFile("target_4.ymd");
            testIO.Work.CreateEmptyFile("target_5.ymd");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteExtensions(testIO.Work.Directory, ["dmy"], 3, Common.Linq.Order.Ascending, ex => true);
            Assert.Equal(2, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(8, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_1.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_2.dmy");

            Assert.Contains(actualFiles, a => a.Name == "target_1.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_2.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_3.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_4.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_5.ymd");
        }

        [Fact]
        public void ExecuteRegex_Double_Asc_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");
            testIO.Work.CreateEmptyFile("target_4.dmy");
            testIO.Work.CreateEmptyFile("target_5.dmy");

            testIO.Work.CreateEmptyFile("target_1.ymd");
            testIO.Work.CreateEmptyFile("target_2.ymd");
            testIO.Work.CreateEmptyFile("target_3.ymd");
            testIO.Work.CreateEmptyFile("target_4.ymd");
            testIO.Work.CreateEmptyFile("target_5.ymd");


            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteExtensions(testIO.Work.Directory, ["dmy", "ymd"], 6, Common.Linq.Order.Ascending, ex => true);
            Assert.Equal(4, actual);
            var actualFiles = testIO.Work.Directory.GetFiles();
            Assert.Equal(6, actualFiles.Length);
            Assert.Contains(actualFiles, a => a.Name == "target_1.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_2.dmy");
            Assert.Contains(actualFiles, a => a.Name == "target_3.dmy");

            Assert.Contains(actualFiles, a => a.Name == "target_1.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_2.ymd");
            Assert.Contains(actualFiles, a => a.Name == "target_3.ymd");
        }

        private sealed class FileSystemTarget2ExceptionProvider: FileSystemProvider
        {
            #region FileSystemProvider

            public override void DeleteFile(string path)
            {
                var name = Path.GetFileName(path);
                if(name == "target_2.dmy") {
                    throw new NotImplementedException();
                }

                base.DeleteFile(path);
            }

            #endregion
        }

        [Fact]
        public void ExceptionCatcher_Ignore_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");

            var fileRotator = new FileRotator(new FileSystemTarget2ExceptionProvider());
            fileRotator.ExecuteExtensions(testIO.Work.Directory, ["dmy"], 0, ex => {
                return true;
            });

            var files = testIO.Work.Directory.GetFiles();
            Assert.Single(files);
            Assert.Contains(files, a => a.Name == "target_2.dmy");
        }

        [Fact]
        public void ExceptionCatcher_Handle_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Work.CreateEmptyFile("target_1.dmy");
            testIO.Work.CreateEmptyFile("target_2.dmy");
            testIO.Work.CreateEmptyFile("target_3.dmy");

            var fileRotator = new FileRotator(new FileSystemTarget2ExceptionProvider());
            fileRotator.ExecuteExtensions(testIO.Work.Directory, ["dmy"], 0, ex => {
                return false;
            });

            var files = testIO.Work.Directory.GetFiles();
            Assert.Equal(2, files.Length);
            Assert.Contains(files, a => a.Name == "target_1.dmy");
            Assert.Contains(files, a => a.Name == "target_2.dmy");
        }

        #endregion
    }
}
