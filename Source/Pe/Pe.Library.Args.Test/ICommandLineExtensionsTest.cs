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
            commandLine.Add("key", true);
            commandLine.Add("KEY", true);
            commandLine.Add("keY", true);
            commandLine.Add("switch", false);

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
        public void GetSwitchTest()
        {
            var commandLine = new CommandLine([
                "--key", "value1",
                "--KEY", "VALUE",
                "--key", "value2",
                "--switch"
            ]);
            commandLine.Add("key", true);
            commandLine.Add("KEY", true);
            commandLine.Add("keY", true);
            commandLine.Add("switch", false);

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
