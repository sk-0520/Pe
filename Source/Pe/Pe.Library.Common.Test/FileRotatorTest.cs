using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class FileRotatorTest
    {
        #region function

        [Fact]
        public void ExecuteRegex_0()
        {
            var dir = new DirectoryInfo("nul");
            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(dir, new TestRegex("."), 10, ex => true);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void ExecuteRegex()
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
        }

        #endregion
    }
}
