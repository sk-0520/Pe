using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineKeyTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual1 = new CommandLineKey("long", false, "desc");
            Assert.Equal("long", actual1.LongKey);
            Assert.False(actual1.HasValue);
            Assert.Equal("desc", actual1.Description);

            var actual2 = new CommandLineKey("long", true, "desc");
            Assert.Equal("long", actual2.LongKey);
            Assert.True(actual2.HasValue);
            Assert.Equal("desc", actual2.Description);

            Assert.Throws<ArgumentNullException>(() => new CommandLineKey(null!, false, string.Empty));
            Assert.Throws<ArgumentException>(() => new CommandLineKey("", false, string.Empty));
        }

        #endregion
    }
}
