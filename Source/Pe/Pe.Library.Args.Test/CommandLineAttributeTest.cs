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
            var actual1 = new CommandLineAttribute("long", CommandLineKeyKind.Switch, "desc");
            Assert.Equal("long", actual1.Key.LongKey);
            Assert.Equal(CommandLineKeyKind.Switch, actual1.Key.kind);
            Assert.Equal("desc", actual1.Key.Description);

            var actual2 = new CommandLineAttribute("long", CommandLineKeyKind.Value, "desc");
            Assert.Equal("long", actual2.Key.LongKey);
            Assert.Equal(CommandLineKeyKind.Value, actual2.Key.kind);
            Assert.Equal("desc", actual2.Key.Description);

            Assert.Throws<ArgumentNullException>(() => new CommandLineAttribute(null!));
            Assert.Throws<ArgumentException>(() => new CommandLineAttribute(""));
        }

        #endregion
    }
}
