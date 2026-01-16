using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test
{
    public class CommandLineParsedResultTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual = new CommandLineParsedResult(
                "command",
                new Dictionary<string, ICommandLineValues>(),
                new HashSet<string>(),
                new Dictionary<int, string>(),
                []
            );
            Assert.Equal("command", actual.Command);
            Assert.Empty(actual.Values);
            Assert.Empty(actual.Switches);
            Assert.Empty(actual.Unknowns);
            Assert.Empty(actual.Raws);
        }

        #endregion
    }
}
