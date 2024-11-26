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
        #region property

        private ProjectPath IOProjectPath { get; } = ProjectPath.Factory.CreateIO();

        #endregion

        #region function

        [Fact]
        public void ExecuteRegex_0()
        {
            var dir = new DirectoryInfo("nul");
            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(dir, new Regex("."), 10, ex => true);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void ExecuteRegex()
        {
            var methodPath = IOProjectPath.CreateMethodDirectory(this);

            methodPath.CreateEmptyFile("target_1.dmy");
            methodPath.CreateEmptyFile("target_2.dmy");
            methodPath.CreateEmptyFile("target_3.dmy");
            methodPath.CreateEmptyFile("target_4.dmy");
            methodPath.CreateEmptyFile("target_5.dmy");

            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(methodPath.Directory, new Regex("^target"), 3, ex => true);
            Assert.Equal(2, actual);
        }

        #endregion
    }
}
