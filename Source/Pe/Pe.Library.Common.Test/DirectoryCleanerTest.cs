using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class DirectoryCleanerTest
    {
        #region function

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_throw_retryCount_Test(int retryCount)
        {
            var exception = Assert.Throws<ArgumentException>(() => new DirectoryCleaner(
                new System.IO.DirectoryInfo("NUL"),
                retryCount,
                TimeSpan.FromSeconds(3),
                NullLoggerFactory.Instance
            ));
            Assert.Equal("retryCount", exception.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_throw_waitTime_Test(long ticks)
        {
            var exception = Assert.Throws<ArgumentException>(() => new DirectoryCleaner(
                new System.IO.DirectoryInfo("NUL"),
                1,
                new TimeSpan(ticks),
                NullLoggerFactory.Instance
            ));
            Assert.Equal("waitTime", exception.ParamName);
        }

        [Fact]
        public void ConstructorTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            var workDir = testIO.Work.CreateDirectory("dir");

            var actual = new DirectoryCleaner(
                workDir.Directory,
                10,
                TimeSpan.FromSeconds(3),
                NullLoggerFactory.Instance
            );

            Assert.Equal(workDir.Directory, actual.Directory);
        }

        [Fact]
        public void Clear_default_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            testIO.Data.Directory.CreateSubdirectory("sub-empty");
            var enumerateItems = testIO.Data.Directory.EnumerateFileSystemInfos("*", System.IO.SearchOption.AllDirectories);
            // Assert.Equal(4, items.Count); // root.txt sub-item/ sub-item/sub.txt sub-empty/

            var directoryCleaner = new DirectoryCleaner(
               testIO.Data.Directory,
               10,
               TimeSpan.FromSeconds(3),
               NullLoggerFactory.Instance
           );
            directoryCleaner.Clear();

            Assert.Empty(enumerateItems);
            
        }

        [Fact]
        public void Clear_default_no_exists_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            var subDir = new DirectoryInfo( testIO.Work.CombinePath("sub"));
            subDir.Refresh();

            Assert.False(subDir.Exists);

            var directoryCleaner = new DirectoryCleaner(
               subDir,
               10,
               TimeSpan.FromSeconds(3),
               NullLoggerFactory.Instance
           );
            directoryCleaner.Clear();

            Assert.True(subDir.Exists);

        }

        #endregion
    }
}
