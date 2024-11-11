using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineAttributeTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual1 = new CommandLineAttribute("long", false, "desc");
            Assert.Equal("long", actual1.Key.LongKey);
            Assert.False(actual1.Key.HasValue);
            Assert.Equal("desc", actual1.Key.Description);

            var actual2 = new CommandLineAttribute("long", true, "desc");
            Assert.Equal("long", actual2.Key.LongKey);
            Assert.True(actual2.Key.HasValue);
            Assert.Equal("desc", actual2.Key.Description);

            Assert.Throws<ArgumentNullException>(() => new CommandLineAttribute(null!));
            Assert.Throws<ArgumentException>(() => new CommandLineAttribute(""));
        }

        #endregion
    }
}
