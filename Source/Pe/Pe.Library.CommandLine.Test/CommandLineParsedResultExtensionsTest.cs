using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test
{
    public class CommandLineParsedResultExtensionsTest
    {
        #region function

        [Fact]
        public void GetValueTest()
        {
            var parser = new CommandLineParser();
            parser.Add("key", CommandLineOptionKind.Value);
            parser.Add("KEY", CommandLineOptionKind.Value);
            parser.Add("keY", CommandLineOptionKind.Value);
            parser.Add("switch", CommandLineOptionKind.Switch);

            var parsedResult = parser.Parse(
                string.Empty,
                [
                    "--key", "value1",
                    "--KEY", "VALUE",
                    "--key", "value2",
                    "--switch"
                ]
            );

            var actual1 = parsedResult.GetValue("key", "value3");
            Assert.Equal("value1", actual1);

            var actual2 = parsedResult.GetValue("KEY", "value3");
            Assert.Equal("VALUE", actual2);

            var actual3 = parsedResult.GetValue("Key", "value3");
            Assert.Equal("value3", actual3);

            var actual4 = parsedResult.GetValue("keY", "value3");
            Assert.Equal("value3", actual4);

            var actual5 = parsedResult.GetValue("switch", "value3");
            Assert.Equal("value3", actual5);
        }

        [Fact]
        public void GetValuesTest()
        {
            var parser = new CommandLineParser();
            parser.Add("key", CommandLineOptionKind.Value);
            parser.Add("KEY", CommandLineOptionKind.Value);
            parser.Add("keY", CommandLineOptionKind.Value);
            parser.Add("switch", CommandLineOptionKind.Switch);

            var parsedResult = parser.Parse(
                string.Empty,
                [
                    "--key", "value1",
                    "--KEY", "VALUE",
                    "--key", "value2",
                    "--switch"
                ]
            );

            var actual1 = parsedResult.GetValues("key");
            Assert.Equal(["value1", "value2"], actual1);

            var actual2 = parsedResult.GetValues("KEY");
            Assert.Equal(["VALUE"], actual2);

            var actual3 = parsedResult.GetValues("Key");
            Assert.Empty(actual3);

            var actual4 = parsedResult.GetValues("keY");
            Assert.Empty(actual4);

            var actual5 = parsedResult.GetValues("switch");
            Assert.Empty(actual5);
        }

        [Fact]
        public void GetSwitchTest()
        {
            var parser = new CommandLineParser();
            parser.Add("key", CommandLineOptionKind.Value);
            parser.Add("KEY", CommandLineOptionKind.Value);
            parser.Add("keY", CommandLineOptionKind.Value);
            parser.Add("switch", CommandLineOptionKind.Switch);

            var parsedResult = parser.Parse(
                string.Empty,
                [
                    "--key", "value1",
                    "--KEY", "VALUE",
                    "--key", "value2",
                    "--switch"
                ]
            );

            var actual1 = parsedResult.ExistsSwitch("key");
            Assert.False(actual1);

            var actual2 = parsedResult.ExistsSwitch("KEY");
            Assert.False(actual2);

            var actual3 = parsedResult.ExistsSwitch("Key");
            Assert.False(actual3);

            var actual4 = parsedResult.ExistsSwitch("keY");
            Assert.False(actual4);

            var actual5 = parsedResult.ExistsSwitch("switch");
            Assert.True(actual5);
        }

        #endregion
    }
}
