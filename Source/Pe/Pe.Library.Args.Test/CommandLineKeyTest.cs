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
            var actual1 = new CommandLineKey("long", CommandLineKeyKind.Switch, "desc");
            Assert.Equal("long", actual1.LongKey);
            Assert.Equal(CommandLineKeyKind.Switch, actual1.kind);
            Assert.Equal("desc", actual1.Description);

            var actual2 = new CommandLineKey("long", CommandLineKeyKind.Value, "desc");
            Assert.Equal("long", actual2.LongKey);
            Assert.Equal(CommandLineKeyKind.Value, actual2.kind);
            Assert.Equal("desc", actual2.Description);

            Assert.Throws<ArgumentNullException>(() => new CommandLineKey(null!, CommandLineKeyKind.Switch, string.Empty));
            Assert.Throws<ArgumentException>(() => new CommandLineKey("", CommandLineKeyKind.Switch, string.Empty));
        }

        #endregion
    }
}
