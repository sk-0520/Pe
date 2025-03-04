using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class ICommandLineExtensionsTest
    {
        #region function

        [Fact]
        public void GetValueTest()
        {
            var commandLine = new CommandLine([
                "--key", "value1",
                "--KEY", "VALUE",
                "--key", "value2",
                "--switch"
            ]);
            commandLine.Add("key", kind: CommandLineKeyKind.Value);
            commandLine.Add("KEY", kind: CommandLineKeyKind.Value);
            commandLine.Add("keY", kind: CommandLineKeyKind.Value);
            commandLine.Add("switch", kind: CommandLineKeyKind.Switch);

            Assert.True(commandLine.Parse());

            var actual1 = commandLine.GetValue("key", "value3");
            Assert.Equal("value1", actual1);

            var actual2 = commandLine.GetValue("KEY", "value3");
            Assert.Equal("VALUE", actual2);

            var actual3 = commandLine.GetValue("Key", "value3");
            Assert.Equal("value3", actual3);

            var actual4 = commandLine.GetValue("keY", "value3");
            Assert.Equal("value3", actual4);

            var actual5 = commandLine.GetValue("switch", "value3");
            Assert.Equal("value3", actual5);
        }

        [Fact]
        public void GetValuesTest()
        {
            var commandLine = new CommandLine([
                "--key", "value1",
                "--KEY", "VALUE",
                "--key", "value2",
                "--switch"
            ]);
            commandLine.Add("key", kind: CommandLineKeyKind.Value);
            commandLine.Add("KEY", kind: CommandLineKeyKind.Value);
            commandLine.Add("keY", kind: CommandLineKeyKind.Value);
            commandLine.Add("switch", kind: CommandLineKeyKind.Switch);

            Assert.True(commandLine.Parse());

            var actual1 = commandLine.GetValues("key");
            Assert.Equal(["value1", "value2"], actual1);

            var actual2 = commandLine.GetValues("KEY");
            Assert.Equal(["VALUE"], actual2);

            var actual3 = commandLine.GetValues("Key");
            Assert.Empty(actual3);

            var actual4 = commandLine.GetValues("keY");
            Assert.Empty(actual4);

            var actual5 = commandLine.GetValues("switch");
            Assert.Empty(actual5);
        }

        [Fact]
        public void GetSwitchTest()
        {
            var commandLine = new CommandLine([
                "--key", "value1",
                "--KEY", "VALUE",
                "--key", "value2",
                "--switch"
            ]);
            commandLine.Add("key", kind: CommandLineKeyKind.Value);
            commandLine.Add("KEY", kind: CommandLineKeyKind.Value);
            commandLine.Add("keY", kind: CommandLineKeyKind.Value);
            commandLine.Add("switch", kind: CommandLineKeyKind.Switch);

            Assert.True(commandLine.Parse());

            var actual1 = commandLine.ExistsSwitch("key");
            Assert.False(actual1);

            var actual2 = commandLine.ExistsSwitch("KEY");
            Assert.False(actual2);

            var actual3 = commandLine.ExistsSwitch("Key");
            Assert.False(actual3);

            var actual4 = commandLine.ExistsSwitch("keY");
            Assert.False(actual4);

            var actual5 = commandLine.ExistsSwitch("switch");
            Assert.True(actual5);
        }

        #endregion
    }
}
