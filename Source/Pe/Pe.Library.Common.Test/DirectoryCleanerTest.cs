using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Fact]
        public void ConstructorTest()
        {
            var pp = ProjectPath.Factory.CreateIO();
            var md = pp.CreateMethodDirectory(this);

            var actual = new DirectoryCleaner(
                md.Directory,
                10,
                TimeSpan.FromSeconds(3),
                NullLoggerFactory.Instance
            );

            Assert.Equal(md.Directory, actual.Directory);
        }

        #endregion
    }
}
